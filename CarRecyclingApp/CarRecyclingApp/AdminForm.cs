using System;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CarRecyclingApp
{
    public partial class AdminForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private int employeeId;
        private string role;
        private bool isLogout = false;
        private System.Windows.Forms.Timer refreshTimer;

        public AdminForm(int employeeId, string role)
        {
            InitializeComponent();
            this.employeeId = employeeId;
            this.role = role;

            this.Load += AdminForm_Load;
            this.FormClosed += AdminForm_FormClosed;

            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            this.btnViewClients.Click += new System.EventHandler(this.btnViewClients_Click);
            this.btnAssignWorker.Click += new System.EventHandler(this.BtnAssignWorker_Click);
            this.btnOpenAddPoint.Click += new System.EventHandler(this.btnOpenAddPoint_Click);
            this.btnExportToExcel.Click += new System.EventHandler(this.BtnExportToExcel_Click);
            this.btnLoadRequests.Click += new System.EventHandler(this.BtnLoadRequests_Click);
            this.btnAddEmployee.Click += new System.EventHandler(this.btnAddEmployee_Click);
            this.btnChangeStatus.Click += new System.EventHandler(this.BtnChangeStatus_Click);

            this.dataGridViewRequests.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewRequests_CellFormatting);
            this.dataGridViewPoints.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewPoints_CellFormatting);

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 30000; // обновление каждые 30 сек
            refreshTimer.Tick += RefreshTimer_Tick;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                isLogout = true;
                refreshTimer.Stop();
                this.Close();
            }
        }

        private void btnViewClients_Click(object sender, EventArgs e)
        {
            ClientProfilesForm clientProfilesForm = new ClientProfilesForm();
            clientProfilesForm.ShowDialog();
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }

            if (!isLogout)
            {
                Application.Exit();
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                int? selectedRequestId = null;
                if (dataGridViewRequests.SelectedRows.Count > 0)
                {
                    selectedRequestId = Convert.ToInt32(dataGridViewRequests.SelectedRows[0].Cells["RequestId"].Value);
                }

                LoadRequestsData();
                LoadRecyclingPointsData();
                LoadWorkers();

                // восстановим выделение после обновления
                if (selectedRequestId.HasValue)
                {
                    foreach (DataGridViewRow row in dataGridViewRequests.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["RequestId"].Value) == selectedRequestId.Value)
                        {
                            row.Selected = true;
                            dataGridViewRequests.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка автоматического обновления данных: " + ex.Message);
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadRecyclingPointsData();
                LoadRequestsData();
                LoadWorkers();
                refreshTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadWorkers()
        {
            try
            {
                var workers = dbHelper.GetWorkers();
                comboBoxWorkers.DataSource = workers;
                comboBoxWorkers.DisplayMember = "Name";
                comboBoxWorkers.ValueMember = "EmployeeId";

                if (workers.Rows.Count == 0)
                {
                    comboBoxWorkers.Enabled = false;
                    btnAssignWorker.Enabled = false;
                }
                else
                {
                    comboBoxWorkers.Enabled = true;
                    btnAssignWorker.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки работников: " + ex.Message, "Ошибка",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAssignWorker_Click(object sender, EventArgs e)
        {
            if (dataGridViewRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявку для назначения", "Внимание",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxWorkers.SelectedItem == null)
            {
                MessageBox.Show("Выберите работника из списка", "Внимание",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewRequests.SelectedRows[0];
            int requestId = Convert.ToInt32(selectedRow.Cells["RequestId"].Value);
            int workerId = Convert.ToInt32(comboBoxWorkers.SelectedValue);

            try
            {
                if (dbHelper.AssignRequestToWorker(requestId, workerId))
                {
                    MessageBox.Show("Заявка успешно назначена работнику", "Успех",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadRequestsData();

                    // подсветим обновлённую строку
                    foreach (DataGridViewRow row in dataGridViewRequests.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["RequestId"].Value) == requestId)
                        {
                            row.Selected = true;
                            dataGridViewRequests.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось назначить заявку", "Ошибка",
                                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при назначении заявки: " + ex.Message, "Ошибка",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenAddPoint_Click(object sender, EventArgs e)
        {
            if (dataGridViewPoints.SelectedRows.Count > 0)
            {
                var row = dataGridViewPoints.SelectedRows[0];
                int pointId = Convert.ToInt32(row.Cells["PointId"].Value);
                string name = row.Cells["Name"].Value.ToString();
                string address = row.Cells["Address"].Value.ToString();
                string phone = row.Cells["PhoneNumber"].Value?.ToString();

                var form = new AddRecyclingPointForm(pointId, name, address, phone);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadRecyclingPointsData();
                }
            }
            else
            {
                var form = new AddRecyclingPointForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadRecyclingPointsData();
                }
            }
        }

        private void BtnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                sfd.FileName = "Заявки.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToExcel(dataGridViewRequests, sfd.FileName);
                    MessageBox.Show("Экспорт завершён.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка экспорта: " + ex.Message);
            }
        }

        private void ExportToExcel(DataGridView dgv, string filename)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = workbook.Sheets[1];

            // заголовки
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
            }

            // данные
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    if (dgv == dataGridViewRequests && dgv.Columns[j].Name == "Status")
                    {
                        string rawStatus = dgv.Rows[i].Cells[j].Value?.ToString() ?? "";
                        worksheet.Cells[i + 2, j + 1] = GetRussianStatus(rawStatus);
                    }
                    else
                    {
                        worksheet.Cells[i + 2, j + 1] = dgv.Rows[i].Cells[j].Value?.ToString() ?? "";
                    }
                }
            }

            workbook.SaveAs(filename);
            workbook.Close();
            excelApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        private string GetRussianStatus(string englishStatus)
        {
            switch (englishStatus)
            {
                case "Accepted": return "Принята";
                case "In Progress": return "В обработке";
                case "At Recycling Point": return "На утилизации";
                case "Awaiting Admin Approval": return "Ожидает подтверждения";
                case "Completed": return "Завершена";
                case "Rejected": return "Отклонена";
                case "Needs Revision": return "Требует доработки";
                default: return englishStatus;
            }
        }

        private void BtnLoadRequests_Click(object sender, EventArgs e)
        {
            LoadRequestsData();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            using (var addEmployeeForm = new AddEmployeeForm())
            {
                if (addEmployeeForm.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Новый работник успешно добавлен в систему.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadWorkers();
                }
            }
        }

        private void BtnChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewRequests.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите заявку для обработки.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dataGridViewRequests.SelectedRows[0];
                int requestId = Convert.ToInt32(selectedRow.Cells["RequestId"].Value);
                string currentStatusFromDb = selectedRow.Cells["Status"].Value?.ToString();

                if (currentStatusFromDb != "Ожидает подтверждения")
                {
                    MessageBox.Show("Эту заявку нельзя обрабатывать. Статус должен быть 'Ожидает подтверждения'.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string carModel = selectedRow.Cells["CarModel"].Value?.ToString();
                string recyclingPoint = selectedRow.Cells["RecyclingPoint"].Value?.ToString();
                string requestInfo = $"Заявка #{requestId}\nАвто: {carModel}\nПункт: {recyclingPoint}";

                using (var reviewForm = new ReviewRequestForm(requestId, requestInfo))
                {
                    if (reviewForm.ShowDialog() == DialogResult.OK)
                    {
                        if (reviewForm.IsApproved)
                        {
                            if (dbHelper.CompleteRequest(requestId, "Completed"))
                                MessageBox.Show("Заявка успешно завершена.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                MessageBox.Show("Ошибка при завершении заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (dbHelper.RejectRequest(requestId, reviewForm.Comment))
                                MessageBox.Show("Заявка возвращена на доработку.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                MessageBox.Show("Ошибка при возврате на доработку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        LoadRequestsData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRequestsData()
        {
            try
            {
                DataTable requests = dbHelper.GetAllRequests();
                if (requests != null)
                {
                    dataGridViewRequests.DataSource = requests;

                    if (dataGridViewRequests.Columns.Contains("RequestId"))
                        dataGridViewRequests.Columns["RequestId"].HeaderText = "ID Заявки";
                    if (dataGridViewRequests.Columns.Contains("CarModel"))
                        dataGridViewRequests.Columns["CarModel"].HeaderText = "Модель Авто";
                    if (dataGridViewRequests.Columns.Contains("RecyclingPoint"))
                        dataGridViewRequests.Columns["RecyclingPoint"].HeaderText = "Пункт Утилизации";
                    if (dataGridViewRequests.Columns.Contains("EmployeeName"))
                        dataGridViewRequests.Columns["EmployeeName"].HeaderText = "Сотрудник";
                    if (dataGridViewRequests.Columns.Contains("Status"))
                        dataGridViewRequests.Columns["Status"].HeaderText = "Статус";
                    if (dataGridViewRequests.Columns.Contains("SubmissionDate"))
                        dataGridViewRequests.Columns["SubmissionDate"].HeaderText = "Дата Подачи";
                    if (dataGridViewRequests.Columns.Contains("CompletionDate"))
                        dataGridViewRequests.Columns["CompletionDate"].HeaderText = "Дата Завершения";
                    if (dataGridViewRequests.Columns.Contains("Cost"))
                        dataGridViewRequests.Columns["Cost"].HeaderText = "Стоимость";
                    if (dataGridViewRequests.Columns.Contains("Description"))
                        dataGridViewRequests.Columns["Description"].HeaderText = "Описание";
                    if (dataGridViewRequests.Columns.Contains("WorkerComment"))
                        dataGridViewRequests.Columns["WorkerComment"].HeaderText = "Комментарий Работника";
                    if (dataGridViewRequests.Columns.Contains("AdminComment"))
                        dataGridViewRequests.Columns["AdminComment"].HeaderText = "Комментарий Админа";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заявок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRecyclingPointsData()
        {
            try
            {
                DataTable points = dbHelper.GetRecyclingPoints();
                if (points != null)
                {
                    dataGridViewPoints.DataSource = points;

                    if (dataGridViewPoints.Columns.Contains("PointId"))
                        dataGridViewPoints.Columns["PointId"].HeaderText = "ID Пункта";
                    if (dataGridViewPoints.Columns.Contains("Name"))
                        dataGridViewPoints.Columns["Name"].HeaderText = "Название Пункта";
                    if (dataGridViewPoints.Columns.Contains("Address"))
                        dataGridViewPoints.Columns["Address"].HeaderText = "Адрес";
                    if (dataGridViewPoints.Columns.Contains("PhoneNumber"))
                        dataGridViewPoints.Columns["PhoneNumber"].HeaderText = "Телефон";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пунктов утилизации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewRequests_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewRequests.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                e.Value = GetRussianStatus(e.Value.ToString());
                e.FormattingApplied = true;
            }
        }

        private void dataGridViewPoints_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // пока пусто, но если появятся столбцы с английскими значениями — переводим здесь
        }
    }
}