using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MMS
{
    public partial class PicVidPath : Form
    {
        public PicVidPath(bool backup)
        {
            InitializeComponent();
            if (backup)
                this.Text = "图片与视频目录 -- 指定要备份的源目录";
            else
                this.Text = "图片与视频目录 -- 指定还原到的目标目录";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public string PicturePath
        {
            get { return textBox1.Text.Trim(); }
            set { textBox1.Text = value; }
        }

        public string VideoPath
        {
            get { return textBox2.Text.Trim(); }
            set { textBox2.Text = value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
            folderBrowserDialog1.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
            folderBrowserDialog1.Dispose();
        }
    }
}
