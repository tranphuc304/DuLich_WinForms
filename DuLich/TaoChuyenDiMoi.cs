using System;
using System.Windows.Forms;
using DuLich.DatabaseUtils;

namespace DuLich
{
    public partial class TaoChuyenDiMoi : Form
    {
        public TaoChuyenDiMoi(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau)
        {
            InitializeComponent();
            this.MaTaiKhoan = maTaiKhoan;
            this.MaChuyenDi = maChuyenDi;
            this.NgayBatDau = ngayBatDau;
        }

        private string MaTaiKhoan;
        private DateTime NgayBatDau;
        private string MaChuyenDi;

        private void btn_sendrequest_Click(object sender, EventArgs e)
        {
            try
            {
                if (nud_soluong.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập số lượng người tham gia!");
                    return;
                }
                if (int.Parse(nud_soluong.Text) < 2)
                {
                    MessageBox.Show("Số lượng người tham gia phải lớn hơn 1!");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Số lượng người tham gia phải là số!");
                return;
            }

            try
            {
                DateTime ngayBatDau = dtp_ngaykhoihanh.Value;
                int soLuong = int.Parse(nud_soluong.Text);
                UserQuery.ThemDanhSachDKy(this.MaTaiKhoan, this.MaChuyenDi, ngayBatDau, soLuong, "ChuaDuyet");
                MessageBox.Show("Đã gửi yêu cầu thành công!");
            }
            catch (Exception)
            {
                MessageBox.Show("Đã gửi yêu cẩu rồi!");
            }


            ChiTietChuyenDi chiTietChuyenDi = new ChiTietChuyenDi(this.MaTaiKhoan, this.MaChuyenDi, this.NgayBatDau);
            this.Hide();
            chiTietChuyenDi.ShowDialog();
            this.Close();
        }

        private void lbl_back_Click(object sender, EventArgs e)
        {
            ChiTietChuyenDi chiTietChuyenDi = new ChiTietChuyenDi(this.MaTaiKhoan, this.MaChuyenDi, this.NgayBatDau);
            this.Hide();
            chiTietChuyenDi.ShowDialog();
            this.Close();
        }
    }
}
