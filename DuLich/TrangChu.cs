using admin;
using DuLich.DatabaseUtils;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DuLich
{
    public partial class TrangChu : Form
    {
        private Dictionary<Guna.UI2.WinForms.Guna2Button, bool> buttonStates = new Dictionary<Guna.UI2.WinForms.Guna2Button, bool>();

        public TrangChu(string ID_TaiKhoan)
        {
            this.ID_TaiKhoan = ID_TaiKhoan;

            InitializeComponent();
        }

        string ID_TaiKhoan = null;

        private void TrangChu_Load(object sender, EventArgs e)
        {
            loadDSChuyenDi(UserQuery.getDSChuyenDi());
        }

        private void loadDSChuyenDi(DataTable source)
        {
            try
            {
                dgv_dschuyendi.DataSource = source;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_timtour_Click(object sender, EventArgs e)
        {
            string destination = txt_diemden.Text;
            DateTime startDay = (cb_ngaykhoihanh.Checked) ? dtp_ngaykhoihanh.Value : new DateTime(1000, 1, 1);
            string price = (cbb_khoanggia.Text == String.Empty) ? "0" : cbb_khoanggia.Text;
            int rate = Convert.ToInt32(nud_sosao.Value);
            loadDSChuyenDi(UserQuery.timTour(destination, startDay, price, rate));
        }

        private void btn_xemchitiet_Click(object sender, EventArgs e)
        {
            int r = this.dgv_dschuyendi.CurrentCell.RowIndex;

            Console.WriteLine(r);

            ChiTietChuyenDi chiTietChuyenDi = new ChiTietChuyenDi(this.ID_TaiKhoan, dgv_dschuyendi.Rows[r].Cells[0].Value.ToString(), DateTime.Parse(dgv_dschuyendi.Rows[r].Cells[3].Value.ToString()));
            
            Hide();
            chiTietChuyenDi.ShowDialog();
            Show();
        }

        private void pic_account_Click(object sender, EventArgs e)
        {
            Hide();

            ThongTinNguoiDung thongTinNguoiDung = new ThongTinNguoiDung(ID_TaiKhoan);
            thongTinNguoiDung.ShowDialog();

            Show();
        }
    }
}
