using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Dangnhap : Form
    {
        // TODO: chỉnh lại đường dẫn mdf cho đúng máy bạn
        private readonly string connStr =
            @"Data Source=(LocalDB)\MSSQLLocalDB;
              AttachDbFilename=D:\cnpm\btl\DoAnGK-main\DoAnGK-main\WindowsFormsApp2 (2)\WindowsFormsApp2\WindowsFormsApp2\Database1.mdf;
              Integrated Security=True;Connect Timeout=30";

        // === Kết quả trả về cho Program ===
        public int LoggedUserId { get; private set; } = 0;
        public HashSet<string> LoggedPerms { get; private set; }

        public Dangnhap()
        {
            InitializeComponent();
        }

        // Đổi tên event cho đúng với Designer: this.Load += Dangnhap_Load;
        private void Dangnhap_Load(object sender, EventArgs e)
        {
            guna2PanelLogin.BorderRadius = 20;
            guna2PanelLogin.BorderColor = Color.White;
            guna2PanelLogin.FillColor = Color.FromArgb(153, 128, 128, 128);
            guna2PanelLogin.BackColor = Color.Transparent;

            Color textBoxFillColor = Color.Gainsboro;

            Textbox_taikhoan.BorderRadius = 0;
            Textbox_taikhoan.BorderThickness = 0;
            Textbox_taikhoan.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            Textbox_taikhoan.FocusedState.BorderColor = Color.Black;
            Textbox_taikhoan.HoverState.BorderColor = Color.Gray;
            Textbox_taikhoan.FillColor = textBoxFillColor;
            Textbox_taikhoan.ForeColor = Color.Black;

            Textbox_matkhau.BorderRadius = 0;
            Textbox_matkhau.BorderThickness = 0;
            Textbox_matkhau.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            Textbox_matkhau.FocusedState.BorderColor = Color.Black;
            Textbox_matkhau.HoverState.BorderColor = Color.Gray;
            Textbox_matkhau.FillColor = textBoxFillColor;
            Textbox_matkhau.ForeColor = Color.Black;
            Textbox_matkhau.UseSystemPasswordChar = true;

            if (Properties.Settings.Default.RememberMe)
            {
                Textbox_taikhoan.Text = Properties.Settings.Default.SavedUser;
                checkbox_giutrangthai.Checked = true;
            }
        }

        private void button_dangnhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Textbox_taikhoan.Text) ||
                string.IsNullOrWhiteSpace(Textbox_matkhau.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!");
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Lấy UserID, PassWord, IsLocked
                    string sql = @"
                        SELECT TOP(1) UserID, PassWord, IsLocked
                        FROM [User]
                        WHERE UserName=@acc OR Email=@acc OR SDT=@acc";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", Textbox_taikhoan.Text.Trim());

                        using (var rd = cmd.ExecuteReader())
                        {
                            if (!rd.Read())
                            {
                                MessageBox.Show("❌ Tài khoản không tồn tại!");
                                return;
                            }

                            int userId = rd.GetInt32(0);
                            string storedHash = rd.IsDBNull(1) ? "" : rd.GetString(1);
                            bool isLocked = !rd.IsDBNull(2) && rd.GetBoolean(2);

                            if (isLocked)
                            {
                                MessageBox.Show("🔒 Tài khoản đã bị khoá. Liên hệ quản trị viên.");
                                return;
                            }

                            string inputHash = HashPassword(Textbox_matkhau.Text);
                            if (!string.Equals(storedHash, inputHash, StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("❌ Sai mật khẩu!");
                                return;
                            }

                            // Đăng nhập thành công
                            LoggedUserId = userId;
                        }
                    }

                    // Tải quyền: User → Roles → Permissions
                    LoggedPerms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    string sqlPerms = @"
                        SELECT DISTINCT p.PermCode
                        FROM UserRoles ur
                        JOIN RolePermissions rp ON ur.RoleId = rp.RoleId
                        JOIN Permissions p ON p.PermId = rp.PermId
                        WHERE ur.UserId = @uid";
                    using (var cmdPerm = new SqlCommand(sqlPerms, conn))
                    {
                        cmdPerm.Parameters.AddWithValue("@uid", LoggedUserId);
                        using (var r2 = cmdPerm.ExecuteReader())
                            while (r2.Read())
                                LoggedPerms.Add(r2.GetString(0));
                    }
                }

                // Remember me
                if (checkbox_giutrangthai.Checked)
                {
                    Properties.Settings.Default.SavedUser = Textbox_taikhoan.Text.Trim();
                    Properties.Settings.Default.RememberMe = true;
                }
                else
                {
                    Properties.Settings.Default.SavedUser = "";
                    Properties.Settings.Default.RememberMe = false;
                }
                Properties.Settings.Default.Save();

                // Trả kết quả cho Program và đóng form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password ?? ""));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private void linkLabel_dangky_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Dangky().ShowDialog(this);
        }

        private void linkLabel_quenmk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Quenmatkhau().ShowDialog(this);
        }
    }
}
