namespace DuLich
{
    partial class DanhGia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DanhGia));
            this.pic_logo = new Guna.UI2.WinForms.Guna2PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nud_sosao = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.btnQuayLai = new System.Windows.Forms.Button();
            this.btnDanhGia = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.rtbNhanXet = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvCacDanhGia = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_logo)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_sosao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCacDanhGia)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_logo
            // 
            this.pic_logo.BackColor = System.Drawing.Color.White;
            this.pic_logo.Image = ((System.Drawing.Image)(resources.GetObject("pic_logo.Image")));
            this.pic_logo.ImageRotate = 0F;
            this.pic_logo.Location = new System.Drawing.Point(34, 38);
            this.pic_logo.Name = "pic_logo";
            this.pic_logo.Size = new System.Drawing.Size(148, 74);
            this.pic_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_logo.TabIndex = 9;
            this.pic_logo.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.nud_sosao);
            this.panel2.Controls.Add(this.btnQuayLai);
            this.panel2.Controls.Add(this.btnDanhGia);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.rtbNhanXet);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.dgvCacDanhGia);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(34, 123);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1509, 818);
            this.panel2.TabIndex = 10;
            // 
            // nud_sosao
            // 
            this.nud_sosao.BackColor = System.Drawing.Color.Transparent;
            this.nud_sosao.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nud_sosao.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nud_sosao.Location = new System.Drawing.Point(1242, 542);
            this.nud_sosao.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.nud_sosao.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nud_sosao.Name = "nud_sosao";
            this.nud_sosao.Size = new System.Drawing.Size(96, 55);
            this.nud_sosao.TabIndex = 15;
            // 
            // btnQuayLai
            // 
            this.btnQuayLai.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(100)))), ((int)(((byte)(171)))));
            this.btnQuayLai.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnQuayLai.FlatAppearance.BorderSize = 0;
            this.btnQuayLai.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuayLai.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnQuayLai.Location = new System.Drawing.Point(1300, 700);
            this.btnQuayLai.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.btnQuayLai.Name = "btnQuayLai";
            this.btnQuayLai.Size = new System.Drawing.Size(165, 65);
            this.btnQuayLai.TabIndex = 14;
            this.btnQuayLai.Text = "Quay lại";
            this.btnQuayLai.UseVisualStyleBackColor = false;
            this.btnQuayLai.Click += new System.EventHandler(this.btnQuayLai_Click);
            // 
            // btnDanhGia
            // 
            this.btnDanhGia.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDanhGia.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnDanhGia.FlatAppearance.BorderSize = 2;
            this.btnDanhGia.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDanhGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(100)))), ((int)(((byte)(171)))));
            this.btnDanhGia.Location = new System.Drawing.Point(1112, 700);
            this.btnDanhGia.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.btnDanhGia.Name = "btnDanhGia";
            this.btnDanhGia.Size = new System.Drawing.Size(165, 65);
            this.btnDanhGia.TabIndex = 13;
            this.btnDanhGia.Text = "Đánh giá";
            this.btnDanhGia.UseVisualStyleBackColor = false;
            this.btnDanhGia.Click += new System.EventHandler(this.btnDanhGia_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1104, 542);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 37);
            this.label5.TabIndex = 5;
            this.label5.Text = "Số sao:";
            // 
            // rtbNhanXet
            // 
            this.rtbNhanXet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.rtbNhanXet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbNhanXet.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbNhanXet.Location = new System.Drawing.Point(38, 542);
            this.rtbNhanXet.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.rtbNhanXet.Name = "rtbNhanXet";
            this.rtbNhanXet.Size = new System.Drawing.Size(1046, 221);
            this.rtbNhanXet.TabIndex = 4;
            this.rtbNhanXet.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(30, 495);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 37);
            this.label4.TabIndex = 3;
            this.label4.Text = "Nhận xét:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 420);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(244, 37);
            this.label3.TabIndex = 2;
            this.label3.Text = "Đánh giá của bạn:";
            // 
            // dgvCacDanhGia
            // 
            this.dgvCacDanhGia.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCacDanhGia.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvCacDanhGia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCacDanhGia.Location = new System.Drawing.Point(38, 92);
            this.dgvCacDanhGia.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.dgvCacDanhGia.Name = "dgvCacDanhGia";
            this.dgvCacDanhGia.ReadOnly = true;
            this.dgvCacDanhGia.RowHeadersWidth = 51;
            this.dgvCacDanhGia.RowTemplate.Height = 24;
            this.dgvCacDanhGia.Size = new System.Drawing.Size(1428, 266);
            this.dgvCacDanhGia.StandardTab = true;
            this.dgvCacDanhGia.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(264, 37);
            this.label2.TabIndex = 0;
            this.label2.Text = "Các đánh giá trước:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1270, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(277, 45);
            this.label1.TabIndex = 20;
            this.label1.Text = "ĐÁNH GIÁ TOUR";
            // 
            // DanhGia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1576, 998);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pic_logo);
            this.Name = "DanhGia";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacificTravel - Đánh Giá Tour";
            ((System.ComponentModel.ISupportInitialize)(this.pic_logo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_sosao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCacDanhGia)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox pic_logo;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Button btnQuayLai;
        public System.Windows.Forms.Button btnDanhGia;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rtbNhanXet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvCacDanhGia;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2NumericUpDown nud_sosao;
        private System.Windows.Forms.Label label1;
    }
}