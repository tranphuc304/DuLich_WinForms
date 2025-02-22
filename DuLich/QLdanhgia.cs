using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuLich.DatabaseUtils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DuLich
{
    public partial class QLDanhGia : Form
    {
        public QLDanhGia()
        {
            InitializeComponent();
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadDanhGia_dgv()
        {
            try
            {

                dgvQLDanhGia.DataSource = AdminQuery.Load_dgvQLDanhGia();
                dgvQLDanhGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvQLDanhGia.AutoResizeColumns();
                dgvQLDanhGia.AllowUserToResizeColumns = true;
                dgvQLDanhGia.AllowUserToOrderColumns = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FormQuanLyDanhGia_Load(object sender, EventArgs e)
        {
            this.cbbMaTour.DataSource = AdminQuery.getDSChuyenDi();
            this.cbbMaTour.DisplayMember = "Mã Tour";
            LoadDanhGia_dgv();
        }

        private void btnAVGRate_Click(object sender, EventArgs e)
        {
            string IDTour = this.cbbMaTour.Text;
            MessageBox.Show("Số Sao Trung Bình Của Mã Tour " + IDTour + ":" + AdminQuery.AvgRate(IDTour));
        }

        private void btn_Find_Click(object sender, EventArgs e)
        {
            string IDTour = this.cbbMaTour.Text;
            dgvQLDanhGia.DataSource = AdminQuery.FilterRate(IDTour);
        }
    }
}
