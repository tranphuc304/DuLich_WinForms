namespace DuLich
{
    partial class QLKhachHang
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
            this.pic_logo = new Guna.UI2.WinForms.Guna2PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lbl_qlyeucau = new Guna.UI2.WinForms.Guna2Button();
            this.btn_qltaikhoan = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.pic_logo)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pic_logo
            // 
            this.pic_logo.Image = global::DuLich.Properties.Resources.Logo;
            this.pic_logo.ImageRotate = 0F;
            this.pic_logo.Location = new System.Drawing.Point(11, 16);
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
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(193, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 25);
            this.label1.TabIndex = 10;
            this.label1.Text = "QUẢN LÝ KHÁCH HÀNG";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.guna2Panel1.Controls.Add(this.lbl_qlyeucau);
            this.guna2Panel1.Controls.Add(this.btn_qltaikhoan);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 79);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(433, 190);
            this.guna2Panel1.TabIndex = 11;
            // 
            // lbl_qlyeucau
            // 
            this.lbl_qlyeucau.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.lbl_qlyeucau.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.lbl_qlyeucau.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.lbl_qlyeucau.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.lbl_qlyeucau.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qlyeucau.ForeColor = System.Drawing.Color.White;
            this.lbl_qlyeucau.Location = new System.Drawing.Point(35, 112);
            this.lbl_qlyeucau.Name = "lbl_qlyeucau";
            this.lbl_qlyeucau.Size = new System.Drawing.Size(365, 45);
            this.lbl_qlyeucau.TabIndex = 2;
            this.lbl_qlyeucau.Text = "Quản Lý Yêu Cầu Của Khách Hàng";
            this.lbl_qlyeucau.Click += new System.EventHandler(this.lbl_qlyeucau_Click);
            // 
            // btn_qltaikhoan
            // 
            this.btn_qltaikhoan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_qltaikhoan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_qltaikhoan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_qltaikhoan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_qltaikhoan.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_qltaikhoan.ForeColor = System.Drawing.Color.White;
            this.btn_qltaikhoan.Location = new System.Drawing.Point(35, 41);
            this.btn_qltaikhoan.Name = "btn_qltaikhoan";
            this.btn_qltaikhoan.Size = new System.Drawing.Size(365, 45);
            this.btn_qltaikhoan.TabIndex = 0;
            this.btn_qltaikhoan.Text = "Quản Lý Tài Khoản";
            this.btn_qltaikhoan.Click += new System.EventHandler(this.btn_qltaikhoan_Click);
            // 
            // QLKhachHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 269);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pic_logo);
            this.Name = "QLKhachHang";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacificTravel - Quản Lý Khách Hàng";
            ((System.ComponentModel.ISupportInitialize)(this.pic_logo)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox pic_logo;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btn_qltaikhoan;
        private Guna.UI2.WinForms.Guna2Button lbl_qlyeucau;
    }
}