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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DuLich
{
    public partial class QuenMatKhau : Form
    {
        public QuenMatKhau()
        {
            InitializeComponent();
        }

        private async void btn_accept_Click(object sender, EventArgs e)
        {
            String username;

            username = txt_username.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Utils.IsValidEmail(username))
            {
                MessageBox.Show("Tên đăng nhập phải là email!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!SystemQuery.IsUsernameExists(username))
            {
                MessageBox.Show("Không tồn tại tài khoản này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newPass = Utils.GenerateSecurePassword(12);

            SystemQuery.UpdatePassword(username, newPass);
            btn_accept.Enabled = false;

            try
            {
                await Task.Run(() => sendMailAsync(username, newPass));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gửi thất bại... " + ex.Message, "Thông báo");
            } finally
            {
                btn_accept.Enabled = true;
            }
        }

        private void lbl_back_Click(object sender, EventArgs e)
        {
            this.Hide();

            DangNhap login = new DangNhap();

            login.ShowDialog();
        }

        private async Task sendMailAsync(string username, string newPassword)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("pacifictravel.dh22tin01@gmail.com", "gaam whlj edac arbu");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("pacifictravel.dh22tin01@gmail.com", "PacificTravel Support");
            mailMessage.To.Add(new MailAddress(username));
            mailMessage.Subject = "Mail lấy lại mật khẩu";

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
                <h2 style='margin: 0;'>Mật Khẩu Đã Thay Đổi</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 20px; text-align: left; color: #333;'>
                <p>Chào bạn,</p>
                <p>Chúng tôi gửi email này dưới lời đề nghị thay đổi mật khẩu của bạn.</p>
                <p>Đây là mật khẩu mới của bạn: <strong>[NEW_PASSWORD]</strong>
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

            html = html.Replace("[NEW_PASSWORD]", newPassword);

            mailMessage.Body = html;
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);

            MessageBox.Show("Mật khẩu mới đã được gửi đến email của bạn!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
