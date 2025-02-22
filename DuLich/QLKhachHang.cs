using System;
using System.Windows.Forms;
using admin;

namespace DuLich
{
    public partial class QLKhachHang : Form
    {
        public QLKhachHang()
        {
            InitializeComponent();
        }

        private void btn_qltaikhoan_Click(object sender, EventArgs e)
        {
            Hide();

            QLTaiKhoan qLTaiKhoan = new QLTaiKhoan();
            qLTaiKhoan.ShowDialog();

            Show();
        }

        private void lbl_qlyeucau_Click(object sender, EventArgs e)
        {
            Hide();

            QLYeuCauDatCho qLYeuCauDatCho = new QLYeuCauDatCho();
            qLYeuCauDatCho.ShowDialog();

            Show();
        }
    }
}
