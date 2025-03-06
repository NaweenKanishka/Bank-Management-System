using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BankManagementSystemProject
{
    public partial class AllCustomersWithLoan : Form
    {
        public string connectionstring = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";
        public AllCustomersWithLoan()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM LoanDetails WHERE NIC LIKE @NIC OR Name LIKE @Name OR Mobile LIKE @Mobile OR Email LIKE @Email", conn))
                    {
                        cmd.Parameters.AddWithValue("@NIC", "%" + guna2TextBox1.Text.Trim() + "%");
                        cmd.Parameters.AddWithValue("@Name", "%" + guna2TextBox1.Text.Trim() + "%");
                        cmd.Parameters.AddWithValue("@Mobile", "%" + guna2TextBox1.Text.Trim() + "%");
                        cmd.Parameters.AddWithValue("@Email", "%" + guna2TextBox1.Text.Trim() + "%");
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if(dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No data available for the given input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            guna2DataGridView1.DataSource = dt;


                            if (guna2DataGridView1.Columns["DOB"] != null)
                            {
                                guna2DataGridView1.Columns["DOB"].HeaderText = "Date of Birth";
                            }
                        }

                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
