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
using System.Net;
using System.Net.Mail;


namespace WindowsFormsApp2
{
    public partial class Quenmatkhau : Form
    {
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;
                   AttachDbFilename=C:\Users\win\Downloads\GiuaKy\WindowsFormsApp2\WindowsFormsApp2\WindowsFormsApp2\Database1.mdf;
                   Integrated Security=True;
                   Connect Timeout=30";
        private string generatedOTP;
        public Quenmatkhau()
        {
            InitializeComponent();
            this.Load += Quenmatkhau_Load;
            button_gui.Click += button_gui_Click;
            button_dmk.Click += button_dmk_Click;
            button_huy.Click += button_huy_Click;

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Quenmatkhau_Load(object sender, EventArgs e)
        {
            guna2Panel1.BorderRadius = 20;
            guna2Panel1.BorderColor = Color.White;
            guna2Panel1.FillColor = Color.FromArgb(153, 128, 128, 128);
            guna2Panel1.BackColor = Color.Transparent;

            // Thiết lập textboxes
            Guna.UI2.WinForms.Guna2TextBox[] textboxes = { textbox_tk, textbox_otp , textbox_mkm, textbox_xnmk };
            foreach (var tb in textboxes)
            {
                tb.BorderRadius = 0;
                tb.BorderThickness = 0;
                tb.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
                tb.FocusedState.BorderColor = Color.Black;
                tb.HoverState.BorderColor = Color.Gray;
                tb.FillColor = Color.Gainsboro;
                tb.ForeColor = Color.Black;
                tb.PlaceholderText = "Nhập nội dung...";
            }

            textbox_mkm.UseSystemPasswordChar = true;
            textbox_xnmk.UseSystemPasswordChar = true;

        }

        private void button_gui_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textbox_tk.Text))
            {
                MessageBox.Show("Vui lòng nhập Email!");
                return;
            }

            // Tạo OTP 6 số
            Random rnd = new Random();
            generatedOTP = rnd.Next(100000, 999999).ToString();

            // Gửi OTP thật
            SendOTP(textbox_tk.Text.Trim(), generatedOTP);

            MessageBox.Show("OTP đã gửi, kiểm tra email của bạn!");
        }

        private void button_dmk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textbox_otp.Text) ||
                string.IsNullOrWhiteSpace(textbox_mkm.Text) ||
                string.IsNullOrWhiteSpace(textbox_xnmk.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            if (textbox_otp.Text != generatedOTP)
            {
                MessageBox.Show("OTP không đúng!");
                return;
            }

            if (textbox_mkm.Text != textbox_xnmk.Text)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!");
                return;
            }

            // Cập nhật mật khẩu trong database
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string updateSql = "UPDATE [User] SET PassWord=@Password WHERE Email=@Email OR Username=@Username";
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.Parameters.AddWithValue("@Password", HashPassword(textbox_mkm.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Email", textbox_tk.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", textbox_tk.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("✅ Đổi mật khẩu thành công!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản với Email/Username này!");
                    }
                }
            }
        }

        private void button_huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
        private void SendOTP(string toEmail, string otp)
        {
            try
            {
                // Thông tin gửi email
                string fromEmail = "youremail@gmail.com"; // Email của bạn
                string password = "yourAppPassword";      // Mật khẩu ứng dụng Gmail (App Password)

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = "OTP đổi mật khẩu";
                mail.Body = $"Mã OTP của bạn là: {otp}";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;

                smtp.Send(mail);

                MessageBox.Show("OTP đã được gửi vào email của bạn!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi OTP thất bại: " + ex.Message);
            }
        }

    }
}
