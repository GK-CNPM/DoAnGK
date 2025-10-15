using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    // Class tiện ích tĩnh, không dùng Designer
    public static class PermissionGuard
    {
        /// <summary>
        /// Duyệt toàn bộ controls và (nếu có) MainMenuStrip để ẩn/disable theo Tag = "perm:CODE"
        /// </summary>
        public static void Apply(Form form, ISet<string> perms, bool hideInsteadOfDisable = true)
        {
            foreach (Control c in form.Controls)
                ApplyRecursive(c, perms, hideInsteadOfDisable);

            if (form.MainMenuStrip != null)
                ApplyMenu(form.MainMenuStrip.Items, perms, hideInsteadOfDisable);
        }

        private static void ApplyRecursive(Control c, ISet<string> perms, bool hide)
        {
            var tag = c.Tag as string;
            if (!string.IsNullOrEmpty(tag) && tag.StartsWith("perm:"))
            {
                var code = tag.Substring(5);
                bool allow = perms != null && perms.Contains(code);
                if (hide) c.Visible = allow; else c.Enabled = allow;
            }

            foreach (Control child in c.Controls)
                ApplyRecursive(child, perms, hide);
        }

        private static void ApplyMenu(ToolStripItemCollection items, ISet<string> perms, bool hide)
        {
            foreach (ToolStripItem it in items)
            {
                var tag = it.Tag as string;
                if (!string.IsNullOrEmpty(tag) && tag.StartsWith("perm:"))
                {
                    var code = tag.Substring(5);
                    bool allow = perms != null && perms.Contains(code);
                    if (hide) it.Visible = allow; else it.Enabled = allow;
                }

                if (it is ToolStripMenuItem mi && mi.DropDownItems.Count > 0)
                    ApplyMenu(mi.DropDownItems, perms, hide);
            }
        }
    }
}
