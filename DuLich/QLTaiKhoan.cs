using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace admin
{
    public partial class QLTaiKhoan : Form
    {
        public QLTaiKhoan()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUserList(); // Gọi hàm để hiển thị danh sách
        }
        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
                DataTable dt = new DataTable();
               

                dt.Rows.Add("software", "U001");
                dt.Rows.Add("Hello", "U002");

                guna2DataGridView1.DataSource = dt;
            

        }
        void LoadUserList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Tên đăng nhập");
            dt.Columns.Add("Mã tài khoản");

            dt.Rows.Add("software", "U001");
            dt.Rows.Add("Hello", "U002");
            dt.Rows.Add("Hihihi", "U003");
            dt.Rows.Add("Truong050123", "U004");

            // Gán dữ liệu vào Guna2DataGridView
            guna2DataGridView1.DataSource = dt;
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
