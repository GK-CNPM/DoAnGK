using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FrmGanVaiTro : Form
    {
        private readonly int _targetUserId;
        private readonly int _actorUserId;

        public FrmGanVaiTro(int targetUserId, int actorUserId)
        {
            InitializeComponent();
            _targetUserId = targetUserId;
            _actorUserId = actorUserId;
        }

        private void FrmGanVaiTro_Load(object sender, EventArgs e)
        {
            dgvRoles.DataSource = RoleService.ListUserRoles(_targetUserId);
            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.ReadOnly = false;
            dgvRoles.Columns["Assigned"].ReadOnly = false;
            foreach (DataGridViewColumn c in dgvRoles.Columns)
                if (c.Name != "Assigned") c.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvRoles.Rows)
            {
                int roleId = (int)row.Cells["RoleId"].Value;
                bool assigned = Convert.ToInt32(row.Cells["Assigned"].Value) == 1;
                RoleService.SetUserRole(_targetUserId, roleId, assigned);
            }

            Audit.Log("Role.Assign", $"Actor={_actorUserId}; Target={_targetUserId}", "SUCCESS");
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}
