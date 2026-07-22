using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CarRecyclingApp
{
    public partial class ClientProfilesForm : Form
    {
        private DatabaseHelper dbHelper;

        public ClientProfilesForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            this.Load += ClientProfilesForm_Load;
            if (this.btnRefresh != null)
            {
                this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            }
        }

        private void ClientProfilesForm_Load(object sender, EventArgs e)
        {
            LoadClientData();
        }

        private void LoadClientData()
        {
            try
            {
                DataTable clients = dbHelper.GetAllClients();

                if (clients != null && clients.Rows.Count > 0)
                {
                    dataGridViewClients.DataSource = clients;

                    if (dataGridViewClients.Columns.Contains("ClientId"))
                        dataGridViewClients.Columns["ClientId"].HeaderText = "ID Клиента";
                    if (dataGridViewClients.Columns.Contains("FirstName"))
                        dataGridViewClients.Columns["FirstName"].HeaderText = "Имя";
                    if (dataGridViewClients.Columns.Contains("LastName"))
                        dataGridViewClients.Columns["LastName"].HeaderText = "Фамилия";
                    if (dataGridViewClients.Columns.Contains("Email"))
                        dataGridViewClients.Columns["Email"].HeaderText = "Email";
                    if (dataGridViewClients.Columns.Contains("PhoneNumber"))
                        dataGridViewClients.Columns["PhoneNumber"].HeaderText = "Телефон";

                    if (dataGridViewClients.Columns.Contains("PasswordHash"))
                        dataGridViewClients.Columns["PasswordHash"].Visible = false;
                }
                else if (clients != null && clients.Rows.Count == 0)
                {
                    dataGridViewClients.DataSource = null;
                    MessageBox.Show("Нет данных о клиентах для отображения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dataGridViewClients.DataSource = null;
                    MessageBox.Show("Не удалось загрузить данные о клиентах.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о клиентах: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadClientData();
        }
    }
}