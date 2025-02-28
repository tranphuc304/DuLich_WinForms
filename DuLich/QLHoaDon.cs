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
using admin;
using System.Drawing.Printing;

namespace DuLich
{
    public partial class QLHoaDon : Form
    {
        public QLHoaDon()
        {
            InitializeComponent();

            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
        }

        private Font fontTitle = new Font("Arial", 14, FontStyle.Bold);
        private Font fontHeader = new Font("Arial", 10, FontStyle.Bold);
        private Font fontContent = new Font("Arial", 10, FontStyle.Regular);
        private Image logo = Properties.Resources.Logo;
        PrintDocument printDocument = new PrintDocument();
        PrintPreviewDialog printReviewDialog = new PrintPreviewDialog();

        private void QLHoaDon_Load(object sender, EventArgs e)
        {
            try
            {
                dgv_qlhoadon.DataSource = AdminQuery.LayDanhSachHoaDon();
            } catch {
                MessageBox.Show("Lấy danh sách hóa đơn thất bại!");
            }
        }

        private void btn_timhoadon_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? ngayKhoiHanh = cb_ngaykhoihanh.Checked ? dtp_ngaykhoihanh.Value : (DateTime?)null;

                dgv_qlhoadon.DataSource = AdminQuery.TimHoaDon(tb_mahoadon.Text, tb_mataikhoan.Text, tb_machuyendi.Text, ngayKhoiHanh);
            }
            catch
            {
                MessageBox.Show("Lấy danh sách hóa đơn thất bại!");
            }
        }

        private void dgv_qlhoadon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int currentIndex = dgv_qlhoadon.CurrentCell.RowIndex;

                string maTaiKhoan = dgv_qlhoadon.Rows[currentIndex].Cells[1].Value.ToString();
                string maChuyenDi = dgv_qlhoadon.Rows[currentIndex].Cells[2].Value.ToString();
                DateTime ngayKhoiHanh = DateTime.Parse(dgv_qlhoadon.Rows[currentIndex].Cells[3].Value.ToString());

                dgv_dsdukhach.DataSource = AdminQuery.LayDanhSachDuKhach(maTaiKhoan, maChuyenDi, ngayKhoiHanh);

                DataTable dthoadon = AdminQuery.LayChiTietHoaDon(int.Parse(dgv_qlhoadon.Rows[currentIndex].Cells[0].Value.ToString()));

                if (dthoadon.Rows.Count > 0)
                {
                    DataRow row = dthoadon.Rows[0]; // Lấy dòng đầu tiên

                    // Tạo StringBuilder để xây dựng chuỗi hiển thị
                    StringBuilder details = new StringBuilder();

                    // Duyệt qua từng cột
                    foreach (DataColumn column in dthoadon.Columns)
                    {
                        string columnName = column.ColumnName; // Lấy tên cột
                        string cellValue = row[column]?.ToString() ?? "N/A"; // Lấy giá trị

                        // Thêm vào chuỗi chi tiết
                        details.AppendLine($"{columnName}: {cellValue}");
                    }

                    // Hiển thị trong TextBox
                    txt_thongtin.Text = details.ToString();
                }
                else
                {
                    txt_thongtin.Text = "Không tìm thấy thông tin hóa đơn.";
                }
            } catch
            {
                MessageBox.Show("Lấy danh sách du khách thất bại!");
            }
        }

        private async void btn_switchhoadonstate_Click(object sender, EventArgs e)
        {
            try
            {
                int currentIndex = dgv_qlhoadon.CurrentCell.RowIndex;

                int idHoaDon = int.Parse(dgv_qlhoadon.Rows[currentIndex].Cells[0].Value.ToString());
                string idTaiKhoan = dgv_qlhoadon.Rows[currentIndex].Cells[1].Value.ToString();
                string idChuyenDi = dgv_qlhoadon.Rows[currentIndex].Cells[2].Value.ToString();
                DateTime ngayBatDau = DateTime.Parse(dgv_qlhoadon.Rows[currentIndex].Cells[3].Value.ToString());
                int soLuong = int.Parse(dgv_qlhoadon.Rows[currentIndex].Cells[4].Value.ToString());

                DialogResult xacNhan;

                xacNhan = MessageBox.Show("Bạn có chắc muốn thay đổi trạng thái thanh toán sang: " + (AdminQuery.LayTrangThaiHoaDon(idHoaDon) == "Chưa Thanh Toán" ? "Đã Thanh Toán" : "Chưa Thanh Toán") + " ?", "Thay đổi?",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (xacNhan == DialogResult.Yes)
                {
                    AdminQuery.ChuyenTrangThaiThanhToan(idHoaDon);

                    dgv_qlhoadon.DataSource = AdminQuery.LayDanhSachHoaDon();

                    string email = SystemQuery.GetEmailFromID(idTaiKhoan);

                    if (AdminQuery.LayTrangThaiHoaDon(idHoaDon) == "Đã Thanh Toán")
                        await Task.Run(() => sendMailAsync(email, idChuyenDi, ngayBatDau, soLuong, AdminQuery.LayDanhSachDuKhach(idTaiKhoan, idChuyenDi, ngayBatDau)));

                    MessageBox.Show("Chuyển trạng thái thành công sang: " + AdminQuery.LayTrangThaiHoaDon(idHoaDon) + "\n Email thanh toán đã được gửi đến khách hàng!");
                }


            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task sendMailAsync(string email, string maChuyenDi, DateTime ngayBatDau, int soLuong, DataTable dsHanhKhach)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("pacifictravel.dh22tin01@gmail.com", "gaam whlj edac arbu");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("pacifictravel.dh22tin01@gmail.com", "PacificTravel Support");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = "Thanh toán chuyến đi thành công";

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
                <h2 style='margin: 0;'>Thanh Toán Thành Công</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 20px; text-align: left; color: #333;'>
                <p>Chào quý khách,</p>
                <p>Chúng tôi gửi email này để thông báo bạn vừa thanh toán thành công chuyến đi.</p>
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

        private void dgv_qlhoadon_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Utils.CheckAndColorizeDataGridView(dgv_qlhoadon);
        }

        private void btn_printhoadon_Click(object sender, EventArgs e)
        {
            printReviewDialog.Document = printDocument;
            printReviewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int startX = 50;
            int startY = 50;
            int lineHeight = 30;

            // Định nghĩa vùng hiển thị văn bản (300px chiều rộng)
            RectangleF layoutRect = new RectangleF(startX + 400, startY, 360, lineHeight * 3);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.Trimming = StringTrimming.Word; // Tự động cắt từ nếu quá dài
            format.FormatFlags = StringFormatFlags.LineLimit; // Giới hạn trong vùng chữ nhật

            int currentIndex = dgv_qlhoadon.CurrentCell.RowIndex;

            int idHoaDon = int.Parse(dgv_qlhoadon.Rows[currentIndex].Cells[0].Value.ToString());
            string idTaiKhoan = dgv_qlhoadon.Rows[currentIndex].Cells[1].Value.ToString();
            string idChuyenDi = dgv_qlhoadon.Rows[currentIndex].Cells[2].Value.ToString();
            DateTime ngayBatDau = DateTime.Parse(dgv_qlhoadon.Rows[currentIndex].Cells[3].Value.ToString());
            int soLuong = int.Parse(dgv_qlhoadon.Rows[currentIndex].Cells[4].Value.ToString());

            DataTable dt = UserQuery.getThongTinCaNhan(idTaiKhoan);

            string hoten = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][0].ToString();
            string cccd = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][1].ToString();
            string sdt = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][2].ToString();
            string diachi = dt.Rows.Count == 0 ? string.Empty : dt.Rows[0][3].ToString();

            DataTable dt2 = UserQuery.LayChiTietChuyenDi(idChuyenDi);

            string tenchuyendi = dt2.Rows.Count == 0 ? string.Empty : dt2.Rows[0][1].ToString();
            string hanhtrinh = dt2.Rows.Count == 0 ? string.Empty : dt2.Rows[0][2].ToString();
            string songaydi = dt2.Rows.Count == 0 ? string.Empty : dt2.Rows[0][3].ToString();

            DataTable dsDuKhach = AdminQuery.LayDanhSachDuKhach(idTaiKhoan, idChuyenDi, ngayBatDau);

            // Vẽ logo
            g.DrawImage(logo, startX - 10, startY, 120, 60);

            // Tiêu đề hóa đơn
            g.DrawString("HÓA ĐƠN #" + idHoaDon, fontTitle, Brushes.Black, startX + 620, startY + 20);
            startY += lineHeight * 2;

            // Thông tin công ty
            Font fontItalic = new Font(fontContent, FontStyle.Italic);

            g.DrawString("168 Nguyễn Văn Cừ Nối Dài, An Bình, Ninh Kiều, Cần Thơ", fontItalic, Brushes.Black, startX, startY);
            startY += lineHeight;

            g.DrawString("Email: pacifictravel.dh22tin01@gmail.com", fontItalic, Brushes.Black, startX, startY);
            startY += lineHeight * 2;

            // Thông tin khách hàng & chuyến đi
            g.DrawString("Thông tin khách hàng:", fontHeader, Brushes.Black, startX, startY);
            g.DrawString("Thông tin chuyến đi:", fontHeader, Brushes.Black, startX + 400, startY);
            startY += lineHeight;

            g.DrawString("Tên:  " + hoten, fontContent, Brushes.Black, startX, startY);
            g.DrawString("Mã Chuyến Đi:  " + idChuyenDi, fontContent, Brushes.Black, startX + 400, startY);
            startY += lineHeight;

            g.DrawString("CCCD:  " + cccd, fontContent, Brushes.Black, startX, startY);
            layoutRect.Y = startY; // Cập nhật vị trí chữ nhật
            g.DrawString("Tên Chuyến Đi:  " + tenchuyendi, fontContent, Brushes.Black, layoutRect, format);
            startY += lineHeight * 2; // Tăng khoảng cách nếu xuống dòng

            g.DrawString("SDT:  " + sdt, fontContent, Brushes.Black, startX, startY);
            layoutRect.Y = startY;
            g.DrawString("Hành Trình:  " + hanhtrinh, fontContent, Brushes.Black, layoutRect, format);
            startY += lineHeight * 2;

            g.DrawString("Địa Chỉ:  " + diachi, fontContent, Brushes.Black, startX, startY);
            g.DrawString("Số Ngày Đi:  " + songaydi, fontContent, Brushes.Black, startX + 400, startY);
            startY += lineHeight;

            g.DrawString("Email:  " + SystemQuery.GetEmailFromID(idTaiKhoan), fontContent, Brushes.Black, startX, startY);


            // Bảng chi tiết hóa đơn
            startY += lineHeight * 3;
            g.DrawString("Họ Tên", fontHeader, Brushes.Black, startX, startY);
            g.DrawString("Số Điện Thoại", fontHeader, Brushes.Black, startX + 200, startY);
            g.DrawString("CCCD", fontHeader, Brushes.Black, startX + 400, startY);
            startY += lineHeight;

            int tableWidth = 750;
            int rowHeight = 30;
            int tableHeight = dsDuKhach.Rows.Count * rowHeight;

            g.DrawRectangle(Pens.Black, startX, startY - 10, tableWidth, tableHeight);

            foreach (DataRow row in dsDuKhach.Rows)
            {
                g.DrawString(row["Họ Tên"].ToString(), fontContent, Brushes.Black, startX, startY);
                g.DrawString(row["Số Điện Thoại"].ToString(), fontContent, Brushes.Black, startX + 200, startY);
                g.DrawString(row["CCCD"].ToString(), fontContent, Brushes.Black, startX + 400, startY);
                startY += rowHeight;
            }

            // Tổng tiền
            startY += 30;
            g.DrawString("Tổng Số Tiền Phải Thanh Toán: " + UserQuery.SoTienThanhToan(idChuyenDi, ngayBatDau, soLuong).ToString(), fontHeader, Brushes.Black, startX + 450, startY);
            startY += lineHeight;
            g.DrawString("Trạng Thái Thanh Toán: " + AdminQuery.LayTrangThaiHoaDon(idHoaDon), fontHeader, Brushes.Black, startX + 450, startY);
            startY += lineHeight * 5;

            // Xác định vị trí bắt đầu của phần ghi chú (cách mép dưới 100px)
            startY = e.PageBounds.Height - 120;

            // Ghi chú thanh toán ở góc dưới bên trái
            g.DrawString("Mọi thanh toán phải được thanh toán tại PacificTravel", fontContent, Brushes.Black, startX, startY);
            startY += lineHeight;
            g.DrawString("Hóa đơn phải thanh toán trước ngày diễn ra tour.", fontContent, Brushes.Black, startX, startY);
            startY += lineHeight;
            g.DrawString("Cảm ơn vì đã sử dụng dịch vụ tại PacificTravel!", fontHeader, Brushes.Black, startX, startY);

        }
    }
}
