﻿namespace DuLich
{
    partial class TaoChuyenDiMoi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaoChuyenDiMoi));
            this.pic_logo = new Guna.UI2.WinForms.Guna2PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lbl_back = new System.Windows.Forms.Label();
            this.btn_sendrequest = new Guna.UI2.WinForms.Guna2Button();
            this.nud_soluong = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.dtp_ngaykhoihanh = new System.Windows.Forms.DateTimePicker();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pic_logo)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_soluong)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_logo
            // 
            this.pic_logo.BackColor = System.Drawing.Color.Transparent;
            this.pic_logo.Image = ((System.Drawing.Image)(resources.GetObject("pic_logo.Image")));
            this.pic_logo.ImageRotate = 0F;
            this.pic_logo.Location = new System.Drawing.Point(24, 26);
            this.pic_logo.Margin = new System.Windows.Forms.Padding(2);
            this.pic_logo.Name = "pic_logo";
            this.pic_logo.Size = new System.Drawing.Size(99, 48);
            this.pic_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_logo.TabIndex = 9;
            this.pic_logo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(222, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "TẠO CHUYẾN MỚI";
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(34, 46);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(106, 19);
            this.guna2HtmlLabel1.TabIndex = 12;
            this.guna2HtmlLabel1.Text = "Ngày Khởi Hành:";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.guna2Panel1.Controls.Add(this.lbl_back);
            this.guna2Panel1.Controls.Add(this.btn_sendrequest);
            this.guna2Panel1.Controls.Add(this.nud_soluong);
            this.guna2Panel1.Controls.Add(this.dtp_ngaykhoihanh);
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel2);
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel1);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 94);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(418, 183);
            this.guna2Panel1.TabIndex = 13;
            // 
            // lbl_back
            // 
            this.lbl_back.AutoSize = true;
            this.lbl_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_back.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_back.ForeColor = System.Drawing.Color.Red;
            this.lbl_back.Location = new System.Drawing.Point(110, 137);
            this.lbl_back.Name = "lbl_back";
            this.lbl_back.Size = new System.Drawing.Size(62, 17);
            this.lbl_back.TabIndex = 17;
            this.lbl_back.Text = "Quay Lại";
            this.lbl_back.Click += new System.EventHandler(this.lbl_back_Click);
            // 
            // btn_sendrequest
            // 
            this.btn_sendrequest.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_sendrequest.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_sendrequest.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_sendrequest.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_sendrequest.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_sendrequest.ForeColor = System.Drawing.Color.White;
            this.btn_sendrequest.Location = new System.Drawing.Point(194, 129);
            this.btn_sendrequest.Name = "btn_sendrequest";
            this.btn_sendrequest.Size = new System.Drawing.Size(162, 33);
            this.btn_sendrequest.TabIndex = 16;
            this.btn_sendrequest.Text = "Gửi Yêu Cầu";
            this.btn_sendrequest.Click += new System.EventHandler(this.btn_sendrequest_Click);
            // 
            // nud_soluong
            // 
            this.nud_soluong.BackColor = System.Drawing.Color.Transparent;
            this.nud_soluong.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nud_soluong.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nud_soluong.Location = new System.Drawing.Point(156, 80);
            this.nud_soluong.Name = "nud_soluong";
            this.nud_soluong.Size = new System.Drawing.Size(69, 19);
            this.nud_soluong.TabIndex = 15;
            // 
            // dtp_ngaykhoihanh
            // 
            this.dtp_ngaykhoihanh.Location = new System.Drawing.Point(156, 46);
            this.dtp_ngaykhoihanh.Name = "dtp_ngaykhoihanh";
            this.dtp_ngaykhoihanh.Size = new System.Drawing.Size(200, 20);
            this.dtp_ngaykhoihanh.TabIndex = 14;
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(34, 80);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(63, 19);
            this.guna2HtmlLabel2.TabIndex = 13;
            this.guna2HtmlLabel2.Text = "Số Lượng:";
            // 
            // TaoChuyenDiMoi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DuLich.Properties.Resources.z6332234645440_726f810a2378ccf071dc90942445f1e5;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(418, 277);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pic_logo);
            this.Controls.Add(this.guna2Panel1);
            this.Name = "TaoChuyenDiMoi";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacificTravel - Tạo Chuyến Đi Mới";
            ((System.ComponentModel.ISupportInitialize)(this.pic_logo)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_soluong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox pic_logo;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btn_sendrequest;
        private Guna.UI2.WinForms.Guna2NumericUpDown nud_soluong;
        private System.Windows.Forms.DateTimePicker dtp_ngaykhoihanh;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private System.Windows.Forms.Label lbl_back;
    }
}