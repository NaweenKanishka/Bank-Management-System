using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankManagementSystemProject
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            dashBoard1.Visible = true;
            dashBoard1.BringToFront();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            transaction1.Visible = true;
            transaction1.BringToFront();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            loan1.Visible = true;
            loan1.BringToFront();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            customerRegistration1.Visible = true;
            customerRegistration1.BringToFront();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            accounts1.Visible = true;
            accounts1.BringToFront();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            accDelUpda1.Visible = true;
            accDelUpda1.BringToFront();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            accUpdate1.Visible = true;
            accUpdate1.BringToFront();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
