using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuLich.DatabaseUtils;
using Microsoft.IdentityModel.Tokens;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DuLich
{
    public partial class NhapThongTinHanhKhach : Form
    {
        public NhapThongTinHanhKhach(string ID_TaiKhoan, string ID_ChuyenDi, DateTime ngayBatDau)
        {
            this.ID_TaiKhoan = ID_TaiKhoan;
            this.ID_ChuyenDi = ID_ChuyenDi;
            this.ngayBatDau = ngayBatDau;

            InitializeComponent();

            dsChuyenDi = new DataTable();
            dsChuyenDi.Columns.Add("Họ Tên", typeof(string));
            dsChuyenDi.Columns.Add("Số Điện Thoại", typeof(string));
            dsChuyenDi.Columns.Add("CCCD", typeof(string));
        }

        string ID_TaiKhoan;
        string ID_ChuyenDi;
        DateTime ngayBatDau;

        int amountRemain = 0;

        string edit = null;

        DataTable dsChuyenDi;

        private void btn_back_Click(object sender, EventArgs e)
        {
            ChiTietChuyenDi chiTietChuyenDi = new ChiTietChuyenDi(ID_TaiKhoan, ID_ChuyenDi, ngayBatDau);
            Hide();
            chiTietChuyenDi.ShowDialog();
            Close();
        }

        private void btn_thanhtoan_Click(object sender, EventArgs e)
        {
            int count = dgv_dshanhkhach.Rows.Count;

            if (count == 0)
            {
                MessageBox.Show("Bạn chưa nhập hành khách nào!");

                return;
            }

            foreach (DataGridViewRow row in dgv_dshanhkhach.Rows)
            {
                if (!row.IsNewRow)
                {
                    string ten = row.Cells["HoTen"].Value.ToString();
                    string cccd = row.Cells["CCCD"].Value.ToString();
                    string sdt = row.Cells["SDT"].Value.ToString();

                    dsChuyenDi.Rows.Add(ten, sdt, cccd);
                }
            }

            if (UserQuery.isInDSDangKy(ID_TaiKhoan, ID_ChuyenDi, this.ngayBatDau))
            {    
                MessageBox.Show("Bạn đã đặt chuyến đi này rồi");
                return;
            }

            ThanhToan thanhToan = new ThanhToan(ID_TaiKhoan, ID_ChuyenDi, this.ngayBatDau, count, dsChuyenDi);
            Hide();
            thanhToan.ShowDialog();
            Close();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_sdt.ResetText();
            txt_hoten.ResetText();
            txt_cccd.ResetText();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_hoten.Text.IsNullOrEmpty() || txt_sdt.Text.IsNullOrEmpty() || txt_cccd.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Bạn phải nhập đủ họ tên, sđt và cccd!");

                return;
            }

            if (amountRemain == 0)
            {
                MessageBox.Show("Bạn không thể mua thêm vé!");

                return;
            }

            if (UserQuery.KiemTraDuKhachTonTai(ID_ChuyenDi, ngayBatDau, txt_cccd.Text))
            {
                MessageBox.Show("Người có tên: " + txt_hoten.Text + " và cccd: " + cccd + " đã có trong danh sách chuyến đi: " + ID_ChuyenDi + " " + ngayBatDau);
                return;
            }

            if (edit != null) // edit
            {
                dgv_dshanhkhach.Rows[int.Parse(edit)].Cells[0].Value = txt_hoten.Text;
                dgv_dshanhkhach.Rows[int.Parse(edit)].Cells[1].Value = txt_sdt.Text;
                dgv_dshanhkhach.Rows[int.Parse(edit)].Cells[2].Value = txt_cccd.Text;

                txt_sdt.ResetText();
                txt_hoten.ResetText();
                txt_cccd.ResetText();

                edit = null;

            } else // add
            {

                if (!IsNumeric(txt_sdt.Text))
                {
                    MessageBox.Show("SDT bạn nhập không phải là số!");

                    return;
                }

                if (txt_sdt.Text.Length < 10)
                {
                    MessageBox.Show("SDT bạn phải nhập đủ 10 số!");

                    return;
                }

                if (!IsNumeric(txt_cccd.Text))
                {
                    MessageBox.Show("CCCD bạn nhập không phải là số!");

                    return;
                }

                if (txt_cccd.Text.Length < 12)
                {
                    MessageBox.Show("CCCD bạn phải nhập đủ 12 số!");

                    return;
                }

                dgv_dshanhkhach.Rows.Add(txt_hoten.Text, txt_sdt.Text, txt_cccd.Text);

                txt_sdt.ResetText();
                txt_hoten.ResetText();
                txt_cccd.ResetText();

                amountRemain -= 1;

                updateAmountRemainLabel();
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dgv_dshanhkhach.Rows.Count == 0) return;

            dgv_dshanhkhach.Rows.RemoveAt(dgv_dshanhkhach.CurrentRow.Index);

            edit = null;

            amountRemain += 1;

            updateAmountRemainLabel();
        }

        private void dgv_dshanhkhach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentIndex = dgv_dshanhkhach.CurrentCell.RowIndex;

            if (currentIndex == 0)
                return;
            // Panel tạo tour
            // Panel thông tin chi tiết tour
            txt_hoten.Text = dgv_dshanhkhach.Rows[currentIndex].Cells[0].Value.ToString();
            txt_sdt.Text = dgv_dshanhkhach.Rows[currentIndex].Cells[1].Value.ToString();
            txt_cccd.Text = dgv_dshanhkhach.Rows[currentIndex].Cells[2].Value.ToString();

            edit = currentIndex.ToString();
        }

        private void NhapThongTinHanhKhach_Load(object sender, EventArgs e)
        {

            amountRemain = int.Parse(AdminQuery.CountTickets(ID_ChuyenDi, ngayBatDau).Rows[0][1].ToString()) - int.Parse(AdminQuery.CountTickets(ID_ChuyenDi, ngayBatDau).Rows[0][0].ToString());

            updateAmountRemainLabel();
        }

        private void updateAmountRemainLabel()
        {
            lbl_count.Text = "(Hiện còn " + amountRemain + " vé đặt tour này)";
        }

        private bool IsNumeric(string str)
        {
            return !string.IsNullOrEmpty(str) && str.All(char.IsDigit);
        }

    }
}
