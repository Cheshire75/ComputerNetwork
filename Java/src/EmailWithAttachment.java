
import java.util.Base64;
/*
public class EmailWithAttachment {
    private String smtpServer;
    private int smtpPort;

    public EmailWithAttachment(String smtpServer) {
        this.smtpServer = smtpServer;
        this.smtpPort = 465; // SSL 기본 포트
    }

    public void sendEmailWithAttachment(String smtpId, String password, String senderEmail, String recipients,
                                        String subject, String body, File attachment) {
        SSLSocket socket = null;
        try {
            // SSL 소켓 생성 및 서버 연결
            SSLSocketFactory factory = (SSLSocketFactory) SSLSocketFactory.getDefault();
            socket = (SSLSocket) factory.createSocket(smtpServer, smtpPort);
            socket.startHandshake();

            BufferedReader reader = new BufferedReader(new InputStreamReader(socket.getInputStream()));
            BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));

            // 서버 응답 읽기
            readResponse(reader);

            // EHLO
            writer.write("EHLO " + smtpServer + "\r\n");
            writer.flush();
            readResponse(reader);

            // AUTH PLAIN
            String authString = "\0" + smtpId + "\0" + password;
            String authBase64 = Base64.getEncoder().encodeToString(authString.getBytes("UTF-8"));
            writer.write("AUTH PLAIN " + authBase64 + "\r\n");
            writer.flush();
            readResponse(reader);

            // MAIL FROM
            writer.write("MAIL FROM:<" + senderEmail + ">\r\n");
            writer.flush();
            readResponse(reader);

            // RCPT TO for each recipient
            String[] recipientList = recipients.split(",");
            for (String recipient : recipientList) {
                writer.write("RCPT TO:<" + recipient.trim() + ">\r\n");
                writer.flush();
                readResponse(reader);
            }

            // DATA
            writer.write("DATA\r\n");
            writer.flush();
            readResponse(reader);

            // MIME 헤더 작성
            String boundary = "----=_Boundary_" + System.currentTimeMillis();
            StringBuilder emailContent = new StringBuilder();
            emailContent.append("From: ").append(senderEmail).append("\r\n");
            emailContent.append("To: ").append(recipients).append("\r\n");
            emailContent.append("Subject: ").append(subject).append("\r\n");
            emailContent.append("MIME-Version: 1.0\r\n");
            emailContent.append("Content-Type: multipart/mixed; boundary=\"").append(boundary).append("\"\r\n\r\n");

            // 이메일 본문
            emailContent.append("--").append(boundary).append("\r\n");
            emailContent.append("Content-Type: text/plain; charset=UTF-8\r\n\r\n");
            emailContent.append(body).append("\r\n\r\n");

            // 첨부 파일 추가
            if (attachment != null && attachment.exists()) {
                emailContent.append("--").append(boundary).append("\r\n");
                emailContent.append("Content-Type: application/octet-stream; name=\"").append(attachment.getName()).append("\"\r\n");
                emailContent.append("Content-Transfer-Encoding: base64\r\n");
                emailContent.append("Content-Disposition: attachment; filename=\"").append(attachment.getName()).append("\"\r\n\r\n");

                // 파일을 Base64로 인코딩하여 본문에 추가
                emailContent.append(encodeFileToBase64(attachment)).append("\r\n\r\n");
            }

            // 메일 본문 종료
            emailContent.append("--").append(boundary).append("--\r\n.\r\n");
            writer.write(emailContent.toString());
            writer.flush();
            readResponse(reader);

            // QUIT
            writer.write("QUIT\r\n");
            writer.flush();
            readResponse(reader);

        } catch (Exception e) {
            e.printStackTrace();
            System.out.println("Error: " + e.getMessage());
        } finally {
            // 소켓 닫기
            try {
                if (socket != null && !socket.isClosed()) {
                    socket.close();
                }
            } catch (Exception ex) {
                ex.printStackTrace();
            }
        }
    }

    private void readResponse(BufferedReader reader) throws Exception {
        String response = reader.readLine().substring(0, 3);

        // 예상치 못한 응답 코드가 있을 경우 오류 메시지 표시
        if (!response.equals("220") && !response.equals("250") && !response.equals("354") &&
            !response.equals("334") && !response.equals("235") && !response.equals("221")) {
            throw new Exception("메일 발송 중 오류가 발생했습니다. 코드: " + response);
        }
    }

    private String encodeFileToBase64(File file) throws IOException {
        try (FileInputStream fileInputStream = new FileInputStream(file);
             ByteArrayOutputStream outputStream = new ByteArrayOutputStream()) {
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = fileInputStream.read(buffer)) != -1) {
                outputStream.write(buffer, 0, bytesRead);
            }
            return Base64.getEncoder().encodeToString(outputStream.toByteArray());
        }
    }

    public static void main(String[] args) {
        String smtpId = "hjzang06"; // NAVER ID
        String password = ""; // NAVER 비밀번호
        String senderEmail = "hjzang06@naver.com"; // 발신자 이메일
        String recipients = "hjzang06@gmail.com, 01081089397a@gmail.com"; // 수신자 이메일 목록
        String subject = "Test Email with Attachment"; // 이메일 제목
        String body = "Hello, this is a test email with an attachment."; // 이메일 본문
        File attachment = new File("/Users/zang/Desktop/컴퓨터네트워크/무제.txt"); // 첨부 파일 경로 설정

        EmailWithAttachment emailClient = new EmailWithAttachment("smtp.naver.com");
        emailClient.sendEmailWithAttachment(smtpId, password, senderEmail, recipients, subject, body, attachment);
=======
*/
import java.io.File;
import java.nio.file.Files;

public class EmailWithAttachment {

    public static String createEmailContent(String senderEmail, String recipient, String subject, String body,
            File attachment) throws Exception {
        String boundary = "----=_Boundary_" + System.currentTimeMillis();
        StringBuilder emailContent = new StringBuilder();

        emailContent.append("From: ").append(senderEmail).append("\r\n");
        emailContent.append("To: ").append(recipient).append("\r\n");
        emailContent.append("Subject: ").append(subject).append("\r\n");
        emailContent.append("MIME-Version: 1.0\r\n");
        emailContent.append("Content-Type: multipart/mixed; boundary=").append(boundary).append("\r\n");
        emailContent.append("\r\n--").append(boundary).append("\r\n");
        emailContent.append("Content-Type: text/plain; charset=UTF-8\r\n");
        emailContent.append("Content-Transfer-Encoding: 7bit\r\n");
        emailContent.append("\r\n").append(body).append("\r\n");

        if (attachment != null && attachment.exists()) {
            String encodedAttachment = Base64.getEncoder().encodeToString(Files.readAllBytes(attachment.toPath()));
            emailContent.append("\r\n--").append(boundary).append("\r\n");
            emailContent.append("Content-Type: application/octet-stream; name=").append(attachment.getName())
                    .append("\r\n");
            emailContent.append("Content-Transfer-Encoding: base64\r\n");
            emailContent.append("Content-Disposition: attachment; filename=").append(attachment.getName())
                    .append("\r\n");
            emailContent.append("\r\n").append(encodedAttachment).append("\r\n");
        }

        emailContent.append("--").append(boundary).append("--\r\n.\r\n");
        return emailContent.toString();
    }
}
