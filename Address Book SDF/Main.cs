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

namespace Address_Book_SDF
{
    public partial class Main : Form
    {
        // This codes are for instantiate global variables for sql
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        Connection dbconn = new Connection();
        SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
        SqlDataReader sdr;
        public Main()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            // connection
            con = new SqlConnection(dbconn.MyConnectionDB());
            // This method loads all contacts you have
            LoadContacts();
        }

        public void LoadContacts()
        {
            //loads all contacts you have
            try
            {
                con.Open();
                dataGridView1.Rows.Clear();
                int i = 0;
                cmd = new SqlCommand("SELECT * FROM TBL_USERPROFILE", con);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i,
                        sdr["ID"].ToString(),
                        sdr["FULLNAME"].ToString(),
                        sdr["PHONENUMBER"].ToString(),
                        sdr["EMAIL"].ToString(),
                        sdr["ADDRESS"].ToString());
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Opens login form and if you close it, it will automatically closes this form too
            this.Hide();
            LoginPage lp = new LoginPage();
            lp.Closed += (s, args) => this.Close();
            lp.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // search part
            try
            {
                con.Open();
                int i = 0;
                dataGridView1.Rows.Clear();
                cmd = new SqlCommand("SELECT * FROM TBL_USERPROFILE WHERE FULLNAME LIKE '%" + textBox1.Text + "%' ", con);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(i,
                        sdr["ID"].ToString(),
                        sdr["FULLNAME"].ToString(),
                        sdr["PHONENUMBER"].ToString(),
                        sdr["EMAIL"].ToString(),
                        sdr["ADDRESS"].ToString());
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // if you click on the show icon it will show all details in groupbox
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Show")
            {
                con.Open();
                cmd = new SqlCommand("SELECT * FROM TBL_USERPROFILE WHERE ID LIKE '" + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()) + "'", con);
                sdr = cmd.ExecuteReader();
                sdr.Read();
                if (sdr.HasRows)
                {
                    lbl_id.Text = sdr["ID"].ToString();
                    txt_fullname.Text = sdr["FULLNAME"].ToString();
                    txt_phoneno.Text = sdr["PHONENUMBER"].ToString();
                    txt_email.Text = sdr["EMAIL"].ToString();
                    txt_address.Text = sdr["ADDRESS"].ToString();
                    rtxt_notes.Text = sdr["NOTES"].ToString();
                }
                con.Close();
            }
        }
    }
}
