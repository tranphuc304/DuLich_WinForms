using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuLich.DatabaseUtils
{
    internal class UserQuery
    {
        static SqlConnection sqlcon = SystemQuery.sqlcon;

        public static DataTable getDSChuyenDi()
        {
            string query = @"
        SELECT cd.ID_ChuyenDi, cd.TenChuyenDi, cd.HanhTrinh, lt.NgayBatDau, 
               cd.SoNgayDi, cd.SoLuong, cd.Gia, 
               ISNULL(AVG(dg.SoSao), 0) AS SoSao
        FROM ChuyenDi cd
        JOIN LichTrinh lt ON cd.ID_ChuyenDi = lt.ID_ChuyenDi
        LEFT JOIN DanhGia dg ON cd.ID_ChuyenDi = dg.ID_ChuyenDi
        GROUP BY cd.ID_ChuyenDi, cd.TenChuyenDi, cd.HanhTrinh, lt.NgayBatDau, 
                 cd.SoNgayDi, cd.SoLuong, cd.Gia";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột với tên mới
                    dt.Columns.Add("Mã Tour", typeof(string));
                    dt.Columns.Add("Tên Tour", typeof(string));
                    dt.Columns.Add("Hành Trình", typeof(string));
                    dt.Columns.Add("Khởi Hành", typeof(DateTime));
                    dt.Columns.Add("Số Ngày Đi", typeof(int));
                    dt.Columns.Add("Số Lượng", typeof(int));
                    dt.Columns.Add("Giá", typeof(int));
                    dt.Columns.Add("Số Sao", typeof(double));

                    // Lặp qua dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["TenChuyenDi"].ToString(),
                            reader["HanhTrinh"].ToString(),
                            reader["NgayBatDau"],
                            Convert.ToInt32(reader["SoNgayDi"]),
                            Convert.ToInt32(reader["SoLuong"]),
                            Convert.ToDecimal(reader["Gia"]),
                            Convert.ToDouble(reader["SoSao"])
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
                sqlcon.Close(); // Đảm bảo kết nối được đóng lại
            }

            return dt;
        }

        public static DataTable timTour(string dest, DateTime start, string priceRange, int rate)
        {
            string query = @"
        SELECT cd.ID_ChuyenDi, cd.TenChuyenDi, cd.HanhTrinh, lt.NgayBatDau, 
               cd.SoNgayDi, cd.SoLuong, cd.Gia, 
               ISNULL(AVG(dg.SoSao), 0) AS SoSao
        FROM ChuyenDi cd
        JOIN LichTrinh lt ON cd.ID_ChuyenDi = lt.ID_ChuyenDi
        LEFT JOIN DanhGia dg ON cd.ID_ChuyenDi = dg.ID_ChuyenDi";

            List<string> conditions = new List<string>();

            if (!string.IsNullOrEmpty(dest))
            {
                conditions.Add("cd.HanhTrinh COLLATE SQL_Latin1_General_CP1_CI_AI LIKE @Dest");
            }

            if (start != new DateTime(1000, 1, 1))
            {
                conditions.Add("CAST(lt.NgayBatDau AS DATE) = @StartDate");
            }

            int minPrice = 0, maxPrice = 0;
            if (!string.IsNullOrEmpty(priceRange))
            {
                string[] priceSplit = priceRange.Replace(".", "").Trim().Split('-');
                
                if (priceSplit.Length == 2)
                {
                    minPrice = int.Parse(priceSplit[0]);
                    maxPrice = int.Parse(priceSplit[1]);

                    conditions.Add("cd.Gia BETWEEN @MinPrice AND @MaxPrice");
                }
            }

            // Nếu có điều kiện, thêm WHERE vào câu truy vấn
            if (conditions.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditions);
            }

            // Nhóm dữ liệu theo ID_ChuyenDi để tính trung bình số sao
            query += " GROUP BY cd.ID_ChuyenDi, cd.TenChuyenDi, cd.HanhTrinh, lt.NgayBatDau, cd.SoNgayDi, cd.SoLuong, cd.Gia";

            // Điều kiện lọc rating phải đặt trong HAVING
            if (rate != 0)
            {
                query += " HAVING ISNULL(AVG(dg.SoSao), 0) >= @Rate";
            }

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    if (!string.IsNullOrEmpty(dest))
                    {
                        cmd.Parameters.Add("@Dest", SqlDbType.NVarChar).Value = "%" + dest + "%";
                    }

                    if (start != new DateTime(1000, 1, 1))
                    {
                        cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = start;
                    }

                    if (!string.IsNullOrEmpty(priceRange))
                    {
                        cmd.Parameters.Add("@MinPrice", SqlDbType.Int).Value = minPrice;
                        cmd.Parameters.Add("@MaxPrice", SqlDbType.Int).Value = maxPrice;
                    }

                    if (rate != 0)
                    {
                        cmd.Parameters.Add("@Rate", SqlDbType.Int).Value = rate;
                    }

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Mã Tour", typeof(string));
                    dt.Columns.Add("Tên Tour", typeof(string));
                    dt.Columns.Add("Hành Trình", typeof(string));
                    dt.Columns.Add("Khởi Hành", typeof(DateTime));
                    dt.Columns.Add("Số Ngày Đi", typeof(int));
                    dt.Columns.Add("Số Lượng", typeof(int));
                    dt.Columns.Add("Giá", typeof(int));
                    dt.Columns.Add("Số Sao", typeof(double));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["TenChuyenDi"].ToString(),
                            reader["HanhTrinh"].ToString(),
                            reader["NgayBatDau"],
                            Convert.ToInt32(reader["SoNgayDi"]),
                            Convert.ToInt32(reader["SoLuong"]),
                            Convert.ToInt32(reader["Gia"]),
                            Convert.ToDouble(reader["SoSao"])
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
                sqlcon.Close();
            }

            return dt;
        }

        public static DataTable LayChiTietChuyenDi(string maChuyenDi, DateTime ngayBatDau)
        {
            string query = @"
        SELECT cd.TenChuyenDi, cd.HanhTrinh, cd.SoNgayDi, cd.SoLuong, cd.Gia, 
               ISNULL(AVG(dg.SoSao), 0) AS Sao, lt.NgayBatDau, cd.ChiTiet
        FROM ChuyenDi cd
        JOIN LichTrinh lt ON cd.ID_ChuyenDi = lt.ID_ChuyenDi
        LEFT JOIN DanhGia dg ON cd.ID_ChuyenDi = dg.ID_ChuyenDi
        WHERE cd.ID_ChuyenDi = @MaChuyenDi AND lt.NgayBatDau = @NgayBatDau
        GROUP BY cd.TenChuyenDi, cd.HanhTrinh, cd.SoNgayDi, cd.SoLuong, 
                 cd.Gia, lt.NgayBatDau, cd.ChiTiet";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable với tên cột tương ứng
                    dt.Columns.Add("Tên Chuyến Đi", typeof(string));
                    dt.Columns.Add("Hành Trình", typeof(string));
                    dt.Columns.Add("Số Ngày Đi", typeof(int));
                    dt.Columns.Add("Số Lượng", typeof(int));
                    dt.Columns.Add("Giá", typeof(string));
                    dt.Columns.Add("Sao", typeof(double)); // Đổi thành double vì AVG()
                    dt.Columns.Add("Ngày Bắt Đầu", typeof(DateTime));
                    dt.Columns.Add("Chi Tiết", typeof(string));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["TenChuyenDi"].ToString(),
                            reader["HanhTrinh"].ToString(),
                            Convert.ToInt32(reader["SoNgayDi"]),
                            Convert.ToInt32(reader["SoLuong"]),
                            reader["Gia"].ToString(),
                            Convert.ToDouble(reader["Sao"]), // Đổi thành double vì AVG()
                            reader["NgayBatDau"],
                            reader["ChiTiet"].ToString()
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
                sqlcon.Close();
            }

            return dt;
        }

        public static DataTable getThongTinCaNhan(string ID_TaiKhoan)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT HoTen, CCCD, SDT, DiaChi FROM ThongTinCaNhan WHERE ID_TaiKhoan = @IDTaiKhoan";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = ID_TaiKhoan;

                    sqlcon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Load dữ liệu từ reader vào DataTable
                        dt.Load(reader);
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

            return dt;
        }

        public static void updateThongTinCaNhan(string ID_TaiKhoan, string HoTen, string CCCD, string SDT, string DiaChi)
        {
            string query = @"
        MERGE INTO ThongTinCaNhan AS target
        USING (SELECT @IDTaiKhoan AS ID_TaiKhoan, @HoTen AS HoTen, @CCCD AS CCCD, 
                      @SDT AS SDT, @DiaChi AS DiaChi) AS source
        ON target.ID_TaiKhoan = source.ID_TaiKhoan
        WHEN MATCHED THEN 
            UPDATE SET target.HoTen = source.HoTen, target.CCCD = source.CCCD, 
                       target.SDT = source.SDT, target.DiaChi = source.DiaChi
        WHEN NOT MATCHED THEN 
            INSERT (ID_TaiKhoan, HoTen, CCCD, SDT, DiaChi)
            VALUES (source.ID_TaiKhoan, source.HoTen, source.CCCD, source.SDT, source.DiaChi);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    // Thêm tham số vào lệnh SQL
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = ID_TaiKhoan;
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = HoTen ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@CCCD", SqlDbType.NVarChar).Value = CCCD ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = SDT ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = DiaChi ?? (object)DBNull.Value;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery(); // Thực thi MERGE
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật hoặc thêm mới thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không có thay đổi nào.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close(); // Đảm bảo kết nối được đóng
            }
        }

        public static void ThemDuKhach(string maChuyenDi, DateTime ngayBatDau, string cCCD, string ten, string sDT, string maTaiKhoan)
        {
            string query = @"
            INSERT INTO DanhSachDuKhach (ID_ChuyenDi, NgayBatDau, CCCD, Ten, SDT, ID_TaiKhoan)
            VALUES (@MaChuyenDi, @NgayBatDau, @CCCD, @Ten, @SDT, @MaTaiKhoan);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;
                    cmd.Parameters.Add("@CCCD", SqlDbType.NVarChar).Value = cCCD;
                    cmd.Parameters.Add("@Ten", SqlDbType.NVarChar).Value = ten;
                    cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = sDT;
                    cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;

                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static bool KiemTraDuKhachTonTai(string idChuyenDi, DateTime ngayBatDau, string cccd)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM DanhSachDuKhach 
        WHERE ID_ChuyenDi = @ID_ChuyenDi 
          AND NgayBatDau = @NgayBatDau 
          AND CCCD = @CCCD";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@ID_ChuyenDi", SqlDbType.NVarChar).Value = idChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;
                    cmd.Parameters.Add("@CCCD", SqlDbType.NVarChar).Value = cccd;

                    sqlcon.Open();
                    int count = (int)cmd.ExecuteScalar();
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
                sqlcon.Close();
            }
        }


        public static void ThemDSDangKy(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau, int soLuong, int? maHoaDon)
        {
            string query = @"
    INSERT INTO DanhSachDangKy (ID_TaiKhoan, ID_ChuyenDi, NgayBatDau, SoLuong, ID_HoaDon)
    VALUES (@MaTaiKhoan, @MaChuyenDi, @NgayBatDau, @SoLuong, @ID_HoaDon);";

            using (SqlCommand cmd = new SqlCommand(query, sqlcon))
            {
                cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;
                cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;
                cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = soLuong;
                cmd.Parameters.Add("@ID_HoaDon", SqlDbType.Int).Value = maHoaDon;

                sqlcon.Open();
                cmd.ExecuteNonQuery();
                sqlcon.Close();
            }
        }

        public static bool isInDSDangKy(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM DanhSachDangKy 
        WHERE ID_TaiKhoan = @MaTaiKhoan 
              AND ID_ChuyenDi = @MaChuyenDi 
              AND NgayBatDau = @NgayBatDau";

            using (SqlCommand cmd = new SqlCommand(query, sqlcon))
            {
                cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;
                cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;

                sqlcon.Open();
                int count = (int)cmd.ExecuteScalar();
                sqlcon.Close();

                return count > 0; // Trả về true nếu đã tồn tại, false nếu chưa có
            }
        }


        public static string SoTienThanhToan(string maChuyenDi, DateTime ngayBatDau, int soLuong)
        {
            string query = "SELECT Gia FROM ChuyenDi WHERE ID_ChuyenDi = @MaChuyenDi";
            int gia = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;

                    sqlcon.Open();
                    object result = cmd.ExecuteScalar();
                    sqlcon.Close();

                    if (result != null && result != DBNull.Value)
                    {
                        gia = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }

            return (gia * soLuong).ToString("C0", CultureInfo.GetCultureInfo("vi-VN"));
        }

        public static int SoTienThanhToanInt(string maChuyenDi, DateTime ngayBatDau, int soLuong)
        {
            string soTienChuoi = SoTienThanhToan(maChuyenDi, ngayBatDau, soLuong);

            // Loại bỏ ký tự không phải số (ví dụ: ký hiệu tiền tệ "₫")
            string soTienSo = new string(soTienChuoi.Where(char.IsDigit).ToArray());

            if (int.TryParse(soTienSo, out int soTien))
            {
                return soTien;
            }
            return 0; // Trả về 0 nếu có lỗi chuyển đổi
        }

        public static DataTable DanhSachDanhGia(string ID_ChuyenDi)
        {
            string query = @"
        SELECT COALESCE(NULLIF(tt.HoTen, ''), tk.Email) AS Ten, dg.BinhLuan, dg.SoSao 
        FROM DanhGia dg
        JOIN TaiKhoan tk ON dg.ID_TaiKhoan = tk.ID_TaiKhoan
        LEFT JOIN ThongTinCaNhan tt ON tk.ID_TaiKhoan = tt.ID_TaiKhoan
        WHERE dg.ID_ChuyenDi = @IDChuyenDi;";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = ID_ChuyenDi;

                    sqlcon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Columns.Add("Tên", typeof(string));
                        dt.Columns.Add("Bình Luận", typeof(string));
                        dt.Columns.Add("Sao", typeof(int));

                        while (reader.Read())
                        {
                            dt.Rows.Add(
                                reader["Ten"].ToString(),
                                reader["BinhLuan"].ToString(),
                                Convert.ToInt32(reader["SoSao"])
                            );
                        }
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

            return dt;
        }


        public static void ThemDanhGia(string maTaiKhoan, string maChuyenDi, string binhLuan, int sao)
        {
            string query = @"
        INSERT INTO DanhGia (ID_TaiKhoan, ID_ChuyenDi, BinhLuan, SoSao)
        VALUES (@IDTaiKhoan, @IDChuyenDi, @BinhLuan, @Sao);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd.Parameters.Add("@BinhLuan", SqlDbType.NVarChar).Value = binhLuan ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Sao", SqlDbType.Int).Value = sao;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm đánh giá thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm đánh giá.");
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

        public static void AddYeuCau(string ID_TaiKhoan, string ID_ChuyenDi, DateTime NgayBatDau, int SoLuong)
        {
            string checkQuery = @"
        SELECT COUNT(*) FROM YeuCau 
        WHERE ID_TaiKhoan = @IDTaiKhoan 
        AND ID_ChuyenDi = @IDChuyenDi 
        AND NgayBatDau = @NgayBatDau;";

            string insertQuery = @"
        INSERT INTO YeuCau (ID_TaiKhoan, ID_ChuyenDi, NgayBatDau, SoLuong) 
        VALUES (@IDTaiKhoan, @IDChuyenDi, @NgayBatDau, @SoLuong);";

            try
            {
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, sqlcon))
                {
                    checkCmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = ID_TaiKhoan;
                    checkCmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = ID_ChuyenDi;
                    checkCmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = NgayBatDau;

                    sqlcon.Open();
                    int count = (int)checkCmd.ExecuteScalar();
                    sqlcon.Close();

                    if (count > 0)
                    {
                        MessageBox.Show("Yêu cầu đã tồn tại. Không thể thêm trùng.");
                        return;
                    }

                    MessageBox.Show("Đã gửi yêu cầu thành công!");

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, sqlcon))
                    {
                        insertCmd.Parameters.Add("@IDTaiKhoan", SqlDbType.NVarChar).Value = ID_TaiKhoan;
                        insertCmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = ID_ChuyenDi;
                        insertCmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = NgayBatDau;
                        insertCmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = SoLuong;

                        sqlcon.Open();
                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        sqlcon.Close();
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

        public static DataTable LayDanhSachDangKyTheoTaiKhoan(string maTaiKhoan)
        {
            string query = @"
        SELECT 
            dsdk.ID_ChuyenDi, 
            dsdk.NgayBatDau, 
            dsdk.SoLuong, 
            hd.TrangThai
        FROM DanhSachDangKy dsdk
        LEFT JOIN HoaDon hd ON dsdk.ID_HoaDon = hd.ID_HoaDon
        WHERE dsdk.ID_TaiKhoan = @MaTaiKhoan";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Kiểm tra và thêm cột nếu chưa có
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add("Mã Chuyến Đi", typeof(string));
                        dt.Columns.Add("Ngày Bắt Đầu", typeof(DateTime));
                        dt.Columns.Add("Số Lượng", typeof(int));
                        dt.Columns.Add("Trạng Thái", typeof(string));
                    }

                    // Đọc dữ liệu từ SQL vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["NgayBatDau"],
                            reader["SoLuong"],
                            reader["TrangThai"] != DBNull.Value ? reader["TrangThai"].ToString() : "Chưa có hóa đơn"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                sqlcon.Close();
            }

            return dt;
        }




        public static DataTable LayDanhSachDuKhach(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau)
        {
            string query = @"
        SELECT
            Ten,
            SDT, 
            CCCD
        FROM DanhSachDuKhach
        WHERE ID_TaiKhoan = @MaTaiKhoan AND ID_ChuyenDi = @MaChuyenDi AND NgayBatDau = @NgayBatDau";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable trước khi duyệt dữ liệu
                    dt.Columns.Add("Họ Tên", typeof(string));
                    dt.Columns.Add("Số Điện Thoại", typeof(string));
                    dt.Columns.Add("CCCD", typeof(string));

                    // Đọc từng dòng và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["Ten"].ToString(),
                            reader["SDT"].ToString(),
                            reader["CCCD"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                sqlcon.Close();
            }

            return dt;
        }

        public static void huyVeChuyenDi(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau)
        {
            string queryXoaDuKhach = @"
        DELETE FROM DanhSachDuKhach
        WHERE ID_TaiKhoan = @MaTaiKhoan 
              AND ID_ChuyenDi = @MaChuyenDi 
              AND NgayBatDau = @NgayBatDau";

            string queryXoaDangKy = @"
        DELETE FROM DanhSachDangKy
        WHERE ID_TaiKhoan = @MaTaiKhoan 
              AND ID_ChuyenDi = @MaChuyenDi 
              AND NgayBatDau = @NgayBatDau";

            try
            {
                using (SqlCommand cmd1 = new SqlCommand(queryXoaDuKhach, sqlcon))
                using (SqlCommand cmd2 = new SqlCommand(queryXoaDangKy, sqlcon))
                {
                    cmd1.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;
                    cmd1.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd1.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;

                    cmd2.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;
                    cmd2.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd2.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;

                    sqlcon.Open();
                    cmd1.ExecuteNonQuery(); // Xóa DanhSachDuKhach trước
                    int rowsAffected = cmd2.ExecuteNonQuery(); // Xóa DanhSachDangKy

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                sqlcon.Close();
            }
        }

        public static DataTable LayChiTietChuyenDi(string maChuyenDi)
        {
            string query = @"
        SELECT 
            ID_ChuyenDi, 
            TenChuyenDi, 
            HanhTrinh, 
            SoNgayDi, 
            Gia
        FROM ChuyenDi
        WHERE ID_ChuyenDi = @MaChuyenDi";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Kiểm tra và thêm cột nếu chưa có
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add("Mã Chuyến Đi", typeof(string));
                        dt.Columns.Add("Tên Chuyến Đi", typeof(string));
                        dt.Columns.Add("Hành Trình", typeof(string));
                        dt.Columns.Add("Số Ngày Đi", typeof(int));
                        dt.Columns.Add("Giá", typeof(int));
                    }

                    // Đọc dữ liệu từ SQL vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["ID_ChuyenDi"].ToString(),
                            reader["TenChuyenDi"].ToString(),
                            reader["HanhTrinh"].ToString(),
                            reader["SoNgayDi"],
                            reader["Gia"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                sqlcon.Close();
            }

            return dt;
        }

        public static bool KiemTraLichTrinh(string idChuyenDi, DateTime ngayBatDau)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM LichTrinh 
        WHERE ID_ChuyenDi = @ID_ChuyenDi 
        AND NgayBatDau = @NgayBatDau";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@ID_ChuyenDi", SqlDbType.NVarChar).Value = idChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;

                    sqlcon.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0; // Trả về true nếu có lịch trình
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
            finally
            {
                sqlcon.Close();
            }
        }

        public static bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            string checkQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE Email = @Email AND MatKhau = HASHBYTES('SHA2_256', @OldPassword)";
            string updateQuery = "UPDATE TaiKhoan SET MatKhau = HASHBYTES('SHA2_256', @NewPassword) WHERE Email = @Email";

            try
            {
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, sqlcon))
                {
                    checkCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = email;
                    checkCmd.Parameters.Add("@OldPassword", SqlDbType.NVarChar, 255).Value = oldPassword;

                    sqlcon.Open();
                    int count = (int)checkCmd.ExecuteScalar();
                    sqlcon.Close();

                    if (count == 0)
                    {
                        MessageBox.Show("Mật khẩu cũ không chính xác!");
                        return false;
                    }
                }

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, sqlcon))
                {
                    updateCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = email;
                    updateCmd.Parameters.Add("@NewPassword", SqlDbType.NVarChar, 255).Value = newPassword;

                    sqlcon.Open();
                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    sqlcon.Close();

                    return rowsAffected > 0;
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

        public static bool InsertHoaDon(string ID_TaiKhoan, string ID_ChuyenDi, DateTime NgayBatDau, int SoLuong, int TongTien, string TrangThai)
        {
            string query = "INSERT INTO HoaDon (ID_TaiKhoan, ID_ChuyenDi, NgayBatDau, SoLuong, TongTien, TrangThai) VALUES (@ID_TaiKhoan, @ID_ChuyenDi, @NgayBatDau, @SoLuong, @TongTien, @TrangThai)";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.AddWithValue("@ID_TaiKhoan", ID_TaiKhoan);
                    cmd.Parameters.AddWithValue("@ID_ChuyenDi", ID_ChuyenDi);
                    cmd.Parameters.AddWithValue("@NgayBatDau", NgayBatDau);
                    cmd.Parameters.AddWithValue("@SoLuong", SoLuong);
                    cmd.Parameters.AddWithValue("@TongTien", TongTien);
                    cmd.Parameters.AddWithValue("@TrangThai", TrangThai);

                    try
                    {
                        sqlcon.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Trả về true nếu thêm thành công
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi thêm hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
            finally
            {
                sqlcon.Close(); // Ensure the connection is closed
            }
        }

        public static int? GetIDHoaDon(string ID_TaiKhoan, string ID_ChuyenDi, DateTime NgayBatDau)
        {
            string query = @"
        SELECT ID_HoaDon 
        FROM HoaDon 
        WHERE ID_TaiKhoan = @ID_TaiKhoan 
        AND ID_ChuyenDi = @ID_ChuyenDi 
        AND NgayBatDau = @NgayBatDau";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@ID_TaiKhoan", SqlDbType.NVarChar).Value = ID_TaiKhoan;
                    cmd.Parameters.Add("@ID_ChuyenDi", SqlDbType.NVarChar).Value = ID_ChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = NgayBatDau;

                    sqlcon.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy ID_HoaDon: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
            }
        }
    }
}
