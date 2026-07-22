using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CarRecyclingApp
{
    public partial class WorkerForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private int userId;
        private string userRole;
        private bool isLogout = false;

        public WorkerForm(int userId, string userRole)
        {
            InitializeComponent();
            this.userId = userId;
            this.userRole = userRole;

            dataGridViewRequests.SelectionChanged += DataGridViewRequests_SelectionChanged;
            btnLogout.Click += btnLogout_Click;
            btnChangeStatus.Click += btnChangeStatus_Click;
            btnShowComment.Click += BtnShowComment_Click;

            this.FormClosed += WorkerForm_FormClosed;
            this.Load += WorkerForm_Load;

            this.dataGridViewRequests.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewRequests_CellFormatting);

            if (this.btnSaveWorkerComment != null)
            {
                this.btnSaveWorkerComment.Click += BtnSaveWorkerComment_Click;
                this.btnSaveWorkerComment.Enabled = false;
            }
            if (this.txtWorkerComment != null)
            {
                this.txtWorkerComment.Enabled = false;
            }
            if (this.comboBoxStatus != null)
            {
                this.comboBoxStatus.Enabled = false;
            }
            if (this.btnChangeStatus != null)
            {
                this.btnChangeStatus.Enabled = false;
            }
            if (this.btnShowComment != null)
            {
                this.btnShowComment.Enabled = false;
            }
        }

        private void WorkerForm_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshRequests();
                if (this.comboBoxStatus != null)
                {
                    comboBoxStatus.Items.Clear();
                    comboBoxStatus.Items.Add("Принята");
                    comboBoxStatus.Items.Add("В обработке");
                    comboBoxStatus.Items.Add("На утилизации");
                    comboBoxStatus.Items.Add("Ожидает подтверждения");
                }
                else
                {
                    MessageBox.Show("Элемент comboBoxStatus не найден на форме.", "Ошибка конфигурации UI");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заявок: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WorkerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isLogout)
            {
                Application.Exit();
            }
        }

        private void btnViewClients_Click(object sender, EventArgs e)
        {
            ClientProfilesForm clientProfilesForm = new ClientProfilesForm();
            clientProfilesForm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                isLogout = true;
                this.Close();
            }
        }

        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            if (dataGridViewRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявку для изменения статуса.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Выберите новый статус.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int requestId = Convert.ToInt32(dataGridViewRequests.SelectedRows[0].Cells["RequestId"].Value);
            string selectedStatus = comboBoxStatus.SelectedItem.ToString();
            string currentStatusDb = dataGridViewRequests.SelectedRows[0].Cells["Status"].Value.ToString();

            if (currentStatusDb == "Завершена" || currentStatusDb == "Отклонена")
            {
                MessageBox.Show($"Заявка со статусом '{currentStatusDb}' не может быть изменена работником.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedStatus == "Принята" && currentStatusDb != "Требует доработки")
            {
                MessageBox.Show("Нельзя вернуть заявку в статус 'Принята' из текущего состояния, кроме как после статуса 'Требует доработки'.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool statusUpdated = false;

            if (selectedStatus == "В обработке" || selectedStatus == "На утилизации")
            {
                statusUpdated = dbHelper.UpdateRequestStatus(requestId, selectedStatus);
            }
            else if (selectedStatus == "Ожидает подтверждения")
            {
                DialogResult dr = MessageBox.Show("Вы уверены, что хотите отправить заявку на подтверждение администратору?\nУбедитесь, что вся необходимая информация и комментарии добавлены.",
                                                 "Подтверждение действия", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    statusUpdated = dbHelper.UpdateRequestStatus(requestId, selectedStatus);
                }
            }
            else if (selectedStatus == "Принята")
            {
                statusUpdated = dbHelper.UpdateRequestStatus(requestId, selectedStatus);
            }
            else
            {
                MessageBox.Show($"Статус '{selectedStatus}' не может быть установлен работником.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (statusUpdated)
            {
                MessageBox.Show($"Статус заявки #{requestId} успешно изменен на '{selectedStatus}'.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshRequests();
            }
            else if (selectedStatus == "Ожидает подтверждения" && !statusUpdated)
            {
            }
            else
            {
                MessageBox.Show($"Не удалось обновить статус заявки #{requestId}.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshRequests()
        {
            try
            {
                LoadRequestData();

                if (dataGridViewRequests.Rows.Count == 0 || dataGridViewRequests.SelectedRows.Count == 0)
                {
                    if (txtWorkerComment != null) txtWorkerComment.Clear();
                    if (btnSaveWorkerComment != null) btnSaveWorkerComment.Enabled = false;
                    if (txtWorkerComment != null) txtWorkerComment.Enabled = false;
                    if (btnShowComment != null) btnShowComment.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении списка заявок: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewRequests_SelectionChanged(object sender, EventArgs e)
        {
            bool rowSelected = dataGridViewRequests.SelectedRows.Count > 0;

            if (txtWorkerComment != null)
            {
                txtWorkerComment.Enabled = rowSelected;
                if (rowSelected)
                {
                    if (dataGridViewRequests.Columns.Contains("WorkerComment"))
                    {
                        txtWorkerComment.Text = dataGridViewRequests.SelectedRows[0].Cells["WorkerComment"].Value?.ToString() ?? "";
                    }
                    else
                    {
                        txtWorkerComment.Text = "";
                    }
                }
                else
                {
                    txtWorkerComment.Clear();
                }
            }

            if (btnSaveWorkerComment != null)
            {
                btnSaveWorkerComment.Enabled = rowSelected;
            }

            if (btnShowComment != null)
            {
                if (rowSelected && dataGridViewRequests.Columns.Contains("AdminComment"))
                {
                    string adminComment = dataGridViewRequests.SelectedRows[0].Cells["AdminComment"].Value?.ToString();
                    btnShowComment.Enabled = !string.IsNullOrWhiteSpace(adminComment);
                }
                else
                {
                    btnShowComment.Enabled = false;
                }
            }

            if (comboBoxStatus != null) comboBoxStatus.Enabled = rowSelected;
            if (btnChangeStatus != null) btnChangeStatus.Enabled = rowSelected;

            if (rowSelected)
            {
                string currentStatusDb = dataGridViewRequests.SelectedRows[0].Cells["Status"].Value?.ToString();

                bool canChangeStatus = !(currentStatusDb == "Завершена" || currentStatusDb == "Отклонена");
                if (comboBoxStatus != null) comboBoxStatus.Enabled = canChangeStatus;
                if (btnChangeStatus != null) btnChangeStatus.Enabled = canChangeStatus;

                if (comboBoxStatus != null && !string.IsNullOrEmpty(currentStatusDb))
                {
                    if (comboBoxStatus.Items.Contains(currentStatusDb))
                    {
                        comboBoxStatus.SelectedItem = currentStatusDb;
                    }
                    else
                    {
                        comboBoxStatus.SelectedIndex = -1;
                    }
                }
                else if (comboBoxStatus != null)
                {
                    comboBoxStatus.SelectedIndex = -1;
                }
            }
        }

        private void BtnShowComment_Click(object sender, EventArgs e)
        {
            if (dataGridViewRequests.SelectedRows.Count > 0)
            {
                if (dataGridViewRequests.Columns.Contains("AdminComment"))
                {
                    string comment = dataGridViewRequests.SelectedRows[0].Cells["AdminComment"].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(comment))
                    {
                        comment = "Комментарий администратора отсутствует.";
                    }
                    MessageBox.Show(comment, "Комментарий администратора к заявке", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Информация о комментарии администратора недоступна.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnSaveWorkerComment_Click(object sender, EventArgs e)
        {
            if (dataGridViewRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Сначала выберите заявку.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int requestId = Convert.ToInt32(dataGridViewRequests.SelectedRows[0].Cells["RequestId"].Value);
            string workerComment = txtWorkerComment.Text.Trim();

            try
            {
                if (dbHelper.AddOrUpdateWorkerComment(requestId, workerComment))
                {
                    MessageBox.Show("Комментарий успешно сохранен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshRequests();
                }
                else
                {
                    MessageBox.Show("Не удалось сохранить комментарий.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении комментария: {ex.Message}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void LoadRequestData()
        {
            try
            {
                DataTable requests = dbHelper.GetRequestsByWorkerId(this.userId);
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
                    if (dataGridViewRequests.Columns.Contains("AdminComment"))
                        dataGridViewRequests.Columns["AdminComment"].HeaderText = "Комм. Админа";
                    if (dataGridViewRequests.Columns.Contains("WorkerComment"))
                        dataGridViewRequests.Columns["WorkerComment"].HeaderText = "Ваш Комм.";

                    if (requests.Rows.Count == 0)
                    {
                        MessageBox.Show("Нет данных о заявках для отображения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о заявках: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewRequests_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }
    }
}