namespace CarRecyclingApp
{
    partial class ReviewRequestForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelBackground;
        private Panel panelHeader;
        private Label lblTitle;
        private PictureBox imgLogo;
        private Label lblRequestInfo;
        private TextBox txtComment;
        private Label lblComment;
        private Button btnApprove;
        private Button btnReject;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReviewRequestForm));
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblRequestInfo = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();

            // panelBackground
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.panelBackground.Controls.Add(this.btnReject);
            this.panelBackground.Controls.Add(this.btnApprove);
            this.panelBackground.Controls.Add(this.lblComment);
            this.panelBackground.Controls.Add(this.txtComment);
            this.panelBackground.Controls.Add(this.lblRequestInfo);
            this.panelBackground.Controls.Add(this.panelHeader);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(500, 350);
            this.panelBackground.TabIndex = 0;

            // panelHeader
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0, 128, 0);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.imgLogo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(500, 60);
            this.panelHeader.TabIndex = 0;

            // imgLogo
            this.imgLogo.Image = (System.Drawing.Image)resources.GetObject("imgLogo.Image");
            this.imgLogo.Location = new System.Drawing.Point(15, 10);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(40, 40);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgLogo.TabIndex = 0;
            this.imgLogo.TabStop = false;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(65, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(210, 25);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Подтверждение заявки";

            // lblRequestInfo
            this.lblRequestInfo.AutoSize = true;
            this.lblRequestInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRequestInfo.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblRequestInfo.Location = new System.Drawing.Point(20, 80);
            this.lblRequestInfo.Name = "lblRequestInfo";
            this.lblRequestInfo.Size = new System.Drawing.Size(0, 19);
            this.lblRequestInfo.TabIndex = 1;

            // txtComment
            this.txtComment.BackColor = System.Drawing.Color.White;
            this.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtComment.Location = new System.Drawing.Point(20, 140);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(460, 100);
            this.txtComment.TabIndex = 2;

            // lblComment
            this.lblComment.AutoSize = true;
            this.lblComment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblComment.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblComment.Location = new System.Drawing.Point(20, 120);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(87, 19);
            this.lblComment.TabIndex = 3;
            this.lblComment.Text = "Комментарий:";

            // btnApprove
            this.btnApprove.BackColor = System.Drawing.Color.FromArgb(50, 205, 50);
            this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprove.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnApprove.ForeColor = System.Drawing.Color.White;
            this.btnApprove.Location = new System.Drawing.Point(20, 260);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(220, 35);
            this.btnApprove.TabIndex = 4;
            this.btnApprove.Text = "✅ Подтвердить завершение";
            this.btnApprove.UseVisualStyleBackColor = false;
            this.btnApprove.Click += new System.EventHandler(this.BtnApprove_Click);

            // btnReject
            this.btnReject.BackColor = System.Drawing.Color.FromArgb(220, 20, 60);
            this.btnReject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReject.ForeColor = System.Drawing.Color.White;
            this.btnReject.Location = new System.Drawing.Point(260, 260);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(220, 35);
            this.btnReject.TabIndex = 5;
            this.btnReject.Text = "❌ Отправить на доработку";
            this.btnReject.UseVisualStyleBackColor = false;
            this.btnReject.Click += new System.EventHandler(this.BtnReject_Click);

            // ReviewRequestForm
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReviewRequestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Проверка заявки";

            this.panelBackground.ResumeLayout(false);
            this.panelBackground.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.ResumeLayout(false);
        }
    }
}