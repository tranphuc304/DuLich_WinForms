using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuLich
{
    public partial class DangKyAdmin : Form
    {
        public DangKyAdmin()
        {
            InitializeComponent();
        }

        private string admincode = "admin2806";

        private void lbl_login_Click(object sender, EventArgs e)
        {
            this.Hide();

            DangNhap login = new DangNhap();

            login.ShowDialog();

        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            String username, password, admincode;

            username = txt_username.Text;
            password = txt_password.Text;
            admincode = txt_admincode.Text;

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

            if (!admincode.Equals(admincode)) 
            {
                MessageBox.Show("Mã đăng ký tài khoản sai!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SystemQuery.RegisterAccount(username, password, "A"))
            {
                MessageBox.Show("Đăng ký thành công! Hãy đăng nhập lại!");

                this.Hide();

                DangNhap login = new DangNhap();

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
