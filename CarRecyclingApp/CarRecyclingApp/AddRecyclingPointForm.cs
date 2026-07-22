using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CarRecyclingApp
{
    public partial class AddRecyclingPointForm : Form
    {
        private int? pointId = null;
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public AddRecyclingPointForm()
        {
            InitializeComponent();
        }

        // Режим редактирования
        public AddRecyclingPointForm(int pointId, string name, string address, string phone)
        {
            InitializeComponent();
            this.pointId = pointId;
            txtName.Text = name;
            txtAddress.Text = address;
            txtPhone.Text = phone;
            this.Text = "Редактирование пункта";
            btnSave.Text = "Сохранить изменения";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string address = txtAddress.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            bool success;

            if (pointId.HasValue)
            {
                // Редактирование
                success = dbHelper.UpdateRecyclingPoint(pointId.Value, name, address, phone);
            }
            else
            {
                // Добавление
                success = dbHelper.AddRecyclingPoint(name, address, phone);
            }

            if (success)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Удалить этот пункт утилизации?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            if (dbHelper.DeleteRecyclingPoint(pointId.Value))
            {
                MessageBox.Show("Пункт удалён.");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка при удалении.");
            }
        }

    }

}
