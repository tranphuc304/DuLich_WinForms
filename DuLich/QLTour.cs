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
using DuLich.DatabaseUtils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DuLich
{
    public partial class QLTour : Form
    {
        public QLTour()
        {
            InitializeComponent();
        }

        private string path = null;

        private void QLTour_Load(object sender, EventArgs e)
        {
            loadDSChuyenDi();
        }

        private void btn_bia_Click(object sender, EventArgs e)
        {

            saveImage("AnhBia.jpg");

        }

        private void btn_anh1_Click(object sender, EventArgs e)
        {
            saveImage("Anh1.jpg");
        }

        private void btn_anh2_Click(object sender, EventArgs e)
        {
            saveImage("Anh2.jpg");
        }

        private void btn_anh3_Click(object sender, EventArgs e)
        {
            saveImage("Anh3.jpg");
        }

        private void btn_chitiet_Click(object sender, EventArgs e)
        {
            saveTextFile("ChiTiet.txt");
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            try
            {
                createTourFolder();

                string IDTour = this.txt_matour.Text;

                if (isTourExist(IDTour) && this.txt_matour.Enabled)
                {
                    MessageBox.Show("Mã tour đã tồn tại! Hãy đổi mã tour khác!");

                    return;
                }

                string TenTour = this.txt_tentour.Text;
                int SoNgayDi = int.Parse(this.txt_songaydi.Text);
                string Gia = this.txt_giave.Text;
                string HanhTrinh = this.txt_hanhtrinh.Text;
                int SoLuong = int.Parse(this.txt_soluong.Text);
                string ChiTiet = path;

                AdminQuery.SaveOrUpdateChuyenDi(IDTour, TenTour, SoNgayDi, int.Parse(Gia), HanhTrinh, SoLuong, ChiTiet);

                MessageBox.Show("Lưu thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadDSChuyenDi();

                emptyTextbox();

                changeButtonToCreateStatus();

                txt_matour.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            try
            {
                int n = this.dgv_dschuyendi.CurrentCell.RowIndex;
                string IDTour = dgv_dschuyendi.Rows[n].Cells[0].Value.ToString();

                AdminQuery.deleteChuyenDi(IDTour);

                MessageBox.Show("Xóa tour thành công");

                loadDSChuyenDi();
            }
            catch (Exception ex)
            {
                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    innerException = innerException.InnerException;
                    MessageBox.Show(innerException.Message);
                }
            }
        }

        private void loadDSChuyenDi()
        {
            try
            {
                dgv_dschuyendi.DataSource = AdminQuery.getDSChuyenDi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dgv_dschuyendi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int n = this.dgv_dschuyendi.CurrentCell.RowIndex;
            // Panel tạo tour
            // Panel thông tin chi tiết tour
            this.txt_matour_thongtin.Text = dgv_dschuyendi.Rows[n].Cells[0].Value.ToString();
            this.txt_tentour_thongtin.Text = dgv_dschuyendi.Rows[n].Cells[1].Value.ToString();
            this.txt_songaydi_thongtin.Text = dgv_dschuyendi.Rows[n].Cells[3].Value.ToString();
            this.txt_giave_thongtin.Text = dgv_dschuyendi.Rows[n].Cells[4].Value.ToString();
            this.txt_hanhtrinh_thongtin.Text = dgv_dschuyendi.Rows[n].Cells[2].Value.ToString();

            this.txt_matour.Text = txt_matour_thongtin.Text;
            this.txt_matour.Enabled = false;
            this.txt_tentour.Text = txt_tentour_thongtin.Text;
            this.txt_songaydi.Text = txt_songaydi_thongtin.Text;
            this.txt_giave.Text = txt_giave_thongtin.Text;
            this.txt_hanhtrinh.Text = txt_hanhtrinh_thongtin.Text;
            this.txt_soluong.Text = dgv_dschuyendi.Rows[n].Cells[5].Value.ToString();
            // 4 Ảnh 
            string path = dgv_dschuyendi.Rows[n].Cells[6].Value.ToString();
            this.picAnhBia.Image = Directory.Exists(path + "AnhBia.jpg") ? Image.FromFile(path + "AnhBia.jpg") : null;
            this.picAnhBia.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picAnh1.Image = Directory.Exists(path + "Anh1.jpg") ? Image.FromFile(path + "Anh1.jpg") : null;
            this.picAnh1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picAnh2.Image = Directory.Exists(path + "Anh2.jpg") ? Image.FromFile(path + "Anh2.jpg") : null;
            this.picAnh2.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picAnh3.Image = Directory.Exists(path + "Anh3.jpg") ? Image.FromFile(path + "Anh3.jpg") : null;
            this.picAnh3.SizeMode = PictureBoxSizeMode.StretchImage;

            this.path = "../../../Details/" + this.txt_matour.Text + "/";

            changeButtonToEditStatus();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            emptyTextbox();

            changeButtonToCreateStatus();

            this.txt_matour.Enabled = true;
        }

        private void emptyTextbox()
        {
            this.txt_matour.Text = string.Empty;
            this.txt_tentour.Text = string.Empty;
            this.txt_songaydi.Text = string.Empty;
            this.txt_giave.Text = string.Empty;
            this.txt_hanhtrinh.Text = string.Empty;
            this.txt_soluong.Text = string.Empty;

            this.txt_matour_thongtin.Text = string.Empty;
            this.txt_tentour_thongtin.Text = string.Empty;
            this.txt_hanhtrinh_thongtin.Text = string.Empty;
            this.txt_giave_thongtin.Text = string.Empty;
            this.txt_songaydi_thongtin.Text = string .Empty;

            this.picAnhBia.Image = null;
            this.picAnh1.Image = null;
            this.picAnh2.Image = null;
            this.picAnh3.Image = null;
        }

        private void changeButtonToEditStatus()
        {
            this.btn_bia.Text = "Sửa Bìa";
            this.btn_anh1.Text = "Sửa Ảnh 1";
            this.btn_anh2.Text = "Sửa Ảnh 2";
            this.btn_anh3.Text = "Sửa Ảnh 3";
            this.btn_chitiet.Text = "Sửa Chi Tiết";
        }

        private void changeButtonToCreateStatus()
        {
            this.btn_bia.Text = "Thêm Bìa";
            this.btn_anh1.Text = "Thêm Ảnh 1";
            this.btn_anh2.Text = "Thêm Ảnh 2";
            this.btn_anh3.Text = "Thêm Ảnh 3";
            this.btn_chitiet.Text = "Thêm Chi Tiết";
        }

        private bool createTourFolder()
        {
            string Ma_Tour = txt_matour.Text;
            string dirPath = "../../../Details/";

            string path = dirPath + Ma_Tour + "/";
            bool exist = Directory.Exists(path);

            // Kiểm tra xem đường dẫn thư mục tồn tại không.
            // Nếu không tồn tại, tạo thư mục này.
            if (!exist)
            {
                // Tạo thư mục.
                Directory.CreateDirectory(path);
                this.path = path;
            }

            return exist;
        }

        private void saveImage(string filename)
        {
            saveFile("Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif", "Chọn một bức ảnh để lưu", filename);
        }

        private void saveTextFile(string filename)
        {
            saveFile("Text Files|*.txt", "Chọn một file text để lưu", filename);
        }

        private void saveFile(string filter, string title, string filename)
        {
            createTourFolder();

            OpenFileDialog sourceDialog = new OpenFileDialog();
            sourceDialog.Filter = filter;
            sourceDialog.Title = title;

            if (sourceDialog.ShowDialog() == DialogResult.OK)
            {
                // Đường dẫn đến thư mục bạn muốn lưu tệp

                // Kết hợp đường dẫn và tên tệp đích
                string destinationFilePath = Path.Combine(path, filename);

                try
                {
                    // Đọc dữ liệu từ file nguồn
                    byte[] fileBytes = File.ReadAllBytes(sourceDialog.FileName);

                    // Ghi dữ liệu vào file đích
                    File.WriteAllBytes(destinationFilePath, fileBytes);

                    MessageBox.Show("Thêm " + filename + " Thành Công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool isTourExist(string IDTour)
        {

            for (int i = 0; i < dgv_dschuyendi.Rows.Count - 1; i++)
            {
                string id = dgv_dschuyendi.Rows[i].Cells[0].Value.ToString();

                if (IDTour.Equals(id))
                    return true;
            }

            return false;
        }

    }
}
