using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Gioithieu : Form
    {
        public Gioithieu()
        {
            InitializeComponent();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Tính toán cỡ font theo chiều rộng của Form
            float newSize = this.Width / 50f;
            if (newSize < 8) newSize = 8; // không cho nhỏ hơn 8

            // Duyệt toàn bộ control trong Form
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label || ctrl is Button)
                {
                    ctrl.Font = new Font(ctrl.Font.FontFamily, newSize);
                }
            }
        }
    }
}
