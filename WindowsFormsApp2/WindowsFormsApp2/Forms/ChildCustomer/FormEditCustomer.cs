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

namespace WindowsFormsApp2.Forms.ChildCustomer
{

    public partial class FormEditCustomer : Form
    {
        private int userId;
        public FormEditCustomer()
        {
            InitializeComponent();
        }
        public FormEditCustomer(int userId) : this()
        {
            this.userId = userId;
            LoadUserData(); // Tự động tải dữ liệu khách hàng lên form
        }
        private readonly string connStr =
            $@"Data Source=(LocalDB)\MSSQLLocalDB;
               AttachDbFilename={Application.StartupPath}\Database1.mdf;
               Integrated Security=True;Connect Timeout=30";
        private void LoadUserData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT FullName, UserName, Email, Address, Gender, Role, SDT 
                             FROM [User] WHERE UserID = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox_hoten.Text = reader["FullName"].ToString();
                            textBox_tendn.Text = reader["UserName"].ToString();
                            textBox_email.Text = reader["Email"].ToString();
                            textBox_diachi.Text = reader["Address"].ToString();
                            comboBox_chucvi.Text = reader["Role"].ToString();
                            textBox_sdt.Text = reader["SDT"].ToString();

                            // ✅ Giới tính
                            string gender = reader["Gender"].ToString();
                            if (gender == "Nam")
                                radioButton_nam.Checked = true;
                            else if (gender == "Nữ")
                                radioButton_nu.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin khách hàng: " + ex.Message);
            }
        }


        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormEditCustomer_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_hoten.Text) || string.IsNullOrWhiteSpace(textBox_tendn.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string gender = radioButton_nam.Checked ? "Nam" :
                                    radioButton_nu.Checked ? "Nữ" : "";

                    string query = @"UPDATE [User]
                             SET FullName = @FullName,
                                 UserName = @UserName,
                                 Email = @Email,
                                 Address = @Address,
                                 Gender = @Gender,
                                 Role = @Role,
                                 SDT = @SDT
                             WHERE UserID = @UserID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FullName", textBox_hoten.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserName", textBox_tendn.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", textBox_email.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", textBox_diachi.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Role", comboBox_chucvi.Text.Trim());
                    cmd.Parameters.AddWithValue("@SDT", textBox_sdt.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close(); // Đóng form sau khi lưu
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khách hàng để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
