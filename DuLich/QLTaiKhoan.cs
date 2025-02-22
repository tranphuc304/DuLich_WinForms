﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuLich;
using DuLich.DatabaseUtils;

namespace admin
{
    public partial class QLTaiKhoan : Form
    {

        string Email;
        string MaTaiKhoan;
        string MatKhau;

        private enum Modify
        {
            Khong,
            Them,
            Sua
        }
        private Modify state;
        public QLTaiKhoan()
        {
            InitializeComponent();
            state = Modify.Khong;
            ChangeStateModify();
            Email = "";
            MaTaiKhoan = "";
        }
        private void ResetTextPnlAccount()
        {
            tb_modify_tendn.ResetText();
            tb_modify_mk.ResetText();
        }
        private void ResetTextPnlInfo()
        {
            tb_ten.ResetText();
            tb_sdt.ResetText();
            tb_diachi.ResetText();
        }
        private void ChangeStateModify()
        {
            ResetTextPnlAccount();
            ResetTextPnlInfo();
            if (state == Modify.Khong || state == Modify.Them)
            {
                ResetTextPnlAccount();
                ResetTextPnlInfo();
                pnl_change_account.Enabled = state == Modify.Khong ? false : true;
                pnl_change_info.Enabled = state == Modify.Khong ? false : true;
            }
            else
            {
                pnl_change_account.Enabled = false;
                pnl_change_info.Enabled = true;
            }
        }
        private void LoadData()
        {
            try
            {
                dgv_dataacc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgv_dataacc.AutoResizeColumns();
                dgv_dataacc.AllowUserToResizeColumns = false;
                dgv_dataacc.AllowUserToOrderColumns = false;
                dgv_dataacc.DataSource = AdminQuery.GetAllDataAccount();
                dgv_dataacc.Columns["Mật khẩu"].Visible = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Không lấy được nội dung!");
            }
        }
        private void FormQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgv_dataacc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dgv_dataacc.CurrentCell.RowIndex;
            if (r < dgv_dataacc.Rows.Count - 1 && r >= 0)
            {
                if (AdminQuery.Is_InfoPersonal_Exist(dgv_dataacc.Rows[r].Cells[2].Value.ToString()))
                {
                    DataTable data_per = AdminQuery.GetDataPersonal(dgv_dataacc.Rows[r].Cells[2].Value.ToString());
                    DataRow row_data = data_per.Rows[0];
                    lb_value_ten.Text = row_data[1].ToString();
                    lb_value_sdt.Text = row_data[2].ToString();
                    lb_value_diachi.Text = row_data[3].ToString();
                    lb_value_email.Text = row_data[4].ToString();

                    try
                    {
                        dgv_dangky.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                        dgv_dangky.AutoResizeColumns();
                        dgv_dangky.AllowUserToResizeColumns = false;
                        dgv_dangky.AllowUserToOrderColumns = false;
                        dgv_dangky.DataSource = AdminQuery.GetDataJoinTour(dgv_dataacc.Rows[r].Cells[2].Value.ToString());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Không lấy được nội dung!");
                    }
                    if (state == Modify.Sua)
                    {
                        tb_modify_tendn.Text = dgv_dataacc.Rows[r].Cells[0].Value.ToString();
                        tb_modify_mk.Text = dgv_dataacc.Rows[r].Cells[1].Value.ToString();

                        tb_ten.Text = row_data[1].ToString();
                        tb_sdt.Text = row_data[2].ToString();
                        tb_diachi.Text = row_data[3].ToString();
                    }
                }
                else { MessageBox.Show("Chưa có thông tin người dùng!"); }

                Email = dgv_dataacc.Rows[r].Cells[0].Value.ToString();
                MaTaiKhoan = dgv_dataacc.Rows[r].Cells[2].Value.ToString();
            }
        }

        private void btn_accthem_Click(object sender, EventArgs e)
        {
            state = Modify.Them;
            ChangeStateModify();
        }

        private void btn_accsua_Click(object sender, EventArgs e)
        {
            state = Modify.Sua;
            ChangeStateModify();
        }

        private void btn_accxoa_Click(object sender, EventArgs e)
        {
            if (Email != "")
            {
                DialogResult traloi;
                traloi = MessageBox.Show("Bạn có chắc chắn xóa tài khoản " + Email + "?", "Trả lời",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (traloi == DialogResult.OK)
                {
                    AdminQuery.DeleteAccount(Email, MaTaiKhoan);
                    LoadData();
                }
            }
            else { MessageBox.Show("Bạn chưa chọn tài khoản muốn xóa!"); }
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            if (state == Modify.Them)
            {
                if (!tb_modify_tendn.Text.Trim().Equals("") && !tb_modify_mk.Text.Trim().Equals(""))
                {
                    if (!AdminQuery.Is_Account_Exist(tb_modify_tendn.Text))
                    {
                        try
                        {
                            SystemQuery.RegisterAccount(tb_modify_tendn.Text, tb_modify_mk.Text, "U");
                            AdminQuery.AddInfoPersonal(AdminQuery.GetIDAccount(tb_modify_tendn.Text), tb_ten.Text, tb_sdt.Text, tb_diachi.Text);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Không thêm được. Lỗi rồi!!!");
                        }
                        state = Modify.Khong;
                    }
                    else MessageBox.Show("Tên đăng nhập đã tồn tại!");
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập Tên tài khoản và mật khẩu!");
                }
            }
            else if (state == Modify.Sua)
            {
                if (!tb_modify_tendn.Text.Trim().Equals("") && !tb_modify_mk.Text.Trim().Equals(""))
                {
                    if (AdminQuery.Is_Account_Exist(tb_modify_tendn.Text))
                    {
                        try
                        {
                            AdminQuery.UpdateInfoPersonal(MaTaiKhoan, tb_ten.Text, tb_sdt.Text, tb_diachi.Text);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Không sửa được. Lỗi rồi!!!");
                        }
                        state = Modify.Khong;
                    }
                    else MessageBox.Show("Tên đăng nhập không tồn tại!");
                }
                else MessageBox.Show("Chọn người dùng để thay đổi thông tin");

            }
            if (state == Modify.Khong)
            {
                ChangeStateModify();
                LoadData();
            }
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            state = Modify.Khong;
            ChangeStateModify();
        }
    }
}
