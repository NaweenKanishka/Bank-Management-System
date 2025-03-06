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
using System.Net.Mail;
using System.Net;
using iText.Kernel.Pdf;
using System.IO;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace BankManagementSystemProject
{
    public partial class Loan : UserControl
    {
        public string connectionstring = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";

        public Loan()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                double principal = Convert.ToDouble(guna2TextBox1.Text);
                double annualInterestRate = Convert.ToDouble(guna2TextBox2.Text);
                double period = Convert.ToDouble(guna2TextBox3.Text);

                double r = (annualInterestRate / 100) / 12;
                double n = period * 12;
                double totalval = principal + (principal * (annualInterestRate / 100));
                double monthlypayment = totalval / (12 * period);
                double interest = period * (principal * (annualInterestRate / 100));
                double totalpayment = principal + interest;

                guna2TextBox4.Text = "Total Interest: " + interest.ToString("F2");
                guna2TextBox5.Text = "Total Monthly Payment: " + monthlypayment.ToString("F2");
                guna2TextBox6.Text = "Total Payment: " + totalpayment.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            LoanApplication loanApplication = new LoanApplication();
            loanApplication.Show();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            AllCustomersWithLoan ACWL = new AllCustomersWithLoan();
            ACWL.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string NICNumber = guna2TextBox9.Text.Trim();
            double newLoanAmount = Convert.ToDouble(guna2TextBox12.Text);

            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    con.Open();
                    string query = "UPDATE LoanDetails SET LoanAmount = LoanAmount - @newLoanAmount WHERE NIC = @NIC";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@NIC", NICNumber);
                        cmd.Parameters.AddWithValue("@newLoanAmount", newLoanAmount);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            string balanceQuery = "SELECT LoanAmount FROM LoanDetails WHERE NIC = @NIC";
                            using (SqlCommand balanceCmd = new SqlCommand(balanceQuery, con))
                            {
                                balanceCmd.Parameters.AddWithValue("@NIC", NICNumber);
                                object balanceResult = balanceCmd.ExecuteScalar();
                                double availableBalance = balanceResult != null ? Convert.ToDouble(balanceResult) : 0;

                                MessageBox.Show("Successfully paid.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Generate receipt
                                GenerateReceipt(NICNumber, newLoanAmount, availableBalance); // Pass available balance to the method

                                // Send Email
                                try
                                {
                                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                                    client.EnableSsl = true;
                                    client.Timeout = 10000;
                                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    client.UseDefaultCredentials = false;
                                    client.Credentials = new NetworkCredential("bankmanagementsystem3@gmail.com", "xdfi jzfd lwgp zpks");

                                    MailMessage msg = new MailMessage();
                                    msg.To.Add(guna2TextBox10.Text); // email in customer registration
                                    msg.From = new MailAddress("bankmanagementsystem3@gmail.com");
                                    msg.Subject = $"NIC No: {NICNumber}";
                                    msg.Body = $"Dear {guna2TextBox8.Text},\n\n" +
                                               $"Your Payment has been completed successfully.\n" +
                                               $"Loan paid Amount: {newLoanAmount:F2}\n" +
                                               $"Available Balance: {availableBalance:F2}\n\n" +
                                               "Best regards,\nBank Management System";

                                    client.Send(msg);
                                    MessageBox.Show("Transaction email sent", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Failed to send email: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No customer found with the entered NIC.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            FetchCustomerDetails(guna2TextBox9.Text.Trim());
        }

        private void guna2TextBox9_TextChanged(object sender, EventArgs e)
        {
            // If needed, handle text changed event here
        }

        private void FetchCustomerDetails(string NIC)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string query = "SELECT Name, Mobile, Email, LoanAmount, MonthlyPremium FROM LoanDetails WHERE NIC LIKE @NIC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NIC", NIC);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            guna2TextBox8.Text = reader["Name"].ToString();
                            guna2TextBox7.Text = reader["Mobile"].ToString();
                            guna2TextBox10.Text = reader["Email"].ToString();
                            guna2TextBox11.Text = reader["LoanAmount"].ToString();
                            guna2TextBox12.Text = reader["MonthlyPremium"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No customer found with the entered NIC.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Loan_Load(object sender, EventArgs e)
        {
            // Any initialization code can go here
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            FetchCustomerDetails(guna2TextBox9.Text.Trim());
        }

        private void GenerateReceipt(string NIC, double newLoanAmount, double availableBalance)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save Receipt As";
                saveFileDialog.FileName = $"Receipt_{NIC}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string receiptFilePath = saveFileDialog.FileName;
                    using (FileStream stream = new FileStream(receiptFilePath, FileMode.Create))
                    {
                        PdfWriter writer = new PdfWriter(stream);
                        PdfDocument pdf = new PdfDocument(writer);
                        Document document = new Document(pdf);

                        document.Add(new Paragraph("Bank Management System"));
                        document.Add(new Paragraph("----------------------------"));
                        document.Add(new Paragraph($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}"));
                        document.Add(new Paragraph($"NIC: {NIC}"));
                        document.Add(new Paragraph($"Transaction Amount: {newLoanAmount:F2}"));
                        document.Add(new Paragraph($"Available Balance: {availableBalance:F2}"));
                        document.Add(new Paragraph("----------------------------"));
                        document.Add(new Paragraph("Thank you for your business!"));

                        document.Close();
                    }

                    MessageBox.Show($"Receipt generated and saved to {receiptFilePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }
}
