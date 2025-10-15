using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsApp2
{
    static class UserService
    {
        public static DataTable ListAll()
        {
            using (var conn = Db.Open())
            using (var da = new SqlDataAdapter(
                @"SELECT UserID, FullName, UserName, Email, SDT, IsLocked 
                  FROM [User] ORDER BY UserID", conn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void Create(string fullName, string userName, string email, string sdt, string password)
        {
            using (var conn = Db.Open())
            {
                // Unique check
                using (var chk = new SqlCommand("SELECT COUNT(1) FROM [User] WHERE Email=@e OR SDT=@p", conn))
                {
                    chk.Parameters.AddWithValue("@e", (object)email ?? DBNull.Value);
                    chk.Parameters.AddWithValue("@p", (object)sdt ?? DBNull.Value);
                    if ((int)chk.ExecuteScalar() > 0) throw new Exception("Email hoặc SĐT đã tồn tại!");
                }

                using (var cmd = new SqlCommand(@"
INSERT INTO [User](FullName,UserName,PassWord,Email,SDT,IsLocked)
VALUES(@FullName,@UserName,@Password,@Email,@SDT,0)", conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", Hash(password));
                    cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SDT", (object)sdt ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Update(int id, string fullName, string email, string sdt)
        {
            using (var conn = Db.Open())
            {
                using (var chk = new SqlCommand(
                    "SELECT COUNT(1) FROM [User] WHERE (Email=@e OR SDT=@p) AND UserID<>@id", conn))
                {
                    chk.Parameters.AddWithValue("@e", (object)email ?? DBNull.Value);
                    chk.Parameters.AddWithValue("@p", (object)sdt ?? DBNull.Value);
                    chk.Parameters.AddWithValue("@id", id);
                    if ((int)chk.ExecuteScalar() > 0) throw new Exception("Email hoặc SĐT đã tồn tại!");
                }

                using (var cmd = new SqlCommand(
                    "UPDATE [User] SET FullName=@n, Email=@e, SDT=@p WHERE UserID=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@n", fullName);
                    cmd.Parameters.AddWithValue("@e", (object)email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p", (object)sdt ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ToggleLock(int id, bool newLocked)
        {
            using (var conn = Db.Open())
            using (var cmd = new SqlCommand("UPDATE [User] SET IsLocked=@v WHERE UserID=@id", conn))
            {
                cmd.Parameters.AddWithValue("@v", newLocked);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void ResetPassword(int id, string newPassword)
        {
            using (var conn = Db.Open())
            using (var cmd = new SqlCommand("UPDATE [User] SET PassWord=@p WHERE UserID=@id", conn))
            {
                cmd.Parameters.AddWithValue("@p", Hash(newPassword));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static string Hash(string s)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(s ?? ""));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
