using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace EmailSender
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            string senderEmail = senderEmailTextBox.Text;
            string appPassword = appPasswordTextBox.Text;
            string recipientEmail = recipientEmailTextBox.Text;
            string emailBody = emailBodyTextBox.Text;

            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(appPassword) || 
                string.IsNullOrWhiteSpace(recipientEmail) || string.IsNullOrWhiteSpace(emailBody))
            {
                MessageBox.Show("모든 필드를 채워주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                SendEmail(senderEmail, appPassword, recipientEmail, emailBody);
                MessageBox.Show("이메일이 성공적으로 전송되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"이메일 전송 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendEmail(string senderEmail, string appPassword, string recipientEmail, string emailBody)
        {
            const string smtpServer = "smtp.gmail.com";
            const int smtpPort = 587;

            using (TcpClient client = new TcpClient(smtpServer, smtpPort))
            using (NetworkStream stream = client.GetStream())
            {
                ReadResponse(stream);
                SendCommand(stream, "EHLO example.com\r\n");
                ReadResponse(stream);
                SendCommand(stream, "AUTH LOGIN\r\n");
                ReadResponse(stream);
                SendCommand(stream, Convert.ToBase64String(Encoding.UTF8.GetBytes(senderEmail)) + "\r\n");
                ReadResponse(stream);
                SendCommand(stream, Convert.ToBase64String(Encoding.UTF8.GetBytes(appPassword)) + "\r\n");
                ReadResponse(stream);
                SendCommand(stream, $"MAIL FROM:<{senderEmail}>\r\n");
                ReadResponse(stream);
                SendCommand(stream, $"RCPT TO:<{recipientEmail}>\r\n");
                ReadResponse(stream);
                SendCommand(stream, "DATA\r\n");
                ReadResponse(stream);
                string emailContent = $"Subject: Test Email\r\n\r\n{emailBody}\r\n.\r\n";
                SendCommand(stream, emailContent);
                ReadResponse(stream);
                SendCommand(stream, "QUIT\r\n");
                ReadResponse(stream);
            }
        }

        private void SendCommand(NetworkStream stream, string command)
        {
            byte[] commandBytes = Encoding.UTF8.GetBytes(command);
            stream.Write(commandBytes, 0, commandBytes.Length);
        }

        private void ReadResponse(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Server response: {response}");
        }
    }
}
