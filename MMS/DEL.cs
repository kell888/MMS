using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MMS
{
    public partial class DEL : Form
    {
        public DEL()
        {
            InitializeComponent();
        }

        public string Tables
        {
            get
            {
                return tb_Tables.Text.Trim();
            }
            set
            {
                tb_Tables.Text = value;
            }
        }
        /// <summary>
        /// 一个月的第一天(如：2013-10-01 00:00:00)
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return new DateTime((int)year.Value, (int)month.Value, 1);
            }
        }
        /// <summary>
        /// 一个月的最后一天(如：2013-10-31 23:59:59)
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                DateTime end = new DateTime((int)year2.Value, (int)month2.Value, 1);
                return new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month), 23, 59, 59);
            }
        }

        public string PicPath
        {
            get
            {
                return tb_Pic.Text.Trim();
            }
        }

        public string VidPath
        {
            get
            {
                return tb_Vid.Text.Trim();
            }
        }

        public bool ShrinkDB
        {
            get
            {
                return shrink.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除吗？如果您之前没有备份过该段时间内的数据，就无法恢复了。", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
