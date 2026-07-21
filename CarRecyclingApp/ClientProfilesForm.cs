using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Добавьте, если нужно для обработки специфических ошибок MySql

namespace CarRecyclingApp
{
    public partial class ClientProfilesForm : Form
    {
        private DatabaseHelper dbHelper;

        public ClientProfilesForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper(); // Инициализируем DatabaseHelper здесь

            // Подписываемся на событие Load формы
            this.Load += ClientProfilesForm_Load;
            // Подписываемся на событие Click кнопки btnRefresh, если она есть на форме
            if (this.btnRefresh != null)
            {
                this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            }
        }

        private void ClientProfilesForm_Load(object sender, EventArgs e)
        {
            LoadClientData(); // Вызываем метод для загрузки и настройки данных
        }

        private void LoadClientData()
        {
            try
            {
                DataTable clients = dbHelper.GetAllClients(); // Получаем все данные о клиентах

                // Проверяем, что DataTable не null и содержит данные
                if (clients != null && clients.Rows.Count > 0)
                {
                    dataGridViewClients.DataSource = clients;

                    // --- БЛОК ДЛЯ ПЕРЕВОДА ЗАГОЛОВКОВ СТОЛБЦОВ ---
                    // В ClientProfilesForm.cs, в методе LoadClientData()
                    if (dataGridViewClients.Columns.Contains("ClientId"))
                        dataGridViewClients.Columns["ClientId"].HeaderText = "ID Клиента";
                    if (dataGridViewClients.Columns.Contains("FirstName")) // ИЗМЕНЕНО
                        dataGridViewClients.Columns["FirstName"].HeaderText = "Имя";
                    if (dataGridViewClients.Columns.Contains("LastName")) // ИЗМЕНЕНО
                        dataGridViewClients.Columns["LastName"].HeaderText = "Фамилия";
                    if (dataGridViewClients.Columns.Contains("Email"))
                        dataGridViewClients.Columns["Email"].HeaderText = "Email";
                    if (dataGridViewClients.Columns.Contains("PhoneNumber"))
                        dataGridViewClients.Columns["PhoneNumber"].HeaderText = "Телефон";
                    // Если есть столбец Address в БД, но его нет на скриншоте, и он вам нужен
                    // if (dataGridViewClients.Columns.Contains("Address"))
                    //     dataGridViewClients.Columns["Address"].HeaderText = "Адрес";

                    // Скрыть PasswordHash
                    if (dataGridViewClients.Columns.Contains("PasswordHash"))
                        dataGridViewClients.Columns["PasswordHash"].Visible = false;
                    // Если у вас есть другие столбцы в таблице клиентов (например, "RegistrationDate", "LastLogin"),
                    // добавьте их сюда с соответствующим переводом.
                    // Пример:
                    // if (dataGridViewClients.Columns.Contains("RegistrationDate"))
                    //     dataGridViewClients.Columns["RegistrationDate"].HeaderText = "Дата Регистрации";
                    // --- КОНЕЦ БЛОКА ПЕРЕВОДА ЗАГОЛОВКОВ ---

                    // Опционально: Скрыть столбцы, которые не должны быть видны пользователю
                    // Например, если PasswordHash хранится в таблице клиентов
                    if (dataGridViewClients.Columns.Contains("PasswordHash")) // Предполагаем, что столбец назван "PasswordHash"
                    {
                        dataGridViewClients.Columns["PasswordHash"].Visible = false;
                    }
                }
                else if (clients != null && clients.Rows.Count == 0) // Если DataTable не null, но пуст
                {
                    dataGridViewClients.DataSource = null; // Очищаем DataGridView, если нет данных
                    MessageBox.Show("Нет данных о клиентах для отображения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Если DataTable вообще null (ошибка в dbHelper.GetAllClients())
                {
                    dataGridViewClients.DataSource = null;
                    MessageBox.Show("Не удалось загрузить данные о клиентах. DataTable равен null.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о клиентах: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // В реальном приложении здесь обязательно должно быть логирование ошибки
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadClientData(); // Обновить данные
        }
    }
}