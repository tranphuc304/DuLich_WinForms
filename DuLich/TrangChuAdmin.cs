using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using admin;

namespace DuLich
{
    public partial class TrangChuAdmin : Form
    {
        public TrangChuAdmin()
        {
            InitializeComponent();
        }

        private void lbl_tour_Click(object sender, EventArgs e)
        {
            Hide();

            QLTour qLTour = new QLTour();
            qLTour.ShowDialog();

            Show();
        }

        private void lbl_lichtour_Click(object sender, EventArgs e)
        {
            Hide();

            QLLichTrinh qLLichTrinh = new QLLichTrinh();
            qLLichTrinh.ShowDialog();

            Show();
        }

        private void lbl_ve_Click(object sender, EventArgs e)
        {
            Hide();

            QLVe qLVe = new QLVe();
            qLVe.ShowDialog();

            Show();
        }

        private void lbl_khachhang_Click(object sender, EventArgs e)
        {
            Hide();

            QLKhachHang qLKhachHang = new QLKhachHang();
            qLKhachHang.ShowDialog();

            Show();
        }

        private void lbl_huongdanvien_Click(object sender, EventArgs e)
        {
            Hide();

            QLHuongDanVien qLHuongDanVien = new QLHuongDanVien();
            qLHuongDanVien.ShowDialog();

            Show();
        }

        private void lbl_danhgia_Click(object sender, EventArgs e)
        {
            Hide();

            QLDanhGia qLDanhGia = new QLDanhGia();
            qLDanhGia.ShowDialog();

            Show();
        }
    }
}
