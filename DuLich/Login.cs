using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DuLich
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.username))
            {
                txt_username.Text = Properties.Settings.Default.username;

                txt_password.Select();
            }
        }

        private void lbl_dangky_Click(object sender, EventArgs e)
        {
            this.Hide();

            Signup signup = new Signup();

            signup.ShowDialog();
        }

        private void btn_dangnhap_Click(object sender, EventArgs e)
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

            if (!SystemQuery.IsUsernameExists(username))
            {
                MessageBox.Show("Không tồn tại tài khoản này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SystemQuery.AuthenticateUser(username, password))
            {
                MessageBox.Show("Đăng nhập thành công!");

                if (switch_saveuser.Checked)
                {
                    Properties.Settings.Default.username = txt_username.Text;
                    Properties.Settings.Default.password = txt_password.Text;
                    Properties.Settings.Default.Save();
                } else
                {
                    Properties.Settings.Default.username = null;
                    Properties.Settings.Default.password = null;
                    Properties.Settings.Default.Save();
                }

                this.Hide();

                TrangChu trangchu = new TrangChu();

                trangchu.ShowDialog();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void lbl_saveuser_Click(object sender, EventArgs e)
        {
            switch_saveuser.Checked = !switch_saveuser.Checked;
        }

        private void lbl_forgotpass_Click(object sender, EventArgs e)
        {
            this.Hide();

            ForgotPassword forgotPassword = new ForgotPassword();

            forgotPassword.ShowDialog();
        }
    }
}
