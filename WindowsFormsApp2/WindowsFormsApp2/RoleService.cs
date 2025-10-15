using System;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp2
{
    static class RoleService
    {
        public static DataTable ListRoles()
        {
            using (var conn = Db.Open())
            using (var da = new SqlDataAdapter("SELECT RoleId, RoleCode, RoleName FROM Roles ORDER BY RoleCode", conn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable ListUserRoles(int userId)
        {
            using (var conn = Db.Open())
            using (var da = new SqlDataAdapter(@"
SELECT r.RoleId, r.RoleCode, r.RoleName,
       CASE WHEN ur.UserId IS NULL THEN 0 ELSE 1 END AS Assigned
FROM Roles r
LEFT JOIN UserRoles ur ON ur.RoleId = r.RoleId AND ur.UserId = @uid
ORDER BY r.RoleCode;", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@uid", userId);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void SetUserRole(int userId, int roleId, bool assign)
        {
            using (var conn = Db.Open())
            {
                if (assign)
                {
                    using (var cmd = new SqlCommand(@"
IF NOT EXISTS(SELECT 1 FROM UserRoles WHERE UserId=@u AND RoleId=@r)
INSERT INTO UserRoles(UserId,RoleId) VALUES(@u,@r);", conn))
                    {
                        cmd.Parameters.AddWithValue("@u", userId);
                        cmd.Parameters.AddWithValue("@r", roleId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var cmd = new SqlCommand(
                        "DELETE FROM UserRoles WHERE UserId=@u AND RoleId=@r;", conn))
                    {
                        cmd.Parameters.AddWithValue("@u", userId);
                        cmd.Parameters.AddWithValue("@r", roleId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
