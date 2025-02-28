using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuLich.DatabaseUtils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DuLich
{
    public partial class QLLichTrinh : Form
    {
        public QLLichTrinh()
        {
            InitializeComponent();
        }

        private void QLLichTrinh_Load(object sender, EventArgs e)
        {
            loadLichTrinh(DateTime.Now.Date, DateTime.Now.Date.AddMonths(1));

            this.cbb_matour_tlt.DataSource = AdminQuery.getDSMaChuyenDi();
            this.cbb_matour_tlt.DisplayMember = "Mã Tour";
            this.cbb_matour_tcd.DataSource = AdminQuery.getDSMaChuyenDi();
            this.cbb_matour_tcd.DisplayMember = "Mã Tour";
            this.cbb_mahdv_tcd.DataSource = AdminQuery.getDSHDV();
            this.cbb_mahdv_tcd.DisplayMember = "Mã Hướng Dẫn Viên";
        }

        private void loadLichTrinh(DateTime from, DateTime to)
        {
            try
            {
                dgv_lichtrinh.DataSource = AdminQuery.getLichTrinh(from, to);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void locLichTrinh(string idTour, string idGuide, int quantity, bool not_eligible)
        {
            try
            {
                dgv_lichtrinh.DataSource = AdminQuery.locLichTrinh(idTour, idGuide, quantity, not_eligible);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_timkiem_tlv_Click(object sender, EventArgs e)
        {
            loadLichTrinh(this.cal_tuanlamviec.SelectionRange.Start, this.cal_tuanlamviec.SelectionRange.End);
        }

        private void btn_themlichtrinh_Click(object sender, EventArgs e)
        {
            string maTour = this.cbb_matour_tlt.Text;
            DateTime ngayKhoiHanh = this.dtp_ngaykhoihanh_tlt.Value;

            // Kiểm tra nếu ngày khởi hành nhỏ hơn ngày hiện tại
            if (ngayKhoiHanh < DateTime.Now.Date)
            {
                MessageBox.Show("Ngày khởi hành không thể nhỏ hơn ngày hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng thực hiện nếu điều kiện không hợp lệ
            }

            // Thêm lịch trình nếu hợp lệ
            AdminQuery.themLichTrinh(maTour, ngayKhoiHanh);
            loadLichTrinh(DateTime.Now.Date, DateTime.Now.Date.AddMonths(1));
        }

        private void btn_timchuyendi_Click(object sender, EventArgs e)
        {
            string maTour = this.cbb_matour_tcd.Text;
            string maHDV = this.cbb_mahdv_tcd.Text;
            int quantity = Convert.ToInt32(this.nud_sokhachtoida.Value);
            bool not_eligible = this.cb_noteligible.Checked;

            locLichTrinh(maTour, maHDV, quantity, not_eligible);
        }

        private void btn_huychuyen_Click(object sender, EventArgs e)
        {
            int currentIndex = this.dgv_lichtrinh.CurrentCell.RowIndex;
            string IDTour = dgv_lichtrinh.Rows[currentIndex].Cells[0].Value.ToString();
            DateTime StartDay = DateTime.Parse(dgv_lichtrinh.Rows[currentIndex].Cells[1].Value.ToString());
            AdminQuery.huyLichTrinh(IDTour, StartDay);
            loadLichTrinh(DateTime.Now.Date, DateTime.Now.Date.AddMonths(1));

            MessageBox.Show("Đã hủy chuyến đi và thông báo cho các hành khách!");
        }

        private void dgv_lichtrinh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentIndex = this.dgv_lichtrinh.CurrentCell.RowIndex;

            this.txt_matour_info.Text = dgv_lichtrinh.Rows[currentIndex].Cells[0].Value.ToString();
            this.dtp_ngaykhoihanh_info.Value = DateTime.Parse(dgv_lichtrinh.Rows[currentIndex].Cells[1].Value.ToString());
            this.txt_tentour_info.Text = AdminQuery.getTourNameByID(this.txt_matour_info.Text);
            this.txt_soluongkhach_info.Text = dgv_lichtrinh.Rows[currentIndex].Cells[4].Value.ToString();

            btn_huychuyen.Enabled = true;
        }
    }
}
