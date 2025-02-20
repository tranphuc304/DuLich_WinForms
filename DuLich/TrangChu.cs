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

        private void btn_logout_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.password = null;
            Properties.Settings.Default.Save();

            this.Hide();

            Login login = new Login();

            login.ShowDialog();
        }

        private void guna2PictureBox8_Click(object sender, EventArgs e)
        {

        }
    }
}
