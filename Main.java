import java.io.File;

public class Main {
    public static void main(String[] args) {
        String smtpId = "0603alswo"; // NAVER ID
        String password = "0603@alswo"; // NAVER 비밀번호
        String senderEmail = "0603alswo@naver.com"; // 발신자 이메일
        String recipient = "0603alswo@gmail.com"; // 수신자 이메일
        String subject = "Test Email with Attachment"; // 이메일 제목
        String body = "Hello, this is a test email with an attachment from Java using Naver SMTP."; // 이메일 본문
        File attachment = new File("path/to/your/file"); // 첨부할 파일 경로

        MailClient mailClient = new MailClient("smtp.naver.com");
        
        try {
            String emailContent = EmailWithAttachment.createEmailContent(senderEmail, recipient, subject, body, attachment);
            mailClient.sendEmail(smtpId, password, senderEmail, recipient, subject, emailContent);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
