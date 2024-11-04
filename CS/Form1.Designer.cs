namespace NetworkSocketProgramming
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SendingAddr = new System.Windows.Forms.TextBox();
            this.ReceivingAddr = new System.Windows.Forms.TextBox();
            this.Title = new System.Windows.Forms.TextBox();
            this.Data = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SendingPW = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "보내는 주소";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "받는 주소";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "제목";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "내용";
            // 
            // SendingAddr
            // 
            this.SendingAddr.Location = new System.Drawing.Point(92, 15);
            this.SendingAddr.Name = "SendingAddr";
            this.SendingAddr.Size = new System.Drawing.Size(280, 21);
            this.SendingAddr.TabIndex = 3;
            // 
            // ReceivingAddr
            // 
            this.ReceivingAddr.Location = new System.Drawing.Point(92, 54);
            this.ReceivingAddr.Name = "ReceivingAddr";
            this.ReceivingAddr.Size = new System.Drawing.Size(280, 21);
            this.ReceivingAddr.TabIndex = 5;
            // 
            // Title
            // 
            this.Title.Location = new System.Drawing.Point(92, 96);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(491, 21);
            this.Title.TabIndex = 6;
            // 
            // Data
            // 
            this.Data.Location = new System.Drawing.Point(11, 157);
            this.Data.Multiline = true;
            this.Data.Name = "Data";
            this.Data.Size = new System.Drawing.Size(646, 234);
            this.Data.TabIndex = 7;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(582, 407);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 8;
            this.SendButton.Text = "전송";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(490, 407);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(86, 22);
            this.ResetButton.TabIndex = 9;
            this.ResetButton.Text = "양식 비우기";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "PW";
            // 
            // SendingPW
            // 
            this.SendingPW.Location = new System.Drawing.Point(410, 15);
            this.SendingPW.Name = "SendingPW";
            this.SendingPW.Size = new System.Drawing.Size(247, 21);
            this.SendingPW.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 442);
            this.Controls.Add(this.SendingPW);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.Data);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.ReceivingAddr);
            this.Controls.Add(this.SendingAddr);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SendingAddr;
        private System.Windows.Forms.TextBox ReceivingAddr;
        private System.Windows.Forms.TextBox Title;
        private System.Windows.Forms.TextBox Data;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SendingPW;
    }
}

