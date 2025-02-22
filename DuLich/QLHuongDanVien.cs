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

namespace admin
{
    public partial class QLHuongDanVien : Form
    {
        public QLHuongDanVien()
        {
            InitializeComponent();
        }


        private void LoadDataGridView()
        {
            try
            {
                dgv_dshdv.DataSource = AdminQuery.getDSHDV();

                dgv_phanconghdv.DataSource = AdminQuery.getDSTourToPhanCong();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void QLHuongDanVien_Load(object sender, EventArgs e)
        {
            loadEverything();
        }

        private void loadEverything()
        {
            LoadDataGridView();

            this.cbb_mahdv_lthd.DataSource = AdminQuery.getDSMaHDV();
            this.cbb_mahdv_lthd.DisplayMember = "Mã Hướng dẫn viên";
            this.cbb_mahdv_pchdv.DataSource = AdminQuery.getDSMaHDV();
            this.cbb_mahdv_pchdv.DisplayMember = "Mã Hướng dẫn viên";
        }

        private void emptyInfoHDV()
        {
            this.txt_mahdv_info.Text = string.Empty;
            this.txt_tenhdv_info.Text = string.Empty;
            this.txt_sdt_info.Text = string.Empty;
            this.txt_email_info.Text = string.Empty;
        }

        private void dgv_dshdv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentIndex = dgv_dshdv.CurrentCell.RowIndex;

            this.txt_mahdv_info.Text = dgv_dshdv.Rows[currentIndex].Cells[0].Value.ToString();
            this.txt_tenhdv_info.Text = dgv_dshdv.Rows[currentIndex].Cells[1].Value.ToString();
            this.txt_sdt_info.Text = dgv_dshdv.Rows[currentIndex].Cells[2].Value.ToString();
            this.txt_email_info.Text = dgv_dshdv.Rows[currentIndex].Cells[3].Value.ToString();

            this.txt_mahdv_info.Enabled = false;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            emptyInfoHDV();

            this.txt_mahdv_info.Enabled = true;
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            if (AdminQuery.isHDVExist(this.txt_mahdv_info.Text) && this.txt_mahdv_info.Enabled)
            {
                MessageBox.Show("Mã HDV đã tồn tại! Hãy đổi mã HDV khác!");

                return;
            }

            AdminQuery.SaveOrUpdateHDV(this.txt_mahdv_info.Text, this.txt_tenhdv_info.Text, this.txt_sdt_info.Text, this.txt_email_info.Text);

            MessageBox.Show("Lưu thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadEverything();

            emptyInfoHDV();

            this.txt_mahdv_info.Enabled = true;
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc chắn xóa?", "Trả lời",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                AdminQuery.xoaHDV(this.txt_mahdv_info.Text);
                LoadDataGridView();
            }
        }

        private void btn_xemlichtrinh_Click(object sender, EventArgs e)
        {
            mc_lichtrinh.RemoveAllBoldedDates();
            Highlight();
            mc_lichtrinh.UpdateBoldedDates();
        }

        private void Highlight()
        {
            DataTable lt = new DataTable();
            string IDGuide_1 = this.cbb_mahdv_lthd.Text;
            lt = AdminQuery.LichTrinhHD(IDGuide_1);

            foreach (DataRow row in lt.Rows)
            {
                DateTime rowDateTime = Convert.ToDateTime(row["NgayBatDau"]);
                if (rowDateTime != null) // So sánh theo ngày (bỏ qua phần thời gian)
                {
                    mc_lichtrinh.AddBoldedDate(rowDateTime.Date);
                }
            }

        }

        private void btn_phancong_Click(object sender, EventArgs e)
        {
            int currentIndex = this.dgv_phanconghdv.CurrentCell.RowIndex;
            string IDTour = dgv_phanconghdv.Rows[currentIndex].Cells[0].Value.ToString();
            DateTime StartDay = DateTime.Parse(dgv_phanconghdv.Rows[currentIndex].Cells[1].Value.ToString());
            string IDGuide = cbb_mahdv_pchdv.Text;
            try
            {
                AdminQuery.capNhatLichTrinh(IDTour, StartDay, IDGuide);
                MessageBox.Show("Phân công thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_huyphancong_Click(object sender, EventArgs e)
        {
            int n = this.dgv_phanconghdv.CurrentCell.RowIndex;
            string IDTour = dgv_phanconghdv.Rows[n].Cells[0].Value.ToString();
            DateTime StartDay = DateTime.Parse(dgv_phanconghdv.Rows[n].Cells[1].Value.ToString());
            string IDGuide = cbb_mahdv_pchdv.Text;
            try
            {
                AdminQuery.huyPhanCong(IDTour, StartDay);
                MessageBox.Show("Hủy phân công thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
