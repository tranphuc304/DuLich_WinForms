using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                conditions.Add("cd.HanhTrinh LIKE @Dest");
            }

            if (start != new DateTime(1000, 1, 1))
            {
                conditions.Add("CAST(lt.NgayBatDau AS DATE) = @StartDate");
            }

            int minPrice = 0, maxPrice = 0;
            if (!string.IsNullOrEmpty(priceRange))
            {
                string[] priceSplit = priceRange.Replace(".", "").Trim().Split('-');
                minPrice = int.Parse(priceSplit[0]);
                maxPrice = int.Parse(priceSplit[1]);

                conditions.Add("cd.Gia BETWEEN @MinPrice AND @MaxPrice");
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

        public static void ThemDuKhachDK(string maChuyenDi, DateTime ngayBatDau, string cCCD, string ten, string sDT)
        {
            string query = @"
        MERGE INTO DanhSachDuKhach AS target
        USING (SELECT @MaChuyenDi AS ID_ChuyenDi, @NgayBatDau AS NgayBatDau, @CCCD AS CCCD, 
                      @Ten AS Ten, @SDT AS SDT) AS source
        ON target.ID_ChuyenDi = source.ID_ChuyenDi AND target.NgayBatDau = source.NgayBatDau AND target.CCCD = source.CCCD
        WHEN NOT MATCHED THEN 
            INSERT (ID_ChuyenDi, NgayBatDau, CCCD, Ten, SDT)
            VALUES (source.ID_ChuyenDi, source.NgayBatDau, source.CCCD, source.Ten, source.SDT);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;
                    cmd.Parameters.Add("@CCCD", SqlDbType.NVarChar).Value = cCCD;
                    cmd.Parameters.Add("@Ten", SqlDbType.NVarChar).Value = ten;
                    cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = sDT;

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

        public static void ThemDanhSachDK(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau, int soLuong, string trangThai)
        {
            string query = @"
        MERGE INTO DanhSachDangKy AS target
        USING (SELECT @MaTaiKhoan AS ID_TaiKhoan, @MaChuyenDi AS ID_ChuyenDi, @NgayBatDau AS NgayBatDau, 
                      @SoLuong AS SoLuong, @TrangThai AS TrangThai) AS source
        ON target.ID_TaiKhoan = source.ID_TaiKhoan AND target.ID_ChuyenDi = source.ID_ChuyenDi AND target.NgayBatDau = source.NgayBatDau
        WHEN MATCHED THEN 
            UPDATE SET target.SoLuong = target.SoLuong + source.SoLuong
        WHEN NOT MATCHED THEN 
            INSERT (ID_TaiKhoan, ID_ChuyenDi, NgayBatDau, SoLuong, TrangThai)
            VALUES (source.ID_TaiKhoan, source.ID_ChuyenDi, source.NgayBatDau, source.SoLuong, source.TrangThai);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;
                    cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = soLuong;
                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar).Value = trangThai;

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

        public static void ThemDanhSachDKy(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau, int soLuong, string trangThai)
        {
            string queryLichTrinh = @"
        MERGE INTO LichTrinh AS target
        USING (SELECT @MaChuyenDi AS ID_ChuyenDi, @NgayBatDau AS NgayBatDau, NULL AS ID_HDV) AS source
        ON target.ID_ChuyenDi = source.ID_ChuyenDi AND target.NgayBatDau = source.NgayBatDau
        WHEN NOT MATCHED THEN 
            INSERT (ID_ChuyenDi, NgayBatDau, ID_HDV)
            VALUES (source.ID_ChuyenDi, source.NgayBatDau, source.ID_HDV);";

            try
            {
                using (SqlCommand cmd = new SqlCommand(queryLichTrinh, sqlcon))
                {
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;

                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                    sqlcon.Close();
                }

                // Gọi hàm thêm đăng ký
                ThemDanhSachDK(maTaiKhoan, maChuyenDi, ngayBatDau, soLuong, trangThai);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static int SoTienThanhToan(string maChuyenDi, DateTime ngayBatDau, int soLuong)
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

            return gia * soLuong;
        }

        public static void CapNhatTrangThaiDangKy(string maTaiKhoan, string maChuyenDi, DateTime ngayBatDau, string trangThaiMoi)
        {
            string query = @"
        UPDATE DanhSachDangKy
        SET TrangThai = @TrangThai
        WHERE ID_TaiKhoan = @MaTaiKhoan AND ID_ChuyenDi = @MaChuyenDi AND NgayBatDau = @NgayBatDau";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@MaTaiKhoan", SqlDbType.NVarChar).Value = maTaiKhoan;
                    cmd.Parameters.Add("@MaChuyenDi", SqlDbType.NVarChar).Value = maChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = ngayBatDau;
                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar).Value = trangThaiMoi;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlcon.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật trạng thái thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bản ghi để cập nhật.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        public static DataTable DanhSachDanhGia(string MaChuyenDi, DateTime NgayBatDau)
        {
            string query = @"
        SELECT tt.Ten, dg.BinhLuan, dg.Sao 
        FROM DanhGia dg
        JOIN TaiKhoan tk ON dg.ID_TaiKhoan = tk.ID_TaiKhoan
        JOIN ThongTinCaNhan tt ON tk.ID_TaiKhoan = tt.ID_TaiKhoan
        WHERE dg.ID_ChuyenDi = @IDChuyenDi AND dg.NgayBatDau = @NgayBatDau;";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@IDChuyenDi", SqlDbType.NVarChar).Value = MaChuyenDi;
                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.Date).Value = NgayBatDau;

                    sqlcon.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Thêm cột vào DataTable
                    dt.Columns.Add("Tên", typeof(string));
                    dt.Columns.Add("Bình Luận", typeof(string));
                    dt.Columns.Add("Sao", typeof(int));

                    // Đọc dữ liệu và thêm vào DataTable
                    while (reader.Read())
                    {
                        dt.Rows.Add(
                            reader["Ten"].ToString(),
                            reader["BinhLuan"].ToString(),
                            Convert.ToInt32(reader["Sao"])
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

        public static void ThemDanhGia(string maTaiKhoan, string maChuyenDi, string binhLuan, int sao)
        {
            string query = @"
        INSERT INTO DanhGia (ID_TaiKhoan, ID_ChuyenDi, BinhLuan, Sao)
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

    }
}
