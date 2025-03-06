using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankManagementSystemProject
{
    public partial class Accounts : UserControl
    {
        //add connection string to form
        public string connectionstring = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";
        public Accounts()
        {
            InitializeComponent();
            SetupDataGridView();

        }

        private void SetupDataGridView()
        {
            
        }




        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("SELECT ACCNO, FirstName, LastName, Deposit FROM RegistrationDetails WHERE ACCNO = @ACCNO", conn))
                    using (SqlCommand cmdd = new SqlCommand("SELECT ACCNO, FirstName, LastName, Deposit FROM RegistrationDetails WHERE PhoneNumber = @PhoneNumber", conn))
                    using (SqlCommand cmddd = new SqlCommand("SELECT ACCNO, FirstName, LastName, Deposit FROM RegistrationDetails WHERE Email = @Email", conn))
                    using (SqlCommand cmdddd = new SqlCommand("SELECT ACCNO, FirstName, LastName, Deposit FROM RegistrationDetails WHERE LastName = @LastName", conn))
                    {
                        cmd.Parameters.AddWithValue("@ACCNO", guna2TextBox1.Text);
                        cmdd.Parameters.AddWithValue("@PhoneNumber", guna2TextBox2.Text);
                        cmddd.Parameters.AddWithValue("@Email", guna2TextBox3.Text);
                        cmdddd.Parameters.AddWithValue("@LastName", guna2TextBox4.Text);

                        if (!string.IsNullOrEmpty(guna2TextBox1.Text))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            guna2DataGridView2.DataSource = dt;

                            guna2DataGridView2.Columns["Deposit"].HeaderText = "Balance";
                            guna2DataGridView2.Columns["ACCNO"].HeaderText = "Account Number";
                            


                        }
                        

                        if (!string.IsNullOrEmpty(guna2TextBox2.Text))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(cmdd);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            guna2DataGridView2.DataSource = dt;

                            guna2DataGridView2.Columns["Deposit"].HeaderText = "Balance";
                            guna2DataGridView2.Columns["ACCNO"].HeaderText = "Account Number";



                        }
                        

                        if (!string.IsNullOrEmpty(guna2TextBox3.Text))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(cmddd);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            guna2DataGridView2.DataSource = dt;

                            guna2DataGridView2.Columns["Deposit"].HeaderText = "Balance";
                            guna2DataGridView2.Columns["ACCNO"].HeaderText = "Account Number";



                        }
                        

                        if (!string.IsNullOrEmpty(guna2TextBox4.Text))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(cmdddd);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            guna2DataGridView2.DataSource = dt;

                            guna2DataGridView2.Columns["Deposit"].HeaderText = "Balance";
                            guna2DataGridView2.Columns["ACCNO"].HeaderText = "Account Number";

                            


                        }

                     
                    }

                 


                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {

                    conn.Open();

                    string query;
                   
                    if (string.IsNullOrEmpty(guna2TextBox1.Text))
                    {
                        query = "SELECT ACCNO, FirstName, LastName, Deposit FROM RegistrationDetails";
                       
                    }
                    else
                    {
                        query = "SELECT ACCNO, FirstName, LastName, Deposit FROM RegistrationDetails WHERE ACCNO LIKE @ACCNO";
                       
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(guna2TextBox1.Text))
                        {
                            cmd.Parameters.AddWithValue("@ACCNO", "%" + guna2TextBox1.Text.Trim() + "%");
                        }
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        guna2DataGridView2.DataSource = dt;

                        guna2DataGridView2.Columns["Deposit"].HeaderText = "Balance";
                        guna2DataGridView2.Columns["ACCNO"].HeaderText = "Account Number";
                    }

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {

                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT ACCNO, FirstName, LastName, Deposit FROM RegistrationDetails WHERE ACCNO LIKE @ACCNO", conn))
                    {
                        cmd.Parameters.AddWithValue("@ACCNO", guna2TextBox1.Text);

                        if (!string.IsNullOrEmpty(guna2TextBox1.Text))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            guna2DataGridView2.DataSource = dt;

                            guna2DataGridView2.Columns["Deposit"].HeaderText = "Balance";
                            guna2DataGridView2.Columns["ACCNO"].HeaderText = "Account Number";



                        }
                        else
                        {
                            MessageBox.Show("Enter Vaild Account Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
