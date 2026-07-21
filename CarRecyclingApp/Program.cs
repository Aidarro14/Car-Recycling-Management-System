using System;
using System.Windows.Forms;

namespace CarRecyclingApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                var loginForm = new LoginForm();
                var loginResult = loginForm.ShowDialog();

                if (loginResult != DialogResult.OK)
                    break;

                int userId = loginForm.LoggedInEmployeeId;
                string role = loginForm.EmployeeRole;

                Form mainForm;

                if (role == "admin")
                    mainForm = new AdminForm(userId, role);
                else
                    mainForm = new WorkerForm(userId, role);

                Application.Run(mainForm);

                // Если форма закрылась, продолжаем цикл (возврат к логину)
            }
        }
    }
}
