using Guna.UI2.WinForms;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

namespace BankManagementSystemProject
{
    public partial class Transaction : UserControl
    {
        string connectionString = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";

        public Transaction()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string accountNumber = guna2TextBox1.Text.Trim();
            decimal amount;

            if (!decimal.TryParse(guna2TextBox4.Text.Trim(), out amount))
            {
                MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string transactionType = guna2ComboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(transactionType))
            {
                MessageBox.Show("Please select a transaction type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Fetch the first name and last name
            string firstName = string.Empty;
            string lastName = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT FirstName, LastName FROM RegistrationDetails WHERE ACCNO = @ACCNO";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ACCNO", accountNumber);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                firstName = reader["FirstName"].ToString();
                                lastName = reader["LastName"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Account not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching account details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Perform Transaction
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string checkBalanceQuery = "SELECT Deposit FROM RegistrationDetails WHERE ACCNO = @ACCNO";
                    using (SqlCommand checkBalanceCmd = new SqlCommand(checkBalanceQuery, connection))
                    {
                        checkBalanceCmd.Parameters.AddWithValue("@ACCNO", accountNumber);
                        object result = checkBalanceCmd.ExecuteScalar();

                        if (result != null)
                        {
                            decimal currentBalance = Convert.ToDecimal(result);

                            if (transactionType == "Debit" && currentBalance < amount)
                            {
                                MessageBox.Show("Account balance is too low for the transaction.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        string updateQuery = transactionType == "Credit"
                            ? "UPDATE RegistrationDetails SET Deposit = Deposit + @Amount WHERE ACCNO = @ACCNO"
                            : "UPDATE RegistrationDetails SET Deposit = Deposit - @Amount WHERE ACCNO = @ACCNO";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                        {
                            updateCmd.Parameters.AddWithValue("@Amount", amount);
                            updateCmd.Parameters.AddWithValue("@ACCNO", accountNumber);

                            int rowsAffected = updateCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Transaction completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                GenerateReceipt(accountNumber, firstName, lastName, transactionType, amount); // Generate receipt

                                string balanceQuery = "SELECT Deposit FROM RegistrationDetails WHERE ACCNO = @ACCNO";
                                using (SqlCommand balanceCmd = new SqlCommand(balanceQuery, connection))
                                {
                                    balanceCmd.Parameters.AddWithValue("@ACCNO", accountNumber);
                                    object balanceResult = balanceCmd.ExecuteScalar();
                                    decimal availableBalance = balanceResult != null ? Convert.ToDecimal(balanceResult) : 0;

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
                                        msg.To.Add(guna2TextBox3.Text); // email in customer registration
                                        msg.From = new MailAddress("bankmanagementsystem3@gmail.com");
                                        msg.Subject = $"{transactionType} - Account No: {accountNumber}";
                                        msg.Body = $"Dear {firstName} {lastName},\n\n" +
                                                   $"Your transaction of type '{transactionType}' has been completed successfully.\n" +
                                                   $"Account Number: {accountNumber}\n" +
                                                   $"Transaction Amount: {amount:F2}\n" +
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
                                MessageBox.Show("Transaction failed. Please check the account number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error performing transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox3.Clear();
            guna2TextBox6.Clear();
            guna2TextBox4.Clear();

        }



        private void GenerateReceipt(string accountNumber, string firstName, string lastName, string transactionType, decimal amount)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\MyPDFs";
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save Transaction Receipt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string pdfPath = saveFileDialog.FileName;
                using (PdfWriter writer = new PdfWriter(pdfPath))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);

                        //Title
                        document.Add(new Paragraph("Transaction Receipt").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20).SetBold());
                        //new line
                        document.Add(new LineSeparator(new SolidLine()));

                        //table initializing
                        Table table = new Table(2).UseAllAvailableWidth();
                        //Add table rows
                        table.AddCell(new Cell().Add(new Paragraph("Account Number:")).SetBold());
                        table.AddCell(new Cell().Add(new Paragraph(accountNumber)));
                        table.AddCell(new Cell().Add(new Paragraph("Account Holder Name:")).SetBold());
                        table.AddCell(new Cell().Add(new Paragraph(firstName + " " + lastName)));
                        table.AddCell(new Cell().Add(new Paragraph("Transaction Type:")).SetBold());
                        table.AddCell(new Cell().Add(new Paragraph(transactionType)));
                        table.AddCell(new Cell().Add(new Paragraph("Amount:")).SetBold());
                        table.AddCell(new Cell().Add(new Paragraph(amount.ToString("F2"))));

                        document.Add(table);

                        document.Close();
                    }
                }
                MessageBox.Show("Receipt saved to PDF successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string accountNumber = guna2TextBox1.Text.Trim();
            if (accountNumber == "")
            {
                guna2TextBox2.Text = "";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT FirstName, LastName, Email FROM RegistrationDetails WHERE ACCNO = @ACCNO";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ACCNO", accountNumber);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                guna2TextBox2.Text = reader["FirstName"].ToString();
                                guna2TextBox6.Text = reader["LastName"].ToString();
                                guna2TextBox3.Text = reader["Email"].ToString();
                            }
                            else
                            {
                                guna2TextBox2.Text = "Account not found";
                                guna2TextBox6.Text = "N/A";
                                guna2TextBox3.Text = "N/A";
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cnn = new SqlCommand("SELECT ACCNO, FirstName, LastName, Deposit, Email FROM RegistrationDetails WHERE ACCNO LIKE @ACCNO", con))

                    {
                        cnn.Parameters.AddWithValue("@ACCNO", guna2TextBox5.Text);

                        SqlDataAdapter adapter1 = new SqlDataAdapter(cnn);

                        DataTable dt = new DataTable();
                        adapter1.Fill(dt);

                        guna2DataGridView1.DataSource = dt;

                        guna2DataGridView1.Columns["Deposit"].HeaderText = "Balance";
                        guna2DataGridView1.Columns["ACCNO"].HeaderText = "Account Number";
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
