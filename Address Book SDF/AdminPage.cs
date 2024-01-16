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
    public partial class AdminPage : Form
    {
        //We use this codes for instantiate global variables for sql
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        Connection dbconn = new Connection();
        SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
        SqlDataReader sdr;

        public AdminPage()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            //Createing a connection entry to database
            con = new SqlConnection(dbconn.MyConnectionDB());
            //Loads contacts from database
            LoadContacts();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            //to add new contact to app
            if(txt_fullname.Text == string.Empty)
            {
                //if we dont type name, error occurs
                MessageBox.Show("Enter contact details!", "EROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                con.Open();
                cmd = new SqlCommand("INSERT INTO TBL_USERPROFILE(FULLNAME, PHONENUMBER, EMAIL, ADDRESS, NOTES) VALUES (@FULLNAME, @PHONENUMBER, @EMAIL, @ADDRESS, @NOTES)", con);
                cmd.Parameters.AddWithValue("@FULLNAME",txt_fullname.Text);
                cmd.Parameters.AddWithValue("@PHONENUMBER", txt_phoneno.Text);
                cmd.Parameters.AddWithValue("@EMAIL", txt_email.Text);
                cmd.Parameters.AddWithValue("@ADDRESS", txt_address.Text);
                cmd.Parameters.AddWithValue("@NOTES", rtxt_notes.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Added successfully!", "ADDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // loads the lastest version of your contacs
                LoadContacts();
                // clears details from groupbox
                ClearText();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        public void ClearText()
        {
            //clears all text from text boxes
            txt_fullname.Clear();
            txt_phoneno.Clear();
            txt_email.Clear();
            txt_address.Clear();
            rtxt_notes.Clear();
        }

        public void LoadContacts()
        {
            //loads all contacts we have
            try
            {
                con.Open();
                dataGridView1.Rows.Clear();
                int i = 0;
                cmd = new SqlCommand("SELECT * FROM TBL_USERPROFILE",con);
                sdr= cmd.ExecuteReader();
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
            //edit and delete functions
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            //if you have clicked on edit icon, you will see all details in panel and then you can chage it from there
            if(colName == "Edit")
            {
                btn_add.Enabled = false;

                con.Open();
                cmd = new SqlCommand("SELECT * FROM TBL_USERPROFILE WHERE ID LIKE '" + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()) + "'", con);
                sdr = cmd.ExecuteReader();
                sdr.Read();
                if(sdr.HasRows)
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
            //if you have clicked on delete icon, the contact will be deleted
            else if (colName == "Delete")
            {
                //this messagebox helps you to run away from missclick and delete wrong contact
                if(MessageBox.Show("Are you sure to DELETE this contact?","DELETE",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    con.Open();
                    cmd = new SqlCommand("DELETE FROM TBL_USERPROFILE WHERE ID LIKE '" + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()) + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Deleted successfully!", "DELETED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadContacts();
                    ClearText();
                }
                else
                {
                    //do nothing
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {        
            //search part
            try
            {
                con.Open();
                int i = 0;
                dataGridView1.Rows.Clear();
                cmd = new SqlCommand("SELECT * FROM TBL_USERPROFILE WHERE FULLNAME LIKE '%"+ textBox1.Text +"%' ",con);
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            //if you have clicked on clear button this clears all details from groupbox
            ClearText();
            btn_add.Enabled = true;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            //this updates all changes you have
            try
            {
                con.Open();
                cmd = new SqlCommand("UPDATE TBL_USERPROFILE SET FULLNAME=@FULLNAME, PHONENUMBER=@PHONENUMBER, EMAIL=@EMAIL, ADDRESS=@ADDRESS, NOTES=@NOTES WHERE ID LIKE '" + int.Parse(lbl_id.Text) + "'", con);
                cmd.Parameters.AddWithValue("@FULLNAME", txt_fullname.Text);
                cmd.Parameters.AddWithValue("@PHONENUMBER", txt_phoneno.Text);
                cmd.Parameters.AddWithValue("@EMAIL", txt_email.Text);
                cmd.Parameters.AddWithValue("@ADDRESS", txt_address.Text);
                cmd.Parameters.AddWithValue("@NOTES", rtxt_notes.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Saved successfully!", "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadContacts();
                btn_add.Enabled=true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
    }
}
