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

            this.AcceptButton = btn_signup;
        }

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

            if (!Utils.IsValidEmail(username))
            {
                MessageBox.Show("Tên đăng nhập phải là email!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DatabaseUtils.RegisterUser(username, password))
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
