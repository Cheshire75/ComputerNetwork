#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <winsock2.h>
#include <openssl/ssl.h>
#include <openssl/err.h>
#include <openssl/bio.h>
#include <openssl/buffer.h>

#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "libssl.lib")
#pragma comment(lib, "libcrypto.lib")
#pragma warning(disable: 4996)

#define SMTP_SERVER "smtp.naver.com"
#define SMTP_PORT 465
#define BUFFER_SIZE 1024

char* base64_encode(const unsigned char* input, size_t length) {
    BIO* bio, * b64;
    BUF_MEM* bufferPtr;
    char* encoded;

    b64 = BIO_new(BIO_f_base64());
    bio = BIO_new(BIO_s_mem());
    bio = BIO_push(b64, bio);

    // Ignore newlines - write everything in one line
    BIO_set_flags(b64, BIO_FLAGS_BASE64_NO_NL);

    BIO_write(bio, input, length);
    BIO_flush(bio);
    BIO_get_mem_ptr(bio, &bufferPtr);

    // Add space for a null terminator
    encoded = (char*)malloc(bufferPtr->length + 1);
    memcpy(encoded, bufferPtr->data, bufferPtr->length);
    encoded[bufferPtr->length] = '\0';

    BIO_free_all(bio);

    return encoded;
}

char* create_auth_string(const char* username, const char* password) {
    // Calculate the total length needed: username\0username\0password
    size_t username_len = strlen(username);
    size_t password_len = strlen(password);
    size_t total_len = username_len + 1 + username_len + 1 + password_len;

    // Allocate memory for the authentication string
    unsigned char* auth = (unsigned char*)malloc(total_len);
    if (!auth) return NULL;

    // Build the authentication string
    size_t pos = 0;
    memcpy(auth + pos, username, username_len);
    pos += username_len;
    auth[pos++] = '\0';
    memcpy(auth + pos, username, username_len);
    pos += username_len;
    auth[pos++] = '\0';
    memcpy(auth + pos, password, password_len);

    // Base64 encode the authentication string
    char* encoded = base64_encode(auth, total_len);

    // Clean up
    free(auth);

    return encoded;
}

int read_smtp_response(SSL* ssl) {
    char buffer[BUFFER_SIZE];
    int response_code = 0;

    memset(buffer, 0, BUFFER_SIZE);
    int bytes = SSL_read(ssl, buffer, BUFFER_SIZE - 1);
    if (bytes > 0) {
        buffer[bytes] = '\0';
        sscanf(buffer, "%d", &response_code);
        printf("Server: %s", buffer);
    }

    return response_code;
}

int send_smtp_command(SSL* ssl, const char* command, const char* param) {
    char buffer[BUFFER_SIZE];
    if (param)
        snprintf(buffer, BUFFER_SIZE, "%s %s\r\n", command, param);
    else
        snprintf(buffer, BUFFER_SIZE, "%s\r\n", command);

    printf("Client: %s", buffer);
    SSL_write(ssl, buffer, strlen(buffer));
    return read_smtp_response(ssl);
}

void send_email(const char* smtp_id, const char* password, const char* sender_email,
    const char* recipient, const char* subject, const char* body) {
    WSADATA wsa;
    if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0) {
        printf("WSA Startup failed\n");
        return;
    }

    SSL_library_init();
    SSL_load_error_strings();
    OpenSSL_add_all_algorithms();

    struct hostent* server = gethostbyname(SMTP_SERVER);
    if (!server) {
        printf("Could not resolve SMTP server\n");
        return;
    }

    SOCKET socket_desc = socket(AF_INET, SOCK_STREAM, 0);
    struct sockaddr_in server_addr = { 0 };
    server_addr.sin_addr.s_addr = *((unsigned long*)server->h_addr);
    server_addr.sin_family = AF_INET;
    server_addr.sin_port = htons(SMTP_PORT);

    if (connect(socket_desc, (struct sockaddr*)&server_addr, sizeof(server_addr)) < 0) {
        printf("Connection failed\n");
        return;
    }

    SSL_CTX* ssl_ctx = SSL_CTX_new(SSLv23_client_method());
    if (!ssl_ctx) {
        printf("SSL context creation failed\n");
        closesocket(socket_desc);
        return;
    }

    SSL* ssl = SSL_new(ssl_ctx);
    SSL_set_fd(ssl, socket_desc);

    if (SSL_connect(ssl) != 1) {
        printf("SSL connection failed\n");
        SSL_free(ssl);
        SSL_CTX_free(ssl_ctx);
        closesocket(socket_desc);
        return;
    }

    // 초기 서버 응답 읽기
    if (read_smtp_response(ssl) != 220) {
        printf("Server not ready\n");
        goto cleanup;
    }

    // EHLO
    if (send_smtp_command(ssl, "EHLO", "localhost") != 250) {
        printf("EHLO failed\n");
        goto cleanup;
    }

    // AUTH PLAIN
    char* auth_str = create_auth_string(smtp_id, password);
    if (!auth_str) {
        printf("Auth string creation failed\n");
        goto cleanup;
    }

    char auth_command[BUFFER_SIZE];
    snprintf(auth_command, BUFFER_SIZE, "AUTH PLAIN %s", auth_str);
    if (send_smtp_command(ssl, auth_command, NULL) != 235) {
        printf("Authentication failed\n");
        free(auth_str);
        goto cleanup;
    }
    free(auth_str);

    // MAIL FROM
    char mail_from[BUFFER_SIZE];
    snprintf(mail_from, BUFFER_SIZE, "<%s>", sender_email);
    if (send_smtp_command(ssl, "MAIL FROM:", mail_from) != 250) {
        printf("MAIL FROM failed\n");
        goto cleanup;
    }

    // RCPT TO
    char rcpt_to[BUFFER_SIZE];
    snprintf(rcpt_to, BUFFER_SIZE, "<%s>", recipient);
    if (send_smtp_command(ssl, "RCPT TO:", rcpt_to) != 250) {
        printf("RCPT TO failed\n");
        goto cleanup;
    }

    // DATA
    if (send_smtp_command(ssl, "DATA", NULL) != 354) {
        printf("DATA command failed\n");
        goto cleanup;
    }

    // Send email content
    char email_content[BUFFER_SIZE];
    snprintf(email_content, BUFFER_SIZE,
        "From: %s\r\n"
        "To: %s\r\n"
        "Subject: %s\r\n"
        "\r\n"
        "%s\r\n"
        ".\r\n",
        sender_email, recipient, subject, body);

    SSL_write(ssl, email_content, strlen(email_content));
    if (read_smtp_response(ssl) != 250) {
        printf("Sending email content failed\n");
        goto cleanup;
    }

    // QUIT
    send_smtp_command(ssl, "QUIT", NULL);

cleanup:
    SSL_shutdown(ssl);
    SSL_free(ssl);
    SSL_CTX_free(ssl_ctx);
    closesocket(socket_desc);
    WSACleanup();
}

int main() {
    const char* smtp_id = "0603alswo";
    const char* password = "0603@alswo";
    const char* sender_email = "0603alswo@naver.com";
    const char* recipient = "0603alswo@gmail.com";
    const char* subject = "Test Email";
    const char* body = "Hello, this is a test email from C using Naver SMTP.";

    send_email(smtp_id, password, sender_email, recipient, subject, body);
    return 0;
}