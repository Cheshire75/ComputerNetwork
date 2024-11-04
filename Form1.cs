using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NetworkSocketProgramming
{
    public partial class Form1 : Form
    {
        public string ID;
        public string PW;
        public Form1()
        {
            InitializeComponent();
        }

        private void ResetButton_Click(object sender, EventArgs e) {
            //양식 비우기 버튼으로 모든 텍스트박스 초기화
            SendingAddr.Text = string.Empty;
            ReceivingAddr.Text = string.Empty;
            SendingPW.Text = string.Empty;
            Title.Text = string.Empty;
            Data.Text = string.Empty;
        }

        private void SendButton_Click(object sender, EventArgs e) {
            //메일 전송에 필수적인 필드만 확인
            if(SendingAddr.Text==string.Empty) {
                MessageBox.Show("보내는 주소가 비어있습니다.");
                return;
            }
            else if(SendingPW.Text==string.Empty) {
                MessageBox.Show("보내는 주소의 비밀번호가 비어있습니다.");
                return;
            }
            else if (ReceivingAddr.Text == string.Empty) {
                MessageBox.Show("받는 주소가 비어있습니다.");
                return;
            }
            else if (Title.Text == string.Empty) {
                MessageBox.Show("메일의 제목이 비어있습니다.");
                return;
            }


            try {
                //입력받은 보내는 주소에서 @ 이후 부분을 호스트로 사용
                string[] hostTmp = SendingAddr.Text.Split('@');
                string host = "smtp." + hostTmp[1];
                mailClient client = new mailClient(host);


                client.SendEmail(SendingAddr.Text, SendingPW.Text, ReceivingAddr.Text, Title.Text, Data.Text);
                MessageBox.Show("메일이 성공적으로 전송되었습니다.");
            }
            catch(Exception error) {
                MessageBox.Show("메세지 발송 중 오류가 발생했습니다.\n메일 양식을 다시 확인바랍니다."+error);
            }
        }
    }
}
