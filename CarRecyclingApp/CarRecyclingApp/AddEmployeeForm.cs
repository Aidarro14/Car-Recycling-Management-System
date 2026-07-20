// AddEmployeeForm.cs
using System;
using System.Text.RegularExpressions; // Для валидации email
using System.Windows.Forms;
using System.Xml.Linq; // Возможно, это не нужно, если XDocument не используется
// using BCrypt.Net; // Эта строка теперь не нужна в этом файле, так как хеширование происходит в DatabaseHelper

namespace CarRecyclingApp
{
    public partial class AddEmployeeForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public AddEmployeeForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text; // Это обычный, НЕхешированный пароль
            string confirmPassword = txtConfirmPassword.Text;

            // Валидация
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Все поля обязательны для заполнения.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Введите корректный Email адрес.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password.Length < 6) // Примерная проверка на длину пароля
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // УДАЛИТЕ СТРОКУ ХЕШИРОВАНИЯ ЗДЕСЬ!
                // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password); // ЭТО БЫЛО НЕПРАВИЛЬНО

                string role = "worker"; // По умолчанию создаем работника с ролью "worker"

                // ПЕРЕДАЙТЕ ОБЫЧНЫЙ ПАРОЛЬ (password), ОН БУДЕТ ХЕШИРОВАН ВНУТРИ AddEmployee
                if (dbHelper.AddEmployee(name, email, password, role))
                {
                    MessageBox.Show("Новый работник успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Сообщение об ошибке (например, дубликат email) будет показано из dbHelper.AddEmployee
                    // или можно добавить более общее здесь, если AddEmployee возвращает false по разным причинам
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении работника: {ex.Message}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Используем простое регулярное выражение для базовой проверки формата email
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}