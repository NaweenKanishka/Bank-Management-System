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
    public partial class DashBoard : UserControl
    {
        public string connectionstring = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";
        public DashBoard()
        {
            InitializeComponent();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            int rowCount = GetRowCount();
            guna2CircleProgressBar1.Value = rowCount;

            int rowCountloan = GetRowCountloan();
            guna2CircleProgressBar2.Value = rowCountloan;
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private int GetRowCount()
        {
            int rowCount = 0;
            string query = "SELECT COUNT(*) FROM RegistrationDetails";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    { 
                        rowCount = (int)cmd.ExecuteScalar();
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:" +
                    $"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } return rowCount; 
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }


        private int GetRowCountloan()
        {
            int rowCountloan = 0;
            string query = "SELECT COUNT(*) FROM LoanDetails";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        rowCountloan = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:" +
                    $"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return rowCountloan;
        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void guna2CircleProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
