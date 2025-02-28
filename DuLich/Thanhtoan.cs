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
        public ThanhToan(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau, int soLuong, DataTable dsHanhKhach)
        {
            InitializeComponent();

            this.maTaiKhoan = maTaiKhoan;
            this.maChuyenDi = maChuyenDi;
            this.ngayBatDau = ngayBatDau;
            this.soLuong = soLuong;
            this.dsHanhKhach = dsHanhKhach;
        }

        private string maTaiKhoan;
        private string maChuyenDi;
        private DateTime ngayBatDau;
        private int soLuong;
        private DataTable dsHanhKhach;

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

            foreach (DataRow row in dsHanhKhach.Rows) // Lặp qua từng dòng trong DataTable
            {
                string ten = row[0].ToString();  // Lấy dữ liệu từ cột "Ten"
                string cccd = row[1].ToString(); // Lấy dữ liệu từ cột "CCCD"
                string sdt = row[2].ToString();  // Lấy dữ liệu từ cột "SDT"

                UserQuery.ThemDuKhach(maChuyenDi, ngayBatDau, cccd, ten, sdt, maTaiKhoan);
            }

            UserQuery.InsertHoaDon(maTaiKhoan, maChuyenDi, ngayBatDau, soLuong, UserQuery.SoTienThanhToanInt(maChuyenDi, ngayBatDau, soLuong), "Chưa Thanh Toán");

            UserQuery.ThemDSDangKy(maTaiKhoan, maChuyenDi, this.ngayBatDau, dsHanhKhach.Rows.Count, UserQuery.GetIDHoaDon(maTaiKhoan, maChuyenDi, ngayBatDau));

            HoaDon hoaDon = new HoaDon(maTaiKhoan, maChuyenDi, ngayBatDau, soLuong, dsHanhKhach);
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
