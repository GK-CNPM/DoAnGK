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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace WindowsFormsApp2.Forms.ChildCustomer
{
    public partial class FormAddCustomer : Form
    {
        private readonly string connStr =
        @"Data Source=(LocalDB)\MSSQLLocalDB;
      AttachDbFilename=C:\Users\win\Downloads\WindowsFormsApp2 (2) - Copy\WindowsFormsApp2 (2)\WindowsFormsApp2\WindowsFormsApp2\Database1.mdf;
      Integrated Security=True;Connect Timeout=30";
        public FormAddCustomer()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void FormAddCustomer_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các control
            string hoTen = textBox_hoten.Text.Trim();
            string email = textBox_email.Text.Trim();
            string diaChi = textBox_diachi.Text.Trim();
            string tenDangNhap = textBox_tendn.Text.Trim();
            string sdt = textBox_sdt.Text.Trim();
            string matKhau = textBox_mk.Text.Trim();

            string gioiTinh = "";
            if (radioButton_nam.Checked)
                gioiTinh = "Nam";
            else if (radioButton_nu.Checked)
                gioiTinh = "Nữ";
            else
            {
                MessageBox.Show("Vui lòng chọn giới tính!");
                return;
            }

            string chucVu = comboBox_chucvi.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(chucVu))
            {
                MessageBox.Show("Vui lòng chọn chức vụ!");
                return;
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(tenDangNhap) ||
                string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(diaChi))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                INSERT INTO [User] 
                (FullName, Email, Address, UserName, SDT, PassWord, Gender, Role, IsLocked, CreatedAt) 
                VALUES 
                (@HoTen, @Email, @DiaChi, @tenDangNhap, @SDT, @matKhau, @Gender, @Role, 0, @CreatedAt)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = hoTen;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
                        cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = diaChi;
                        cmd.Parameters.Add("@TenDangNhap", SqlDbType.NVarChar).Value = tenDangNhap;
                        cmd.Parameters.Add("@SDT", SqlDbType.NVarChar).Value = sdt;
                        cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar).Value = matKhau;
                        cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = gioiTinh;
                        cmd.Parameters.Add("@Role", SqlDbType.NVarChar).Value = chucVu;
                        cmd.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm khách hàng thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
