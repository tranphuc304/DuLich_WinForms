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
    public partial class HoaDon : Form
    {
        public HoaDon(string ID_TaiKhoan, string maChuyenDi, DateTime ngayBatDau, int soLuong, DataTable dsHanhKhach)
        {
            InitializeComponent();
            this.ID_TaiKhoan = ID_TaiKhoan;
            this.maChuyenDi = maChuyenDi;
            this.ngayBatDau = ngayBatDau;
            this.soLuong = soLuong;
            this.dsHanhKhach = dsHanhKhach;
        }

        string ID_TaiKhoan;
        private string maChuyenDi;
        private DateTime ngayBatDau;
        private int soLuong;
        private DataTable dsHanhKhach;

        private void btn_back_Click(object sender, EventArgs e)
        {
            Hide();

            TrangChu trangchu = new TrangChu(ID_TaiKhoan);
            trangchu.ShowDialog();
        }

        private async void HoaDon_Load(object sender, EventArgs e)
        {
            lbl_matour.Text = maChuyenDi;
            lbl_ngaybatdau.Text = ngayBatDau.ToString("dd/MM/yyyy");
            lbl_soluong.Text = soLuong.ToString();
            lbl_tongtien.Text = UserQuery.SoTienThanhToan(maChuyenDi, ngayBatDau, soLuong).ToString();

            string email = SystemQuery.GetEmailFromID(ID_TaiKhoan);
            await Task.Run(() => sendMailAsync(email));
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
            mailMessage.Subject = "Bạn có hóa đơn cần thanh toán";

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
                <h2 style='margin: 0;'>Hóa Đơn Chuyến Đi</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 20px; text-align: left; color: #333;'>
                <p>Chào quý khách,</p>
                <p>Chúng tôi gửi email này để thông báo bạn vừa đặt chuyến đi.</p>
                <p>Email thanh toán sẽ được gửi sau khi bạn thanh toán thành công.</p>
                <p></p>
                <p>Thông Tin:</p>
                <p>Mã Tour: <strong>[TOURID]</strong></p>
                <p>Tên Tour: <strong>[TOURNAME]</strong></p>
                <p>Ngày Bắt Đầu: <strong>[STARTDAY]</strong></p>
                <p>Số người tham gia: <strong>[AMOUNT]</strong></p>
                <p>Tổng tiền: <strong>[MONEY]</strong></p>
                <p></p>
                <p><strong>Danh sách người tham gia:</strong></p>
                [DSNguoiThamGia]
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
            html = html.Replace("[TOURNAME]", UserQuery.LayChiTietChuyenDi(maChuyenDi, ngayBatDau).Rows[0][0].ToString());
            html = html.Replace("[STARTDAY]", ngayBatDau.ToString("dd/MM/yyyy"));
            html = html.Replace("[AMOUNT]", soLuong.ToString());
            html = html.Replace("[MONEY]", UserQuery.SoTienThanhToan(maChuyenDi, ngayBatDau, soLuong).ToString());
            html = html.Replace("[DSNguoiThamGia]", ConvertDataTableToHTML(dsHanhKhach));

            mailMessage.Body = html;
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }

        public string ConvertDataTableToHTML(DataTable dt)
        {
            StringBuilder html = new StringBuilder();

            // CSS cho bảng
            html.Append(@"<style>
        table { border-collapse: collapse; width: 100%; font-family: Arial, sans-serif; }
        th, td { border: 1px solid #ddd; padding: 8px; }
        th { background-color: #E67E22; color: white; text-align: center; }
        tr:nth-child(even) { background-color: #f2f2f2; }
        tr:hover { background-color: #ddd; }
        td { text-align: left; }
    </style>");

            html.Append("<table>");

            // Thêm tiêu đề cột
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>" + column.ColumnName + "</th>");
            }
            html.Append("</tr>");

            // Thêm dữ liệu
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>" + row[column].ToString() + "</td>");
                }
                html.Append("</tr>");
            }

            html.Append("</table>");
            return html.ToString();
        }
    }
}
