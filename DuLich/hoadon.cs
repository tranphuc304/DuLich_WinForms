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
    public partial class HoaDon : Form
    {
        public HoaDon(string ID_TaiKhoan)
        {
            InitializeComponent();
            this.ID_TaiKhoan = ID_TaiKhoan;
        }

        string ID_TaiKhoan;

        private void btn_back_Click(object sender, EventArgs e)
        {
            Hide();

            TrangChu trangchu = new TrangChu(ID_TaiKhoan);
            trangchu.ShowDialog();
        }
    }
}
