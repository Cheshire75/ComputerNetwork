//using System;
//using System.IO;
//using System.Net.Sockets;
//using System.Net.Security;
//using System.Text;
//using System.Windows.Forms;

//class mailClient
//{
//    private string SMTP_SERVER;
//    private int SMTP_PORT;

//    public mailClient(string host) {
//        SMTP_SERVER = host;
//        SMTP_PORT = 465;
//    }
//    /*
//    static void Main()
//    {
//        string smtpId = "smtpID"; // NAVER ID
//        string password = "password"; // NAVER 비밀번호
//        string senderEmail = "example@naver.com"; // 발신자 이메일
//        string recipient = "example@gmail.com"; // 수신자 이메일
//        string subject = "Test Email"; // 이메일 제목
//        string body = "Hello, this is a test email from C# using Naver SMTP."; // 이메일 본문

//        SendEmail(smtpId, password, senderEmail, recipient, subject, body);
//    }
//    */

//    public void SendEmail(string senderEmail, string password, string recipient, string subject, string body)
//    {
//        try {
//            using (TcpClient client = new TcpClient(SMTP_SERVER, SMTP_PORT))
//            using (SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true))) {
//                sslStream.AuthenticateAsClient(SMTP_SERVER);
//                using (StreamReader reader = new StreamReader(sslStream))
//                using (StreamWriter writer = new StreamWriter(sslStream) { AutoFlush = true }) {
//                    // 서버 응답 읽기
//                    ReadResponse(reader);

//                    // EHLO
//                    writer.WriteLine($"EHLO {SMTP_SERVER}");
//                    ReadResponse(reader);

//                    // AUTH PLAIN
//                    string authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{senderEmail}\0{password}"));
//                    writer.WriteLine($"AUTH PLAIN {authString}");
//                    ReadResponse(reader);

//                    // MAIL FROM
//                    writer.WriteLine($"MAIL FROM:<{senderEmail}>");
//                    ReadResponse(reader);

//                    // RCPT TO
//                    writer.WriteLine($"RCPT TO:<{recipient}>");
//                    ReadResponse(reader);

//                    // DATA
//                    writer.WriteLine("DATA");
//                    ReadResponse(reader);

//                    // 이메일 본문 전송
//                    StringBuilder emailContent = new StringBuilder();
//                    emailContent.AppendLine($"From: {senderEmail}");
//                    emailContent.AppendLine($"To: {recipient}");
//                    emailContent.AppendLine($"Subject: {subject}");
//                    emailContent.AppendLine();
//                    emailContent.AppendLine(body);
//                    emailContent.AppendLine(".");
//                    writer.WriteLine(emailContent.ToString());
//                    ReadResponse(reader);

//                    // QUIT
//                    writer.WriteLine("QUIT");
//                    ReadResponse(reader);
//                }
//            }
//        }
//        catch (Exception ex) {
//            MessageBox.Show("Error: " + ex.Message);
//        }
//    }

//    static void ReadResponse(StreamReader reader)
//    {
//        string response = reader.ReadLine().Substring(0,3);
//        if(response !="220" && response!="250" &&response!="354" &&response!="334" &&response!="235") {
//            MessageBox.Show($"메일 발송 중 오류가 발생했습니다. {response}");
//        }
//    }
//}

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.Windows.Forms;

class mailClient
{
    private string SMTP_SERVER;
    private int SMTP_PORT;

    public mailClient(string host)
    {
        SMTP_SERVER = host;
        SMTP_PORT = 465;
    }

    public void SendEmail(string senderEmail, string password, string recipient, string subject, string body)
    {
        Socket socket = null;
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 서버에 연결
            socket.Connect(SMTP_SERVER, SMTP_PORT);

            // SSL 스트림 생성 및 인증
            using (NetworkStream networkStream = new NetworkStream(socket, ownsSocket: false))
            using (SslStream sslStream = new SslStream(networkStream, false, new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)))
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
                    string authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{senderEmail}\0{password}"));
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
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
        finally
        {
            // 소켓 닫기
            if (socket != null && socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }

    static void ReadResponse(StreamReader reader)
    {
        string response = reader.ReadLine();
        string code = response.Substring(0, 3);

        // 예상치 못한 응답 코드가 있을 경우 오류 메시지 표시
        if (code != "220" && code != "250" && code != "354" && code != "334" && code != "235" && code != "221")
        {
            MessageBox.Show($"메일 발송 중 오류가 발생했습니다. {code}");
        }
    }
}
