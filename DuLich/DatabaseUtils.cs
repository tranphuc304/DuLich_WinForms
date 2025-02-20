using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuLich
{
    class DatabaseUtils
    {
         static SqlConnection sqlcon = new SqlConnection(@"Data Source=MEANKHOIII;Initial Catalog=DuLichDatabase;Integrated Security=True");

        public static bool RegisterUser(string username, string password)
        {
            string query = "IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE Email = @Username) " +
                           "INSERT INTO TaiKhoan (ID_TaiKhoan, Email, MatKhau) VALUES (@UserId, @Username, HASHBYTES('SHA2_256', @Password));";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = GenerateUserId();
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

                    sqlcon.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
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

        private static string GenerateUserId()
        {
            string newId = "U0001"; // ID mặc định nếu chưa có tài khoản nào
            string query = "SELECT MAX(ID_TaiKhoan) FROM TaiKhoan WHERE ID_TaiKhoan LIKE 'U%'";

            try
            {
                sqlcon.Open();
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        string lastId = result.ToString(); // Ví dụ: "U0025"
                        int number = int.Parse(lastId.Substring(1)); // Lấy 4 số cuối
                        newId = $"U{(number + 1):D4}"; // Tăng lên 1, giữ định dạng 4 chữ số
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

            return newId;
        }

        public static bool AuthenticateUser(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE Email = @Username AND MatKhau = HASHBYTES('SHA2_256', @Password)";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

                    sqlcon.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
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

        public static bool IsUsernameExists(string username)
        {
            bool exists = false;
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE Email = @Username";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    try
                    {
                        sqlcon.Open();
                        int count = (int)cmd.ExecuteScalar();
                        exists = count > 0; // Nếu count > 0 thì tên đăng nhập đã tồn tại
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi kết nối database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            return exists;
        }

        public static bool UpdatePassword(string email, string newPassword)
        {
            string query = "UPDATE TaiKhoan SET MatKhau = HASHBYTES('SHA2_256', @NewPassword) WHERE Email = @Email";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                    try
                    {
                        sqlcon.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
