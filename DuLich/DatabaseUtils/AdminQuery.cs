using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IdentityModel.Metadata;

namespace DuLich.DatabaseUtils
{
    internal class AdminQuery
    {

        static SqlConnection sqlcon = SystemQuery.sqlcon;

        public static void SaveOrUpdateChuyenDi(string IDTour, string TenTour, int SoNgayDi, int Gia, string HanhTrinh, int SoLuong, string ChiTiet)
        {

            string query = @"
        MERGE INTO [dbo].[ChuyenDi] AS target
        USING (SELECT @IDTour AS ID_ChuyenDi) AS source
        ON target.ID_ChuyenDi = source.ID_ChuyenDi
        WHEN MATCHED THEN
            UPDATE SET 
                TenChuyenDi = @TenTour,
                HanhTrinh = @HanhTrinh,
                SoNgayDi = @SoNgayDi,
                Gia = @Gia,
                SoLuong = @SoLuong,
                ChiTiet = ISNULL(@ChiTiet, target.ChiTiet)
        WHEN NOT MATCHED THEN
            INSERT (ID_ChuyenDi, TenChuyenDi, HanhTrinh, SoNgayDi, Gia, SoLuong, ChiTiet)
            VALUES (@IDTour, @TenTour, @HanhTrinh, @SoNgayDi, @Gia, @SoLuong, @ChiTiet);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.AddWithValue("@IDTour", IDTour);
                    cmd.Parameters.AddWithValue("@TenTour", TenTour);
                    cmd.Parameters.AddWithValue("@HanhTrinh", HanhTrinh);
                    cmd.Parameters.AddWithValue("@SoNgayDi", SoNgayDi);
                    cmd.Parameters.AddWithValue("@Gia", Gia);
                    cmd.Parameters.AddWithValue("@SoLuong", SoLuong);
                    cmd.Parameters.AddWithValue("@ChiTiet", ChiTiet);

                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }
        }

        public static DataTable getDSChuyenDi()
        {
            DataTable dt = new DataTable();
            string query = "SELECT ID_ChuyenDi, TenChuyenDi, HanhTrinh, SoNgayDi, Gia, SoLuong, ChiTiet FROM [dbo].[ChuyenDi]";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        dt.Columns["ID_ChuyenDi"].ColumnName = "Mã Tour";
                        dt.Columns["TenChuyenDi"].ColumnName = "Tên Tour";
                        dt.Columns["HanhTrinh"].ColumnName = "Hành Trình";
                        dt.Columns["SoNgayDi"].ColumnName = "Số Ngày Đi";
                        dt.Columns["Gia"].ColumnName = "Giá";
                        dt.Columns["SoLuong"].ColumnName = "Số Lượng";
                        dt.Columns["ChiTiet"].ColumnName = "Chi Tiết";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return dt;
        }

        public static void deleteChuyenDi(string ID_ChuyenDi)
        {

            try
            {
                sqlcon.Open();
                SqlTransaction transaction = sqlcon.BeginTransaction(); // Bắt đầu transaction

                try
                {
                    // Xóa các bảng có khóa ngoại liên kết với ChuyenDi
                    string[] deleteQueries = new string[]
                    {
                    "DELETE FROM DanhSachDuKhach WHERE ID_ChuyenDi = @ID_ChuyenDi",
                    "DELETE FROM DanhSachDangKy WHERE ID_ChuyenDi = @ID_ChuyenDi",
                    "DELETE FROM YeuCau WHERE ID_ChuyenDi = @ID_ChuyenDi",
                    "DELETE FROM DanhGia WHERE ID_ChuyenDi = @ID_ChuyenDi",
                    "DELETE FROM LichTrinh WHERE ID_ChuyenDi = @ID_ChuyenDi",
                    "DELETE FROM ChuyenDi WHERE ID_ChuyenDi = @ID_ChuyenDi"
                    };

                    foreach (string query in deleteQueries)
                    {
                        using (SqlCommand cmd = new SqlCommand(query, sqlcon, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ID_ChuyenDi", ID_ChuyenDi);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit(); // Nếu không có lỗi, commit transaction
                    Console.WriteLine("Xóa thành công chuyến đi và tất cả dữ liệu liên quan.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Nếu lỗi, rollback transaction
                    Console.WriteLine("Lỗi khi xóa chuyến đi: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

        }

        public static DataTable getLichTrinh(DateTime fromdate, DateTime todate)
        {
            string query = @"
        SELECT 
            lt.ID_ChuyenDi,
            lt.NgayBatDau,
            lt.ID_HDV,
            ISNULL(COUNT(dk.ID_ChuyenDi), 0) AS SoLuongNow,
            cd.SoLuong
        FROM 
            LichTrinh lt
        INNER JOIN 
            ChuyenDi cd ON lt.ID_ChuyenDi = cd.ID_ChuyenDi
        LEFT JOIN 
            DanhSachDuKhach dk ON lt.ID_ChuyenDi = dk.ID_ChuyenDi AND lt.NgayBatDau = dk.NgayBatDau
        WHERE 
            lt.NgayBatDau >= @FromDate AND lt.NgayBatDau <= @ToDate
        GROUP BY 
            lt.ID_ChuyenDi, lt.NgayBatDau, lt.ID_HDV, cd.SoLuong";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromdate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Add columns to DataTable
                    dt.Columns.Add("Mã Tour");
                    dt.Columns.Add("Ngày khởi hành");
                    dt.Columns.Add("Mã hướng dẫn viên");
                    dt.Columns.Add("Số lượng đã đăng ký");
                    dt.Columns.Add("Số lượng tối đa");

                    // Populate DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"],
                            reader["NgayBatDau"],
                            reader["ID_HDV"] ?? "",
                            reader["SoLuongNow"],
                            reader["SoLuong"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return dt;
        }

        public static DataTable getDSMaChuyenDi()
        {
            string query = "SELECT ID_ChuyenDi FROM ChuyenDi";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Add columns to DataTable
                    dt.Columns.Add("Mã Tour", typeof(string));

                    // Populate DataTable with the results
                    while (reader.Read())
                    {
                        dt.Rows.Add(reader["ID_ChuyenDi"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return dt;
        }

        public static DataTable getDSMaHDV()
        {
            string query = "SELECT ID_HDV FROM HuongDanVien";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Add columns to DataTable
                    dt.Columns.Add("Mã Hướng dẫn viên", typeof(string));

                    // Populate DataTable with the results
                    while (reader.Read())
                    {
                        dt.Rows.Add(reader["ID_HDV"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return dt;
        }

        public static DataTable locLichTrinh(string idTour, string idGuide, int quantity, bool not_eligible)
        {
            // Base SQL query
            string query = @"
        SELECT 
            lt.ID_ChuyenDi,
            lt.NgayBatDau,
            lt.ID_HDV,
            ISNULL(COUNT(dk.ID_ChuyenDi), 0) AS SoLuongHienTai,
            cd.SoLuong
        FROM 
            LichTrinh lt
        INNER JOIN 
            ChuyenDi cd ON lt.ID_ChuyenDi = cd.ID_ChuyenDi
        LEFT JOIN 
            DanhSachDuKhach dk ON lt.ID_ChuyenDi = dk.ID_ChuyenDi AND lt.NgayBatDau = dk.NgayBatDau";

            // List to store WHERE conditions
            List<string> whereConditions = new List<string>();
            whereConditions.Add("cd.SoLuong <= @Quantity"); // Default condition

            if (!string.IsNullOrEmpty(idTour))
            {
                whereConditions.Add("lt.ID_ChuyenDi = @IdTour");
            }

            if (!string.IsNullOrEmpty(idGuide))
            {
                whereConditions.Add("lt.ID_HDV = @IdGuide");
            }

            // Append WHERE conditions if any exist
            if (whereConditions.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", whereConditions);
            }

            // Add GROUP BY clause
            query += " GROUP BY lt.ID_ChuyenDi, lt.NgayBatDau, lt.ID_HDV, cd.SoLuong";

            // Add HAVING clause to filter aggregated results
            if (not_eligible)
            {
                query += " HAVING (cd.SoLuong * 0.5 > ISNULL(COUNT(dk.ID_ChuyenDi), 0) OR ISNULL(COUNT(dk.ID_ChuyenDi), 0) = 0 OR lt.ID_HDV IS NULL)";
            }
            else
            {
                query += " HAVING cd.SoLuong * 0.5 <= ISNULL(COUNT(dk.ID_ChuyenDi), 0)";
            }

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;

                    if (!string.IsNullOrEmpty(idTour))
                    {
                        cmd.Parameters.Add("@IdTour", SqlDbType.NVarChar).Value = idTour;
                    }

                    if (!string.IsNullOrEmpty(idGuide))
                    {
                        cmd.Parameters.Add("@IdGuide", SqlDbType.NVarChar).Value = idGuide;
                    }

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Add columns to DataTable
                    dt.Columns.Add("Mã Tour", typeof(string));
                    dt.Columns.Add("Ngày khởi hành", typeof(DateTime));
                    dt.Columns.Add("Mã hướng dẫn viên", typeof(string));
                    dt.Columns.Add("Số lượng đã đăng ký", typeof(int));
                    dt.Columns.Add("Số lượng tối đa", typeof(int));

                    // Populate DataTable with the results
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"],
                            reader["NgayBatDau"],
                            reader["ID_HDV"] ?? "",
                            reader["SoLuongHienTai"],
                            reader["SoLuong"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return dt;
        }



        public static void themLichTrinh(string MaTour, DateTime NgayKhoiHanh)
        {
            string query = "INSERT INTO LichTrinh (ID_ChuyenDi, NgayBatDau) VALUES (@ID_ChuyenDi, @NgayKhoiHanh)";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    // Thêm tham số vào câu lệnh SQL
                    cmd.Parameters.Add("@ID_ChuyenDi", SqlDbType.NVarChar).Value = MaTour;
                    cmd.Parameters.Add("@NgayKhoiHanh", SqlDbType.DateTime).Value = NgayKhoiHanh;

                    // Mở kết nối và thực thi câu lệnh SQL
                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Đảm bảo kết nối được đóng lại
            }
        }


        public async static void huyLichTrinh(string IDTour, DateTime StartDay)
        {
            string deleteTourQuery = "DELETE FROM LichTrinh WHERE ID_ChuyenDi = @IDTour AND NgayBatDau = @StartDay";
            string deletePassengersQuery = "DELETE FROM DanhSachDuKhach WHERE ID_ChuyenDi = @IDTour AND NgayBatDau = @StartDay";
            string getEmailQuery = @"
        SELECT t.Email 
        FROM TaiKhoan t
        INNER JOIN DanhSachDangKy dsk ON t.ID_TaiKhoan = dsk.ID_TaiKhoan
        WHERE dsk.ID_ChuyenDi = @IDTour AND dsk.NgayBatDau = @StartDay";
            string deleteRegistrationQuery = "DELETE FROM DanhSachDangKy WHERE ID_ChuyenDi = @IDTour AND NgayBatDau = @StartDay";

            try
            {
                // 1. Xóa lịch trình
                using (SqlCommand cmd = new SqlCommand(deleteTourQuery, sqlcon))
                {
                    cmd.Parameters.Add("@IDTour", SqlDbType.NVarChar).Value = IDTour;
                    cmd.Parameters.Add("@StartDay", SqlDbType.DateTime).Value = StartDay;

                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                }

                // 2. Xóa danh sách hành khách
                using (SqlCommand cmd = new SqlCommand(deletePassengersQuery, sqlcon))
                {
                    cmd.Parameters.Add("@IDTour", SqlDbType.NVarChar).Value = IDTour;
                    cmd.Parameters.Add("@StartDay", SqlDbType.DateTime).Value = StartDay;

                    cmd.ExecuteNonQuery();
                }

                // 3. Lấy danh sách email từ bảng DanhSachDangKy
                List<string> emails = new List<string>();
                using (SqlCommand cmd = new SqlCommand(getEmailQuery, sqlcon))
                {
                    cmd.Parameters.Add("@IDTour", SqlDbType.NVarChar).Value = IDTour;
                    cmd.Parameters.Add("@StartDay", SqlDbType.DateTime).Value = StartDay;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            emails.Add(reader["Email"].ToString());
                        }
                    }
                }

                // 4. Gửi email thông báo hủy lịch trình
                foreach (string email in emails)
                {
                    try
                    {
                        await sendCancelTourEmail(email, IDTour, StartDay);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                // 5. Xóa các bản ghi liên quan đến chuyến đi trong bảng DanhSachDangKy
                using (SqlCommand cmd = new SqlCommand(deleteRegistrationQuery, sqlcon))
                {
                    cmd.Parameters.Add("@IDTour", SqlDbType.NVarChar).Value = IDTour;
                    cmd.Parameters.Add("@StartDay", SqlDbType.DateTime).Value = StartDay;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Đảm bảo kết nối được đóng lại
            }
        }

        private async static Task sendCancelTourEmail(string recipientEmail, string IDTour, DateTime StartDay)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("pacifictravel.dh22tin01@gmail.com", "PacificTravel Support");
                mail.To.Add(recipientEmail);
                mail.Subject = "Thông báo hủy lịch trình chuyến đi";

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
                <h2 style='margin: 0;'>Chuyến Đi Đã Bị Hủy</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 20px; text-align: left; color: #333;'>
                <p>Kính gửi quý khách,</p>
                <p>Chuyến đi với mã tour [IDTour] vào ngày [StartDay] đã bị hủy.</p>
                <p>Xin vui lòng liên hệ chúng tôi để biết thêm chi tiết.</p>
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

                html.Replace("[IDTour]", IDTour);
                html.Replace("[StartDay]", StartDay.ToString("dd/MM/yyyy"));

                mail.Body = html;
                mail.IsBodyHtml = true;

                smtpServer.Port = 587;  // Cổng SMTP (có thể thay đổi tùy vào nhà cung cấp email)
                smtpServer.Credentials = new NetworkCredential("pacifictravel.dh22tin01@gmail.com", "gaam whlj edac arbu");
                smtpServer.EnableSsl = true;

                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }
        }


        public static string getTourNameByID(string IDChuyenDi)
        {
            string query = "SELECT TenChuyenDi FROM ChuyenDi WHERE ID_ChuyenDi = @IDChuyenDi";
            string tourName = string.Empty;

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    // Thêm tham số vào câu lệnh SQL
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = IDChuyenDi;

                    // Mở kết nối và thực thi câu lệnh SQL
                    sqlcon.Open();
                    var result = cmd.ExecuteScalar(); // Lấy giá trị đầu tiên của cột "TenChuyenDi"

                    if (result != null)
                    {
                        tourName = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Đảm bảo kết nối được đóng lại
            }

            return tourName;
        }

        public static DataTable getDSHDV()
        {
            string query = "SELECT ID_HDV, Ten, SDT, Email FROM HuongDanVien";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Add columns to DataTable
                    dt.Columns.Add("Mã Hướng dẫn viên", typeof(string));
                    dt.Columns.Add("Tên Hướng dẫn viên", typeof(string));
                    dt.Columns.Add("Số Điện Thoại", typeof(string));
                    dt.Columns.Add("Email", typeof(string));

                    // Populate DataTable with the results
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_HDV"].ToString(),
                            reader["Ten"].ToString(),
                            reader["SDT"].ToString(),
                            reader["Email"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return dt;
        }

        public static DataTable getDSTourToPhanCong()
        {
            string query = "SELECT ID_ChuyenDi, NgayBatDau, ID_HDV FROM LichTrinh";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Add columns to DataTable
                    dt.Columns.Add("Mã Chuyến Đi", typeof(string));
                    dt.Columns.Add("Ngày bắt đầu", typeof(DateTime));
                    dt.Columns.Add("Mã HDV", typeof(string));

                    // Populate DataTable with the results
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["NgayBatDau"],
                            reader["ID_HDV"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return dt;
        }

        public static bool isHDVExist(string ID_HDV)
        {
            string query = "SELECT COUNT(1) FROM HuongDanVien WHERE ID_HDV = @ID_HDV";
            int count = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@ID_HDV", SqlDbType.NVarChar).Value = ID_HDV;
                    sqlcon.Open();
                    count = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return count > 0; // Nếu count > 0 thì hướng dẫn viên đã tồn tại
        }

        public static void SaveOrUpdateHDV(string ID_HDV, string Ten, string SDT, string Email)
        {
            string query = @"
        MERGE INTO HuongDanVien AS target
        USING (SELECT @ID_HDV AS ID_HDV, @Ten AS Ten, @SDT AS SDT, @Email AS Email) AS source
        ON target.ID_HDV = source.ID_HDV
        WHEN MATCHED THEN
            UPDATE SET target.Ten = source.Ten, target.SDT = source.SDT, target.Email = source.Email
        WHEN NOT MATCHED THEN
            INSERT (ID_HDV, Ten, SDT, Email)
            VALUES (source.ID_HDV, source.Ten, source.SDT, source.Email);
    ";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    // Thêm tham số vào câu lệnh SQL
                    cmd.Parameters.Add("@ID_HDV", SqlDbType.NVarChar).Value = ID_HDV;
                    cmd.Parameters.Add("@Ten", SqlDbType.NVarChar).Value = Ten;
                    cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = SDT;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;

                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Đảm bảo kết nối được đóng lại
            }
        }

        public static void xoaHDV(string ID_HDV)
        {
            string updateQuery = "UPDATE LichTrinh SET ID_HDV = NULL WHERE ID_HDV = @ID_HDV";
            string deleteQuery = "DELETE FROM HuongDanVien WHERE ID_HDV = @ID_HDV";

            try
            {
                using (SqlCommand cmd = new SqlCommand(updateQuery, sqlcon))
                {
                    cmd.Parameters.Add("@ID_HDV", SqlDbType.NVarChar).Value = ID_HDV;
                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(deleteQuery, sqlcon))
                {
                    cmd.Parameters.Add("@ID_HDV", SqlDbType.NVarChar).Value = ID_HDV;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }
        }

        public static DataTable LichTrinhHD(string ID_HDV)
        {
            string query = "SELECT ID_ChuyenDi, NgayBatDau, ID_HDV FROM LichTrinh WHERE ID_HDV = @ID_HDV";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@ID_HDV", SqlDbType.NVarChar).Value = ID_HDV;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Add columns to DataTable
                    dt.Columns.Add("Mã Chuyến Đi", typeof(string));
                    dt.Columns.Add("NgayBatDau", typeof(DateTime));
                    dt.Columns.Add("Mã HDV", typeof(string));

                    // Populate DataTable with the results
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["NgayBatDau"],
                            reader["ID_HDV"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }

            return dt;
        }

        public static void capNhatLichTrinh(string ID_ChuyenDi, DateTime NgayBatDau, string ID_HDV)
        {
            string query = @"
        UPDATE LichTrinh 
        SET ID_HDV = @ID_HDV
        WHERE ID_ChuyenDi = @ID_ChuyenDi 
        AND CAST(NgayBatDau AS DATE) = @NgayBatDau";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    // Thêm tham số vào câu lệnh SQL
                    cmd.Parameters.Add("@ID_ChuyenDi", SqlDbType.NVarChar).Value = ID_ChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = NgayBatDau;
                    cmd.Parameters.Add("@ID_HDV", SqlDbType.NVarChar).Value = ID_HDV;

                    // Mở kết nối và thực thi câu lệnh SQL
                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Không tìm thấy lịch trình cần cập nhật.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Đảm bảo kết nối được đóng lại
            }
        }

        public static void huyPhanCong(string ID_ChuyenDi, DateTime NgayBatDau)
        {
            string query = @"
        UPDATE LichTrinh 
        SET ID_HDV = NULL
        WHERE ID_ChuyenDi = @ID_ChuyenDi 
        AND CAST(NgayBatDau AS DATE) = @NgayBatDau";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    // Thêm tham số vào câu lệnh SQL
                    cmd.Parameters.Add("@ID_ChuyenDi", SqlDbType.NVarChar).Value = ID_ChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = NgayBatDau;

                    // Mở kết nối và thực thi câu lệnh SQL
                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Không tìm thấy lịch trình để hủy phân công.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlcon.Close(); // Đảm bảo kết nối được đóng lại
            }
        }

        public static DataTable LoadAllTickets()
        {
            string query = "SELECT ID_ChuyenDi, NgayBatDau, CCCD, Ten, SDT FROM DanhSachDuKhach;";
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Mã Chuyến Đi", typeof(string));
                    dt.Columns.Add("Ngày khởi hành", typeof(DateTime));
                    dt.Columns.Add("CCCD", typeof(string));
                    dt.Columns.Add("Tên Du khách", typeof(string));
                    dt.Columns.Add("Số điện thoại", typeof(string));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["NgayBatDau"],
                            reader["CCCD"].ToString(),
                            reader["Ten"].ToString(),
                            reader["SDT"].ToString()
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static DataTable LoadTickets(string IDTour, DateTime date)
        {
            string query = "SELECT ID_ChuyenDi, NgayBatDau, CCCD, Ten, SDT FROM DanhSachDuKhach WHERE ID_ChuyenDi = @IDChuyenDi AND NgayBatDau = @NgayBatDau";
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = IDTour;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = date;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Mã Chuyến Đi", typeof(string));
                    dt.Columns.Add("Ngày khởi hành", typeof(DateTime));
                    dt.Columns.Add("CCCD", typeof(string));
                    dt.Columns.Add("Tên Du khách", typeof(string));
                    dt.Columns.Add("Số điện thoại", typeof(string));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["NgayBatDau"],
                            reader["CCCD"].ToString(),
                            reader["Ten"].ToString(),
                            reader["SDT"].ToString()
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static DataTable CountTickets(string IDTour, DateTime date)
        {
            string query = @"
        SELECT 
            COUNT(*) AS SoLuongHienTai, 
            (SELECT SoLuong FROM ChuyenDi WHERE ID_ChuyenDi = @IDChuyenDi) AS SoLuongToiDa
        FROM DanhSachDuKhach
        WHERE ID_ChuyenDi = @IDChuyenDi AND NgayBatDau = @NgayBatDau;";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = IDTour;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = date;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Số lượng hiện tại", typeof(int));
                    dt.Columns.Add("Số lượng tối đa", typeof(int));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            Convert.ToInt32(reader["SoLuongHienTai"]),
                            Convert.ToInt32(reader["SoLuongToiDa"])
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static DataTable LoadDateFollowTours(string maChuyenDi)
        {
            string query = @"
        SELECT DISTINCT NgayBatDau 
        FROM DanhSachDuKhach 
        WHERE ID_ChuyenDi = @IDChuyenDi 
        ORDER BY NgayBatDau;";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Ngày khởi hành", typeof(DateTime));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(Convert.ToDateTime(reader["NgayBatDau"]));
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static void DeletePassenger(string iDTour, DateTime startDay, string CCCD)
        {
            string query = @"
        DELETE FROM DanhSachDuKhach 
        WHERE ID_ChuyenDi = @IDChuyenDi AND NgayBatDau = @NgayBatDau AND CCCD = @CCCD;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = iDTour;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = startDay;
                    cmd.Parameters.Add("@CCCD", SqlDbType.NVarChar).Value = CCCD;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa hành khách thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hành khách để xóa.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }


        public static void AddPassenger(string iDTour, DateTime startDay, string CCCD, string name, string sdt)
        {
            string query = @"
        INSERT INTO DanhSachDuKhach (ID_ChuyenDi, NgayBatDau, CCCD, Ten, SDT) 
        VALUES (@IDChuyenDi, @NgayBatDau, @CCCD, @Ten, @SDT);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = iDTour;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = startDay;
                    cmd.Parameters.Add("@CCCD", SqlDbType.NVarChar).Value = CCCD;
                    cmd.Parameters.Add("@Ten", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = sdt;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm hành khách thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm hành khách.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public static DataTable FindPassenger(string id, DateTime ngaydi, string cccd, string name, string sdt)
        {
            string query = @"
        SELECT ID_ChuyenDi, NgayBatDau, CCCD, Ten, SDT 
        FROM DanhSachDuKhach
        WHERE (@IDChuyenDi = '' OR ID_ChuyenDi = @IDChuyenDi)
        AND (@NgayBatDau = '0001-01-01' OR NgayBatDau = @NgayBatDau)
        AND (@CCCD = '' OR CCCD = @CCCD)
        AND (@Ten = '' OR Ten = @Ten)
        AND (@SDT = '' OR SDT = @SDT);";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = id;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = (ngaydi == new DateTime(1, 1, 1)) ? DBNull.Value : (object)ngaydi;
                    cmd.Parameters.Add("@CCCD", SqlDbType.NVarChar).Value = cccd;
                    cmd.Parameters.Add("@Ten", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = sdt;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Mã Chuyến Đi", typeof(string));
                    dt.Columns.Add("Ngày khởi hành", typeof(DateTime));
                    dt.Columns.Add("CCCD", typeof(string));
                    dt.Columns.Add("Tên Du khách", typeof(string));
                    dt.Columns.Add("Số điện thoại", typeof(string));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["NgayBatDau"],
                            reader["CCCD"].ToString(),
                            reader["Ten"].ToString(),
                            reader["SDT"].ToString()
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static DataTable GetDataRequest()
        {
            string query = "SELECT ID_TaiKhoan, ID_ChuyenDi, NgayBatDau, SoLuong FROM YeuCau;";
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Mã tài khoản", typeof(string));
                    dt.Columns.Add("Mã chuyến đi", typeof(string));
                    dt.Columns.Add("Ngày bắt đầu", typeof(DateTime));
                    dt.Columns.Add("Số lượng", typeof(int));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_TaiKhoan"].ToString(),
                            reader["ID_ChuyenDi"].ToString(),
                            Convert.ToDateTime(reader["NgayBatDau"]),
                            Convert.ToInt32(reader["SoLuong"])
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static void DeleteRequest(string matk, string macd, DateTime ngaydi)
        {
            string query = @"
        DELETE FROM YeuCau 
        WHERE ID_TaiKhoan = @IDTaiKhoan AND ID_ChuyenDi = @IDChuyenDi AND NgayBatDau = @NgayBatDau;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = macd;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngaydi;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa yêu cầu thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy yêu cầu để xóa.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public static DataTable GetDataEmail()
        {
            string query = "SELECT Email FROM TaiKhoan;";
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Email", typeof(string));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(reader["Email"].ToString());
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static string GetIDAccount(string email)
        {
            string query = "SELECT ID_TaiKhoan FROM TaiKhoan WHERE Email = @Email;";
            string idTaiKhoan = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

                    sqlcon.Open();
                    object result = cmd.ExecuteScalar();
                    sqlcon.Close();

                    if (result != null && result != DBNull.Value)
                    {
                        idTaiKhoan = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return idTaiKhoan;
        }

        public static DataTable GetAllDataAccount()
        {
            string query = "SELECT Email, MatKhau, ID_TaiKhoan FROM TaiKhoan;";
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("Mật khẩu", typeof(string));
                    dt.Columns.Add("Mã tài khoản", typeof(string));

                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["Email"].ToString(),
                            reader["MatKhau"].ToString(),
                            reader["ID_TaiKhoan"].ToString()
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static DataTable GetDataPersonal(string matk)
        {
            string query = "SELECT ID_TaiKhoan, Ten, SDT, DiaChi FROM ThongTinCaNhan WHERE ID_TaiKhoan = @IDTaiKhoan;";
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    dt.Columns.Add("Mã tài khoản", typeof(string));
                    dt.Columns.Add("Tên", typeof(string));
                    dt.Columns.Add("SDT", typeof(string));
                    dt.Columns.Add("Địa chỉ", typeof(string));

                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_TaiKhoan"].ToString(),
                            reader["Ten"].ToString(),
                            reader["SDT"].ToString(),
                            reader["DiaChi"].ToString()
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static void AddAccount(string email, string mk, string matk)
        {
            string query = "INSERT INTO TaiKhoan (Email, MatKhau, ID_TaiKhoan) VALUES (@Email, @MatKhau, @IDTaiKhoan);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
                    cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar).Value = mk;
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm tài khoản thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm tài khoản.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public static bool Is_Account_Exist(string email)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE Email = @Email;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

                    sqlcon.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    sqlcon.Close();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public static void DeleteAccount(string email, string matk)
        {
            string query1 = "DELETE FROM ThongTinCaNhan WHERE ID_TaiKhoan = @IDTaiKhoan;";
            string query2 = "DELETE FROM TaiKhoan WHERE Email = @Email AND ID_TaiKhoan = @IDTaiKhoan;";

            try
            {
                using (SqlCommand cmd1 = new SqlCommand(query1, sqlcon))
                {
                    cmd1.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;
                    sqlcon.Open();
                    cmd1.ExecuteNonQuery();
                    sqlcon.Close();
                }

                using (SqlCommand cmd2 = new SqlCommand(query2, sqlcon))
                {
                    cmd2.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
                    cmd2.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;

                    sqlcon.Open();
                    int rowsAffected = cmd2.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa tài khoản thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản để xóa.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public static DataTable GetDataJoinTour(string matk)
        {
            string query = @"
        SELECT ID_TaiKhoan, ID_ChuyenDi, NgayBatDau, SoLuong, TrangThai
        FROM DanhSachDangKy
        WHERE ID_TaiKhoan = @IDTaiKhoan;";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Mã tài khoản", typeof(string));
                    dt.Columns.Add("Mã chuyến đi", typeof(string));
                    dt.Columns.Add("Ngày bắt đầu", typeof(DateTime));
                    dt.Columns.Add("Số Lượng", typeof(int));
                    dt.Columns.Add("Trạng thái", typeof(string));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_TaiKhoan"].ToString(),
                            reader["ID_ChuyenDi"].ToString(),
                            Convert.ToDateTime(reader["NgayBatDau"]),
                            Convert.ToInt32(reader["SoLuong"]),
                            reader["TrangThai"].ToString()
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static bool Is_InfoPersonal_Exist(string matk)
        {
            string query = "SELECT COUNT(*) FROM ThongTinCaNhan WHERE ID_TaiKhoan = @IDTaiKhoan;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;

                    sqlcon.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    sqlcon.Close();

                    return count > 0; // Trả về true nếu có ít nhất 1 dòng
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public static void AddInfoPersonal(string matk, string ten, string sdt, string diachi)
        {
            string query = @"
        INSERT INTO ThongTinCaNhan (ID_TaiKhoan, Ten, SDT, DiaChi, Email) 
        VALUES (@IDTaiKhoan, @Ten, @SDT, @DiaChi, @Email);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;
                    cmd.Parameters.Add("@Ten", SqlDbType.NVarChar).Value = ten;
                    cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = sdt;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = diachi;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm thông tin cá nhân thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm thông tin cá nhân.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public static void UpdateInfoPersonal(string matk, string ten, string sdt, string diachi)
        {
            string query = @"
        UPDATE ThongTinCaNhan 
        SET Ten = @Ten, SDT = @SDT, DiaChi = @DiaChi, Email = @Email
        WHERE ID_TaiKhoan = @IDTaiKhoan;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = matk;
                    cmd.Parameters.Add("@Ten", SqlDbType.NVarChar).Value = ten;
                    cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = sdt;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = diachi;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin cá nhân thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản để cập nhật.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

        public static DataTable Load_dgvQLDanhGia()
        {
            string query = "SELECT ID_ChuyenDi, ID_TaiKhoan, SoSao, BinhLuan FROM DanhGia;";
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Mã Tour", typeof(string));
                    dt.Columns.Add("Mã Tài Khoản", typeof(string));
                    dt.Columns.Add("Số Sao", typeof(int));
                    dt.Columns.Add("Bình Luận", typeof(string));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["ID_TaiKhoan"].ToString(),
                            Convert.ToInt32(reader["SoSao"]),
                            reader["BinhLuan"].ToString()
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static DataTable FilterRate(string IDTour)
        {
            string query = @"
        SELECT ID_ChuyenDi, ID_TaiKhoan, SoSao, BinhLuan
        FROM DanhGia
        WHERE ID_ChuyenDi = @IDChuyenDi;";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = IDTour;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Mã Tour", typeof(string));
                    dt.Columns.Add("Mã Tài Khoản", typeof(string));
                    dt.Columns.Add("Số Sao", typeof(int));
                    dt.Columns.Add("Bình Luận", typeof(string));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["ID_TaiKhoan"].ToString(),
                            Convert.ToInt32(reader["SoSao"]),
                            reader["BinhLuan"].ToString()
                        );
                    }

                    reader.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return dt;
        }

        public static string AvgRate(string IDTour)
        {
            string query = @"
        SELECT AVG(SoSao) AS AvgSao
        FROM DanhGia
        WHERE ID_ChuyenDi = @IDChuyenDi;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = IDTour;

                    sqlcon.Open();
                    object result = cmd.ExecuteScalar();
                    sqlcon.Close();

                    if (result != DBNull.Value && result != null)
                    {
                        return Convert.ToDouble(result).ToString("0.00"); // Hiển thị 2 chữ số thập phân
                    }
                    else
                    {
                        return "0"; // Nếu không có đánh giá nào, trả về 0
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return "0";
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }

    }
}
