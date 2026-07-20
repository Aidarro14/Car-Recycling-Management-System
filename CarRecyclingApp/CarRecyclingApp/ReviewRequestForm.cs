using System;
using System.Windows.Forms;

namespace CarRecyclingApp
{
    public partial class ReviewRequestForm : Form
    {
        public int RequestId { get; set; }
        public string Comment => txtComment.Text;
        public bool IsApproved { get; private set; }

        public ReviewRequestForm(int requestId, string requestInfo)
        {
            InitializeComponent();
            RequestId = requestId;
            lblRequestInfo.Text = requestInfo;
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            IsApproved = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtComment.Text))
            {
                MessageBox.Show("Пожалуйста, укажите причину возврата на доработку",
                              "Требуется комментарий",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            IsApproved = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}