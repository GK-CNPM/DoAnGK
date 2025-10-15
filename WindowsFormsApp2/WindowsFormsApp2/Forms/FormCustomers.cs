using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Forms.ChildCustomer;

namespace WindowsFormsApp2.Forms
{
    public partial class FormCustomers : Form
    {
        // TODO: chỉnh lại đường dẫn mdf cho đúng máy bạn
        private readonly string connStr =
    @"Data Source=(LocalDB)\MSSQLLocalDB;
      AttachDbFilename=C:\Users\win\Downloads\WindowsFormsApp2 (2) - Copy\WindowsFormsApp2 (2)\WindowsFormsApp2\WindowsFormsApp2\Database1.mdf;
      Integrated Security=True;Connect Timeout=30";


        public FormCustomers()
        {
            InitializeComponent();

        }
        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.ForeColor = Color.White;
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                    label5.ForeColor = ThemeColor.SecondaryColor;

                }
            }
        }
        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                                    {
                    string query = @"
                SELECT 
                    UserID AS [ID], 
                    FullName, 
                    UserName, 
                    Email, 
                    Address, 
                    Gender AS [GioiTinh], 
                    Role AS [ChucVu], 
                    CreatedAt,
                    SDT, 
                    CASE 
                        WHEN IsLocked = 1 THEN N'Bị khóa' 
                        ELSE N'Hoạt động' 
                    END AS [TrangThai]
                FROM [User]";



                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    MessageBox.Show($"Số dòng lấy được: {dt.Rows.Count}");

                    // Nếu bạn đã tạo cột sẵn, set AutoGenerateColumns = false
                   
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }


        private void FormCustomers_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                Name = "columnID",
                DataPropertyName = "ID"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Họ tên",
                Name = "columnName",
                DataPropertyName = "FullName"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên đăng nhập",
                Name = "columnUserName",
                DataPropertyName = "UserName"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                Name = "columnEmail",
                DataPropertyName = "Email"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Địa chỉ",
                Name = "columnAddress",
                DataPropertyName = "Address"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Giới tính",
                Name = "columnGender",
                DataPropertyName = "GioiTinh"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Chức vụ",
                Name = "columnRole",
                DataPropertyName = "ChucVu"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Ngày tạo",
                Name = "columnCreatedAt",
                DataPropertyName = "CreatedAt"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SĐT",
                Name = "columnSDT",
                DataPropertyName = "SDT"
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Trạng thái",
                Name = "columnStatus",
                DataPropertyName = "TrangThai"
            });

            LoadTheme();
            LoadUsers();
        }


        private void label5_Click(object sender, EventArgs e)
        {
            FormAddCustomer addForm = new FormAddCustomer();
            // Đăng ký sự kiện để khi form Add đóng thì reload dữ liệu
            addForm.FormClosed += (s, args) => LoadUsers();
            addForm.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormAddCustomer formAdd = new FormAddCustomer();
            formAdd.ShowDialog();  // Mở modal

            // Sau khi formAdd đóng, reload dữ liệu
            LoadUsers();
        }

        private void button_sua_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedUserId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["columnID"].Value);

            FormEditCustomer editForm = new FormEditCustomer(selectedUserId);
            editForm.FormClosed += (s, args) => LoadUsers();
            editForm.ShowDialog();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void button_khoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Lấy UserID từ dòng được chọn (cột của bạn tên là "columnID")
            int selectedUserId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["columnID"].Value);

            DialogResult result = MessageBox.Show("Bạn có chắc muốn khóa tài khoản này không?",
                                                  "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        // ✅ Cập nhật lại theo đúng tên cột trong DB (IsLocked)
                        string query = "UPDATE [User] SET IsLocked = 1 WHERE UserID = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selectedUserId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Đã khóa tài khoản thành công!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Làm mới danh sách
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi khóa tài khoản: " + ex.Message, "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_mokhoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần mở khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedUserId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["columnID"].Value);

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "UPDATE [User] SET IsLocked = 0 WHERE UserID = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedUserId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Đã mở khóa tài khoản thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở khóa tài khoản: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

