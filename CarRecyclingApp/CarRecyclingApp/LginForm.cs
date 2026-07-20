// Файл: LoginForm.cs
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace CarRecyclingApp
{
    public partial class LoginForm : Form
    {
        public int LoggedInEmployeeId { get; private set; }
        public string EmployeeRole { get; private set; }

        // Если LoggedInUserId и UserRole не используются для Employee (а похоже, что так),
        // их можно удалить или закомментировать, чтобы не вводить в заблуждение.
        // public int LoggedInUserId { get; private set; }
        // public string UserRole { get; private set; }

        private DatabaseHelper dbHelper = new DatabaseHelper();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text; // Пароль в открытом виде от пользователя

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите email и пароль.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor; // Показать курсор ожидания
            btnLogin.Enabled = false;         // Отключить кнопку на время запроса

            try
            {
                // Вызываем ваш обновленный (и теперь безопасный) метод LoginEmployee
                var employee = dbHelper.LoginEmployee(email, password);

                if (employee != null)
                {
                    LoggedInEmployeeId = employee.Value.employeeId;
                    EmployeeRole = employee.Value.role;

                    // Если LoggedInUserId и UserRole нужны и являются тем же, что и для Employee:
                    // LoggedInUserId = employee.Value.employeeId;
                    // UserRole = employee.Value.role;

                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный email или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear(); // Очистить поле пароля
                    txtEmail.Focus();    // Вернуть фокус на поле email (или txtPassword.Focus() если предпочитаете)
                }
            }
            catch (MySqlException dbEx) // Более специфичная обработка ошибок БД
            {
                // Здесь можно логировать dbEx.ToString() для детальной информации
                MessageBox.Show($"Ошибка при обращении к базе данных: {dbEx.Message}\nПроверьте подключение или обратитесь к администратору.", "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Общая обработка других непредвиденных ошибок
            {
                // Здесь можно логировать ex.ToString()
                MessageBox.Show($"Произошла непредвиденная ошибка: {ex.Message}\nПожалуйста, попробуйте позже или обратитесь к администратору.", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default; // Вернуть обычный курсор
                btnLogin.Enabled = true;      // Включить кнопку обратно
            }
        }
    }
}