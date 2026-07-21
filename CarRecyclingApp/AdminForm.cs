using System;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel; // Убедитесь, что у вас есть ссылка на Microsoft.Office.Interop.Excel

namespace CarRecyclingApp
{
    public partial class AdminForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private int employeeId;
        private string role;
        private bool isLogout = false;

        // Объявляем таймер как поле класса
        private System.Windows.Forms.Timer refreshTimer;

        public AdminForm(int employeeId, string role)
        {
            InitializeComponent();
            this.employeeId = employeeId;
            this.role = role;

            this.Load += AdminForm_Load;
            this.FormClosed += AdminForm_FormClosed;

            // Подписываемся на события кнопок
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            this.btnViewClients.Click += new System.EventHandler(this.btnViewClients_Click);
            this.btnAssignWorker.Click += new System.EventHandler(this.BtnAssignWorker_Click);
            this.btnOpenAddPoint.Click += new System.EventHandler(this.btnOpenAddPoint_Click);
            this.btnExportToExcel.Click += new System.EventHandler(this.BtnExportToExcel_Click);
            this.btnLoadRequests.Click += new System.EventHandler(this.BtnLoadRequests_Click);
            this.btnAddEmployee.Click += new System.EventHandler(this.btnAddEmployee_Click);
            this.btnChangeStatus.Click += new System.EventHandler(this.BtnChangeStatus_Click); // Кнопка для изменения статуса заявки админом

            // Подписываемся на события CellFormatting для обеих таблиц
            this.dataGridViewRequests.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewRequests_CellFormatting);
            this.dataGridViewPoints.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewPoints_CellFormatting);


            // --- Добавляем инициализацию и настройку таймера ---
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 30000; // Интервал в миллисекундах (30 секунд)
                                           // Вы можете изменить это значение. Например, 60000 для 1 минуты.
            refreshTimer.Tick += RefreshTimer_Tick; // Подписываемся на событие Tick таймера
            // Таймер будет запущен в AdminForm_Load после первоначальной загрузки данных
            // --- Конец добавления таймера ---
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                isLogout = true; // пометили, что это выход, а не крестик
                refreshTimer.Stop(); // Останавливаем таймер перед закрытием формы
                this.Close();    // закроем текущую форму
            }
        }

        private void btnViewClients_Click(object sender, EventArgs e)
        {
            ClientProfilesForm clientProfilesForm = new ClientProfilesForm();
            clientProfilesForm.ShowDialog(); // Открыть форму как модальное окно
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Убедимся, что таймер остановлен при закрытии формы
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose(); // Освобождаем ресурсы таймера
            }

            if (!isLogout)
            {
                Application.Exit();  // пользователь закрыл окно крестиком - завершаем приложение
            }
            // если isLogout == true — просто закрываем форму, цикл в Program.cs запустит логин снова
        }

        // --- Новый метод для обработки события Tick таймера ---
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // Здесь мы обновляем данные в DataGridView, чтобы видеть новые заявки
            try
            {
                // Запоминаем текущую выделенную строку, чтобы после обновления вернуться к ней
                int? selectedRequestId = null;
                if (dataGridViewRequests.SelectedRows.Count > 0)
                {
                    selectedRequestId = Convert.ToInt32(dataGridViewRequests.SelectedRows[0].Cells["RequestId"].Value);
                }

                // Загружаем данные для обеих таблиц и переводим заголовки/содержимое
                LoadRequestsData();
                LoadRecyclingPointsData();
                LoadWorkers(); // Перезагружаем работников, если это нужно (могут появиться новые)

                // Пытаемся восстановить выделение
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
                // Можно просто логировать ошибку, чтобы не прерывать работу приложения
                Console.WriteLine("Ошибка автоматического обновления данных: " + ex.Message);
            }
        }
        // --- Конец нового метода ---


        private void AdminForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Первоначальная загрузка данных для обеих таблиц и перевод
                LoadRecyclingPointsData(); // Загружаем и переводим данные для пунктов
                LoadRequestsData();        // Загружаем и переводим данные для заявок
                LoadWorkers();             // Загружаем работников

                // Запускаем таймер после того, как все данные загружены
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

                    // Обновляем данные, используя наш новый метод
                    LoadRequestsData();

                    // Подсвечиваем обновленную строку
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
                // редактирование
                var row = dataGridViewPoints.SelectedRows[0];
                // Убедимся, что столбцы существуют, прежде чем пытаться их прочитать
                int pointId = Convert.ToInt32(row.Cells["PointId"].Value);
                string name = row.Cells["Name"].Value.ToString();
                string address = row.Cells["Address"].Value.ToString();
                string phone = row.Cells["PhoneNumber"].Value?.ToString();

                var form = new AddRecyclingPointForm(pointId, name, address, phone);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadRecyclingPointsData(); // Обновляем данные после редактирования
                }
            }
            else
            {
                // добавление нового
                var form = new AddRecyclingPointForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadRecyclingPointsData(); // Обновляем данные после добавления
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

            // Заголовки
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText; // Используем HeaderText (переведенные заголовки)
            }

            // Данные
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    // Для столбца "Status" в DataGridViewRequests мы форматируем значение
                    // Если это столбец Status в DataGridViewRequests, берем отформатированное значение
                    if (dgv == dataGridViewRequests && dgv.Columns[j].Name == "Status")
                    {
                        // Придется вручную перевести, так как CellFormatting не срабатывает при прямом чтении
                        // или же нужно использовать DataGridViewCell.FormattedValue
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

            // Освобождаем COM-объекты
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        // Вспомогательный метод для перевода статусов (для экспорта в Excel)
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
            LoadRequestsData(); // Теперь использует новый метод для загрузки и перевода
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            using (var addEmployeeForm = new AddEmployeeForm())
            {
                if (addEmployeeForm.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Новый работник успешно добавлен в систему.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadWorkers(); // Обновляем список работников
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
                // ВАЖНО: Получаем оригинальный статус из ячейки (он будет на английском, если CellFormatting только для отображения)
                string currentStatusFromDb = selectedRow.Cells["Status"].Value?.ToString();

                // Изменено: Проверяем статус "Awaiting Admin Approval" (как он хранится в БД)
                // Заявка должна быть в статусе "Awaiting Admin Approval", чтобы ее админ мог подтвердить/отклонить
                if (currentStatusFromDb != "Ожидает подтверждения")
                {
                    MessageBox.Show("Эту заявку нельзя обрабатывать. Статус должен быть 'Ожидает подтверждения'.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string carModel = selectedRow.Cells["CarModel"].Value?.ToString(); // Добавим модель машины в информацию
                string recyclingPoint = selectedRow.Cells["RecyclingPoint"].Value?.ToString(); // Добавим пункт утилизации
                string requestInfo = $"Заявка #{requestId}\nАвто: {carModel}\nПункт: {recyclingPoint}";

                using (var reviewForm = new ReviewRequestForm(requestId, requestInfo))
                {
                    if (reviewForm.ShowDialog() == DialogResult.OK)
                    {
                        if (reviewForm.IsApproved)
                        {
                            // Подтверждаем заявку как Завершена (Completed)
                            if (dbHelper.CompleteRequest(requestId, "Completed")) // Имя статуса должно совпадать с тем, что в БД
                                MessageBox.Show("Заявка успешно завершена.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                MessageBox.Show("Ошибка при завершении заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            // Отправляем на доработку (Needs Revision) или Отклонена (Rejected)
                            // Важно: в ReviewRequestForm должно быть поле для комментария при отклонении/отправке на доработку
                            // Предполагаем, что RejectRequest использует статус "Needs Revision"
                            if (dbHelper.RejectRequest(requestId, reviewForm.Comment)) // Имя статуса должно совпадать с тем, что в БД
                                MessageBox.Show("Заявка возвращена на доработку.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                MessageBox.Show("Ошибка при возврате на доработку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Обновляем таблицу после изменения статуса
                        LoadRequestsData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- МЕТОДЫ ДЛЯ ЗАГРУЗКИ ДАННЫХ И ПЕРЕВОДА ЗАГОЛОВКОВ ---

        private void LoadRequestsData()
        {
            try
            {
                DataTable requests = dbHelper.GetAllRequests(); // Предполагается, что GetAllRequests возвращает нужные столбцы
                if (requests != null)
                {
                    dataGridViewRequests.DataSource = requests;

                    // Установка HeaderText для DataGridViewRequests
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
                DataTable points = dbHelper.GetRecyclingPoints(); // Предполагается, что GetRecyclingPoints возвращает нужные столбцы
                if (points != null)
                {
                    dataGridViewPoints.DataSource = points;

                    // Установка HeaderText для DataGridViewPoints
                    if (dataGridViewPoints.Columns.Contains("PointId"))
                        dataGridViewPoints.Columns["PointId"].HeaderText = "ID Пункта";
                    if (dataGridViewPoints.Columns.Contains("Name"))
                        dataGridViewPoints.Columns["Name"].HeaderText = "Название Пункта";
                    if (dataGridViewPoints.Columns.Contains("Address"))
                        dataGridViewPoints.Columns["Address"].HeaderText = "Адрес";
                    if (dataGridViewPoints.Columns.Contains("PhoneNumber"))
                        dataGridViewPoints.Columns["PhoneNumber"].HeaderText = "Телефон";
                    // Добавьте другие столбцы, если они есть в вашей таблице RecyclingPoints
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пунктов утилизации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- МЕТОДЫ CellFormatting ДЛЯ ПЕРЕВОДА СОДЕРЖИМОГО ЯЧЕЕК ---

        private void dataGridViewRequests_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Перевод статусов для DataGridViewRequests
            if (dataGridViewRequests.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                e.Value = GetRussianStatus(e.Value.ToString());
                e.FormattingApplied = true;
            }
            // Здесь можно добавить форматирование для других столбцов в dataGridViewRequests, если нужно
        }

        private void dataGridViewPoints_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Пример: если в таблице RecyclingPoints есть столбец "Type" с английскими значениями
            // if (dataGridViewPoints.Columns[e.ColumnIndex].Name == "Type" && e.Value != null)
            // {
            //     string englishType = e.Value.ToString();
            //     if (englishType == "Large") e.Value = "Крупный";
            //     else if (englishType == "Small") e.Value = "Малый";
            //     e.FormattingApplied = true;
            // }
            // Добавьте логику форматирования для dataGridViewPoints, если у вас есть такие столбцы
        }
    }
}