using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Xml;
using System.Transactions;
using System.Configuration;
using MergeQueryUtil;

namespace MMS
{
    public partial class MainForm : Form
    {
        private bool sure;
        private string connStr;
        private string backupConnStr;
        private string Pwd;
        private string tables;
        private string backupPath;
        SQLDMO.BackupSink_PercentCompleteEventHandler backupProgress;
        SQLDMO.RestoreSink_PercentCompleteEventHandler restoreProgress;
        SQLDMO.Backup oBackup;
        SQLDMO.Restore oRestore;
        bool restoring;
        bool backuping;

        public MainForm()
        {
            InitializeComponent();
        }

        private void UpdateConnectionString(string connString)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.ExecutablePath + ".config");
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("//connectionStrings");
            xElem = (XmlElement)xNode.SelectSingleNode("//add[@name='connString']");
            xElem.SetAttribute("connectionString", connString);
            xDoc.Save(Application.ExecutablePath + ".config");
        }

        private void UpdateConnectionStringBackup(string connString)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.ExecutablePath + ".config");
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("//connectionStrings");
            xElem = (XmlElement)xNode.SelectSingleNode("//add[@name='connStringBackup']");
            xElem.SetAttribute("connectionString", connString);
            xDoc.Save(Application.ExecutablePath + ".config");
        }

        private void UpdatePwd(string pwd)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.ExecutablePath + ".config");
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("//appSettings");
            xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='pwd']");
            xElem.SetAttribute("value", pwd);
            xDoc.Save(Application.ExecutablePath + ".config");
        }

        private void UpdateTables(string tables)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.ExecutablePath + ".config");
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("//appSettings");
            xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='tables']");
            xElem.SetAttribute("value", tables);
            xDoc.Save(Application.ExecutablePath + ".config");
        }

        private void UpdatePath(string path)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.ExecutablePath + ".config");
            XmlNode xNode;
            XmlElement xElem;
            xNode = xDoc.SelectSingleNode("//appSettings");
            xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='path']");
            xElem.SetAttribute("value", path);
            xDoc.Save(Application.ExecutablePath + ".config");
        }

        private void ChangeDB(string connString)
        {
            ConnectDB();
        }

        private void ConnectDB()
        {
            bool opened = false;
            SqlHelper SqlHelper = new SqlHelper("connString");
            if (SqlHelper.CanConnect)
            {
                if (SqlHelper.Conn.State == ConnectionState.Open)
                {
                    opened = true;
                }
                else
                {
                    SqlHelper.Conn.Open();
                    opened = SqlHelper.IsOpened;
                }
            }
            if (opened)
            {
                删除ToolStripMenuItem.Enabled = button1.Enabled = button2.Enabled = true;
                this.Text = "数据维护管理系统 -- " + SqlHelper.Conn.Database;
                toolStripStatusLabel1.Text = "数据库已连接";
                toolStripStatusLabel1.ForeColor = Color.Blue;
                LoadBackupHistory();
            }
            else
            {
                删除ToolStripMenuItem.Enabled = button1.Enabled = button2.Enabled = false;
                this.Text = "数据维护管理系统 -- 不存在指定的数据库[" + SqlHelper.Conn.Database + "]";
                toolStripStatusLabel1.Text = "数据库已断开";
                toolStripStatusLabel1.ForeColor = SystemColors.ControlText;
            }
        }

        private void UpdateAndChangeDB(string connString)
        {
            UpdateConnectionString(connString);
            //ChangeDB(connString);
            ConnectDB();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sure)
            {
                if (MessageBox.Show("确定要退出本系统吗？", "退出提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    if (backuping)
                    {
                        if (MessageBox.Show("备份进行中，退出将强制取消当前的备份操作！确定要退出程序吗？", "退出提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                        {
                            Common.CancelDbBackup(oBackup, Backup_PercentComplete);
                        }
                        else
                        {
                            e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                            return;
                        }
                    }
                    if (restoring)
                    {
                        if (MessageBox.Show("恢复进行中，退出将强制取消当前的恢复操作！确定要退出程序吗？", "退出提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                        {
                            Common.CancelDbRestore(oRestore, Restore_PercentComplete);
                        }
                        else
                        {
                            e.Cancel = true;
                            this.WindowState = FormWindowState.Minimized;
                            this.ShowInTaskbar = false;
                            return;
                        }
                    }
                    notifyIcon1.Dispose();
                    Environment.Exit(0);
                }
                else
                {
                    sure = false;
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ViewSyncProcess();
        }

        private void 查看维护情况ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewSyncProcess();
        }

        private void ViewSyncProcess()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            if (!this.Visible)
                this.Show();
            this.BringToFront();
            this.Focus();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sure = true;
            this.Close();
        }

        int i;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int c = i % 3;
            if (c == 0)
                notifyIcon1.Icon = MMS.Properties.Resources._1;
            else if (c == 1)
                notifyIcon1.Icon = MMS.Properties.Resources._2;
            else if (c == 2)
                notifyIcon1.Icon = MMS.Properties.Resources._3;
            Application.DoEvents();
            i++;
            if (i == 3) i = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PWD pwd = new PWD();
            if (pwd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                backupConnStr = ConfigurationManager.ConnectionStrings["connStringBackup"].ConnectionString;
                Pwd = new RSA(System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["privateKey"]))).Decrypt(ConfigurationManager.AppSettings["pwd"]);
                tables = ConfigurationManager.AppSettings["tables"];
                backupPath = ConfigurationManager.AppSettings["path"];
                ConnectDB();
                backupProgress = new SQLDMO.BackupSink_PercentCompleteEventHandler(Backup_PercentComplete);
                restoreProgress = new SQLDMO.RestoreSink_PercentCompleteEventHandler(Restore_PercentComplete);
            }
            else
            {
                MessageBox.Show("您取消授权码登录，程序将退出...");
                notifyIcon1.Dispose();
                Environment.Exit(0);
            }
            退出ToolStripMenuItem.Enabled = true;
        }

        private void LoadBackupHistory()
        {
            SqlHelper SqlHelper = new SqlHelper("connString");
            DataTable dt = Common.GetBackupHistory(SqlHelper.Conn.Database);
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //要先选定备份的记录
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //选多行时，只取第一行
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                Restore(id);
            }
            else
            {
                MessageBox.Show("先选定一个您要恢复的备份记录！");
            }
        }

        private void Restore(int id)
        {
            string path = "";
            string originPath = Common.GetBackupPathById(id);
            if (Directory.Exists(originPath))
            {
                path = originPath;
            }
            else
            {
                MessageBox.Show("原来的备份目录被意外删除或者存储设备已经被拔除，请接下来指定一个新的目录，以供恢复。");
                if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = folderBrowserDialog1.SelectedPath;
                }
                folderBrowserDialog1.Dispose();
            }
            PicVidPath pvp = new PicVidPath(false);
            if (pvp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string picPath = pvp.PicturePath;
                string vidPath = pvp.VideoPath;
                button2.Enabled = false;
                if (!timer1.Enabled)
                    timer1.Start();
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel1.Text = "数据库恢复进行中...";
                statusStrip1.Refresh();
                Application.DoEvents();
                restoring = true;
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (this.InvokeRequired)
                        this.Invoke(new ProcessRestoreHandler(ProcessRestore), id, path, picPath, vidPath);
                    else
                        ProcessRestore(id, path, picPath, vidPath);
                });
            }
        }

        public delegate void ProcessRestoreHandler(int id, string path, string picPath, string vidPath);
        private void ProcessRestore(int id, string path, string picPath, string vidPath)
        {
            bool db = true;
            bool pic = true;
            bool vid = true;
            db = Common.SQLDbRestore(id, restoreProgress, out oRestore);
            //db = Common.RestoreDB(id);
            restoring = false;
            toolStripProgressBar1.Visible = false;
            statusStrip1.Refresh();
            Application.DoEvents();
            if (db)
            {
                toolStripStatusLabel1.Text = "图片恢复进行中...";
                statusStrip1.Refresh();
                Application.DoEvents();
                try
                {
                    Common.CopyDirectory(path + "\\Picture", picPath);
                }
                catch (Exception e)
                {
                    //Logs.Error("图片恢复过程出错：" + e.Message);
                    pic = false;
                }
                toolStripStatusLabel1.Text = "视频恢复进行中...";
                statusStrip1.Refresh();
                Application.DoEvents();
                try
                {
                    Common.CopyDirectory(path + "\\Video", vidPath);
                }
                catch (Exception e)
                {
                    //Logs.Error("视频恢复过程出错：" + e.Message);
                    vid = false;
                }
            }
            if (db)
            {
                MessageBox.Show("恢复完毕！");
                toolStripStatusLabel1.Text = "恢复完成";
            }
            else
            {
                MessageBox.Show("数据库恢复失败！");
                toolStripStatusLabel1.Text = "数据库恢复失败";
                if (!pic)
                {
                    toolStripStatusLabel1.Text += ";图片恢复失败";
                }
                if (!vid)
                {
                    toolStripStatusLabel1.Text += ";视频恢复失败";
                }
            }
            if (timer1.Enabled)
                timer1.Stop();
            button2.Enabled = true;
        }

        void Restore_PercentComplete(string Message, int Percent)
        {
            UpdateProgress(Message, Percent);
        }

        private void UpdateProgress(string Message, int Percent)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    toolStripProgressBar1.Value = Percent;
                    toolStripProgressBar1.ToolTipText = Message;
                    statusStrip1.Refresh();
                    Application.DoEvents();
                }));
            }
            else
            {
                toolStripProgressBar1.Value = Percent;
                toolStripProgressBar1.ToolTipText = Message;
                statusStrip1.Refresh();
                Application.DoEvents();
            }
        }

        void Backup_PercentComplete(string Message, int Percent)
        {
            UpdateProgress(Message, Percent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Backup();
        }

        private void Backup()
        {
            folderBrowserDialog1.SelectedPath = backupPath;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                backupPath = path;
                UpdatePath(path);
                PicVidPath pvp = new PicVidPath(true);
                if (pvp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string picPath = pvp.PicturePath;
                    string vidPath = pvp.VideoPath;
                    button1.Enabled = false;
                    if (!timer1.Enabled)
                        timer1.Start();
                    toolStripProgressBar1.Visible = true;
                    toolStripStatusLabel1.Text = "数据库备份进行中...";
                    statusStrip1.Refresh();
                    Application.DoEvents();
                    backuping = true;
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        if (this.InvokeRequired)
                            this.Invoke(new ProcessBackupHandler(ProcessBackup), path, picPath, vidPath);
                        else
                            ProcessBackup(path, picPath, vidPath);
                    });
                }
            }
            folderBrowserDialog1.Dispose();
        }
            
        public delegate void ProcessBackupHandler(string path, string picPath, string vidPath);
        private void ProcessBackup(string path, string picPath, string vidPath)
        {
            bool db = true;
            bool pic = true;
            bool vid = true;
            db = Common.SQLDbBackup(path, backupProgress, out oBackup, textBox1.Text.Trim());     
            //db = Common.BackupDB(path, textBox1.Text.Trim());
            backuping = false;
            toolStripProgressBar1.Visible = false;
            statusStrip1.Refresh();
            Application.DoEvents();
            if (db)
            {
                toolStripStatusLabel1.Text = "图片备份进行中...";
                statusStrip1.Refresh();
                Application.DoEvents();
                try
                {
                    Common.CopyDirectory(picPath, path + "\\Picture");
                }
                catch (Exception e)
                {
                    //Logs.Error("图片备份过程出错：" + e.Message);
                    pic = false;
                }
                toolStripStatusLabel1.Text = "视频备份进行中...";
                statusStrip1.Refresh();
                Application.DoEvents();
                try
                {
                    Common.CopyDirectory(vidPath, path + "\\Video");
                }
                catch (Exception e)
                {
                    //Logs.Error("视频备份过程出错：" + e.Message);
                    vid = false;
                }
            }
            if (db)
            {
                MessageBox.Show("备份完毕！");
                Delete();//删除操作
                toolStripStatusLabel1.Text = "备份完成";
            }
            else
            {
                MessageBox.Show("备份数据库失败！");
                toolStripStatusLabel1.Text = "数据库备份失败";
                if (!pic)
                {
                    toolStripStatusLabel1.Text += ";图片备份失败";
                }
                if (!vid)
                {
                    toolStripStatusLabel1.Text += ";视频备份失败";
                }
            }
            LoadBackupHistory();
            if (timer1.Enabled)
                timer1.Stop();
            button1.Enabled = true;
        }

        private void 数据库连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBSetting db = new DBSetting();
            db.ConnectionString = connStr;
            db.BackupConnectionString = backupConnStr;
            if (db.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                connStr = db.ConnectionString;
                backupConnStr = db.BackupConnectionString;
                UpdateConnectionString(connStr);//这里一定要先更新配置文档！！
                UpdateConnectionStringBackup(backupConnStr);//这里一定要先更新配置文档！！
                ChangeDB(db.ConnectionString);
            }
        }

        private void 授权管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Auth au = new Auth();
            au.Pwd = Pwd;
            if (au.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Pwd = au.Pwd;
                UpdatePwd(Pwd);
                MessageBox.Show("修改成功！");
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void Delete()
        {
            if (MessageBox.Show("要进行删除操作吗？该操作会删除数据库中的数据和指定目录下的图片和视频！", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "删除中...";
                if (!timer1.Enabled)
                    timer1.Start();
                DEL del = new DEL();
                del.Tables = tables;
                if (del.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DateTime start = del.StartTime;
                    DateTime end = del.EndTime;
                    tables = del.Tables;
                    string pathPic = del.PicPath;
                    string pathVid = del.VidPath;
                    bool shrink = del.ShrinkDB;
                    string[] ts = tables.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    List<string> tables1 = new List<string>();
                    foreach (string t in ts)
                    {
                        tables1.Add(t.Trim());
                    }
                    bool flag = Common.DeleteDB(start.Year, start.Month, end.Year, end.Month, tables1);
                    if (flag)
                    {
                        if (shrink)
                        {
                            flag = Common.ShrinkDB();
                            if (!flag)
                            {
                                MessageBox.Show("收缩数据库失败！");
                            }
                        }
                        flag = Common.DeleteDirectory(pathPic, start, end);
                        if (!flag)
                        {
                            MessageBox.Show("删除图片失败！");
                        }
                        flag = Common.DeleteDirectory(pathVid, start, end);
                        if (!flag)
                        {
                            MessageBox.Show("删除视频失败！");
                        }
                        MessageBox.Show("操作完毕！");
                    }
                    else
                    {
                        MessageBox.Show("删除数据库失败，删除操作被迫中止。");
                    }
                    UpdateTables(tables);
                    toolStripStatusLabel1.Text = "删除操作完毕";
                }
                else
                {
                    toolStripStatusLabel1.Text = "取消删除操作";
                }
                if (timer1.Enabled)
                    timer1.Stop();
            }
        }
    }
}
