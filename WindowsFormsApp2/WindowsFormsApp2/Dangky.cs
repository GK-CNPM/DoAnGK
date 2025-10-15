using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Dangky : Form
    {
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;
                   AttachDbFilename=D:\cnpm\btl\DoAnGK-main\DoAnGK-main\WindowsFormsApp2 (2)\WindowsFormsApp2\WindowsFormsApp2\Database1.mdf;
                   Integrated Security=True;
                   Connect Timeout=30";


        public Dangky()
        {
           
            InitializeComponent();
            this.Load += FormLoad_Dangky;                 // Gán sự kiện Load
            button_dangky.Click += ButtonDangky_Click;    // Gán sự kiện đăng ký
                  
        }

        private void FormLoad_Dangky(object sender, EventArgs e)
        {
            // Thiết lập panel
            panel_dangky.BorderRadius = 20;
            panel_dangky.BorderColor = Color.White;
            panel_dangky.FillColor = Color.FromArgb(153, 128, 128, 128);
            panel_dangky.BackColor = Color.Transparent;

            Color fillColor = Color.Gainsboro;
            Guna.UI2.WinForms.Guna2TextBox[] textboxes = { textbox_hoten, textbox_tendangnhap, textbox_matkhau,
                textbox_xnmatkhau, textbox_email, textbox_diachi, textbox_sdt };

            foreach (var tb in textboxes)
            {
                tb.BorderRadius = 0;
                tb.BorderThickness = 0;
                tb.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
                tb.FocusedState.BorderColor = Color.Black;
                tb.HoverState.BorderColor = Color.Gray;
                tb.FillColor = fillColor;
                tb.ForeColor = Color.Black;
                tb.PlaceholderText = "Nhập nội dung...";
            }

            textbox_matkhau.UseSystemPasswordChar = true;
            textbox_xnmatkhau.UseSystemPasswordChar = true;
        }

        private void ButtonDangky_Click(object sender, EventArgs e)
        {
            // Copy toàn bộ logic đăng ký từ btnRegister_Click vào đây
            if (string.IsNullOrWhiteSpace(textbox_hoten.Text) ||
                string.IsNullOrWhiteSpace(textbox_tendangnhap.Text) ||
                string.IsNullOrWhiteSpace(textbox_matkhau.Text) ||
                string.IsNullOrWhiteSpace(textbox_xnmatkhau.Text) ||
                string.IsNullOrWhiteSpace(textbox_email.Text) ||
                string.IsNullOrWhiteSpace(textbox_diachi.Text) ||
                string.IsNullOrWhiteSpace(textbox_sdt.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            if (textbox_matkhau.Text != textbox_xnmatkhau.Text)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!");
                return;
            }

            if (!Regex.IsMatch(textbox_email.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ!");
                return;
            }

            if (!checkbox_chapnhan.Checked)
            {
                MessageBox.Show("Bạn phải chấp nhận điều khoản để đăng ký!");
                return;
            }

            string gender = "";
            if (checkbox_nam.Checked) gender = "Nam";
            else if (checkbox_nu.Checked) gender = "Nữ";
            else
            {
                MessageBox.Show("Vui lòng chọn giới tính!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string checkSql = "SELECT COUNT(*) FROM [User] WHERE Username=@username OR Email=@Email OR SDT=@SDT";
                using (SqlCommand cmdCheck = new SqlCommand(checkSql, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@username", textbox_tendangnhap.Text.Trim());
                    cmdCheck.Parameters.AddWithValue("@Email", textbox_email.Text.Trim());
                    cmdCheck.Parameters.AddWithValue("@SDT", textbox_sdt.Text.Trim());

                    int count = (int)cmdCheck.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Tên đăng nhập, email hoặc SĐT đã tồn tại!");
                        return;
                    }
                }

                string insertSql = "INSERT INTO [User] (FullName, Username, PassWord, Email, Address, Gender, SDT) " +
                                   "VALUES (@FullName, @Username, @Password, @Email, @Address, @Gender, @SDT)";
                using (SqlCommand cmdInsert = new SqlCommand(insertSql, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@FullName", textbox_hoten.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Username", textbox_tendangnhap.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Password", HashPassword(textbox_matkhau.Text.Trim()));
                    cmdInsert.Parameters.AddWithValue("@Email", textbox_email.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Address", textbox_diachi.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Gender", gender);
                    cmdInsert.Parameters.AddWithValue("@SDT", textbox_sdt.Text.Trim());

                    cmdInsert.ExecuteNonQuery();
                    MessageBox.Show(" Đăng ký thành công!");
                    this.Close();
                }
            }
        }

      
        private void ButtonHuy_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form
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
        // Nếu panel_dangky có sự kiện Paint
        private void panel_dangky_Paint(object sender, PaintEventArgs e)
        {
            // Không cần xử lý, để trống
        }

        // Nếu có nút guna2Button2 và sự kiện Click
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Không cần xử lý, để trống
        }

        // Nếu có textbox guna2TextBox6 và sự kiện TextChanged
        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {
            // Không cần xử lý, để trống
        }

        // Nếu Form có sự kiện Load tên Form3cs_Load
        private void Form3cs_Load(object sender, EventArgs e)
        {
            // Không cần xử lý, để trống
        }

    }
}
