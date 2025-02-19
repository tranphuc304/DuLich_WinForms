using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuLich
{
    public partial class TrangChu : Form
    {
        private Dictionary<Guna.UI2.WinForms.Guna2Button, bool> buttonStates = new Dictionary<Guna.UI2.WinForms.Guna2Button, bool>();

        public TrangChu()
        {

            InitializeComponent();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

        }

        private void ChangeButtonIcon(Guna2Button clickedButton, string defaultIcon, string selectedIcon)
        {
            // 🔹 Reset trạng thái của tất cả button
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Guna2Button btn)
                {
                    btn.FillColor = Color.White;  // Reset tất cả về màu trắng
                    btn.ForeColor = Color.Black; // Reset chữ về màu đen

                    // 🔹 Đặt icon mặc định cho tất cả button
                    string defaultImagePath = Path.Combine(Application.StartupPath, "Resources", defaultIcon);
                    if (File.Exists(defaultImagePath))
                    {
                        btn.Image = Image.FromFile(defaultImagePath);
                    }
                }
            }

            // 🔹 Đổi màu button được chọn
            clickedButton.FillColor = Color.SeaGreen;
            clickedButton.ForeColor = Color.White;

            // 🔹 Đổi icon của button được chọn
            string selectedImagePath = Path.Combine(Application.StartupPath, "Resources", selectedIcon);
            if (File.Exists(selectedImagePath))
            {
                clickedButton.Image = Image.FromFile(selectedImagePath);
            }
            else
            {
                MessageBox.Show($"Không tìm thấy hình ảnh: {selectedImagePath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ChangeButtonIcon(guna2Button1, "icons8-home-50.png", "icons8-home-50 (1).png");
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ChangeButtonIcon(guna2Button2, "icons8-google-100.png", "icons8-google-100 (1).png");
        }

        private void TrangChu_Load(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        // Gọi hàm khi button được nhấn


    }
}
