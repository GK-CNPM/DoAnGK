using System.Data.SqlClient;

namespace WindowsFormsApp2
{
    static class Db
    {
        public static SqlConnection Open()
        {
            var conn = new SqlConnection(Program.ConnStr);
            conn.Open();
            return conn;
        }
    }
}
