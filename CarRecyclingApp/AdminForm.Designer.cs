namespace CarRecyclingApp
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel panelBackground;
        private Label lblTitle;
        private DataGridView dataGridViewPoints;
        private DataGridView dataGridViewRequests;
        private Button btnLoadRequests;
        private Button btnChangeStatus;
        private Label labelPoints;
        private Label labelRequests;
        private Button btnExportToExcel;
        private Button btnOpenAddPoint;
        private Panel panelHeader;
        private PictureBox imgLogo;
        private ComboBox comboBoxWorkers;
        private Button btnAssignWorker;
        private Label lblAssignWorker;
        private System.Windows.Forms.Button btnAddEmployee;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnViewClients; // Объявление кнопки

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.labelPoints = new System.Windows.Forms.Label();
            this.dataGridViewPoints = new System.Windows.Forms.DataGridView();
            this.labelRequests = new System.Windows.Forms.Label();
            this.dataGridViewRequests = new System.Windows.Forms.DataGridView();
            this.btnLoadRequests = new System.Windows.Forms.Button();
            this.btnChangeStatus = new System.Windows.Forms.Button();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.btnOpenAddPoint = new System.Windows.Forms.Button();
            this.comboBoxWorkers = new System.Windows.Forms.ComboBox();
            this.btnAssignWorker = new System.Windows.Forms.Button();
            this.lblAssignWorker = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnAddEmployee = new System.Windows.Forms.Button();
            this.btnViewClients = new System.Windows.Forms.Button(); // Инициализация кнопки

            this.panelBackground.SuspendLayout();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRequests)).BeginInit();
            this.SuspendLayout();

            //
            // AdminForm
            //
            this.DoubleBuffered = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.panelBackground); // PanelBackground будет содержать все элементы
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Панель администратора";

            //
            // panelHeader
            //
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0, 128, 0); // Темно-зеленый
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.imgLogo);
            this.panelHeader.Controls.Add(this.btnLogout);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1000, 80);
            this.panelHeader.TabIndex = 0;

            //
            // btnLogout
            //
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(220, 53, 69); // Красный
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(880, 25);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 35);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "Выход";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            //
            // imgLogo
            //
            this.imgLogo.Image = Properties.Resources.Logo2; // Убедитесь, что у вас есть этот ресурс
            this.imgLogo.Location = new System.Drawing.Point(20, 10);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(60, 60);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgLogo.TabIndex = 0;
            this.imgLogo.TabStop = false;

            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(90, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 30);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Панель администратора";

            //
            // panelBackground
            //
            this.panelBackground.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.panelBackground.Controls.Add(this.panelHeader); // Header внутри Background
            this.panelBackground.Controls.Add(this.labelPoints);
            this.panelBackground.Controls.Add(this.dataGridViewPoints);
            this.panelBackground.Controls.Add(this.labelRequests);
            this.panelBackground.Controls.Add(this.dataGridViewRequests);
            this.panelBackground.Controls.Add(this.lblAssignWorker);
            this.panelBackground.Controls.Add(this.comboBoxWorkers);
            this.panelBackground.Controls.Add(this.btnAssignWorker);
            this.panelBackground.Controls.Add(this.btnLoadRequests);
            this.panelBackground.Controls.Add(this.btnChangeStatus);
            this.panelBackground.Controls.Add(this.btnExportToExcel);
            this.panelBackground.Controls.Add(this.btnAddEmployee); // Перемещено
            this.panelBackground.Controls.Add(this.btnOpenAddPoint); // Перемещено
            this.panelBackground.Controls.Add(this.btnViewClients); // ДОБАВЛЕНО И ПЕРЕМЕЩЕНО
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(1000, 700);
            this.panelBackground.TabIndex = 0;


            //
            // labelPoints
            //
            this.labelPoints.AutoSize = true;
            this.labelPoints.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelPoints.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.labelPoints.Location = new System.Drawing.Point(20, 100); // Отступ от шапки
            this.labelPoints.Name = "labelPoints";
            this.labelPoints.Size = new System.Drawing.Size(195, 25);
            this.labelPoints.TabIndex = 1;
            this.labelPoints.Text = "Пункты утилизации:";

            //
            // dataGridViewPoints
            //
            this.dataGridViewPoints.AllowUserToAddRows = false;
            this.dataGridViewPoints.AllowUserToDeleteRows = false;
            this.dataGridViewPoints.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewPoints.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPoints.Location = new System.Drawing.Point(20, 130); // Под заголовком
            this.dataGridViewPoints.Name = "dataGridViewPoints";
            this.dataGridViewPoints.ReadOnly = true;
            this.dataGridViewPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPoints.Size = new System.Drawing.Size(960, 160); // Чуть уменьшил высоту, чтобы поместилось больше
            this.dataGridViewPoints.TabIndex = 0;

            //
            // labelRequests
            //
            this.labelRequests.AutoSize = true;
            this.labelRequests.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelRequests.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.labelRequests.Location = new System.Drawing.Point(20, 300); // Отступ после первой таблицы
            this.labelRequests.Name = "labelRequests";
            this.labelRequests.Size = new System.Drawing.Size(79, 25);
            this.labelRequests.TabIndex = 2;
            this.labelRequests.Text = "Заявки:";

            //
            // dataGridViewRequests
            //
            this.dataGridViewRequests.AllowUserToAddRows = false;
            this.dataGridViewRequests.AllowUserToDeleteRows = false;
            this.dataGridViewRequests.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewRequests.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRequests.Location = new System.Drawing.Point(20, 330); // Под заголовком заявок
            this.dataGridViewRequests.Name = "dataGridViewRequests";
            this.dataGridViewRequests.ReadOnly = true;
            this.dataGridViewRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRequests.Size = new System.Drawing.Size(960, 180); // Увеличил ширину и высоту
            this.dataGridViewRequests.TabIndex = 1;
            this.dataGridViewRequests.AutoGenerateColumns = true; // Убедимся, что столбцы генерируются

            //
            // lblAssignWorker
            //
            this.lblAssignWorker.AutoSize = true;
            this.lblAssignWorker.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblAssignWorker.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblAssignWorker.Location = new System.Drawing.Point(20, 520); // Отступ от таблицы заявок
            this.lblAssignWorker.Name = "lblAssignWorker";
            this.lblAssignWorker.Size = new System.Drawing.Size(185, 20);
            this.lblAssignWorker.TabIndex = 8;
            this.lblAssignWorker.Text = "Назначить заявку работнику:";

            //
            // comboBoxWorkers
            //
            this.comboBoxWorkers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorkers.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.comboBoxWorkers.FormattingEnabled = true;
            this.comboBoxWorkers.Location = new System.Drawing.Point(20, 545); // Под лейблом
            this.comboBoxWorkers.Name = "comboBoxWorkers";
            this.comboBoxWorkers.Size = new System.Drawing.Size(250, 28);
            this.comboBoxWorkers.TabIndex = 6;

            //
            // btnAssignWorker
            //
            this.btnAssignWorker.BackColor = System.Drawing.Color.FromArgb(70, 130, 180); // Стальной синий
            this.btnAssignWorker.FlatAppearance.BorderSize = 0;
            this.btnAssignWorker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAssignWorker.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAssignWorker.ForeColor = System.Drawing.Color.White;
            this.btnAssignWorker.Location = new System.Drawing.Point(280, 545); // Рядом с ComboBox
            this.btnAssignWorker.Name = "btnAssignWorker";
            this.btnAssignWorker.Size = new System.Drawing.Size(180, 28); // Размер под ComboBox
            this.btnAssignWorker.TabIndex = 7;
            this.btnAssignWorker.Text = "Назначить Работника";
            this.btnAssignWorker.UseVisualStyleBackColor = false;
            this.btnAssignWorker.Click += new System.EventHandler(this.BtnAssignWorker_Click);

            //
            // btnLoadRequests
            //
            this.btnLoadRequests.BackColor = System.Drawing.Color.FromArgb(0, 128, 0); // Темно-зеленый
            this.btnLoadRequests.FlatAppearance.BorderSize = 0;
            this.btnLoadRequests.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadRequests.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLoadRequests.ForeColor = System.Drawing.Color.White;
            this.btnLoadRequests.Location = new System.Drawing.Point(20, 600); // Новая позиция, снизу
            this.btnLoadRequests.Name = "btnLoadRequests";
            this.btnLoadRequests.Size = new System.Drawing.Size(200, 35);
            this.btnLoadRequests.TabIndex = 2;
            this.btnLoadRequests.Text = "Загрузить Заявки";
            this.btnLoadRequests.UseVisualStyleBackColor = false;
            this.btnLoadRequests.Click += new System.EventHandler(this.BtnLoadRequests_Click);

            //
            // btnChangeStatus
            //
            this.btnChangeStatus.BackColor = System.Drawing.Color.FromArgb(0, 128, 0); // Темно-зеленый
            this.btnChangeStatus.FlatAppearance.BorderSize = 0;
            this.btnChangeStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnChangeStatus.ForeColor = System.Drawing.Color.White;
            this.btnChangeStatus.Location = new System.Drawing.Point(230, 600); // Рядом с "Загрузить Заявки"
            this.btnChangeStatus.Name = "btnChangeStatus";
            this.btnChangeStatus.Size = new System.Drawing.Size(200, 35);
            this.btnChangeStatus.TabIndex = 3;
            this.btnChangeStatus.Text = "Завершить Заявку";
            this.btnChangeStatus.UseVisualStyleBackColor = false;
            this.btnChangeStatus.Click += new System.EventHandler(this.BtnChangeStatus_Click);

            //
            // btnExportToExcel
            //
            this.btnExportToExcel.BackColor = System.Drawing.Color.FromArgb(70, 130, 180); // Стальной синий
            this.btnExportToExcel.FlatAppearance.BorderSize = 0;
            this.btnExportToExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportToExcel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnExportToExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportToExcel.Location = new System.Drawing.Point(20, 645); // Под "Загрузить Заявки"
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(200, 35);
            this.btnExportToExcel.TabIndex = 4;
            this.btnExportToExcel.Text = "Экспорт в Excel";
            this.btnExportToExcel.UseVisualStyleBackColor = false;
            this.btnExportToExcel.Click += new System.EventHandler(this.BtnExportToExcel_Click);

            //
            // btnAddEmployee
            //
            this.btnAddEmployee.BackColor = System.Drawing.Color.FromArgb(70, 130, 180); // Стальной синий
            this.btnAddEmployee.FlatAppearance.BorderSize = 0;
            this.btnAddEmployee.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddEmployee.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddEmployee.ForeColor = System.Drawing.Color.White;
            this.btnAddEmployee.Location = new System.Drawing.Point(780, 520); // Справа, выше других админ-кнопок
            this.btnAddEmployee.Name = "btnAddEmployee";
            this.btnAddEmployee.Size = new System.Drawing.Size(200, 35);
            this.btnAddEmployee.TabIndex = 10;
            this.btnAddEmployee.Text = "Добавить Работника";
            this.btnAddEmployee.UseVisualStyleBackColor = false;
            this.btnAddEmployee.Click += new System.EventHandler(this.btnAddEmployee_Click);

            //
            // btnViewClients
            //
            this.btnViewClients.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0))))); // Темно-зеленый
            this.btnViewClients.FlatAppearance.BorderSize = 0;
            this.btnViewClients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewClients.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnViewClients.ForeColor = System.Drawing.Color.White;
            this.btnViewClients.Location = new System.Drawing.Point(780, 565); // Под btnAddEmployee
            this.btnViewClients.Name = "btnViewClients";
            this.btnViewClients.Size = new System.Drawing.Size(200, 35);
            this.btnViewClients.TabIndex = 11;
            this.btnViewClients.Text = "Просмотр Клиентов";
            this.btnViewClients.UseVisualStyleBackColor = false;
            this.btnViewClients.Click += new System.EventHandler(this.btnViewClients_Click);

            //
            // btnOpenAddPoint
            //
            this.btnOpenAddPoint.BackColor = System.Drawing.Color.FromArgb(70, 130, 180); // Стальной синий
            this.btnOpenAddPoint.FlatAppearance.BorderSize = 0;
            this.btnOpenAddPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenAddPoint.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnOpenAddPoint.ForeColor = System.Drawing.Color.White;
            this.btnOpenAddPoint.Location = new System.Drawing.Point(780, 610); // Под btnViewClients
            this.btnOpenAddPoint.Name = "btnOpenAddPoint";
            this.btnOpenAddPoint.Size = new System.Drawing.Size(200, 35);
            this.btnOpenAddPoint.TabIndex = 5;
            this.btnOpenAddPoint.Text = "Управление Пунктами";
            this.btnOpenAddPoint.UseVisualStyleBackColor = false;
            this.btnOpenAddPoint.Click += new System.EventHandler(this.btnOpenAddPoint_Click);


            //
            // End layout
            //
            this.panelBackground.ResumeLayout(false);
            this.panelBackground.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRequests)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}