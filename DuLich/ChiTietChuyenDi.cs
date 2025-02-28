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
using DuLich.DatabaseUtils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DuLich
{
    public partial class ChiTietChuyenDi : Form
    {
        public ChiTietChuyenDi(string ID_TaiKhoan, string ID_ChuyenDi, DateTime ngayBatDau)
        {
            this.ID_TaiKhoan = ID_TaiKhoan;
            this.ID_ChuyenDi = ID_ChuyenDi;
            this.ngayBatDau = ngayBatDau;

            InitializeComponent();
        }

        string ID_TaiKhoan;
        string ID_ChuyenDi;
        DateTime ngayBatDau;

        private void ChiTietChuyenDi_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = UserQuery.LayChiTietChuyenDi(this.ID_ChuyenDi, this.ngayBatDau);

            txt_matour_info.Text = dt.Rows[0][0].ToString();
            txt_hanhtrinh_info.Text = dt.Rows[0][1].ToString();
            txt_songaydi_info.Text = dt.Rows[0][2].ToString();
            txt_soluongve_info.Text = dt.Rows[0][3].ToString();
            txt_giave_info.Text = dt.Rows[0][4].ToString();
            txt_sosao_info.Text = dt.Rows[0][5].ToString();
            txt_ngaykhoihanh_info.Text = DateTime.Parse(dt.Rows[0][6].ToString()).ToString("dd/MM/yyyy");
            string path = dt.Rows[0][7].ToString();
            string contents = File.ReadAllText(path + "ChiTiet.txt");
            txt_chitiet.Text = contents;

            this.pic_anhbia.Image = File.Exists(path + "AnhBia.jpg") ? Image.FromFile(path + "AnhBia.jpg") : null;
            this.pic_anhbia.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic_anh1.Image = File.Exists(path + "Anh1.jpg") ? Image.FromFile(path + "Anh1.jpg") : null;
            this.pic_anh1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic_anh2.Image = File.Exists(path + "Anh2.jpg") ? Image.FromFile(path + "Anh2.jpg") : null;
            this.pic_anh2.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic_anh3.Image = File.Exists(path + "Anh3.jpg") ? Image.FromFile(path + "Anh3.jpg") : null;
            this.pic_anh3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            Hide();

            TrangChu trangChu = new TrangChu(ID_TaiKhoan);
            trangChu.ShowDialog();
        }

        private void btn_danhgia_Click(object sender, EventArgs e)
        {
            Hide();

            DanhGia danhGia = new DanhGia(ID_ChuyenDi, ID_TaiKhoan, ngayBatDau);
            danhGia.ShowDialog();
        }

        private void btn_newtour_Click(object sender, EventArgs e)
        {
            Hide();

            TaoChuyenDiMoi taoChuyenDiMoi = new TaoChuyenDiMoi(ID_TaiKhoan, ID_ChuyenDi, ngayBatDau);
            taoChuyenDiMoi.ShowDialog();
        }

        private void btn_dattour_Click(object sender, EventArgs e)
        {
            if (UserQuery.isInDSDangKy(ID_TaiKhoan, ID_ChuyenDi, ngayBatDau))
            {
                MessageBox.Show("Bạn đã đặt tour này rồi, hủy tour tại Trang Chủ > Tài Khoản > Danh Sách Vé!");

                return;
            }

            Hide();

            NhapThongTinHanhKhach nhapThongTinHanhKhach = new NhapThongTinHanhKhach(ID_TaiKhoan, ID_ChuyenDi, ngayBatDau);
            nhapThongTinHanhKhach.ShowDialog();
        }

    }
}
