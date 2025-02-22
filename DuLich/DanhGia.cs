﻿using System;
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
    public partial class DanhGia : Form
    {
        public DanhGia()
        {
            InitializeComponent();
        }

        public DanhGia(string MaChuyenDi, string MaTaiKhoan, DateTime NgayBatDau)
        {
            InitializeComponent();
            this.MaChuyenDi = MaChuyenDi;
            this.MaTaiKhoan = MaTaiKhoan;
            this.NgayBatDau = NgayBatDau;
            LoadData();
        }

        private string MaChuyenDi;
        private string MaTaiKhoan;
        private DateTime NgayBatDau;

        private void LoadData()
        {
            DataTable dt = new DataTable();
            dt = UserQuery.DanhSachDanhGia(MaChuyenDi, NgayBatDau);
            dgvCacDanhGia.DataSource = dt;
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            ChiTietChuyenDi chiTietChuyenDi = new ChiTietChuyenDi(this.MaTaiKhoan, this.MaChuyenDi, this.NgayBatDau);
            this.Hide();
            chiTietChuyenDi.ShowDialog();
            this.Close();
        }

        private void btnDanhGia_Click(object sender, EventArgs e)
        {
            int sao;
            string BinhLuan = rtbNhanXet.Text;

            try
            {
                sao = (int)nud_sosao.Value;
                if (sao < 1 || sao > 5)
                {
                    MessageBox.Show("Số sao phải từ 1 đến 5!");
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Số sao phải là số!");
                return;
            }

            if (string.IsNullOrWhiteSpace(BinhLuan))
            {
                MessageBox.Show("Vui lòng nhập nhận xét!");
                return;
            }

            try
            {
                UserQuery.ThemDanhGia(this.MaChuyenDi, this.MaTaiKhoan, BinhLuan, sao);
                MessageBox.Show("Đã gửi đánh giá thành công!");
            }
            catch
            {

                MessageBox.Show("Bạn đã đánh giá chuyến đi rồi!");
            }
        }
    }
}
