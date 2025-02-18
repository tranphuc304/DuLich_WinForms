using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace DuLich
{
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
        }

        SqlConnection sqlcon = new SqlConnection(@"Data Source=MEANKHOIII;Initial Catalog=DuLichDatabase;Integrated Security=True");

        private void lbl_login_Click(object sender, EventArgs e)
        {
            this.Hide();

            Login login = new Login();

            login.ShowDialog();

        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            String username, password;

            username = txt_username.Text;
            password = txt_password.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (RegisterUser(username, password))
            {
                MessageBox.Show("Đăng ký thành công! Hãy đăng nhập lại!");

                this.Hide();

                Login login = new Login();

                login.ShowDialog();
            }
            else
            {
                MessageBox.Show("Tài khoản đó đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool RegisterUser(string username, string password)
        {
            string query = "IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = @Username) " +
                           "INSERT INTO TaiKhoan (TenDangNhap, MatKhau) VALUES (@Username, HASHBYTES('SHA2_256', @Password));";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlcon))
                {
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

        private void btn_passeyeopen_Click(object sender, EventArgs e)
        {
            if (!txt_password.UseSystemPasswordChar)
            {
                txt_password.UseSystemPasswordChar = true;

                btn_passeyeclose.BringToFront();
            }
        }

        private void btn_passeyeclose_Click(object sender, EventArgs e)
        {
            if (txt_password.UseSystemPasswordChar)
            {
                txt_password.UseSystemPasswordChar = false;
                txt_password.PasswordChar = '\0'; // Allow text to be visible

                btn_passeyeopen.BringToFront();
            }
        }
    }
}
