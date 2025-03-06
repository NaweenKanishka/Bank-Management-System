using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace BankManagementSystemProject
{
    public partial class AccUpdate : UserControl
    {
        private string connectionstring = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";

        public AccUpdate()
        {
            InitializeComponent();
            
        }

       

        private void AccUpdate_Load(object sender, EventArgs e)
        {
            // Initialization code (if any)
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

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    con.Open();
                    using (SqlCommand cnn = new SqlCommand("SELECT * FROM RegistrationDetails WHERE FirstName LIKE @FirstName", con))
                    {
                        cnn.Parameters.AddWithValue("@FirstName", "%" + guna2TextBox2.Text.Trim() + "%");
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

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    con.Open();
                    using (SqlCommand cnn = new SqlCommand("SELECT * FROM RegistrationDetails WHERE Email LIKE @Email", con))
                    {
                        cnn.Parameters.AddWithValue("@Email", "%" + guna2TextBox3.Text.Trim() + "%");
                        SqlDataAdapter adapter = new SqlDataAdapter(cnn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        guna2DataGridView1.DataSource = dt;

                        //change header name
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            string gender = "";
            if (guna2RadioButton1.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
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
                    List<string> fieldsToUpdate = new List<string>();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    if (!string.IsNullOrEmpty(guna2TextBox12.Text))
                    {
                        fieldsToUpdate.Add("FirstName = @FirstName");
                        command.Parameters.AddWithValue("@FirstName", guna2TextBox12.Text);
                    }
                    if (!string.IsNullOrEmpty(guna2TextBox11.Text))
                    {
                        fieldsToUpdate.Add("LastName = @LastName");
                        command.Parameters.AddWithValue("@LastName", guna2TextBox11.Text);
                    }
                    if (!string.IsNullOrEmpty(guna2TextBox7.Text))
                    {
                        fieldsToUpdate.Add("Age = @Age");
                        command.Parameters.AddWithValue("@Age", guna2TextBox7.Text);
                    }
                    if (!string.IsNullOrEmpty(guna2TextBox13.Text))
                    {
                        fieldsToUpdate.Add("DOB = @DOB");
                        command.Parameters.AddWithValue("@DOB", guna2TextBox13.Text);
                    }
                    if (!string.IsNullOrEmpty(guna2TextBox5.Text))
                    {
                        fieldsToUpdate.Add("Job = @Job");
                        command.Parameters.AddWithValue("@Job", guna2TextBox5.Text);
                    }
                    if (!string.IsNullOrEmpty(guna2TextBox6.Text))
                    {
                        fieldsToUpdate.Add("Address = @Address");
                        command.Parameters.AddWithValue("@Address", guna2TextBox6.Text);
                    }
                    if (guna2DateTimePicker1.Value != DateTime.MinValue)
                    {
                        fieldsToUpdate.Add("Date = @Date");
                        command.Parameters.AddWithValue("@Date", guna2DateTimePicker1.Value);
                    }
                    if (!string.IsNullOrEmpty(gender))
                    {
                        fieldsToUpdate.Add("Gender = @Gender");
                        command.Parameters.AddWithValue("@Gender", gender);
                    }
                    if (guna2ComboBox2.SelectedItem != null)
                    {
                        fieldsToUpdate.Add("CivilStates = @CivilStates");
                        command.Parameters.AddWithValue("@CivilStates", guna2ComboBox2.SelectedItem);
                    }
                    if (!string.IsNullOrEmpty(guna2TextBox8.Text))
                    {
                        fieldsToUpdate.Add("Nationality = @Nationality");
                        command.Parameters.AddWithValue("@Nationality", guna2TextBox8.Text);
                    }
                    if (!string.IsNullOrEmpty(guna2TextBox4.Text))
                    {
                        fieldsToUpdate.Add("PhoneNumber = @PhoneNumber");
                        command.Parameters.AddWithValue("@PhoneNumber", guna2TextBox4.Text);
                    }
                    if (!string.IsNullOrEmpty(guna2TextBox10.Text))
                    {
                        fieldsToUpdate.Add("Email = @Email");
                        command.Parameters.AddWithValue("@Email", guna2TextBox10.Text);
                    }
                    if (guna2ComboBox1.SelectedItem != null)
                    {
                        fieldsToUpdate.Add("AccountType = @AccountType");
                        command.Parameters.AddWithValue("@AccountType", guna2ComboBox1.SelectedItem);
                    }

                    if (fieldsToUpdate.Count > 0)
                    {
                        string query = $"UPDATE RegistrationDetails SET {string.Join(", ", fieldsToUpdate)} WHERE ACCNO = @ACCNO";
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@ACCNO", accountNumber);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No account found with the provided Account Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No fields were changed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
