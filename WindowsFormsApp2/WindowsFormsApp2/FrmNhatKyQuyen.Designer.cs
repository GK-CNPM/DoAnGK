using System.Windows.Forms;

namespace WindowsFormsApp2
{
    partial class FrmNhatKyQuyen
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgvAudit;
        private DateTimePicker dtFrom;
        private DateTimePicker dtTo;
        private TextBox txtActor;
        private TextBox txtTarget;
        private Button btnReload;
        private Button btnExport;
        private Label lblFrom;
        private Label lblTo;
        private Label lblActor;
        private Label lblTarget;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvAudit = new System.Windows.Forms.DataGridView();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.txtActor = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblActor = new System.Windows.Forms.Label();
            this.lblTarget = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAudit)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAudit
            // 
            this.dgvAudit.AllowUserToAddRows = false;
            this.dgvAudit.AllowUserToDeleteRows = false;
            this.dgvAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAudit.Location = new System.Drawing.Point(12, 64);
            this.dgvAudit.MultiSelect = false;
            this.dgvAudit.Name = "dgvAudit";
            this.dgvAudit.ReadOnly = true;
            this.dgvAudit.RowHeadersVisible = false;
            this.dgvAudit.RowHeadersWidth = 51;
            this.dgvAudit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAudit.Size = new System.Drawing.Size(776, 374);
            this.dgvAudit.TabIndex = 7;
            this.dgvAudit.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAudit_CellContentClick);
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "yyyy-MM-dd";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(58, 22);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(110, 22);
            this.dtFrom.TabIndex = 0;
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "yyyy-MM-dd";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(212, 22);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(110, 22);
            this.dtTo.TabIndex = 1;
            // 
            // txtActor
            // 
            this.txtActor.ForeColor = System.Drawing.Color.Gray;
            this.txtActor.Location = new System.Drawing.Point(372, 22);
            this.txtActor.Name = "txtActor";
            this.txtActor.Size = new System.Drawing.Size(130, 22);
            this.txtActor.TabIndex = 2;
            this.txtActor.Text = "Actor username";
            this.txtActor.GotFocus += new System.EventHandler(this.txtActor_GotFocus);
            this.txtActor.LostFocus += new System.EventHandler(this.txtActor_LostFocus);
            // 
            // txtTarget
            // 
            this.txtTarget.ForeColor = System.Drawing.Color.Gray;
            this.txtTarget.Location = new System.Drawing.Point(562, 22);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(130, 22);
            this.txtTarget.TabIndex = 3;
            this.txtTarget.Text = "Target username";
            this.txtTarget.GotFocus += new System.EventHandler(this.txtTarget_GotFocus);
            this.txtTarget.LostFocus += new System.EventHandler(this.txtTarget_LostFocus);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(708, 18);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(80, 30);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "Tải dữ liệu";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(708, 434);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 30);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Xuất CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(15, 26);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(26, 16);
            this.lblFrom.TabIndex = 8;
            this.lblFrom.Text = "Từ:";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(178, 26);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(34, 16);
            this.lblTo.TabIndex = 9;
            this.lblTo.Text = "Đến:";
            // 
            // lblActor
            // 
            this.lblActor.AutoSize = true;
            this.lblActor.Location = new System.Drawing.Point(331, 26);
            this.lblActor.Name = "lblActor";
            this.lblActor.Size = new System.Drawing.Size(41, 16);
            this.lblActor.TabIndex = 10;
            this.lblActor.Text = "Actor:";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(512, 26);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(50, 16);
            this.lblTarget.TabIndex = 11;
            this.lblTarget.Text = "Target:";
            // 
            // FrmNhatKyQuyen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 476);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.lblActor);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.txtActor);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.dgvAudit);
            this.Name = "FrmNhatKyQuyen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nhật ký thay đổi vai trò";
            this.Load += new System.EventHandler(this.FrmNhatKyQuyen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAudit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
