namespace CarRecyclingApp
{
    partial class AddRecyclingPointForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.PictureBox imgLogo;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddRecyclingPointForm));
            panelBackground = new Panel();
            btnDelete = new Button();
            btnSave = new Button();
            txtPhone = new TextBox();
            lblPhone = new Label();
            txtAddress = new TextBox();
            lblAddress = new Label();
            txtName = new TextBox();
            lblName = new Label();
            panelHeader = new Panel();
            lblTitle = new Label();
            imgLogo = new PictureBox();
            panelBackground.SuspendLayout();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgLogo).BeginInit();
            SuspendLayout();
            // 
            // panelBackground
            // 
            panelBackground.BackColor = Color.FromArgb(240, 240, 240);
            panelBackground.Controls.Add(btnSave);
            panelBackground.Controls.Add(btnDelete);
            panelBackground.Controls.Add(txtPhone);
            panelBackground.Controls.Add(lblPhone);
            panelBackground.Controls.Add(txtAddress);
            panelBackground.Controls.Add(lblAddress);
            panelBackground.Controls.Add(txtName);
            panelBackground.Controls.Add(lblName);
            panelBackground.Controls.Add(panelHeader);
            panelBackground.Dock = DockStyle.Fill;
            panelBackground.Location = new Point(0, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(400, 300);
            panelBackground.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(0, 128, 0);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(50, 240);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 35);
            btnSave.TabIndex = 4;
            btnSave.Text = "Добавить пункт";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(0, 128, 0);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(200, 240);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(150, 35);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Удалить пункт";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // txtPhone
            // 
            txtPhone.BackColor = Color.White;
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.Font = new Font("Segoe UI", 10F);
            txtPhone.Location = new Point(50, 200);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(300, 25);
            txtPhone.TabIndex = 3;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Font = new Font("Segoe UI", 10F);
            lblPhone.ForeColor = Color.FromArgb(80, 80, 80);
            lblPhone.Location = new Point(50, 180);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(66, 19);
            lblPhone.TabIndex = 3;
            lblPhone.Text = "Телефон:";
            // 
            // txtAddress
            // 
            txtAddress.BackColor = Color.White;
            txtAddress.BorderStyle = BorderStyle.FixedSingle;
            txtAddress.Font = new Font("Segoe UI", 10F);
            txtAddress.Location = new Point(50, 150);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(300, 25);
            txtAddress.TabIndex = 2;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Font = new Font("Segoe UI", 10F);
            lblAddress.ForeColor = Color.FromArgb(80, 80, 80);
            lblAddress.Location = new Point(50, 130);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(50, 19);
            lblAddress.TabIndex = 2;
            lblAddress.Text = "Адрес:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.White;
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.Font = new Font("Segoe UI", 10F);
            txtName.Location = new Point(50, 100);
            txtName.Name = "txtName";
            txtName.Size = new Size(300, 25);
            txtName.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 10F);
            lblName.ForeColor = Color.FromArgb(80, 80, 80);
            lblName.Location = new Point(50, 80);
            lblName.Name = "lblName";
            lblName.Size = new Size(72, 19);
            lblName.TabIndex = 1;
            lblName.Text = "Название:";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(0, 128, 0);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(imgLogo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(400, 60);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(65, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(239, 25);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Добавить пункт приёма";
            // 
            // imgLogo
            // 
            imgLogo.Image = Properties.Resources.Logo2; 
            imgLogo.Location = new Point(15, 12);
            imgLogo.Name = "imgLogo";
            imgLogo.Size = new Size(44, 38);
            imgLogo.SizeMode = PictureBoxSizeMode.Zoom;
            imgLogo.TabIndex = 0;
            imgLogo.TabStop = false;
            // 
            // AddRecyclingPointForm
            // 
            ClientSize = new Size(400, 300);
            Controls.Add(panelBackground);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddRecyclingPointForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Добавление пункта утилизации";
            panelBackground.ResumeLayout(false);
            panelBackground.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgLogo).EndInit();
            ResumeLayout(false);
        }
    }
}
