using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
// nếu dùng InputBox
using Microsoft.VisualBasic;

namespace WindowsFormsApp2
{
    public partial class Main : Form
    {
        // ====== KHAI BÁO CHUNG ======
        private readonly string _connStr = Program.ConnStr;


        public Main()
        {
            InitializeComponent();

        }

        // ====== LOAD FORM ======
        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                if (Program.CurrentPerms != null)
                    PermissionGuard.Apply(this, Program.CurrentPerms, hideInsteadOfDisable: false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể áp quyền UI: " + ex.Message);
            }

            LoadUsers();
        }

        // ====== NẠP DANH SÁCH USER LÊN LƯỚI ======
        private void LoadUsers()
        {
            using (var conn = new SqlConnection(_connStr))
            using (var da = new SqlDataAdapter(
                @"SELECT UserID, FullName, UserName, Email, SDT, IsLocked 
                  FROM [User] ORDER BY UserID", conn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                dgvUsers.DataSource = dt;
            }

            if (dgvUsers.Columns.Contains("IsLocked"))
                dgvUsers.Columns["IsLocked"].HeaderText = "Bị khoá";
        }

        // ====== MỞ MÀN NHẬT KÝ QUYỀN (BÁO CÁO) ======
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (Program.CurrentPerms == null || !Program.CurrentPerms.Contains("Report.View"))
            {
                MessageBox.Show("Bạn không có quyền xem nhật ký.");
                return;
            }

            using (var f = new FrmNhatKyQuyen())
                f.ShowDialog(this);
        }

        // ====== GÁN VAI TRÒ (FR-1 + FR-3 của 4.4) ======
        private void menuAssignRole_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) { MessageBox.Show("Chọn người dùng."); return; }
            int targetUserId = Convert.ToInt32(((DataRowView)dgvUsers.CurrentRow.DataBoundItem)["UserID"]);

            using (var dlg = new FrmGanVaiTro(targetUserId, Program.CurrentUserId))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Nếu gán quyền cho chính user đang đăng nhập, có thể reload perms + áp lại UI:
                    // Program.CurrentPerms = ReloadPerms(Program.CurrentUserId);
                    // PermissionGuard.Apply(this, Program.CurrentPerms, true);
                }
            }
        }

        private HashSet<string> ReloadPerms(int userId)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                var sql = @"
                    SELECT DISTINCT p.PermCode
                    FROM UserRoles ur
                    JOIN RolePermissions rp ON ur.RoleId = rp.RoleId
                    JOIN Permissions p ON p.PermId = rp.PermId
                    WHERE ur.UserId = @uid";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read()) set.Add(rd.GetString(0));
                }
            }
            return set;
        }

        // ====== 4.3 — TẠO NGƯỜI DÙNG ======
        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            string fullName = Interaction.InputBox("Nhập họ tên:", "Tạo người dùng", "");
            if (string.IsNullOrWhiteSpace(fullName)) return;

            string username = Interaction.InputBox("Nhập tên đăng nhập:", "Tạo người dùng", "");
            if (string.IsNullOrWhiteSpace(username)) return;

            string email = Interaction.InputBox("Nhập Email:", "Tạo người dùng", "");
            string sdt = Interaction.InputBox("Nhập SĐT:", "Tạo người dùng", "");
            string password = Interaction.InputBox("Nhập mật khẩu:", "Tạo người dùng", "");

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();

                // FR-2: Không trùng Email/SDT
                var check = new SqlCommand("SELECT COUNT(1) FROM [User] WHERE Email=@e OR SDT=@p", conn);
                check.Parameters.AddWithValue("@e", email);
                check.Parameters.AddWithValue("@p", sdt);
                int exist = (int)check.ExecuteScalar();
                if (exist > 0) { MessageBox.Show("Email hoặc SĐT đã tồn tại!"); return; }

                // Insert
                var cmd = new SqlCommand(@"
                    INSERT INTO [User](FullName,UserName,PassWord,Email,SDT,IsLocked)
                    VALUES(@FullName,@UserName,@Password,@Email,@SDT,0)", conn);

                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", HashPassword(password));
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.ExecuteNonQuery();
            }

            LoadUsers();
            MessageBox.Show("✅ Đã tạo tài khoản thành công!");
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password ?? ""));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        // ====== 4.3 — SỬA NGƯỜI DÙNG ======
        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) { MessageBox.Show("Chọn người dùng cần sửa!"); return; }

            int id = Convert.ToInt32(((DataRowView)dgvUsers.CurrentRow.DataBoundItem)["UserID"]);
            string newName = Interaction.InputBox("Nhập họ tên mới:", "Sửa người dùng", "");
            string newEmail = Interaction.InputBox("Nhập email mới:", "Sửa người dùng", "");
            string newPhone = Interaction.InputBox("Nhập SĐT mới:", "Sửa người dùng", "");

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();

                var check = new SqlCommand(
                    "SELECT COUNT(1) FROM [User] WHERE (Email=@e OR SDT=@p) AND UserID<>@id", conn);
                check.Parameters.AddWithValue("@e", newEmail);
                check.Parameters.AddWithValue("@p", newPhone);
                check.Parameters.AddWithValue("@id", id);
                int exist = (int)check.ExecuteScalar();
                if (exist > 0) { MessageBox.Show("Email hoặc SĐT đã tồn tại!"); return; }

                var cmd = new SqlCommand(@"
                    UPDATE [User] 
                    SET FullName=@n, Email=@e, SDT=@p 
                    WHERE UserID=@id", conn);
                cmd.Parameters.AddWithValue("@n", newName);
                cmd.Parameters.AddWithValue("@e", newEmail);
                cmd.Parameters.AddWithValue("@p", newPhone);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            LoadUsers();
            MessageBox.Show("✅ Cập nhật thành công!");
        }

        // ====== 4.3 — KHOÁ / MỞ KHOÁ ======
        private void btnToggleLock_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) { MessageBox.Show("Chọn người dùng cần khoá/mở!"); return; }

            var row = (DataRowView)dgvUsers.CurrentRow.DataBoundItem;
            int id = Convert.ToInt32(row["UserID"]);
            bool isLocked = row["IsLocked"] != DBNull.Value && (bool)row["IsLocked"];

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE [User] SET IsLocked=@v WHERE UserID=@id", conn);
                cmd.Parameters.AddWithValue("@v", !isLocked);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            LoadUsers();
            MessageBox.Show(isLocked ? "🔓 Đã mở khoá tài khoản!" : "🔒 Đã khoá tài khoản!");
        }

        // ====== 4.3 — RESET MẬT KHẨU ======
        private void btnResetPw_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) { MessageBox.Show("Chọn người dùng cần reset mật khẩu!"); return; }

            var row = (DataRowView)dgvUsers.CurrentRow.DataBoundItem;
            int id = Convert.ToInt32(row["UserID"]);
            string email = Convert.ToString(row["Email"]);

            string newPass = Interaction.InputBox("Nhập mật khẩu mới:", "Reset mật khẩu", "");
            if (string.IsNullOrWhiteSpace(newPass)) return;

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE [User] SET PassWord=@p WHERE UserID=@id", conn);
                cmd.Parameters.AddWithValue("@p", HashPassword(newPass));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show($"✅ Mật khẩu của {email} đã được đặt lại!");
        }

        // ====== SỰ KIỆN LƯỚI (nếu có dùng) ======
        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvUsers_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            // 1) Ẩn Main trong lúc hiển thị màn đăng nhập mới
            this.Hide();

            using (var login = new Dangnhap())
            {
                var rs = login.ShowDialog(this);  // Dangnhap sẽ set DialogResult.OK khi đăng nhập thành công

                if (rs == DialogResult.OK)
                {
                    // 2) Cập nhật lại phiên đăng nhập mới
                    Program.CurrentUserId = login.LoggedUserId;
                    Program.CurrentPerms = login.LoggedPerms ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    // 3) Áp lại quyền UI + nạp lại dữ liệu theo user mới
                    PermissionGuard.Apply(this, Program.CurrentPerms, hideInsteadOfDisable: false);
                    LoadUsers();

                    // 4) Hiện lại Main
                    this.Show();
                }
                else
                {
                    // Người dùng bấm Hủy/Close ở màn đăng nhập -> thoát app
                    this.Close();
                }
            }
        }

    }
}
