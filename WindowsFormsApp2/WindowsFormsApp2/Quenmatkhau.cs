using Guna.UI2.WinForms;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Quenmatkhau : Form
    {
        // DÙNG CHUNG CHUỖI KẾT NỐI
        private readonly string connStr = Program.ConnStr;

        // OTP runtime
        private string generatedOTP;
        private DateTime otpExpireAt;
        private int wrongOtpCount = 0;

        public Quenmatkhau()
        {
            InitializeComponent();
           // this.Load += Quenmatkhau_Load;

           // button_gui.Click += button_gui_Click;
           // button_dmk.Click += button_dmk_Click;
           // button_huy.Click += (s, e) => this.Close();
        }
        
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            // Không cần xử lý gì cũng được
        }

        private void button_huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Quenmatkhau_Load(object sender, EventArgs e)
        {
            guna2Panel1.BorderRadius = 20;
            guna2Panel1.BorderColor = Color.White;
            guna2Panel1.FillColor = Color.FromArgb(153, 128, 128, 128);
            guna2Panel1.BackColor = Color.Transparent;

            var textboxes = new[] { textbox_tk, textbox_otp, textbox_mkm, textbox_xnmk };
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

        // GỬI OTP
        private void button_gui_Click(object sender, EventArgs e)
        {
            string acc = textbox_tk.Text.Trim();
            if (string.IsNullOrWhiteSpace(acc))
            {
                MessageBox.Show("Vui lòng nhập Email hoặc Tài khoản!");
                return;
            }

            // 1) Kiểm tra tài khoản có tồn tại & không bị khoá
            int userId = 0;
            string email = null;
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    var sql = @"
SELECT TOP(1) UserID, Email, IsLocked
FROM [User]
WHERE Email=@acc OR UserName=@acc;";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", acc);
                        using (var rd = cmd.ExecuteReader())
                        {
                            if (!rd.Read())
                            {
                                MessageBox.Show("❌ Không tìm thấy tài khoản với Email/Tài khoản này!");
                                return;
                            }
                            userId = rd.GetInt32(0);
                            email = rd.IsDBNull(1) ? null : rd.GetString(1);
                            bool isLocked = !rd.IsDBNull(2) && rd.GetBoolean(2);
                            if (isLocked)
                            {
                                MessageBox.Show("🔒 Tài khoản đã bị khoá. Liên hệ quản trị viên.");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra tài khoản: " + ex.Message);
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Tài khoản này chưa có Email để nhận OTP. Liên hệ quản trị viên.");
                return;
            }

            // 2) Tạo OTP 6 số an toàn + hạn dùng 5 phút
            generatedOTP = GenerateOtp6Digits();
            otpExpireAt = DateTime.Now.AddMinutes(5);
            wrongOtpCount = 0;

            // 3) Gửi email OTP
            try
            {
                SendOTP(email, generatedOTP);
                MessageBox.Show("✅ OTP đã gửi, kiểm tra Email của bạn (hạn dùng 5 phút)!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi OTP thất bại: " + ex.Message);
            }
        }

        // ĐỔI MẬT KHẨU
        private void button_dmk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textbox_otp.Text) ||
                string.IsNullOrWhiteSpace(textbox_mkm.Text) ||
                string.IsNullOrWhiteSpace(textbox_xnmk.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ OTP, mật khẩu và xác nhận!");
                return;
            }

            if (generatedOTP == null)
            {
                MessageBox.Show("Vui lòng yêu cầu OTP trước.");
                return;
            }

            if (DateTime.Now > otpExpireAt)
            {
                MessageBox.Show("OTP đã hết hạn. Vui lòng yêu cầu OTP mới.");
                return;
            }

            if (textbox_otp.Text.Trim() != generatedOTP)
            {
                wrongOtpCount++;
                if (wrongOtpCount >= 5)
                {
                    MessageBox.Show("Bạn đã nhập sai OTP quá số lần cho phép. Hãy yêu cầu OTP mới.");
                    generatedOTP = null; // vô hiệu OTP cũ
                }
                else
                {
                    MessageBox.Show("OTP không đúng!");
                }
                return;
            }

            if (textbox_mkm.Text != textbox_xnmk.Text)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!");
                return;
            }

            // Cập nhật mật khẩu
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    var sql = @"
UPDATE [User]
SET PassWord=@Password
WHERE Email=@acc OR UserName=@acc;";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", HashPassword(textbox_mkm.Text.Trim()));
                        cmd.Parameters.AddWithValue("@acc", textbox_tk.Text.Trim());
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("✅ Đổi mật khẩu thành công!");
                            generatedOTP = null; // thu hồi OTP
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy tài khoản với Email/Tài khoản này!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đổi mật khẩu: " + ex.Message);
            }
        }

        // ===== Helpers =====
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password ?? ""));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private static string GenerateOtp6Digits()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                int val = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1_000_000;
                return val.ToString("D6");
            }
        }


        private void SendOTP(string toEmail, string otp)
        {
            // TODO: thay bằng email & app password thực của bạn
            string fromEmail = "youremail@gmail.com";
            string appPassword = "yourAppPassword"; // Gmail App Password (2FA bắt buộc)

            using (var mail = new MailMessage())
            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                mail.From = new MailAddress(fromEmail, "CalSmart");
                mail.To.Add(toEmail);
                mail.Subject = "Mã OTP đổi mật khẩu";
                mail.Body = $"Mã OTP của bạn là: {otp}\nHiệu lực đến: {otpExpireAt:HH:mm dd/MM/yyyy}";

                smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
    }
}
