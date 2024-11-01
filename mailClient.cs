using System;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;

class mailClient
{
    const string SMTP_SERVER = "smtp.naver.com";
    const int SMTP_PORT = 465;

    static void Main()
    {
        string smtpId = "smtpID"; // NAVER ID
        string password = "password"; // NAVER 비밀번호
        string senderEmail = "example@naver.com"; // 발신자 이메일
        string recipient = "example@gmail.com"; // 수신자 이메일
        string subject = "Test Email"; // 이메일 제목
        string body = "Hello, this is a test email from C# using Naver SMTP."; // 이메일 본문

        SendEmail(smtpId, password, senderEmail, recipient, subject, body);
    }

    static void SendEmail(string smtpId, string password, string senderEmail, string recipient, string subject, string body)
    {
        using (TcpClient client = new TcpClient(SMTP_SERVER, SMTP_PORT))
        using (SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)))
        {
            sslStream.AuthenticateAsClient(SMTP_SERVER);
            using (StreamReader reader = new StreamReader(sslStream))
            using (StreamWriter writer = new StreamWriter(sslStream) { AutoFlush = true })
            {
                // 서버 응답 읽기
                ReadResponse(reader);

                // EHLO
                writer.WriteLine($"EHLO {SMTP_SERVER}");
                ReadResponse(reader);

                // AUTH PLAIN
                string authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{smtpId}\0{password}"));
                writer.WriteLine($"AUTH PLAIN {authString}");
                ReadResponse(reader);

                // MAIL FROM
                writer.WriteLine($"MAIL FROM:<{senderEmail}>");
                ReadResponse(reader);

                // RCPT TO
                writer.WriteLine($"RCPT TO:<{recipient}>");
                ReadResponse(reader);

                // DATA
                writer.WriteLine("DATA");
                ReadResponse(reader);

                // 이메일 본문 전송
                StringBuilder emailContent = new StringBuilder();
                emailContent.AppendLine($"From: {senderEmail}");
                emailContent.AppendLine($"To: {recipient}");
                emailContent.AppendLine($"Subject: {subject}");
                emailContent.AppendLine();
                emailContent.AppendLine(body);
                emailContent.AppendLine(".");
                writer.WriteLine(emailContent.ToString());
                ReadResponse(reader);

                // QUIT
                writer.WriteLine("QUIT");
                ReadResponse(reader);
            }
        }
    }

    static void ReadResponse(StreamReader reader)
    {
        string response = reader.ReadLine();
        Console.WriteLine($"Server: {response}");
    }
}
