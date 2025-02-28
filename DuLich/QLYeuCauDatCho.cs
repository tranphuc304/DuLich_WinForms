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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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
        int soLuong = 0;

        private void LoadData()
        {
            try
            {
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
            soLuong = 0;
        }
        private void dgv_yeucau_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dgv_yeucau.CurrentCell.RowIndex;
            if (r < dgv_yeucau.Rows.Count - 1 && r >= 0)
            {
                MaTaiKhoan = dgv_yeucau.Rows[r].Cells[0].Value.ToString();
                MaChuyenDi = dgv_yeucau.Rows[r].Cells[1].Value.ToString();
                NgayBatDau = Convert.ToDateTime(dgv_yeucau.Rows[r].Cells[2].Value);
                soLuong = int.Parse(dgv_yeucau.Rows[r].Cells[3].Value.ToString());
            }
            else
            {
                Reset_Current();
            }
        }

        private async void btn_nhan_Click(object sender, EventArgs e)
        {
            if (MaChuyenDi != "")
            {
                DialogResult traloi;
                traloi = MessageBox.Show("Yêu cầu tạo chuyến đi mới. \n Mã chuyến đi: " + MaChuyenDi +
                    "\n Ngày khởi hành: " + NgayBatDau + " \n Chấp nhận yêu cầu?", "Yêu cầu tạo chuyến đi",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (traloi == DialogResult.Yes)
                {
                    string email = SystemQuery.GetEmailFromID(MaTaiKhoan);

                    MessageBox.Show("Đang Gửi Mail...");

                    AdminQuery.themLichTrinh(MaChuyenDi, NgayBatDau);
                    AdminQuery.DeleteRequest(MaTaiKhoan, MaChuyenDi, NgayBatDau);
                    Reset_Current();
                    LoadData();

                    await Task.Run(() => sendMailAcceptYeuCau(email, MaChuyenDi, NgayBatDau, soLuong));

                    MessageBox.Show("Gửi thành công.", "Thông báo");
                }
                else if (traloi == DialogResult.No)
                {
                    MessageBox.Show("Hủy thành công.", "Thông báo");
                }
            }
        }

        private async void btn_huy_Click(object sender, EventArgs e)
        {
            if (MaTaiKhoan != "")
            {
                DialogResult traloi;
                traloi = MessageBox.Show("Bạn có chắc chắn xóa yêu cầu của " + MaTaiKhoan +
                    " muốn tạo chuyến đi " + MaChuyenDi + " vào ngày " + NgayBatDau + " không?", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (traloi == DialogResult.OK)
                {
                    string email = SystemQuery.GetEmailFromID(MaTaiKhoan);

                    MessageBox.Show("Đang Gửi Mail...");

                    AdminQuery.DeleteRequest(MaTaiKhoan, MaChuyenDi, NgayBatDau);
                    LoadData();

                    await Task.Run(() => sendMailDenyYeuCau(email, MaChuyenDi, NgayBatDau, soLuong));

                    MessageBox.Show("Gửi thành công.", "Thông báo");
                }
            }
            else { MessageBox.Show("Bạn chưa chọn yêu cầu để xóa!"); }
        }

        private async void btn_send_Click(object sender, EventArgs e)
        {
            btn_send.Enabled = false;

            string email = cbb_idacc.Text;

            MessageBox.Show("Đang Gửi Mail...");

            await Task.Run(() => sendMailContent(email));
        }

        private async void sendMailContent(string email)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("pacifictravel.dh22tin01@gmail.com", "gaam whlj edac arbu");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("pacifictravel.dh22tin01@gmail.com", "PacificTravel Support");
            mailMessage.To.Add(new MailAddress(SystemQuery.GetEmailFromID(AdminQuery.GetIDAccount(email))));

            mailMessage.Subject = tb_tieude.Text;

            string html = @"<!DOCTYPE html>
<html>
  <head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reset Your Password</title>
  </head>
  <body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <table width='100%' border='0' cellpadding='0' cellspacing='0'>
      <tr>
        <td align='center' style='padding: 20px 0;'>
          <table width='600px' style='background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0px 2px 6px rgba(0,0,0,0.1);'>
            <tr>
              <td align='center' style='padding: 10px;'>
                <img src='https://i.ibb.co/wZWTyYQY/Logo.png' alt='PacificTravel' style='max-width: 150px;'>
              </td>
            </tr>
            <tr>
              <td align='center' style='background-color: #1a1a40; color: white; padding: 20px; border-radius: 8px 8px 0 0;'>
                <h2 style='margin: 0;'>Thông Báo</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 20px; text-align: left; color: #333;'>
                <p>Chào quý khách,</p>
                <p>[TEXT]</p>
                </p>
              </td>
            </tr>
            <tr>
              <td align='center' style='padding: 10px; font-size: 12px; color: #888;'> &copy; 2025 PacificTravel. All rights reserved. </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>";

            html = html.Replace("[TEXT]", tb_noidung.Text);

            mailMessage.Body = html;
            mailMessage.IsBodyHtml = true;

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                MessageBox.Show("Gửi thành công.", "Thông báo");

                btn_send.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gửi thất bại... " + ex.Message, "Thông báo");
            }
        }

        private async Task sendMailAcceptYeuCau(string email, string maChuyenDi, DateTime ngayBatDau, int soLuong)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("pacifictravel.dh22tin01@gmail.com", "gaam whlj edac arbu");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("pacifictravel.dh22tin01@gmail.com", "PacificTravel Support");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = "Yêu cầu cho chuyến đi mới";

            string html = @"<!DOCTYPE html>
<html>
  <head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reset Your Password</title>
  </head>
  <body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <table width='100%' border='0' cellpadding='0' cellspacing='0'>
      <tr>
        <td align='center' style='padding: 20px 0;'>
          <table width='600px' style='background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0px 2px 6px rgba(0,0,0,0.1);'>
            <tr>
              <td align='center' style='padding: 10px;'>
                <img src='https://i.ibb.co/wZWTyYQY/Logo.png' alt='PacificTravel' style='max-width: 150px;'>
              </td>
            </tr>
            <tr>
              <td align='center' style='background-color: #1a1a40; color: white; padding: 20px; border-radius: 8px 8px 0 0;'>
                <h2 style='margin: 0;'>Yêu Cầu Đã Được Chấp Nhận</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 20px; text-align: left; color: #333;'>
                <p>Chào quý khách,</p>
                <p>Chúng tôi gửi email này để thông báo yêu cầu chuyến đi của bạn đã được chấp nhận.</p>
                <p></p>
                <p>Thông Tin Chuyến Đi Được Yêu Cầu:</p>
                <p>Mã Tour: <strong>[TOURID]</strong></p>
                <p>Tên Tour: <strong>[TOURNAME]</strong></p>
                <p>Ngày Bắt Đầu: <strong>[STARTDAY]</strong></p>
                <p>Số người tham gia: <strong>[AMOUNT]</strong></p>
                <p></p>
                <p></p>
                <p>Xin cảm ơn vì đã dùng dịch vụ tại PacificTravel!</p>
                </p>
              </td>
            </tr>
            <tr>
              <td align='center' style='padding: 10px; font-size: 12px; color: #888;'> &copy; 2025 PacificTravel. All rights reserved. </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>";

            html = html.Replace("[TOURID]", maChuyenDi);
            html = html.Replace("[TOURNAME]", AdminQuery.GetTenChuyenDi(maChuyenDi));
            html = html.Replace("[STARTDAY]", ngayBatDau.ToString("dd/MM/yyyy"));
            html = html.Replace("[AMOUNT]", soLuong.ToString());

            mailMessage.Body = html;
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }

        private async Task sendMailDenyYeuCau(string email, string maChuyenDi, DateTime ngayBatDau, int soLuong)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("pacifictravel.dh22tin01@gmail.com", "gaam whlj edac arbu");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("pacifictravel.dh22tin01@gmail.com", "PacificTravel Support");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = "Yêu cầu cho chuyến đi mới";

            string html = @"<!DOCTYPE html>
<html>
  <head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reset Your Password</title>
  </head>
  <body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <table width='100%' border='0' cellpadding='0' cellspacing='0'>
      <tr>
        <td align='center' style='padding: 20px 0;'>
          <table width='600px' style='background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0px 2px 6px rgba(0,0,0,0.1);'>
            <tr>
              <td align='center' style='padding: 10px;'>
                <img src='https://i.ibb.co/wZWTyYQY/Logo.png' alt='PacificTravel' style='max-width: 150px;'>
              </td>
            </tr>
            <tr>
              <td align='center' style='background-color: #1a1a40; color: white; padding: 20px; border-radius: 8px 8px 0 0;'>
                <h2 style='margin: 0;'>Yêu Cầu Đã Bị Từ Chối</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 20px; text-align: left; color: #333;'>
                <p>Chào quý khách,</p>
                <p>Chúng tôi gửi email này để thông báo yêu cầu chuyến đi của bạn đã bị từ chối.</p>
                <p></p>
                <p>Thông Tin Chuyến Đi Được Yêu Cầu:</p>
                <p>Mã Tour: <strong>[TOURID]</strong></p>
                <p>Tên Tour: <strong>[TOURNAME]</strong></p>
                <p>Ngày Bắt Đầu: <strong>[STARTDAY]</strong></p>
                <p>Số người tham gia: <strong>[AMOUNT]</strong></p>
                <p></p>
                <p></p>
                <p>Xin cảm ơn vì đã dùng dịch vụ tại PacificTravel!</p>
                </p>
              </td>
            </tr>
            <tr>
              <td align='center' style='padding: 10px; font-size: 12px; color: #888;'> &copy; 2025 PacificTravel. All rights reserved. </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>";

            html = html.Replace("[TOURID]", maChuyenDi);
            html = html.Replace("[TOURNAME]", AdminQuery.GetTenChuyenDi(maChuyenDi));
            html = html.Replace("[STARTDAY]", ngayBatDau.ToString("dd/MM/yyyy"));
            html = html.Replace("[AMOUNT]", soLuong.ToString());

            mailMessage.Body = html;
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
