using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CarRecyclingApp
{
    public partial class WorkerForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private int userId; // Это EmployeeId работника
        private string userRole;
        private bool isLogout = false;

        

        public WorkerForm(int userId, string userRole)
        {
            InitializeComponent();
            this.userId = userId;
            this.userRole = userRole;

            // Существующие подписки
            dataGridViewRequests.SelectionChanged += DataGridViewRequests_SelectionChanged;
            btnLogout.Click += btnLogout_Click;
            btnChangeStatus.Click += btnChangeStatus_Click;
            btnShowComment.Click += BtnShowComment_Click;

            this.FormClosed += WorkerForm_FormClosed;
            this.Load += WorkerForm_Load;

            
            this.dataGridViewRequests.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewRequests_CellFormatting);


            // Проверки на null для контролов, которые могли быть не инициализированы
            if (this.btnSaveWorkerComment != null)
            {
                this.btnSaveWorkerComment.Click += BtnSaveWorkerComment_Click;
                this.btnSaveWorkerComment.Enabled = false; // Изначально неактивна
            }
            if (this.txtWorkerComment != null)
            {
                this.txtWorkerComment.Enabled = false; // Изначально неактивен
            }
            if (this.comboBoxStatus != null)
            {
                this.comboBoxStatus.Enabled = false; // Изначально неактивен
            }
            if (this.btnChangeStatus != null)
            {
                this.btnChangeStatus.Enabled = false; // Изначально неактивен
            }
            if (this.btnShowComment != null)
            {
                this.btnShowComment.Enabled = false; // Изначально неактивен
            }
        }

        private void WorkerForm_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshRequests(); // Обновляем DataGridView и переводим заголовки
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
            else if (selectedStatus == "Принята") // Если работник может сам выставить "Принята"
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
                RefreshRequests(); // Обновляем таблицу
            }
            else if (selectedStatus == "Ожидает подтверждения" && !statusUpdated)
            {
                // Не показываем ошибку, если пользователь отменил отправку на подтверждение
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
                // Вызываем основной метод загрузки и перевода заголовков
                LoadRequestData();

                // Очищаем поля и деактивируем кнопки, если нет выбранных строк
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
                    // Убедимся, что столбец WorkerComment существует, прежде чем пытаться его прочитать
                    if (dataGridViewRequests.Columns.Contains("WorkerComment"))
                    {
                        txtWorkerComment.Text = dataGridViewRequests.SelectedRows[0].Cells["WorkerComment"].Value?.ToString() ?? "";
                    }
                    else
                    {
                        txtWorkerComment.Text = ""; // Если столбца нет, очищаем
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

            if (btnShowComment != null) // Кнопка для просмотра комментария админа
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

            // Логика для comboBoxStatus и btnChangeStatus
            if (comboBoxStatus != null) comboBoxStatus.Enabled = rowSelected;
            if (btnChangeStatus != null) btnChangeStatus.Enabled = rowSelected;

            if (rowSelected)
            {
                // Получаем оригинальное значение статуса из БД для использования в логике
                // --- ИЗМЕНЕНИЕ ---
                // currentStatusDb теперь будет на русском, напрямую из БД.
                string currentStatusDb = dataGridViewRequests.SelectedRows[0].Cells["Status"].Value?.ToString();

                // Разрешаем менять статус, если он не финальный
                // --- ИЗМЕНЕНИЕ ---
                // Сравниваем с русскими значениями.
                bool canChangeStatus = !(currentStatusDb == "Завершена" || currentStatusDb == "Отклонена");
                if (comboBoxStatus != null) comboBoxStatus.Enabled = canChangeStatus;
                if (btnChangeStatus != null) btnChangeStatus.Enabled = canChangeStatus;

                // Заполнить comboBoxStatus текущим статусом заявки (в русском виде)
                // --- ИЗМЕНЕНИЕ ---
                // Больше нет необходимости в switch для преобразования.
                // Значение из БД (currentStatusDb) уже русское.
                if (comboBoxStatus != null && !string.IsNullOrEmpty(currentStatusDb))
                {
                    if (comboBoxStatus.Items.Contains(currentStatusDb))
                    {
                        comboBoxStatus.SelectedItem = currentStatusDb;
                    }
                    else
                    {
                        comboBoxStatus.SelectedIndex = -1; // Сбросить выбор, если текущего статуса нет в списке
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
                    RefreshRequests(); // Обновляем данные
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
                // Предполагаем, что dbHelper.GetRequestsByWorkerId(this.userId) возвращает DataTable
                // из вашего DatabaseHelper.cs.
                // В этом запросе должны быть столбцы с точными именами из SQL (RequestId, CarModel, Status, WorkerComment, AdminComment и т.д.)
                DataTable requests = dbHelper.GetRequestsByWorkerId(this.userId);
                if (requests != null)
                {
                    dataGridViewRequests.DataSource = requests;

                    // --- БЛОК ДЛЯ ПЕРЕВОДА ЗАГОЛОВКОВ СТОЛБЦОВ ---
                    // Проверяем наличие столбцов и переводим их заголовки
                    if (dataGridViewRequests.Columns.Contains("RequestId"))
                        dataGridViewRequests.Columns["RequestId"].HeaderText = "ID Заявки";
                    if (dataGridViewRequests.Columns.Contains("CarModel"))
                        dataGridViewRequests.Columns["CarModel"].HeaderText = "Модель Авто";
                    if (dataGridViewRequests.Columns.Contains("RecyclingPoint"))
                        dataGridViewRequests.Columns["RecyclingPoint"].HeaderText = "Пункт Утилизации";
                    if (dataGridViewRequests.Columns.Contains("EmployeeName"))
                        dataGridViewRequests.Columns["EmployeeName"].HeaderText = "Сотрудник";
                    if (dataGridViewRequests.Columns.Contains("Status"))
                        // --- ИЗМЕНЕНИЕ ---
                        // Теперь статус уже на русском из БД, CellFormatting не нужен для значений
                        dataGridViewRequests.Columns["Status"].HeaderText = "Статус";
                    if (dataGridViewRequests.Columns.Contains("SubmissionDate"))
                        dataGridViewRequests.Columns["SubmissionDate"].HeaderText = "Дата Подачи";
                    if (dataGridViewRequests.Columns.Contains("CompletionDate"))
                        dataGridViewRequests.Columns["CompletionDate"].HeaderText = "Дата Завершения";
                    if (dataGridViewRequests.Columns.Contains("Cost"))
                        dataGridViewRequests.Columns["Cost"].HeaderText = "Стоимость";
                    if (dataGridViewRequests.Columns.Contains("Description"))
                        dataGridViewRequests.Columns["Description"].HeaderText = "Описание";
                    // ИСПРАВЛЕНО: Используем AdminComment и WorkerComment, как в SQL-запросе
                    if (dataGridViewRequests.Columns.Contains("AdminComment"))
                        dataGridViewRequests.Columns["AdminComment"].HeaderText = "Комм. Админа";
                    if (dataGridViewRequests.Columns.Contains("WorkerComment"))
                        dataGridViewRequests.Columns["WorkerComment"].HeaderText = "Ваш Комм.";
                    // --- КОНЕЦ БЛОКА ПЕРЕВОДА ЗАГОЛОВКОВ ---

                    // Опционально: Скрыть столбцы, которые не нужны или дублируются
                    // if (dataGridViewRequests.Columns.Contains("AdminComment"))
                    //      dataGridViewRequests.Columns["AdminComment"].Visible = false;
                    // if (dataGridViewRequests.Columns.Contains("WorkerComment"))
                    //      dataGridViewRequests.Columns["WorkerComment"].Visible = false;

                    if (requests.Rows.Count == 0)
                    {
                        MessageBox.Show("Нет данных о заявках для отображения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о заявках: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Логирование ошибки
            }
        }

        
        private void dataGridViewRequests_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Проверяем, что это столбец "Status" и что значение не null
            // ЭТОТ БЛОК ТЕПЕРЬ МОЖНО УДАЛИТЬ, так как статусы уже приходят на русском из БД
            if (dataGridViewRequests.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                // Если вы уверены, что из БД всегда приходят русские статусы,
                // то здесь ничего переводить не нужно.
                // e.Value уже будет содержать русское значение.
                // Этот блок кода становится избыточным.
                // e.FormattingApplied = true; // Это все, что вам нужно, если вы хотите указать, что форматирование было обработано.
            }
            // Здесь можно добавить другие столбцы для перевода, если потребуется
            // Например:
            // if (dataGridViewRequests.Columns[e.ColumnIndex].Name == "CarType" && e.Value != null)
            // {
            //      string englishType = e.Value.ToString();
            //      if (englishType == "Sedan") e.Value = "Седан";
            //      else if (englishType == "SUV") e.Value = "Внедорожник";
            //      e.FormattingApplied = true;
            // }
        }
        // --- КОНЕЦ МЕТОДА CellFormatting ---
    }
}