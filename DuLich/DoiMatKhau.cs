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
using admin;
using DuLich.DatabaseUtils;
using Guna.UI2.WinForms;

namespace DuLich
{
    public partial class DoiMatKhau : Form
    {
        public DoiMatKhau(string ID_TaiKhoan)
        {
            InitializeComponent();

            this.ID_TaiKhoan = ID_TaiKhoan;
        }

        private string ID_TaiKhoan;

        private void DoiMatKhau_Load(object sender, EventArgs e)
        {
            btn_passeyeopen_oldpass.Tag = "old";
            btn_passeyeclose_oldpass.Tag = "old";
            btn_passeyeopen_newpass.Tag = "new";
            btn_passeyeclose_newpass.Tag = "new";
        }

        private void TogglePassword(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            // Xác định TextBox cần xử lý dựa vào Tag của Button
            Guna2TextBox targetTextBox = (btn.Tag.ToString() == "old") ? txt_oldpassword : txt_newpassword;

            // Đảo trạng thái hiển thị mật khẩu
            targetTextBox.UseSystemPasswordChar = !targetTextBox.UseSystemPasswordChar;

            // Chuyển đổi hiển thị giữa hai nút eyeopen và eyeclose
            if (targetTextBox.UseSystemPasswordChar)
            {
                targetTextBox.PasswordChar = '\0';

                if (btn.Tag.ToString() == "old")
                    btn_passeyeclose_oldpass.BringToFront();
                else
                    btn_passeyeclose_newpass.BringToFront();
            }
            else
            {
                if (btn.Tag.ToString() == "old")
                    btn_passeyeopen_oldpass.BringToFront();
                else
                    btn_passeyeopen_newpass.BringToFront();
            }
        }

        private void lbl_back_Click(object sender, EventArgs e)
        {
            Hide();

            ThongTinNguoiDung thongTinNguoiDung = new ThongTinNguoiDung(ID_TaiKhoan);
            thongTinNguoiDung.ShowDialog();

            Close();
        }

        private async void btn_accept_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult xacNhan;

                xacNhan = MessageBox.Show("Bạn có chắc muốn thay đổi mật khẩu?", "Đổi mật khẩu?",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (xacNhan == DialogResult.Yes)
                {
                    string email = SystemQuery.GetEmailFromID(ID_TaiKhoan);

                    if (UserQuery.ChangePassword(email, txt_oldpassword.Text, txt_newpassword.Text))
                    {
                        DialogResult result = MessageBox.Show("Đổi mật khẩu thành công!");

                        if (result == DialogResult.OK)
                        {
                            Hide();

                            ThongTinNguoiDung thongTinNguoiDung = new ThongTinNguoiDung(ID_TaiKhoan);
                            thongTinNguoiDung.ShowDialog();

                            Close();
                        }

                        await Task.Run(() => sendMailAsync(email));
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task sendMailAsync(string email)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("pacifictravel.dh22tin01@gmail.com", "gaam whlj edac arbu");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("pacifictravel.dh22tin01@gmail.com", "PacificTravel Support");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = "Cảnh báo bảo mật";

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
                <h2 style='margin: 0;'>Tải khoản vừa được đổi mật khẩu</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 20px; text-align: left; color: #333;'>
                <p>Chào quý khách,</p>
                <p>Chúng tôi gửi email này để thông báo là bạn vừa đổi mật khẩu.</p>
                <p>Nếu bạn không phải là người thực hiện hành động này, vui lòng liên hệ lại ngay với chúng tôi qua email này!</p>
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

            mailMessage.Body = html;
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
