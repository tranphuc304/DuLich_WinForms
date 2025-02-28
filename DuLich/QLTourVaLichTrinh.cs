using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using admin;

namespace DuLich
{
    public partial class QLTourVaLichTrinh : Form
    {
        public QLTourVaLichTrinh()
        {
            InitializeComponent();
        }

        private void btn_qltour_Click(object sender, EventArgs e)
        {
            Hide();

            QLTour qLTour = new QLTour();
            qLTour.ShowDialog();

            Show();
        }

        private void lbl_qllichtrinh_Click(object sender, EventArgs e)
        {
            Hide();

            QLLichTrinh qLLichTrinh = new QLLichTrinh();
            qLLichTrinh.ShowDialog();

            Show();
        }
    }
}
