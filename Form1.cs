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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(guna2TextBox1.Text == "Admin" &&  guna2TextBox2.Text == "Admin")
            {
                MainForm main = new MainForm();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username or Password is wrong. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            { 
                guna2TextBox2.PasswordChar ='\0';
            }
            else
            {
                guna2TextBox2.PasswordChar = '*';

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
