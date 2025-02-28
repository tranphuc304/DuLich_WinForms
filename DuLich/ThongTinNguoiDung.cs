using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuLich;
using DuLich.DatabaseUtils;

namespace admin
{
    public partial class ThongTinNguoiDung : Form
    {
        public ThongTinNguoiDung(string iD_TaiKhoan)
        {
            InitializeComponent();
            ID_TaiKhoan = iD_TaiKhoan;
        }

        string ID_TaiKhoan;

        private void ThongTinNguoiDung_Load(object sender, EventArgs e)
        {
            DataTable dt = UserQuery.getThongTinCaNhan(ID_TaiKhoan);

            this.txt_idtaikhoan.Text = ID_TaiKhoan;
            this.txt_hoten.Text = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][0].ToString();
            this.txt_cccd.Text = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][1].ToString();
            this.txt_sdt.Text = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][2].ToString();
            this.txt_diachi.Text = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][3].ToString();

            dgv_dsvecuaban.DataSource = UserQuery.LayDanhSachDangKyTheoTaiKhoan(ID_TaiKhoan);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            UserQuery.updateThongTinCaNhan(this.txt_idtaikhoan.Text, this.txt_hoten.Text, this.txt_cccd.Text, this.txt_sdt.Text, this.txt_diachi.Text);
        }

        private void lbl_dangxuat_Click(object sender, EventArgs e)
        {
            Hide();

            DuLich.Properties.Settings.Default.password = null;
            DuLich.Properties.Settings.Default.Save();

            DangNhap dangNhap = new DangNhap();
            dangNhap.ShowDialog();

            Close();
        }

        private void dgv_dsvecuaban_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            try
            {
                int currentIndex = dgv_dsvecuaban.CurrentCell.RowIndex;

                string maTaiKhoan = ID_TaiKhoan;
                string maChuyenDi = dgv_dsvecuaban.Rows[currentIndex].Cells[0].Value.ToString();
                DateTime ngayKhoiHanh = DateTime.Parse(dgv_dsvecuaban.Rows[currentIndex].Cells[1].Value.ToString());

                dgv_dsnguoithamgia.DataSource = UserQuery.LayDanhSachDuKhach(maTaiKhoan, maChuyenDi, ngayKhoiHanh);

                DataTable dtChuyenDi = UserQuery.LayChiTietChuyenDi(maChuyenDi);

                if (dtChuyenDi.Rows.Count > 0)
                {
                    DataRow row = dtChuyenDi.Rows[0]; // Lấy dòng đầu tiên

                    // Tạo StringBuilder để xây dựng chuỗi hiển thị
                    StringBuilder details = new StringBuilder();

                    // Duyệt qua từng cột
                    foreach (DataColumn column in dtChuyenDi.Columns)
                    {
                        string columnName = column.ColumnName; // Lấy tên cột
                        string cellValue = row[column]?.ToString() ?? "N/A"; // Lấy giá trị

                        // Thêm vào chuỗi chi tiết
                        details.AppendLine($"{columnName}: {cellValue}");
                    }

                    // Hiển thị trong TextBox
                    tb_thongtinchuyendi.Text = details.ToString();
                }
                else
                {
                    tb_thongtinchuyendi.Text = "Không tìm thấy thông tin chuyến đi.";
                }

            }
            catch
            {
                MessageBox.Show("Lấy danh sách du khách thất bại!");
            }
        }

        private void btn_huytour_Click(object sender, EventArgs e)
        {
            try
            {
                int currentIndex = dgv_dsvecuaban.CurrentCell.RowIndex;

                string maTaiKhoan = ID_TaiKhoan;
                string maChuyenDi = dgv_dsvecuaban.Rows[currentIndex].Cells[0].Value.ToString();
                DateTime ngayKhoiHanh = DateTime.Parse(dgv_dsvecuaban.Rows[currentIndex].Cells[1].Value.ToString());

                DialogResult traloi;
                traloi = MessageBox.Show("Bạn có chắc muốn hủy vé. \n Mã chuyến đi: " + maChuyenDi +
                    "\n Ngày khởi hành: " + ngayKhoiHanh.ToString("dd/MM/yyyy"), "Hủy vé",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (traloi == DialogResult.Yes)
                {
                    UserQuery.huyVeChuyenDi(maTaiKhoan, maChuyenDi, ngayKhoiHanh);

                    dgv_dsvecuaban.DataSource = UserQuery.LayDanhSachDangKyTheoTaiKhoan(ID_TaiKhoan);
                    dgv_dsnguoithamgia.Rows.Clear();
                    tb_thongtinchuyendi.Text = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btn_doimatkhau_Click(object sender, EventArgs e)
        {
            Hide();

            DoiMatKhau doiMatKhau = new DoiMatKhau(ID_TaiKhoan);
            doiMatKhau.ShowDialog();

            Close();
        }

        private void dgv_dsvecuaban_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            string status = dgv_dsvecuaban.Rows[e.RowIndex].Cells["Trạng Thái"].Value?.ToString();

            if (status == "Đã Thanh Toán")
            {
                dgv_dsvecuaban.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }

        private void dgv_dsvecuaban_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Utils.CheckAndColorizeDataGridView(dgv_dsvecuaban);
        }
    }
}
