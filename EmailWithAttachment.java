import java.io.File;
import java.nio.file.Files;
import java.util.Base64;

public class EmailWithAttachment {

    public static String createEmailContent(String senderEmail, String recipient, String subject, String body, File attachment) throws Exception {
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
            emailContent.append("Content-Type: application/octet-stream; name=").append(attachment.getName()).append("\r\n");
            emailContent.append("Content-Transfer-Encoding: base64\r\n");
            emailContent.append("Content-Disposition: attachment; filename=").append(attachment.getName()).append("\r\n");
            emailContent.append("\r\n").append(encodedAttachment).append("\r\n");
        }

        emailContent.append("--").append(boundary).append("--\r\n.\r\n");
        return emailContent.toString();
    }
}
