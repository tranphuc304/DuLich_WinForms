using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuLich;
using DuLich.DatabaseUtils;
using static System.Windows.Forms.AxHost;

namespace admin
{
    public partial class QLTaiKhoan : Form
    {

        string Email;
        string MaTaiKhoan;

        public QLTaiKhoan()
        {
            InitializeComponent();
            ResetInfo();
            Email = "";
            MaTaiKhoan = "";

            deactiveButtons();
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
        private void ResetInfo()
        {
            ResetTextPnlAccount();
            ResetTextPnlInfo();

            dgv_dangky.ReadOnly = false;
            dgv_dangky.Rows.Clear();
            dgv_dangky.ReadOnly = true;
        }

        private void activeButtons()
        {
            btn_reset.Enabled = true;
            btn_accxoa.Enabled = true;
            btn_changeaccounttype.Enabled = true;
        }

        private void deactiveButtons()
        {
            btn_reset.Enabled = false;
            btn_accxoa.Enabled = false;
            btn_changeaccounttype.Enabled = false;
        }

        private void LoadData()
        {
            try
            {
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

                    if (data_per.Rows.Count == 0)
                        return;

                    DataRow row_data = data_per.Rows[0];
                    lb_value_ten.Text = row_data[1].ToString();
                    lb_value_sdt.Text = row_data[2].ToString();
                    lb_value_diachi.Text = row_data[4].ToString();
                    lb_value_cccd.Text = row_data[3].ToString();

                    try
                    {
                        dgv_dangky.DataSource = AdminQuery.GetDataJoinTour(dgv_dataacc.Rows[r].Cells[2].Value.ToString());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Không lấy được nội dung!");
                    }

                    tb_modify_tendn.Text = dgv_dataacc.Rows[r].Cells[0].Value.ToString();
                    tb_modify_mk.Text = dgv_dataacc.Rows[r].Cells[1].Value.ToString();

                    tb_ten.Text = row_data[1].ToString();
                    tb_sdt.Text = row_data[2].ToString();
                    tb_cccd.Text = row_data[3].ToString();
                    tb_diachi.Text = row_data[4].ToString();
                }
                else
                {
                    tb_modify_tendn.Text = dgv_dataacc.Rows[r].Cells[0].Value.ToString();
                    tb_modify_mk.Text = dgv_dataacc.Rows[r].Cells[1].Value.ToString();
                }

                Email = dgv_dataacc.Rows[r].Cells[0].Value.ToString();
                MaTaiKhoan = dgv_dataacc.Rows[r].Cells[2].Value.ToString();

                tb_modify_tendn.Enabled = false;
                tb_modify_mk.Enabled = false;

                activeButtons();
            }
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

                    ResetInfo();
                    Email = "";
                    MaTaiKhoan = "";

                    tb_modify_tendn.Enabled = true;
                    tb_modify_mk.Enabled = true;

                    deactiveButtons();
                }
            }
            else { MessageBox.Show("Bạn chưa chọn tài khoản muốn xóa!"); }
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            if (!tb_modify_tendn.Text.Trim().Equals("") && !tb_modify_mk.Text.Trim().Equals(""))
            {
                if (!AdminQuery.Is_Account_Exist(tb_modify_tendn.Text))
                {
                    try
                    {
                        SystemQuery.RegisterAccount(tb_modify_tendn.Text, tb_modify_mk.Text, "U");
                        AdminQuery.AddInfoPersonal(AdminQuery.GetIDAccount(tb_modify_tendn.Text), tb_ten.Text, tb_sdt.Text, tb_diachi.Text, tb_cccd.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Không thêm được. Lỗi rồi!!!");
                    }
                } else
                {
                    UserQuery.updateThongTinCaNhan(AdminQuery.GetIDAccount(tb_modify_tendn.Text), tb_ten.Text, tb_cccd.Text, tb_sdt.Text, tb_diachi.Text);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập Tên tài khoản và mật khẩu!");
            }

            ResetInfo();
            LoadData();
        }

        private void btn_huy_Click(object sender, EventArgs e)
        {
            ResetInfo();
            Email = "";
            MaTaiKhoan = "";

            tb_modify_tendn.Enabled = true;
            tb_modify_mk.Enabled = true;

            deactiveButtons();
        }

        private void btn_changeaccounttype_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult xacNhan;

                xacNhan = MessageBox.Show("Bạn có chắc muốn thay đổi loại tài khoản sang: " + (MaTaiKhoan.Contains("A") ? "Admin" : "User") + " ?", "Thay đổi?",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (xacNhan == DialogResult.Yes)
                {
                    AdminQuery.DoiMaTaiKhoan(MaTaiKhoan);

                    LoadData();

                    string newID = SystemQuery.GetIDFromEmail(Email);

                    MessageBox.Show("Chuyển loại tài khoản thành công sang: " + (newID.Contains("A") ? "Admin" : "User"));
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
