using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Dangnhap : Form
    {
        
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;
                   AttachDbFilename=C:\Users\win\Downloads\GiuaKy\WindowsFormsApp2\WindowsFormsApp2\WindowsFormsApp2\Database1.mdf;
                   Integrated Security=True;
                   Connect Timeout=30";



        public Dangnhap()
        {
            InitializeComponent();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
           




        }

        private void guna2Panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }


        private void Form2_Load(object sender, EventArgs e)
        {
            guna2PanelLogin.BorderRadius = 20;
            guna2PanelLogin.BorderColor = Color.White;
            guna2PanelLogin.FillColor = Color.FromArgb(153, 128, 128, 128); // xám mờ 60%
            guna2PanelLogin.BackColor = Color.Transparent;

            // Màu nền TextBox hài hòa, không trong suốt
            Color textBoxFillColor = Color.Gainsboro;

            Textbox_taikhoan.BorderRadius = 0;
            Textbox_taikhoan.BorderThickness = 0;
            Textbox_taikhoan.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            Textbox_taikhoan.FocusedState.BorderColor = Color.Black;
            Textbox_taikhoan.HoverState.BorderColor = Color.Gray;
            Textbox_taikhoan.FillColor = textBoxFillColor;
            Textbox_taikhoan.ForeColor = Color.Black;
            Textbox_taikhoan.PlaceholderText = "Nhập nội dung...";

            Textbox_matkhau.BorderRadius = 0;
            Textbox_matkhau.BorderThickness = 0;
            Textbox_matkhau.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            Textbox_matkhau.FocusedState.BorderColor = Color.Black;
            Textbox_matkhau.HoverState.BorderColor = Color.Gray;
            Textbox_matkhau.FillColor = textBoxFillColor;
            Textbox_matkhau.ForeColor = Color.Black;
            Textbox_matkhau.PlaceholderText = "Nhập nội dung...";

            if (Properties.Settings.Default.RememberMe)
            {
                Textbox_taikhoan.Text = Properties.Settings.Default.SavedUser;
                checkbox_giutrangthai.Checked = true;
            }
        
        }




        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Textbox_taikhoan_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button_dangnhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Textbox_taikhoan.Text) || string.IsNullOrWhiteSpace(Textbox_matkhau.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT Password FROM [User]WHERE Username=@acc OR Email=@acc OR SDT = @acc";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@acc", Textbox_taikhoan.Text);

                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    MessageBox.Show("❌ Tài khoản không tồn tại!");
                    return;
                }

                string storedHash = result.ToString();
                string inputHash = HashPassword(Textbox_matkhau.Text);

                if (storedHash == inputHash)
                {
                    MessageBox.Show(" Đăng nhập thành công!");

                    // Lưu trạng thái nếu checkbox được chọn
                    if (checkbox_giutrangthai.Checked)
                    {
                        Properties.Settings.Default.SavedUser = Textbox_taikhoan.Text;
                        Properties.Settings.Default.RememberMe = true;
                    }
                    else
                    {
                        Properties.Settings.Default.SavedUser = "";
                        Properties.Settings.Default.RememberMe = false;
                    }

                    Properties.Settings.Default.Save();

                    // TODO: Mở form chính
                   
                    FormMainMenu main = new FormMainMenu();
                    main.Show();
                     this.Hide();
                }
                else
                {
                    MessageBox.Show("❌ Sai mật khẩu!");
                }
            }
        }

        private void linkLabel_dangky_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Dangky frm = new Dangky();
            frm.ShowDialog();
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private void linkLabel_quenmk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Quenmatkhau frm = new Quenmatkhau();
            frm.ShowDialog();
        }
    }
}
