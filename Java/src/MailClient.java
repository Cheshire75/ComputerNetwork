

import javax.net.ssl.SSLSocket;
import javax.net.ssl.SSLSocketFactory;
import javax.swing.JOptionPane;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.Base64;

public class MailClient {
    private String SMTP_SERVER;
    private int SMTP_PORT;

    public MailClient(String host) {
        this.SMTP_SERVER = host;
        this.SMTP_PORT = 465;
    }

    public void sendEmail(String senderEmail, String password, String recipient, String subject, String body) {
        SSLSocket socket = null;
        try {
            // SSL 소켓 생성 및 서버 연결
            SSLSocketFactory factory = (SSLSocketFactory) SSLSocketFactory.getDefault();
            socket = (SSLSocket) factory.createSocket(SMTP_SERVER, SMTP_PORT);
            socket.startHandshake();

            BufferedReader reader = new BufferedReader(new InputStreamReader(socket.getInputStream()));
            BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));

            // 서버 응답 읽기
            readResponse(reader);

            // EHLO
            writer.write("EHLO " + SMTP_SERVER + "\r\n");
            writer.flush();
            readResponse(reader);

            // AUTH PLAIN
            String authString = "\0" + senderEmail + "\0" + password;
            String authBase64 = Base64.getEncoder().encodeToString(authString.getBytes("UTF-8"));
            writer.write("AUTH PLAIN " + authBase64 + "\r\n");
            writer.flush();
            readResponse(reader);

            // MAIL FROM
            writer.write("MAIL FROM:<" + senderEmail + ">\r\n");
            writer.flush();
            readResponse(reader);

            // RCPT TO
            writer.write("RCPT TO:<" + recipient + ">\r\n");
            writer.flush();
            readResponse(reader);

            // DATA
            writer.write("DATA\r\n");
            writer.flush();
            readResponse(reader);

            // 이메일 본문 전송
            StringBuilder emailContent = new StringBuilder();
            emailContent.append("From: ").append(senderEmail).append("\r\n");
            emailContent.append("To: ").append(recipient).append("\r\n");
            emailContent.append("Subject: ").append(subject).append("\r\n");
            emailContent.append("\r\n").append(body).append("\r\n.\r\n");
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
            JOptionPane.showMessageDialog(null, e.getMessage(),"Warning",JOptionPane.WARNING_MESSAGE);
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
    /*
    public static void main(String[] args) {
    	/*
        String smtpId = "0603alswo"; // NAVER ID
        String password = "0603@alswo"; // NAVER 비밀번호
        String senderEmail = "0603alswo@naver.com"; // 발신자 이메일
        String recipient = "0603alswo@gmail.com"; // 수신자 이메일
        String subject = "Test Email"; // 이메일 제목
        String body = "Hello, this is a test email from Java using Naver SMTP."; // 이메일 본문

        MailClient mailClient = new MailClient("smtp.naver.com");
        mailClient.sendEmail(senderEmail, recipient, subject, body);
        
    }
*/
    
}
