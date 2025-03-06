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
using Guna.UI2.WinForms;

namespace BankManagementSystemProject
{
    public partial class LoanApplication : Form
    {
        public string connectionstring = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";
        public LoanApplication()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string NIC = guna2TextBox7.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();

                    // Check if NIC already exists
                    string checkQuery = "SELECT COUNT(*) FROM LoanDetails WHERE NIC = @NIC";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@NIC", NIC);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("NIC already exists. Please try with different NIC number", "warning", MessageBoxButtons.OK ,MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // Insert new record
                            string query = "INSERT INTO LoanDetails (Name, NIC, DOB, Email, Mobile, HMobile, Address, LoanAmount, LoanPeriod, Date, InterestRate, MonthlyPremium) VALUES (@Name, @NIC, @DOB, @Email, @Mobile, @HMobile, @Address, @LoanAmount, @LoanPeriod, @Date, @InterestRate, @MonthlyPremium)";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@Name", guna2TextBox1.Text);
                                cmd.Parameters.AddWithValue("@NIC", guna2TextBox7.Text);
                                cmd.Parameters.AddWithValue("@DOB", guna2DateTimePicker1.Value);
                                cmd.Parameters.AddWithValue("@Email", guna2TextBox2.Text);
                                cmd.Parameters.AddWithValue("@Mobile", guna2TextBox3.Text);
                                cmd.Parameters.AddWithValue("@HMobile", guna2TextBox4.Text);
                                cmd.Parameters.AddWithValue("@Address", guna2TextBox5.Text);
                                cmd.Parameters.AddWithValue("@LoanAmount", guna2TextBox8.Text);
                                cmd.Parameters.AddWithValue("@LoanPeriod", guna2TextBox10.Text);
                                cmd.Parameters.AddWithValue("@Date", guna2DateTimePicker2.Value);
                                cmd.Parameters.AddWithValue("@InterestRate", guna2TextBox13.Text);
                                cmd.Parameters.AddWithValue("@MonthlyPremium", guna2TextBox6.Text);

                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Saved","Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
