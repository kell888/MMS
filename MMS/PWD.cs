using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace MMS
{
    public partial class PWD : Form
    {
        public PWD()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pwd = ConfigurationManager.AppSettings["pwd"];
            if (!string.IsNullOrEmpty(pwd))
            {
                string input = textBox1.Text.Trim();
                string encrypt = new RSA(System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["privateKey"]))).Encrypt(input);
                if (encrypt != pwd)
                {
                    MessageBox.Show("授权码错误！请重试。");
                    textBox1.Focus();
                    textBox1.SelectAll();
                    return;
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }
    }
}
