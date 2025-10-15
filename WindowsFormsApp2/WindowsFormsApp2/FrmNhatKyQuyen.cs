using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FrmNhatKyQuyen : Form
    {
        // Dùng chung chuỗi kết nối
        private readonly string _connStr = Program.ConnStr;

        public FrmNhatKyQuyen()
        {
            InitializeComponent();
        }

        private void FrmNhatKyQuyen_Load(object sender, EventArgs e)
        {
            dtTo.Value = DateTime.Today;
            dtFrom.Value = DateTime.Today.AddDays(-7);
            LoadAudit();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadAudit();
        }

        private void LoadAudit()
        {
            try
            {
                // 1) Lấy range theo local và convert sang UTC để so sánh
                DateTime fromLocal = dtFrom.Value.Date;
                DateTime toLocal = dtTo.Value.Date.AddDays(1); // exclusive
                DateTime fromUtc = DateTime.SpecifyKind(fromLocal, DateTimeKind.Local).ToUniversalTime();
                DateTime toUtc = DateTime.SpecifyKind(toLocal, DateTimeKind.Local).ToUniversalTime();

                string actor = (txtActor.Text ?? "").Trim();
                string target = (txtTarget.Text ?? "").Trim();
                if (actor == "Actor username") actor = "";
                if (target == "Target username") target = "";

                // 2) SQL: lấy thẳng UTC, chuyển sang giờ máy ở C#
                using (var conn = new SqlConnection(_connStr))
                using (var da = new SqlDataAdapter(@"
SELECT 
    a.AuditId,
    a.At          AS AtUtc,         -- giữ UTC
    au.UserName   AS Actor,
    tu.UserName   AS Target,
    a.RoleCode,
    a.[Action]
FROM AuditRoleAssign a
LEFT JOIN [User] au ON au.UserID = a.ActorUserId
LEFT JOIN [User] tu ON tu.UserID = a.TargetUserId
WHERE a.At >= @fromUtc AND a.At < @toUtc
  AND (@actor = ''  OR au.UserName LIKE @actorLike)
  AND (@target = '' OR tu.UserName LIKE @targetLike)
ORDER BY a.AuditId DESC;", conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@fromUtc", fromUtc);
                    da.SelectCommand.Parameters.AddWithValue("@toUtc", toUtc);
                    da.SelectCommand.Parameters.AddWithValue("@actor", actor);
                    da.SelectCommand.Parameters.AddWithValue("@actorLike", "%" + actor + "%");
                    da.SelectCommand.Parameters.AddWithValue("@target", target);
                    da.SelectCommand.Parameters.AddWithValue("@targetLike", "%" + target + "%");

                    var dt = new DataTable();
                    da.Fill(dt);

                    // 3) Thêm cột AtLocal (local time) để hiển thị
                    if (!dt.Columns.Contains("AtLocal"))
                        dt.Columns.Add("AtLocal", typeof(DateTime));

                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["AtUtc"] != DBNull.Value)
                        {
                            var utc = (DateTime)r["AtUtc"]; // giả định At là datetime2(0) UTC
                            r["AtLocal"] = DateTime.SpecifyKind(utc, DateTimeKind.Utc).ToLocalTime();
                        }
                    }

                    // 4) Sắp xếp hiển thị, ẩn cột AtUtc
                    dgvAudit.DataSource = dt;
                    if (dgvAudit.Columns.Contains("AtUtc")) dgvAudit.Columns["AtUtc"].Visible = false;
                    if (dgvAudit.Columns.Contains("AuditId")) dgvAudit.Columns["AuditId"].HeaderText = "ID";
                    if (dgvAudit.Columns.Contains("AtLocal")) dgvAudit.Columns["AtLocal"].HeaderText = "Thời gian";
                    if (dgvAudit.Columns.Contains("Actor")) dgvAudit.Columns["Actor"].HeaderText = "Người thực hiện";
                    if (dgvAudit.Columns.Contains("Target")) dgvAudit.Columns["Target"].HeaderText = "Tài khoản";
                    if (dgvAudit.Columns.Contains("RoleCode")) dgvAudit.Columns["RoleCode"].HeaderText = "Vai trò";
                    if (dgvAudit.Columns.Contains("Action")) dgvAudit.Columns["Action"].HeaderText = "Hành động";

                    dgvAudit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp dữ liệu nhật ký: " + ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var dt = dgvAudit.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.");
                return;
            }

            using (var sfd = new SaveFileDialog { Filter = "CSV file|*.csv", FileName = "audit_role_assign.csv" })
            {
                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                var sb = new StringBuilder();

                // Header
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!dt.Columns[i].ColumnName.Equals("AtUtc", StringComparison.OrdinalIgnoreCase))
                    {
                        if (sb.Length > 0) sb.Append(",");
                        sb.Append(dt.Columns[i].ColumnName);
                    }
                }
                sb.AppendLine();

                // Rows
                foreach (DataRow row in dt.Rows)
                {
                    bool first = true;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName.Equals("AtUtc", StringComparison.OrdinalIgnoreCase)) continue;

                        if (!first) sb.Append(",");
                        first = false;

                        var val = row[col] == null ? "" : row[col].ToString();
                        val = val.Replace("\"", "\"\"");
                        sb.Append("\"").Append(val).Append("\"");
                    }
                    sb.AppendLine();
                }

                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Đã xuất: " + sfd.FileName);
            }
        }

        // Placeholders cho placeholder text
        private void txtActor_GotFocus(object sender, EventArgs e)
        {
            if (txtActor.Text == "Actor username")
            {
                txtActor.Text = "";
                txtActor.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtActor_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtActor.Text))
            {
                txtActor.Text = "Actor username";
                txtActor.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtTarget_GotFocus(object sender, EventArgs e)
        {
            if (txtTarget.Text == "Target username")
            {
                txtTarget.Text = "";
                txtTarget.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtTarget_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTarget.Text))
            {
                txtTarget.Text = "Target username";
                txtTarget.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void dgvAudit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
