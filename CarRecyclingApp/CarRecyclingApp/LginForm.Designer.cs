using ExCSS;
using System.Resources;

namespace CarRecyclingApp
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.PictureBox imgLogo;
        private System.Windows.Forms.Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelBackground = new Panel();
            lblTitle = new Label();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            imgLogo = new PictureBox();
            panelBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgLogo).BeginInit();
            SuspendLayout();
            // 
            // panelBackground
            // 
            panelBackground.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            panelBackground.Controls.Add(lblTitle);
            panelBackground.Controls.Add(lblEmail);
            panelBackground.Controls.Add(txtEmail);
            panelBackground.Controls.Add(lblPassword);
            panelBackground.Controls.Add(txtPassword);
            panelBackground.Controls.Add(btnLogin);
            panelBackground.Controls.Add(imgLogo);
            panelBackground.Dock = DockStyle.Fill;
            panelBackground.Location = new System.Drawing.Point(0, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(400, 350);
            panelBackground.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            lblTitle.Location = new System.Drawing.Point(10, 103);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(387, 30);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Система утилизации автомобилей";
            // 
            // lblEmail
            // 
            lblEmail.Font = new Font("Segoe UI", 10F);
            lblEmail.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblEmail.Location = new System.Drawing.Point(80, 140);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 23);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = System.Drawing.Color.White;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Location = new System.Drawing.Point(80, 160);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 23);
            txtEmail.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.Font = new Font("Segoe UI", 10F);
            lblPassword.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblPassword.Location = new System.Drawing.Point(80, 200);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(100, 23);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Пароль:";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = System.Drawing.Color.White;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Location = new System.Drawing.Point(80, 220);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(250, 23);
            txtPassword.TabIndex = 5;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = System.Drawing.Color.FromArgb(0, 128, 0);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.Location = new System.Drawing.Point(80, 260);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(110, 35);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Войти";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // imgLogo
            // 
            imgLogo.Image = Properties.Resources.Logo2;
            imgLogo.Location = new System.Drawing.Point(160, 20);
            imgLogo.Name = "imgLogo";
            imgLogo.Size = new Size(80, 80);
            imgLogo.SizeMode = PictureBoxSizeMode.Zoom;
            imgLogo.TabIndex = 0;
            imgLogo.TabStop = false;
            // 
            // LoginForm
            // 
            ClientSize = new Size(400, 350);
            Controls.Add(panelBackground);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Вход в систему";
            panelBackground.ResumeLayout(false);
            panelBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgLogo).EndInit();
            ResumeLayout(false);
        }
    }
}
