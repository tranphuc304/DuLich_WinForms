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
            lbl_soluong.Text = soLuong.ToString();
            lbl_tongtien.Text = UserQuery.SoTienThanhToan(maChuyenDi, ngayBatDau, soLuong).ToString();
        }
    }
}
