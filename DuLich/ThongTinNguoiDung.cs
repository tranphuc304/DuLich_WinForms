using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuLich;
using DuLich.DatabaseUtils;

namespace admin
{
    public partial class ThongTinNguoiDung : Form
    {
        public ThongTinNguoiDung(string iD_TaiKhoan)
        {
            InitializeComponent();
            ID_TaiKhoan = iD_TaiKhoan;
        }

        string ID_TaiKhoan;

        private void ThongTinNguoiDung_Load(object sender, EventArgs e)
        {
            DataTable dt = UserQuery.getThongTinCaNhan(ID_TaiKhoan);

            this.txt_idtaikhoan.Text = ID_TaiKhoan;
            this.txt_hoten.Text = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][0].ToString();
            this.txt_cccd.Text = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][1].ToString();
            this.txt_sdt.Text = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][2].ToString();
            this.txt_diachi.Text = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][3].ToString();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            UserQuery.updateThongTinCaNhan(this.txt_idtaikhoan.Text, this.txt_hoten.Text, this.txt_cccd.Text, this.txt_sdt.Text, this.txt_diachi.Text);
        }

        private void lbl_dangxuat_Click(object sender, EventArgs e)
        {
            Hide();

            DuLich.Properties.Settings.Default.password = null;
            DuLich.Properties.Settings.Default.Save();

            DangNhap dangNhap = new DangNhap();
            dangNhap.ShowDialog();
        }
    }
}
