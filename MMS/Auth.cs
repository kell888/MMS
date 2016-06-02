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
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        public string Pwd
        {
            get { return textBox1.Text.Trim(); }
            set { textBox1.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}