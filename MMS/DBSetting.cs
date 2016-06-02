using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MMS
{
    public partial class DBSetting : Form
    {
        public DBSetting()
        {
            InitializeComponent();
        }

        public string ConnectionString
        {
            get { return textBox1.Text.Trim(); }
            set { textBox1.Text = value; }
        }

        public string BackupConnectionString
        {
            get { return textBox2.Text.Trim(); }
            set { textBox2.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
