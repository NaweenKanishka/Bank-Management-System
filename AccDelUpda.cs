using Guna.UI2.WinForms;
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

namespace BankManagementSystemProject
{
    public partial class AccDelUpda : UserControl
    {
        private string connectionstring = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";

        public AccDelUpda()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {


            string accountNumberText = guna2TextBox1.Text.Trim(); 
            if (string.IsNullOrEmpty(accountNumberText))
            { 
                MessageBox.Show("Please enter a valid Account Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                return; 
            }
            if (!int.TryParse(accountNumberText, out int accountNumber)) 
            { 
                MessageBox.Show("Account number must be an integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                return; 
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();

                    string query = "DELETE FROM RegistrationDetails WHERE ACCNO = @ACCNO";

                    // Ask for confirmation before deleting
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this account?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ACCNO", accountNumber);
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Account deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("No account found with the provided Account Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Account deletion canceled.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    con.Open();
                    using (SqlCommand cnn = new SqlCommand("SELECT * FROM RegistrationDetails WHERE ACCNO LIKE @ACCNO", con))
                    {
                        cnn.Parameters.AddWithValue("@ACCNO", "%" + guna2TextBox1.Text.Trim() + "%");
                        SqlDataAdapter adapter = new SqlDataAdapter(cnn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;

                        if (guna2DataGridView1.Columns["ACCNO"] != null)
                        {
                            guna2DataGridView1.Columns["ACCNO"].HeaderText = "Account Number";
                        }
                        if (guna2DataGridView1.Columns["Deposit"] != null)
                        { 
                            guna2DataGridView1.Columns["Deposit"].HeaderText = "Balance";
                        }
                        if (guna2DataGridView1.Columns["DOB"] != null)
                        {
                            guna2DataGridView1.Columns["DOB"].HeaderText = "Date of Birth";
                        }
                        if (guna2DataGridView1.Columns["JOB"] != null)
                        {
                            guna2DataGridView1.Columns["JOB"].HeaderText = "Post";
                        }
                        if (guna2DataGridView1.Columns["Date"] != null)
                        {
                            guna2DataGridView1.Columns["Date"].HeaderText = "Account open date";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AccDelUpda_Load(object sender, EventArgs e)
        {

        }
    }
}
