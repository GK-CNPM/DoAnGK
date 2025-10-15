using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WindowsFormsApp2
{
    static class PermissionService
    {
        public static HashSet<string> LoadPermissions(int userId)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var conn = Db.Open())
            using (var cmd = new SqlCommand(@"
SELECT DISTINCT p.PermCode
FROM UserRoles ur
JOIN RolePermissions rp ON ur.RoleId = rp.RoleId
JOIN Permissions p ON p.PermId = rp.PermId
WHERE ur.UserId = @uid;", conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read()) set.Add(rd.GetString(0));
            }
            return set;
        }
    }
}
