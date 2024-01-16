using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Address_Book_SDF
{
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if you have typed true values admin page will be started otherwise you will see warning messagebox
            if (user_tb.Text == "Admin" && pswd_tb.Text == "admin123")
            {
                this.Hide();
                AdminPage ap = new AdminPage();
                ap.Closed += (s, args) => this.Close();
                ap.Show();
            }
            else
            {
                MessageBox.Show("Wrong Username or Password!","Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pswd_tb_TextChanged(object sender, EventArgs e)
        {
            //makes password unvisible
            pswd_tb.PasswordChar = '*';
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if you check the box password will be visible
            pswd_tb.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
    }
}
