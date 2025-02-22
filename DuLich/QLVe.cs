﻿using System;
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
    public partial class QLVe : Form
    {
        private string IDTour;
        private DateTime date;
        private int curr = 0;
        public QLVe()
        {
            InitializeComponent();
        }

        private void QLVe_Load(object sender, EventArgs e)
        {
            LoadData();
            this.cbbListTour.DataSource = AdminQuery.getDSChuyenDi();
            this.cbbListTour.DisplayMember = "Mã Tour";
            this.cbbListDate.DataSource = AdminQuery.LoadDateFollowTours(cbbListTour.Text);
            this.cbbListDate.DisplayMember = "Ngày khởi hành";
        }
        private void LoadData()
        {
            try
            {
                dgvTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvTicket.AllowUserToResizeColumns = false;
                dgvTicket.AllowUserToOrderColumns = false;
                dgvTicket.AllowUserToResizeRows = false;
                dgvTicket.DataSource = AdminQuery.LoadAllTickets();
            }
            catch (Exception)
            {
                MessageBox.Show("Không lấy được dữ liệu yêu cầu của khách hàng!");
            }
        }
        private void StatisticsTickets(string IDTour, DateTime date)
        {
            try
            {
                this.dgvTicket.DataSource = AdminQuery.LoadTickets(IDTour, date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void StatictisQuantity(string IDTour, DateTime date)
        {
            try
            {
                this.lblNow.Text = AdminQuery.CountTickets(IDTour, date).Rows[0][0].ToString();
                this.lblMax.Text = AdminQuery.CountTickets(IDTour, date).Rows[0][1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_Statistics_Click(object sender, EventArgs e)
        {
            if (this.cbbListDate.Text != "")
            {
                this.IDTour = this.cbbListTour.Text;
                this.date = Convert.ToDateTime(this.cbbListDate.Text);
                StatisticsTickets(this.IDTour, this.date);
                StatictisQuantity(this.IDTour, this.date);
            }
            else MessageBox.Show("Chuyến đi chưa có lịch trình hoạt động!", "Thông báo");
        }

        private void dgvTicket_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            curr = this.dgvTicket.CurrentCell.ColumnIndex;
            int n = this.dgvTicket.CurrentCell.RowIndex;
            this.IDTour = dgvTicket.Rows[n].Cells[0].Value.ToString();
            this.date = Convert.ToDateTime(dgvTicket.Rows[n].Cells[1].Value);
            this.txtName.Text = dgvTicket.Rows[n].Cells[3].Value.ToString();
            this.txtCCCD.Text = dgvTicket.Rows[n].Cells[2].Value.ToString();
            this.txtSDT.Text = dgvTicket.Rows[n].Cells[4].Value.ToString();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int n = this.dgvTicket.CurrentCell.RowIndex;
            string IDTour = this.dgvTicket.Rows[n].Cells[0].Value.ToString();
            string CCCD = this.dgvTicket.Rows[n].Cells[2].Value.ToString();
            DateTime startDay = Convert.ToDateTime(this.dgvTicket.Rows[n].Cells[1].Value);
            AdminQuery.DeletePassenger(IDTour, startDay, CCCD);
            StatisticsTickets(this.IDTour, this.date);
            StatictisQuantity(this.IDTour, this.date);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.lblNow.Text == "0")
            {
                MessageBox.Show("Bạn chưa chọn Chuyến đi !");
            }
            else
            {
                string CCCD = this.txtCCCD.Text;
                string name = this.txtName.Text;
                string sdt = this.txtSDT.Text;
                AdminQuery.AddPassenger(this.IDTour, this.date, CCCD, name, sdt);
                StatisticsTickets(this.IDTour, this.date);
                StatictisQuantity(this.IDTour, this.date);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string id = curr == 0 ? IDTour : "";
            DateTime ngaydi = curr == 1 ? date : new DateTime(1, 1, 1);
            string cccd = curr == 2 ? this.txtCCCD.Text : "";
            string name = curr == 3 ? this.txtName.Text : "";
            string sdt = curr == 4 ? this.txtSDT.Text : "";
            try
            {
                this.dgvTicket.DataSource = AdminQuery.FindPassenger(id, ngaydi, cccd, name, sdt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cbbListTour_SelectedValueChanged(object sender, EventArgs e)
        {
            this.cbbListDate.ResetText();
            this.cbbListDate.DataSource = AdminQuery.LoadDateFollowTours(cbbListTour.Text);
            this.cbbListDate.DisplayMember = "Ngày khởi hành";
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            LoadData();
            this.lblNow.Text = "0";
            this.lblMax.Text = "0";
            this.txtName.Text = String.Empty;
            this.txtCCCD.Text = String.Empty;
            this.txtSDT.Text = String.Empty;
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            this.dgvTicket.DataSource = AdminQuery.FindPassenger("", new DateTime(1, 1, 1), this.txtCCCD.Text, this.txtName.Text, this.txtSDT.Text);
        }
    }
}
