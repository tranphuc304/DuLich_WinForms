namespace DuLich
{
    partial class QLDanhGia
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QLDanhGia));
            this.btn_Find = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbMaTour = new System.Windows.Forms.ComboBox();
            this.btnQuayLai = new System.Windows.Forms.Button();
            this.btnAVGRate = new System.Windows.Forms.Button();
            this.dgvQLDanhGia = new System.Windows.Forms.DataGridView();
            this.guna2HtmlLabel7 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pic_logo = new Guna.UI2.WinForms.Guna2PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQLDanhGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_logo)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Find
            // 
            this.btn_Find.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(100)))), ((int)(((byte)(171)))));
            this.btn_Find.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Find.ForeColor = System.Drawing.Color.White;
            this.btn_Find.Location = new System.Drawing.Point(846, 108);
            this.btn_Find.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Find.Name = "btn_Find";
            this.btn_Find.Size = new System.Drawing.Size(156, 58);
            this.btn_Find.TabIndex = 40;
            this.btn_Find.Text = "Tìm kiếm";
            this.btn_Find.UseVisualStyleBackColor = false;
            this.btn_Find.Click += new System.EventHandler(this.btn_Find_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(100)))), ((int)(((byte)(171)))));
            this.label2.Location = new System.Drawing.Point(483, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 39;
            this.label2.Text = "Mã Tour";
            // 
            // cbbMaTour
            // 
            this.cbbMaTour.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbMaTour.FormattingEnabled = true;
            this.cbbMaTour.Location = new System.Drawing.Point(612, 116);
            this.cbbMaTour.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbbMaTour.Name = "cbbMaTour";
            this.cbbMaTour.Size = new System.Drawing.Size(205, 29);
            this.cbbMaTour.TabIndex = 38;
            // 
            // btnQuayLai
            // 
            this.btnQuayLai.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(100)))), ((int)(((byte)(171)))));
            this.btnQuayLai.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnQuayLai.FlatAppearance.BorderSize = 0;
            this.btnQuayLai.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuayLai.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnQuayLai.Location = new System.Drawing.Point(738, 443);
            this.btnQuayLai.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnQuayLai.Name = "btnQuayLai";
            this.btnQuayLai.Size = new System.Drawing.Size(261, 60);
            this.btnQuayLai.TabIndex = 37;
            this.btnQuayLai.Text = "Quay Lại Trang Chủ";
            this.btnQuayLai.UseVisualStyleBackColor = false;
            this.btnQuayLai.Click += new System.EventHandler(this.btnQuayLai_Click);
            // 
            // btnAVGRate
            // 
            this.btnAVGRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(100)))), ((int)(((byte)(171)))));
            this.btnAVGRate.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnAVGRate.FlatAppearance.BorderSize = 0;
            this.btnAVGRate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAVGRate.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAVGRate.Location = new System.Drawing.Point(489, 443);
            this.btnAVGRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAVGRate.Name = "btnAVGRate";
            this.btnAVGRate.Size = new System.Drawing.Size(240, 60);
            this.btnAVGRate.TabIndex = 36;
            this.btnAVGRate.Text = "Số Sao Trung Bình";
            this.btnAVGRate.UseVisualStyleBackColor = false;
            this.btnAVGRate.Click += new System.EventHandler(this.btnAVGRate_Click);
            // 
            // dgvQLDanhGia
            // 
            this.dgvQLDanhGia.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvQLDanhGia.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvQLDanhGia.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.dgvQLDanhGia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQLDanhGia.Location = new System.Drawing.Point(27, 168);
            this.dgvQLDanhGia.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvQLDanhGia.Name = "dgvQLDanhGia";
            this.dgvQLDanhGia.RowHeadersWidth = 51;
            this.dgvQLDanhGia.Size = new System.Drawing.Size(972, 265);
            this.dgvQLDanhGia.TabIndex = 35;
            // 
            // guna2HtmlLabel7
            // 
            this.guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel7.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel7.Location = new System.Drawing.Point(737, 34);
            this.guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            this.guna2HtmlLabel7.Size = new System.Drawing.Size(262, 39);
            this.guna2HtmlLabel7.TabIndex = 65;
            this.guna2HtmlLabel7.Text = "QUẢN LÝ ĐÁNH GIÁ";
            // 
            // pic_logo
            // 
            this.pic_logo.Image = ((System.Drawing.Image)(resources.GetObject("pic_logo.Image")));
            this.pic_logo.ImageRotate = 0F;
            this.pic_logo.Location = new System.Drawing.Point(27, 25);
            this.pic_logo.Margin = new System.Windows.Forms.Padding(2);
            this.pic_logo.Name = "pic_logo";
            this.pic_logo.Size = new System.Drawing.Size(99, 48);
            this.pic_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_logo.TabIndex = 64;
            this.pic_logo.TabStop = false;
            // 
            // QLDanhGia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1035, 535);
            this.Controls.Add(this.guna2HtmlLabel7);
            this.Controls.Add(this.pic_logo);
            this.Controls.Add(this.btn_Find);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbbMaTour);
            this.Controls.Add(this.btnQuayLai);
            this.Controls.Add(this.btnAVGRate);
            this.Controls.Add(this.dgvQLDanhGia);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "QLDanhGia";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacificTravel - Quản Lý Đánh Giá";
            this.Load += new System.EventHandler(this.FormQuanLyDanhGia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQLDanhGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Find;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbMaTour;
        public System.Windows.Forms.Button btnQuayLai;
        public System.Windows.Forms.Button btnAVGRate;
        private System.Windows.Forms.DataGridView dgvQLDanhGia;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel7;
        private Guna.UI2.WinForms.Guna2PictureBox pic_logo;
    }
}