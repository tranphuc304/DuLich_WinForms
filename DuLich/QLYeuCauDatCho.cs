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

namespace DuLich
{
    public partial class QLYeuCauDatCho : Form
    {
        public QLYeuCauDatCho()
        {
            InitializeComponent();
        }

        string MaTaiKhoan = "";
        string MaChuyenDi = "";
        DateTime NgayBatDau = DateTime.Now;

        private void LoadData()
        {
            try
            {
                dgv_yeucau.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgv_yeucau.AllowUserToResizeColumns = false;
                dgv_yeucau.AllowUserToOrderColumns = false;
                dgv_yeucau.AllowUserToResizeRows = false;
                dgv_yeucau.DataSource = AdminQuery.GetDataRequest();
            }
            catch (Exception)
            {
                MessageBox.Show("Không lấy được dữ liệu yêu cầu của khách hàng!");
            }
            cbb_idacc.DataSource = AdminQuery.GetDataEmail();
            cbb_idacc.DisplayMember = "Email";
        }
        private void FormQuanLyYeuCau_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void Reset_Current()
        {
            MaTaiKhoan = "";
            MaChuyenDi = "";
            NgayBatDau = DateTime.Now;
        }
        private void dgv_yeucau_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dgv_yeucau.CurrentCell.RowIndex;
            if (r < dgv_yeucau.Rows.Count - 1 && r >= 0)
            {
                MaTaiKhoan = dgv_yeucau.Rows[r].Cells[0].Value.ToString();
                MaChuyenDi = dgv_yeucau.Rows[r].Cells[1].Value.ToString();
                NgayBatDau = Convert.ToDateTime(dgv_yeucau.Rows[r].Cells[2].Value);
            }
            else
            {
                Reset_Current();
            }
        }

        private void btn_nhan_Click(object sender, EventArgs e)
        {
            if (MaChuyenDi != "")
            {
                DialogResult traloi;
                traloi = MessageBox.Show("Yêu cầu tạo chuyến đi mới. \n Mã chuyến đi: " + MaChuyenDi +
                    "\n Ngày khởi hành: " + NgayBatDau + " \n Chấp nhận yêu cầu?", "Yêu cầu tạo chuyến đi",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (traloi == DialogResult.Yes)
                {
                    AdminQuery.themLichTrinh(MaChuyenDi, NgayBatDau);
                    AdminQuery.DeleteRequest(MaTaiKhoan, MaChuyenDi, NgayBatDau);
                    Reset_Current();
                    LoadData();
                }
                else if (traloi == DialogResult.No)
                {
                    MessageBox.Show("Hủy thành công.", "Thông báo");
                }
            }
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            if (MaTaiKhoan != "")
            {
                DialogResult traloi;
                traloi = MessageBox.Show("Bạn có chắc chắn xóa yêu cầu của " + MaTaiKhoan +
                    " muốn tạo chuyến đi " + MaChuyenDi + " vào ngày " + NgayBatDau + " không?", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (traloi == DialogResult.OK)
                {
                    AdminQuery.DeleteRequest(MaTaiKhoan, MaChuyenDi, NgayBatDau);
                    LoadData();
                }
            }
            else { MessageBox.Show("Bạn chưa chọn yêu cầu để xóa!"); }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {


            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("pacifictravel.dh22tin01@gmail.com", "gaam whlj edac arbu");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("pacifictravel.dh22tin01@gmail.com", "PacificTravel Support");
            mailMessage.To.Add(new MailAddress(SystemQuery.GetEmailFromID(AdminQuery.GetIDAccount(cbb_idacc.Text))));
            mailMessage.Subject = tb_tieude.Text;
            mailMessage.Body = tb_noidung.Text;

            try
            {
                smtpClient.Send(mailMessage);
                MessageBox.Show("Gửi thành công.", "Thông báo");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gửi thất bại... " + ex.Message, "Thông báo");
            }
        }
    }
}
