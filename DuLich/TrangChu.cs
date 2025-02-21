using DuLich.DatabaseUtils;
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

        private void TrangChu_Load(object sender, EventArgs e)
        {
            loadDSChuyenDi(UserQuery.getDSChuyenDi());
        }

        private void loadDSChuyenDi(DataTable source)
        {
            try
            {
                dgv_dschuyendi.DataSource = source;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
