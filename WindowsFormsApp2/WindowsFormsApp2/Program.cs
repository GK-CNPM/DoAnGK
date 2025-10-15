using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public static class Program
    {
        public static string ConnStr =
            @"Data Source=(LocalDB)\MSSQLLocalDB;
          AttachDbFilename=D:\cnpm\btl\DoAnGK-main\DoAnGK-main\WindowsFormsApp2 (2)\WindowsFormsApp2\WindowsFormsApp2\Database1.mdf;
          Integrated Security=True;Connect Timeout=30";

        public static int CurrentUserId = 0;
        public static HashSet<string> CurrentPerms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMainMenu());
            using (var login = new Dangnhap())
            {
                if (login.ShowDialog() != DialogResult.OK) return;
                CurrentUserId = login.LoggedUserId;
                CurrentPerms = login.LoggedPerms ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            //new Main()
          
        }



        // ================== SEED ADMIN + FULL QUYỀN ==================
        private static void EnsureDefaultAdminAndGrantAllPerms()
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) Tạo user admin nếu chưa có
                        int adminId = GetInt(conn, tx, "SELECT UserID FROM [User] WHERE UserName='admin'");
                        if (adminId == 0)
                        {
                            var cmd = new SqlCommand(@"
INSERT INTO [User](FullName,UserName,PassWord,Email,SDT,IsLocked)
VALUES(N'Quản trị hệ thống','admin',@pw,'admin@example.com','0000000000',0);
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tx);
                            cmd.Parameters.AddWithValue("@pw", HashPassword("12345"));
                            adminId = (int)cmd.ExecuteScalar();
                        }

                        // 2) Tạo role ADMIN nếu chưa có
                        int adminRoleId = GetInt(conn, tx, "SELECT RoleId FROM Roles WHERE RoleCode='ADMIN'");
                        if (adminRoleId == 0)
                        {
                            var cmd = new SqlCommand(@"
INSERT INTO Roles(RoleCode,RoleName)
OUTPUT INSERTED.RoleId
VALUES('ADMIN', N'Quản trị hệ thống');", conn, tx);
                            adminRoleId = (int)cmd.ExecuteScalar();
                        }

                        // 3) Gán ADMIN cho user admin nếu chưa gán
                        var urExists = GetInt(conn, tx,
                            "SELECT COUNT(*) FROM UserRoles WHERE UserId=@u AND RoleId=@r",
                            ("@u", adminId), ("@r", adminRoleId));
                        if (urExists == 0)
                        {
                            var cmd = new SqlCommand("INSERT INTO UserRoles(UserId,RoleId) VALUES(@u,@r)",
                                conn, tx);
                            cmd.Parameters.AddWithValue("@u", adminId);
                            cmd.Parameters.AddWithValue("@r", adminRoleId);
                            cmd.ExecuteNonQuery();
                        }

                        // 4) Cấp FULL mọi quyền hiện có trong Permissions cho role ADMIN
                        var grantAll = new SqlCommand(@"
INSERT INTO RolePermissions(RoleId, PermId)
SELECT @roleId, p.PermId
FROM Permissions p
WHERE NOT EXISTS (
    SELECT 1 FROM RolePermissions rp 
    WHERE rp.RoleId=@roleId AND rp.PermId=p.PermId
);", conn, tx);
                        grantAll.Parameters.AddWithValue("@roleId", adminRoleId);
                        grantAll.ExecuteNonQuery();

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        // Helpers
        private static int GetInt(SqlConnection conn, SqlTransaction tx, string sql, params (string, object)[] prms)
        {
            using (var cmd = new SqlCommand(sql, conn, tx))
            {
                foreach (var (n, v) in prms) cmd.Parameters.AddWithValue(n, v ?? DBNull.Value);
                var obj = cmd.ExecuteScalar();
                if (obj == null || obj == DBNull.Value) return 0;
                return Convert.ToInt32(obj);
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? ""));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
