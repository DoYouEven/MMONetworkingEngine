namespace ClientUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbLogin = new System.Windows.Forms.Button();
            this.rtbChatWindow = new System.Windows.Forms.RichTextBox();
            this.tbChatBox = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnPositionUpdateUP = new System.Windows.Forms.Button();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tbAveragePing = new System.Windows.Forms.TextBox();
            this.Label23 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPing = new System.Windows.Forms.TextBox();
            this.rtbReceivedPacket = new System.Windows.Forms.RichTextBox();
            this.btnPing = new System.Windows.Forms.Button();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLogTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(62, 66);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(128, 20);
            this.tbUsername.TabIndex = 0;
            this.tbUsername.Text = "perei155";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(62, 92);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(128, 20);
            this.tbPassword.TabIndex = 1;
            this.tbPassword.Text = "password";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(52, 140);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(75, 23);
            this.tbLogin.TabIndex = 2;
            this.tbLogin.Text = "Login";
            this.tbLogin.UseVisualStyleBackColor = true;
            this.tbLogin.Click += new System.EventHandler(this.tbLogin_Click);
            // 
            // rtbChatWindow
            // 
            this.rtbChatWindow.BackColor = System.Drawing.SystemColors.InfoText;
            this.rtbChatWindow.CausesValidation = false;
            this.rtbChatWindow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rtbChatWindow.Location = new System.Drawing.Point(291, 50);
            this.rtbChatWindow.Name = "rtbChatWindow";
            this.rtbChatWindow.Size = new System.Drawing.Size(281, 131);
            this.rtbChatWindow.TabIndex = 3;
            this.rtbChatWindow.Text = "";
            // 
            // tbChatBox
            // 
            this.tbChatBox.Location = new System.Drawing.Point(291, 187);
            this.tbChatBox.Name = "tbChatBox";
            this.tbChatBox.Size = new System.Drawing.Size(180, 20);
            this.tbChatBox.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(497, 184);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(42, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(110, 23);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnPositionUpdateUP
            // 
            this.btnPositionUpdateUP.Location = new System.Drawing.Point(52, 204);
            this.btnPositionUpdateUP.Name = "btnPositionUpdateUP";
            this.btnPositionUpdateUP.Size = new System.Drawing.Size(120, 23);
            this.btnPositionUpdateUP.TabIndex = 7;
            this.btnPositionUpdateUP.Text = "Move Forward";
            this.btnPositionUpdateUP.UseVisualStyleBackColor = true;
            this.btnPositionUpdateUP.Click += new System.EventHandler(this.btnPositionUpdateUP_Click);
            // 
            // rtbConsole
            // 
            this.rtbConsole.BackColor = System.Drawing.SystemColors.MenuText;
            this.rtbConsole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rtbConsole.Location = new System.Drawing.Point(88, 293);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.Size = new System.Drawing.Size(366, 247);
            this.rtbConsole.TabIndex = 8;
            this.rtbConsole.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(52, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Move Backward";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbAveragePing
            // 
            this.tbAveragePing.Location = new System.Drawing.Point(852, 60);
            this.tbAveragePing.Name = "tbAveragePing";
            this.tbAveragePing.Size = new System.Drawing.Size(100, 20);
            this.tbAveragePing.TabIndex = 10;
            this.tbAveragePing.TextChanged += new System.EventHandler(this.tbAveragePing_TextChanged);
            // 
            // Label23
            // 
            this.Label23.AutoSize = true;
            this.Label23.Location = new System.Drawing.Point(620, 63);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(226, 13);
            this.Label23.TabIndex = 11;
            this.Label23.Text = "Average RoundTripTime[Medium Size Packet]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(620, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Ping:";
            // 
            // tbPing
            // 
            this.tbPing.Location = new System.Drawing.Point(852, 25);
            this.tbPing.Name = "tbPing";
            this.tbPing.Size = new System.Drawing.Size(100, 20);
            this.tbPing.TabIndex = 13;
            // 
            // rtbReceivedPacket
            // 
            this.rtbReceivedPacket.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.rtbReceivedPacket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.rtbReceivedPacket.Location = new System.Drawing.Point(607, 293);
            this.rtbReceivedPacket.Name = "rtbReceivedPacket";
            this.rtbReceivedPacket.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbReceivedPacket.ShowSelectionMargin = true;
            this.rtbReceivedPacket.Size = new System.Drawing.Size(366, 247);
            this.rtbReceivedPacket.TabIndex = 14;
            this.rtbReceivedPacket.Text = "";
            // 
            // btnPing
            // 
            this.btnPing.Location = new System.Drawing.Point(973, 25);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(75, 23);
            this.btnPing.TabIndex = 15;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(62, 41);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(128, 20);
            this.tbEmail.TabIndex = 16;
            this.tbEmail.Text = "test2@test.ca";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Email";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Password";
            // 
            // btnLogTest
            // 
            this.btnLogTest.Location = new System.Drawing.Point(88, 568);
            this.btnLogTest.Name = "btnLogTest";
            this.btnLogTest.Size = new System.Drawing.Size(75, 23);
            this.btnLogTest.TabIndex = 20;
            this.btnLogTest.Text = "LogTest1";
            this.btnLogTest.UseVisualStyleBackColor = true;
            this.btnLogTest.Click += new System.EventHandler(this.btnLogTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 622);
            this.Controls.Add(this.btnLogTest);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.rtbReceivedPacket);
            this.Controls.Add(this.tbPing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Label23);
            this.Controls.Add(this.tbAveragePing);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rtbConsole);
            this.Controls.Add(this.btnPositionUpdateUP);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbChatBox);
            this.Controls.Add(this.rtbChatWindow);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUsername);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button tbLogin;
        private System.Windows.Forms.RichTextBox rtbChatWindow;
        private System.Windows.Forms.TextBox tbChatBox;
        private System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnPositionUpdateUP;
        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbAveragePing;
        private System.Windows.Forms.Label Label23;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPing;
        private System.Windows.Forms.RichTextBox rtbReceivedPacket;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLogTest;
    }
}

