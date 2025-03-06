using Guna.UI2.WinForms;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Net.Mail;
using System.Net;

namespace BankManagementSystemProject
{
    public partial class CustomerRegistration : UserControl
    {
        private string formattedAccountNumber;
        private string gender;
        private decimal deposit;

        public string connectionstring = "Data Source=DESKTOP-NT54DQO\\SQLEXPRESS;Initial Catalog=ESOFTFINALPROJECTBANKMGTDB20241129;Integrated Security=True;TrustServerCertificate=True";
        public CustomerRegistration()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            

            if (guna2RadioButton1.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            if (!decimal.TryParse(guna2TextBox7.Text.Trim(), out deposit))
            {
                MessageBox.Show("Please Enter a valid Deposit Amount.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Account registration and store data in SQL Table named RegistrationDetails
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    string query = "INSERT INTO RegistrationDetails (FirstName, LastName, Age, DOB, Job, Address, Date, Gender, CivilStates, Nationality, PhoneNumber, Email, AccountType, Deposit) OUTPUT INSERTED.ACCNO VALUES (@FirstName, @LastName, @Age, @DOB, @Job, @Address, @Date, @Gender, @CivilStates, @Nationality, @PhoneNumber, @Email, @AccountType, @Deposit)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                       
                        command.Parameters.AddWithValue("@FirstName", guna2TextBox1.Text);
                        command.Parameters.AddWithValue("@LastName", guna2TextBox2.Text);
                        command.Parameters.AddWithValue("@Age", guna2TextBox3.Text);

                        command.Parameters.AddWithValue("@DOB", guna2TextBox4.Text);

                        command.Parameters.AddWithValue("@Job", guna2TextBox5.Text);
                        command.Parameters.AddWithValue("@Address", guna2TextBox6.Text);
                        command.Parameters.AddWithValue("@Date", guna2DateTimePicker1.Value);
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@CivilStates", guna2ComboBox2.SelectedItem);
                        command.Parameters.AddWithValue("@Nationality", guna2TextBox8.Text);
                        command.Parameters.AddWithValue("@PhoneNumber", guna2TextBox9.Text);
                        command.Parameters.AddWithValue("@Email", guna2TextBox10.Text);
                        command.Parameters.AddWithValue("@AccountType", guna2ComboBox1.SelectedItem);
                        command.Parameters.AddWithValue("@Deposit", deposit);

                        // Use ExecuteScalar to get the new account number
                        object newAccountNumberObj = command.ExecuteScalar();
                        if (newAccountNumberObj != null)
                        {
                            int newAccountNumber = Convert.ToInt32(newAccountNumberObj);
                            formattedAccountNumber = newAccountNumber.ToString().PadLeft(6, '0');

                            guna2TextBox11.Text = formattedAccountNumber;
                            guna2TextBox12.Text = guna2ComboBox1.SelectedItem.ToString();
                            guna2TextBox13.Text = deposit.ToString("F2");
                            guna2TextBox14.Text = guna2TextBox1.Text.ToString() + " " + guna2TextBox2.Text.ToString();
                            MessageBox.Show("New account registered successfully with Account Number: " + formattedAccountNumber, "Success" + " " + formattedAccountNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //Send Email
                            try
                            {
                                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                                client.EnableSsl = true; client.Timeout = 10000;
                                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                client.UseDefaultCredentials = false;
                                client.Credentials = new NetworkCredential("bankmanagementsystem3@gmail.com", "xdfi jzfd lwgp zpks");
                                MailMessage msg = new MailMessage(); msg.To.Add(guna2TextBox10.Text); // email in customer registration
                                msg.From = new MailAddress("bankmanagementsystem3@gmail.com");
                                msg.Subject = $"Registration Success - Account No: {formattedAccountNumber}";
                                msg.Body = $"Dear {guna2TextBox1.Text},\n\nThank you for registering with our bank. Your account number is {formattedAccountNumber}. Below are your registration details:\n\n" + $"Name: {guna2TextBox1.Text} {guna2TextBox2.Text}\n" + $"Account Type: {guna2ComboBox1.SelectedItem}\n" + $"Deposit Amount: {deposit:F2}\n\n" + "Best regards,\nBank Management System";
                                client.Send(msg);
                                MessageBox.Show("Confirmation email sent", "Success ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
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
            string accountNumber = guna2TextBox15.Text.Trim();

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show("Please enter a valid Account Number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    string query = "SELECT * FROM RegistrationDetails WHERE ACCNO = @ACCNO";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ACCNO", accountNumber);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve details
                                string firstName = reader["FirstName"].ToString();
                                string lastName = reader["LastName"].ToString();
                                string age = reader["Age"].ToString();
                                string dob = reader["DOB"].ToString();
                                string job = reader["Job"].ToString();
                                string address = reader["Address"].ToString();
                                string date = reader["Date"].ToString();
                                gender = reader["Gender"].ToString();
                                string civilStates = reader["CivilStates"].ToString();
                                string nationality = reader["Nationality"].ToString();
                                string phoneNumber = reader["PhoneNumber"].ToString();
                                string email = reader["Email"].ToString();
                                string accountType = reader["AccountType"].ToString();
                                string deposit = reader["Deposit"].ToString();

                                SaveFileDialog saveFileDialog = new SaveFileDialog();
                                saveFileDialog.InitialDirectory = @"C:\MyPDFs"; //Where the PDF save, methanin thama PDF eka save vena thana set karala thinne
                                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                                saveFileDialog.Title = "Save Registration Details";
                                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    string pdfPath = saveFileDialog.FileName;
                                    using (PdfWriter writer = new PdfWriter(pdfPath))
                                    {
                                        using (PdfDocument pdf = new PdfDocument(writer))
                                        {
                                            Document document = new Document(pdf);
                                            document.Add(new Paragraph("Customer Registration Details").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));
                                            document.Add(new Paragraph("Account Number: " + accountNumber));
                                            document.Add(new Paragraph("First Name: " + firstName));
                                            document.Add(new Paragraph("Last Name: " + lastName));
                                            document.Add(new Paragraph("Age: " + age));
                                            document.Add(new Paragraph("Date of Birth: " + dob));
                                            document.Add(new Paragraph("Job: " + job));
                                            document.Add(new Paragraph("Address: " + address));
                                            document.Add(new Paragraph("Date: " + date));
                                            document.Add(new Paragraph("Gender: " + gender));
                                            document.Add(new Paragraph("Civil States: " + civilStates));
                                            document.Add(new Paragraph("Nationality: " + nationality));
                                            document.Add(new Paragraph("Phone Number: " + phoneNumber));
                                            document.Add(new Paragraph("Email: " + email));
                                            document.Add(new Paragraph("Account Type: " + accountType));
                                            document.Add(new Paragraph("Deposit: " + deposit));
                                            document.Close();
                                        }
                                    }
                                    MessageBox.Show("Details exported to PDF successfully.","Succss", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                   
                                }
                            }
                            else
                            {
                                MessageBox.Show("No details found for the entered Account Number.");
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox3.Clear();
            guna2TextBox4.Clear();
            guna2TextBox5.Clear();
            guna2TextBox6.Clear();
            guna2TextBox7.Clear();
            guna2TextBox8.Clear();
            guna2TextBox9.Clear();
            guna2TextBox10.Clear();
        }
    }
}
