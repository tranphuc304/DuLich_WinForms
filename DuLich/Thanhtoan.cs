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

namespace DuLich
{
    public partial class ThanhToan : Form
    {
        public ThanhToan(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau, int soLuong)
        {
            InitializeComponent();

            this.maTaiKhoan = maTaiKhoan;
            this.maChuyenDi = maChuyenDi;
            this.ngayBatDau = ngayBatDau;
            this.soLuong = soLuong;
        }

        private string maTaiKhoan;
        private string maChuyenDi;
        private DateTime ngayBatDau;
        private int soLuong;

        private void ThanhToan_Load(object sender, EventArgs e)
        {
            lbl_matour.Text = maChuyenDi;
            lbl_ngaybatdau.Text = ngayBatDau.ToString("dd/MM/yyyy");
            lbl_soluong.Text = soLuong.ToString();
            lbl_tongtien.Text = UserQuery.SoTienThanhToan(maChuyenDi, ngayBatDau, soLuong).ToString();
        }

        private void btn_xacnhan_Click(object sender, EventArgs e)
        {
            Hide();

            UserQuery.CapNhatTrangThaiDangKy(maTaiKhoan, maChuyenDi, ngayBatDau, "Đã Thanh Toán");

            HoaDon hoaDon = new HoaDon(maTaiKhoan, maChuyenDi, ngayBatDau, soLuong);
            hoaDon.ShowDialog();

            Close();
        }

        private void lbl_back_Click(object sender, EventArgs e)
        {
            ChiTietChuyenDi chiTietChuyenDi = new ChiTietChuyenDi(this.maTaiKhoan, this.maChuyenDi, this.ngayBatDau);
            this.Hide();
            chiTietChuyenDi.ShowDialog();
            this.Close();
        }
    }
}
