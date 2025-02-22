using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuLich.DatabaseUtils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DuLich
{
    public partial class NhapThongTinHanhKhach : Form
    {
        public NhapThongTinHanhKhach(string ID_TaiKhoan, string ID_ChuyenDi, DateTime ngayBatDau)
        {
            this.ID_ChuyenDi = ID_TaiKhoan;
            this.ID_ChuyenDi = ID_ChuyenDi;
            this.ngayBatDau = ngayBatDau;

            InitializeComponent();
        }

        string ID_TaiKhoan;
        string ID_ChuyenDi;
        DateTime ngayBatDau;

        string edit = null;

        private void btn_back_Click(object sender, EventArgs e)
        {
            ChiTietChuyenDi chiTietChuyenDi = new ChiTietChuyenDi(ID_TaiKhoan, ID_ChuyenDi, ngayBatDau);
            Hide();
            chiTietChuyenDi.ShowDialog();
            Close();
        }

        private void btn_thanhtoan_Click(object sender, EventArgs e)
        {
            int count = dgv_dshanhkhach.Rows.Count;

            if (count == 0)
            {
                MessageBox.Show("Bạn chưa nhập hành khách nào!");

                return;
            }

            foreach (DataGridViewRow row in dgv_dshanhkhach.Rows)
            {
                if (!row.IsNewRow)
                {
                    string ten = row.Cells["HoTen"].Value.ToString();
                    string cccd = row.Cells["cccd"].Value.ToString();
                    string sdt = row.Cells["SDT"].Value.ToString();

                    UserQuery.ThemDuKhachDK(ID_TaiKhoan, this.ngayBatDau, cccd, ten, sdt);
                }
            }
            try
            {
                UserQuery.ThemDanhSachDK(ID_TaiKhoan, ID_TaiKhoan, this.ngayBatDau, dgv_dshanhkhach.Rows.Count, "Chưa thanh toán");
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn đã đặt chuyến đi này rồi");
                return;
            }

            ThanhToan thanhToan = new ThanhToan(ID_TaiKhoan, ID_TaiKhoan, this.ngayBatDau, count);
            Hide();
            thanhToan.ShowDialog();
            Close();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_sdt.ResetText();
            txt_hoten.ResetText();
            txt_cccd.ResetText();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (edit != null)
            {

            }
        }
    }
}
