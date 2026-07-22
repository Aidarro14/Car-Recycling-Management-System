namespace CarRecyclingApp
{
    partial class WorkerForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox imgLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLogout;

        private System.Windows.Forms.Label labelRequests;
        private System.Windows.Forms.DataGridView dataGridViewRequests;

        private System.Windows.Forms.Label lblSelectedRequestActions;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.Button btnChangeStatus;

        private System.Windows.Forms.Label lblWorkerComment;
        private System.Windows.Forms.TextBox txtWorkerComment;
        private System.Windows.Forms.Button btnSaveWorkerComment;
        private System.Windows.Forms.Button btnShowComment;
        private System.Windows.Forms.Button btnViewClients;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkerForm));
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();

            this.labelRequests = new System.Windows.Forms.Label();
            this.dataGridViewRequests = new System.Windows.Forms.DataGridView();

            this.lblSelectedRequestActions = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.btnChangeStatus = new System.Windows.Forms.Button();

            this.lblWorkerComment = new System.Windows.Forms.Label();
            this.txtWorkerComment = new System.Windows.Forms.TextBox();
            this.btnSaveWorkerComment = new System.Windows.Forms.Button();
            this.btnShowComment = new System.Windows.Forms.Button();
            this.btnViewClients = new System.Windows.Forms.Button();

            this.panelBackground.SuspendLayout();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRequests)).BeginInit();
            this.SuspendLayout();

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this.panelBackground);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "WorkerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Панель работника - Утилизация авто";

            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0, 128, 0);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.imgLogo);
            this.panelHeader.Controls.Add(this.btnLogout);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(900, 70);
            this.panelHeader.TabIndex = 0;

            this.imgLogo.Image = Properties.Resources.Logo2;
            this.imgLogo.Location = new System.Drawing.Point(15, 10);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(50, 50);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgLogo.TabIndex = 0;
            this.imgLogo.TabStop = false;

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(75, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(190, 25);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Панель работника";

            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(800, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 30);
            this.btnLogout.TabIndex = 100;
            this.btnLogout.Text = "Выход";
            this.btnLogout.UseVisualStyleBackColor = false;

            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.panelBackground.Controls.Add(this.panelHeader);
            this.panelBackground.Controls.Add(this.labelRequests);
            this.panelBackground.Controls.Add(this.dataGridViewRequests);
            this.panelBackground.Controls.Add(this.lblSelectedRequestActions);
            this.panelBackground.Controls.Add(this.lblStatus);
            this.panelBackground.Controls.Add(this.comboBoxStatus);
            this.panelBackground.Controls.Add(this.btnChangeStatus);
            this.panelBackground.Controls.Add(this.lblWorkerComment);
            this.panelBackground.Controls.Add(this.txtWorkerComment);
            this.panelBackground.Controls.Add(this.btnSaveWorkerComment);
            this.panelBackground.Controls.Add(this.btnShowComment);
            this.panelBackground.Controls.Add(this.btnViewClients);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(900, 650);
            this.panelBackground.TabIndex = 0;

            this.labelRequests.AutoSize = true;
            this.labelRequests.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelRequests.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.labelRequests.Location = new System.Drawing.Point(20, 85);
            this.labelRequests.Name = "labelRequests";
            this.labelRequests.Size = new System.Drawing.Size(209, 21);
            this.labelRequests.TabIndex = 1;
            this.labelRequests.Text = "Мои/Доступные заявки:";

            this.dataGridViewRequests.AllowUserToAddRows = false;
            this.dataGridViewRequests.AllowUserToDeleteRows = false;
            this.dataGridViewRequests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRequests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRequests.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewRequests.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRequests.Location = new System.Drawing.Point(20, 110);
            this.dataGridViewRequests.MultiSelect = false;
            this.dataGridViewRequests.Name = "dataGridViewRequests";
            this.dataGridViewRequests.ReadOnly = true;
            this.dataGridViewRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRequests.Size = new System.Drawing.Size(860, 250);
            this.dataGridViewRequests.TabIndex = 2;

            this.lblSelectedRequestActions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedRequestActions.AutoSize = true;
            this.lblSelectedRequestActions.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSelectedRequestActions.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblSelectedRequestActions.Location = new System.Drawing.Point(20, 380);
            this.lblSelectedRequestActions.Name = "lblSelectedRequestActions";
            this.lblSelectedRequestActions.Size = new System.Drawing.Size(220, 20);
            this.lblSelectedRequestActions.TabIndex = 3;
            this.lblSelectedRequestActions.Text = "Действия с выбранной заявкой:";

            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.Location = new System.Drawing.Point(20, 410);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 19);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Новый статус:";

            this.comboBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.Location = new System.Drawing.Point(20, 432);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.Size = new System.Drawing.Size(200, 25);
            this.comboBoxStatus.TabIndex = 5;

            this.btnChangeStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChangeStatus.BackColor = System.Drawing.Color.FromArgb(0, 128, 0);
            this.btnChangeStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnChangeStatus.ForeColor = System.Drawing.Color.White;
            this.btnChangeStatus.Location = new System.Drawing.Point(230, 430);
            this.btnChangeStatus.Name = "btnChangeStatus";
            this.btnChangeStatus.Size = new System.Drawing.Size(150, 30);
            this.btnChangeStatus.TabIndex = 6;
            this.btnChangeStatus.Text = "Изменить статус";
            this.btnChangeStatus.UseVisualStyleBackColor = false;

            this.lblWorkerComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblWorkerComment.AutoSize = true;
            this.lblWorkerComment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWorkerComment.Location = new System.Drawing.Point(20, 475);
            this.lblWorkerComment.Name = "lblWorkerComment";
            this.lblWorkerComment.Size = new System.Drawing.Size(143, 19);
            this.lblWorkerComment.TabIndex = 7;
            this.lblWorkerComment.Text = "Ваш комментарий:";

            this.txtWorkerComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkerComment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtWorkerComment.Location = new System.Drawing.Point(20, 497);
            this.txtWorkerComment.Multiline = true;
            this.txtWorkerComment.Name = "txtWorkerComment";
            this.txtWorkerComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWorkerComment.Size = new System.Drawing.Size(550, 80);
            this.txtWorkerComment.TabIndex = 8;

            this.btnSaveWorkerComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveWorkerComment.BackColor = System.Drawing.Color.FromArgb(70, 130, 180);
            this.btnSaveWorkerComment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveWorkerComment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveWorkerComment.ForeColor = System.Drawing.Color.White;
            this.btnSaveWorkerComment.Location = new System.Drawing.Point(580, 497);
            this.btnSaveWorkerComment.Name = "btnSaveWorkerComment";
            this.btnSaveWorkerComment.Size = new System.Drawing.Size(150, 35);
            this.btnSaveWorkerComment.TabIndex = 9;
            this.btnSaveWorkerComment.Text = "Сохр. комментарий";
            this.btnSaveWorkerComment.UseVisualStyleBackColor = false;

            this.btnShowComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowComment.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.btnShowComment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowComment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnShowComment.ForeColor = System.Drawing.Color.Black;
            this.btnShowComment.Location = new System.Drawing.Point(740, 497);
            this.btnShowComment.Name = "btnShowComment";
            this.btnShowComment.Size = new System.Drawing.Size(140, 35);
            this.btnShowComment.TabIndex = 10;
            this.btnShowComment.Text = "Комментарий админа";
            this.btnShowComment.UseVisualStyleBackColor = false;

            this.btnViewClients.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewClients.BackColor = System.Drawing.Color.FromArgb(0, 128, 0);
            this.btnViewClients.FlatAppearance.BorderSize = 0;
            this.btnViewClients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewClients.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnViewClients.ForeColor = System.Drawing.Color.White;
            this.btnViewClients.Location = new System.Drawing.Point(680, 600);
            this.btnViewClients.Name = "btnViewClients";
            this.btnViewClients.Size = new System.Drawing.Size(200, 35);
            this.btnViewClients.TabIndex = 11;
            this.btnViewClients.Text = "Просмотр Клиентов";
            this.btnViewClients.UseVisualStyleBackColor = false;
            this.btnViewClients.Click += new System.EventHandler(this.btnViewClients_Click);

            this.panelBackground.ResumeLayout(false);
            this.panelBackground.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRequests)).EndInit();
            this.ResumeLayout(false);
        }
    }
}