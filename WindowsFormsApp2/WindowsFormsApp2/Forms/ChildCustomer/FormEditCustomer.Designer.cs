﻿namespace WindowsFormsApp2.Forms.ChildCustomer
{
    partial class FormEditCustomer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_chucvi = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton_nu = new System.Windows.Forms.RadioButton();
            this.radioButton_nam = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_mk = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_diachi = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_sdt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_email = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_tendn = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_hoten = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.comboBox_chucvi);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.radioButton_nu);
            this.panel1.Controls.Add(this.radioButton_nam);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBox_mk);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBox_diachi);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBox_sdt);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBox_email);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox_tendn);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox_hoten);
            this.panel1.Location = new System.Drawing.Point(45, 131);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 289);
            this.panel1.TabIndex = 18;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(387, 178);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 16);
            this.label9.TabIndex = 18;
            this.label9.Text = "Chức vị";
            // 
            // comboBox_chucvi
            // 
            this.comboBox_chucvi.FormattingEnabled = true;
            this.comboBox_chucvi.Items.AddRange(new object[] {
            "Admin",
            "Người dùng"});
            this.comboBox_chucvi.Location = new System.Drawing.Point(498, 175);
            this.comboBox_chucvi.Name = "comboBox_chucvi";
            this.comboBox_chucvi.Size = new System.Drawing.Size(157, 24);
            this.comboBox_chucvi.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(293, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 40);
            this.button1.TabIndex = 15;
            this.button1.Text = "Cập nhật";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Giới tính";
            // 
            // radioButton_nu
            // 
            this.radioButton_nu.AutoSize = true;
            this.radioButton_nu.Location = new System.Drawing.Point(198, 179);
            this.radioButton_nu.Name = "radioButton_nu";
            this.radioButton_nu.Size = new System.Drawing.Size(45, 20);
            this.radioButton_nu.TabIndex = 13;
            this.radioButton_nu.TabStop = true;
            this.radioButton_nu.Text = "Nữ";
            this.radioButton_nu.UseVisualStyleBackColor = true;
            // 
            // radioButton_nam
            // 
            this.radioButton_nam.AutoSize = true;
            this.radioButton_nam.Location = new System.Drawing.Point(115, 179);
            this.radioButton_nam.Name = "radioButton_nam";
            this.radioButton_nam.Size = new System.Drawing.Size(57, 20);
            this.radioButton_nam.TabIndex = 12;
            this.radioButton_nam.TabStop = true;
            this.radioButton_nam.Text = "Nam";
            this.radioButton_nam.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(387, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "Mật khẩu";
            // 
            // textBox_mk
            // 
            this.textBox_mk.Location = new System.Drawing.Point(498, 133);
            this.textBox_mk.Name = "textBox_mk";
            this.textBox_mk.Size = new System.Drawing.Size(157, 22);
            this.textBox_mk.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Địa chỉ";
            // 
            // textBox_diachi
            // 
            this.textBox_diachi.Location = new System.Drawing.Point(115, 130);
            this.textBox_diachi.Name = "textBox_diachi";
            this.textBox_diachi.Size = new System.Drawing.Size(157, 22);
            this.textBox_diachi.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(387, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Số điện thoại";
            // 
            // textBox_sdt
            // 
            this.textBox_sdt.Location = new System.Drawing.Point(498, 86);
            this.textBox_sdt.Name = "textBox_sdt";
            this.textBox_sdt.Size = new System.Drawing.Size(157, 22);
            this.textBox_sdt.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Email";
            // 
            // textBox_email
            // 
            this.textBox_email.Location = new System.Drawing.Point(115, 83);
            this.textBox_email.Name = "textBox_email";
            this.textBox_email.Size = new System.Drawing.Size(157, 22);
            this.textBox_email.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(387, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tên đăng nhập";
            // 
            // textBox_tendn
            // 
            this.textBox_tendn.Location = new System.Drawing.Point(498, 41);
            this.textBox_tendn.Name = "textBox_tendn";
            this.textBox_tendn.Size = new System.Drawing.Size(157, 22);
            this.textBox_tendn.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Họ tên";
            // 
            // textBox_hoten
            // 
            this.textBox_hoten.Location = new System.Drawing.Point(115, 38);
            this.textBox_hoten.Name = "textBox_hoten";
            this.textBox_hoten.Size = new System.Drawing.Size(157, 22);
            this.textBox_hoten.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.button2.Location = new System.Drawing.Point(34, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 33);
            this.button2.TabIndex = 20;
            this.button2.Text = "<";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label8.Location = new System.Drawing.Point(40, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(180, 25);
            this.label8.TabIndex = 19;
            this.label8.Text = "Chỉnh sửa thông tin";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // FormEditCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label8);
            this.Name = "FormEditCustomer";
            this.Text = "FormEditCustomer";
            this.Load += new System.EventHandler(this.FormEditCustomer_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox_chucvi;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButton_nu;
        private System.Windows.Forms.RadioButton radioButton_nam;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_mk;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_diachi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_sdt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_email;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_tendn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_hoten;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
    }
}