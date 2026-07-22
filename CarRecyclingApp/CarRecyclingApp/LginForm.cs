using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace CarRecyclingApp
{
    public partial class LoginForm : Form
    {
        public int LoggedInEmployeeId { get; private set; }
        public string EmployeeRole { get; private set; }

        private DatabaseHelper dbHelper = new DatabaseHelper();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите email и пароль.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            btnLogin.Enabled = false;

            try
            {
                var employee = dbHelper.LoginEmployee(email, password);

                if (employee != null)
                {
                    LoggedInEmployeeId = employee.Value.employeeId;
                    EmployeeRole = employee.Value.role;

                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный email или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtEmail.Focus();
                }
            }
            catch (MySqlException dbEx)
            {
                MessageBox.Show($"Ошибка при обращении к базе данных: {dbEx.Message}\nПроверьте подключение или обратитесь к администратору.", "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла непредвиденная ошибка: {ex.Message}\nПожалуйста, попробуйте позже или обратитесь к администратору.", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnLogin.Enabled = true;
            }
        }
    }
}