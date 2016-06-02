using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MergeQueryUtil;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MMS
{
    public static class Common
    {
        /// < summary>
        /// SQL数据库备份
        /// < /summary>
        /// < param name="path">备份到的路径< /param>
        /// <param name="Backup_PercentComplete">进度</param>
        /// <param name="oBackup">数据库备份服务对象</param>
        /// <param name="remark">备份备注</param>
        public static bool SQLDbBackup(string path, SQLDMO.BackupSink_PercentCompleteEventHandler Backup_PercentComplete, out SQLDMO.Backup oBackup, string remark = null)
        {
            string dir = path + "\\Database";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            SqlConnectionStringBuilder connStrBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            string ServerIP = connStrBuilder.DataSource;
            string LoginUserName = connStrBuilder.UserID;
            string LoginPass = connStrBuilder.Password;
            string DBName = connStrBuilder.InitialCatalog;
            string DBFile = DBName + DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = dir + "\\" + DBFile + ".bak";
            if (!File.Exists(filename))
            {
                try
                {
                    FileStream fs = File.Create(filename);
                    fs.Close();
                }
                catch (Exception e)
                {
                    //Logs.Error("无法创建 [" + filename + "] 数据库备份文件！" + Environment.NewLine + e.Message);
                }
            }
            oBackup = new SQLDMO.BackupClass();
            SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
            try
            {
                oSQLServer.LoginSecure = false;
                oSQLServer.Connect(ServerIP, LoginUserName, LoginPass);
                oBackup.Action = SQLDMO.SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
                oBackup.PercentComplete += Backup_PercentComplete;
                oBackup.Database = DBName;
                oBackup.Files = @"" + string.Format("[{0}]", filename) + "";
                oBackup.BackupSetName = DBFile;
                oBackup.BackupSetDescription = "备份集" + DBFile;
                oBackup.Initialize = true;
                oBackup.SQLBackup(oSQLServer);//这里可能存在问题！
                SqlHelper SqlHelper = new SqlHelper("connStringBackup");
                SqlHelper.ExecuteNonQuery("insert into Record (DB, Path, Remark) values ('" + DBName + "', '" + filename + "', '" + remark + "')");
                return true;
            }
            catch (Exception e)
            {
                //Logs.Error("备份时出错：" + e.Message);
            }
            finally
            {
                oSQLServer.DisConnect();
            }
            return false;
        }

        public static void CancelDbBackup(SQLDMO.Backup oBackup, SQLDMO.BackupSink_PercentCompleteEventHandler Backup_PercentComplete)
        {
            try
            {
                oBackup.Abort();
                oBackup.PercentComplete -= Backup_PercentComplete;
            }
            catch (Exception e)
            {
                //Logs.Error("取消数据库备份失败：" + e.Message);
            }
        }

        public static void CancelDbRestore(SQLDMO.Restore oRestore, SQLDMO.RestoreSink_PercentCompleteEventHandler Restore_PercentComplete)
        {
            try
            {
                oRestore.Abort();
                oRestore.PercentComplete -= Restore_PercentComplete;
            }
            catch (Exception e)
            {
                //Logs.Error("取消数据库还原失败：" + e.Message);
            }
        }

        /// < summary>
        /// SQL恢复数据库
        /// < /summary>
        /// <param name="id">备份集ID</param>
        /// <param name="Restore_PercentComplete">进度</param>
        /// <param name="oRestore">数据库还原服务对象</param>
        public static bool SQLDbRestore(int id, SQLDMO.RestoreSink_PercentCompleteEventHandler Restore_PercentComplete, out SQLDMO.Restore oRestore)
        {
            oRestore = null;
            SqlHelper SqlHelper = new SqlHelper("connStringBackup");
            object obj = SqlHelper.ExecuteScalar("select Path from Record where ID=" + id);
            if (obj != null && obj != DBNull.Value)
            {
                string filename = obj.ToString();
                if (File.Exists(filename))
                {
                    SqlConnectionStringBuilder connStrBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
                    string ServerIP = connStrBuilder.DataSource;
                    string LoginUserName = connStrBuilder.UserID;
                    string LoginPass = connStrBuilder.Password;
                    string DBName = connStrBuilder.InitialCatalog;
                    oRestore = new SQLDMO.RestoreClass();
                    SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
                    try
                    {
                        oSQLServer.LoginSecure = false;
                        oSQLServer.Connect(ServerIP, LoginUserName, LoginPass);
                        //因为数据库正在使用，所以无法获得对数据库的独占访问权。不一定是由于其他进程的占用，还有其他的原因，所以要脱机再联机...
                        KillProcess(DBName);
                        //KillSqlProcess(oSQLServer, DBName);
                        OffAndOnLine(DBName);
                        oRestore.Action = SQLDMO.SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
                        oRestore.PercentComplete += Restore_PercentComplete;
                        oRestore.Database = DBName;
                        oRestore.Files = @"" + string.Format("[{0}]", filename) + "";
                        oRestore.FileNumber = 1;
                        oRestore.ReplaceDatabase = true;
                        oRestore.SQLRestore(oSQLServer);//这里可能存在问题！
                        return true;
                    }
                    catch (Exception e)
                    {
                        //Logs.Error("恢复时出错：" + e.Message);
                    }
                    finally
                    {
                        oSQLServer.DisConnect();
                    }
                }
                else
                {
                    //Logs.Error("找不到要还原的备份数据库文件 [" + filename + "]");
                }
            }
            return false;
        }

        private static void OnLine(string DBName)
        {
            string sql = "alter database " + DBName + " set online with ROLLBACK IMMEDIATE";
            SqlHelper SqlHelper = new MergeQueryUtil.SqlHelper("connString");
            SqlHelper.ExecuteNonQuery(sql);
        }

        private static void OffLine(string DBName)
        {
            string sql = "alter database " + DBName + " set offline with ROLLBACK IMMEDIATE";
            SqlHelper SqlHelper = new MergeQueryUtil.SqlHelper("connString");
            SqlHelper.ExecuteNonQuery(sql);
        }

        private static void OffAndOnLine(string DBName)
        {
            string sql = "alter database " + DBName + " set offline with ROLLBACK IMMEDIATE;alter database " + DBName + " set online with ROLLBACK IMMEDIATE";
            SqlHelper SqlHelper = new MergeQueryUtil.SqlHelper("connString");
            SqlHelper.ExecuteNonQuery(sql);
        }

        private static void KillProcess(string DbName)
        {
            string sql = "select spid from sys.sysprocesses where dbid in (select dbid from master..sysdatabases where name = '" + DbName + "')";
            SqlHelper SqlHelper = new MergeQueryUtil.SqlHelper("connString");
            DataTable dt = SqlHelper.ExecuteQueryDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery("kill " + dt.Rows[i][0].ToString());
                    }
                    catch { }//避免KILL自身进程造成错误！
                }
            }
        }

        private static void KillSqlProcess(SQLDMO.SQLServer svr, string DbName)
        {
            SQLDMO.QueryResults qr = svr.EnumProcesses(-1);
            int iColPIDNum = -1;
            int iColDbName = -1;
            for (int i = 1; i <= qr.Columns; i++)
            {
                string strName = qr.get_ColumnName(i);
                if (strName.ToUpper().Trim() == "SPID")
                {
                    iColPIDNum = i;
                }
                else if (strName.ToUpper().Trim() == "DBNAME")
                {
                    iColDbName = i;
                }
                if (iColPIDNum != -1 && iColDbName != -1)
                    break;
            }
            //杀死使用DbName数据库的进程
            for (int i = 1; i <= qr.Rows; i++)
            {
                int lPID = qr.GetColumnLong(i, iColPIDNum);
                string strDBName = qr.GetColumnString(i, iColDbName);
                if (strDBName.ToUpper() == DbName.ToUpper())
                {
                    svr.KillProcess(lPID);
                }
            }
        }

        public static string GetBackupPathById(int id)
        {
            SqlHelper SqlHelper = new SqlHelper("connStringBackup");
            object obj = SqlHelper.ExecuteScalar("select Path from Record where ID=" + id);
            if (obj != null && obj != DBNull.Value)
            {
                string filename = obj.ToString();
                FileInfo fi = new FileInfo(filename);
                if (fi.Directory != null)
                    return fi.Directory.Parent.FullName;
            }
            return "";
        }

        public static void CopyDirectory(string srcDir, string dstDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(dstDir);

            if (!source.Exists)
            {
                return;
            }

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        }

        public static bool DeleteDB(int year, int month, int year2, int month2, List<string> tables)
        {
            int count = 0;
            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year2, month2, 1);
            if (end >= start)
            {
                int months = 0;
                if (end.Year > start.Year)//2013.10-2014.8
                {
                    months += 12 - start.Month + 1;
                    months += 12 * (end.Year - start.Year - 1);
                    months += end.Month;
                }
                else//2013.8-2013.10
                {
                    months = end.Month - start.Month + 1;
                }
                for (int i = 0; i < months; i++)
                {
                    DateTime current = start.AddMonths(i);
                    string YYYYMM = current.Year.ToString().PadLeft(4, '0') + current.Month.ToString().PadLeft(2, '0');
                    SqlHelper SqlHelper = new SqlHelper("connString");
                    foreach (string table in tables)
                    {
                        string sql = "DROP TABLE " + table + YYYYMM;
                        try
                        {
                            SqlHelper.ExecuteNonQuery(sql);
                            count++;
                        }
                        catch { }//不存在的表出错时略过...
                    }
                }
            }
            return count > 0;
        }
        /// <summary>
        /// 删除指定时间范围内的目录，时间的范围的粒度是天(连同跟时间格式匹配的所有子目录均被删除！)
        /// </summary>
        /// <param name="path">指定的图片或者视频根目录</param>
        /// <param name="start">删除的开始时间</param>
        /// <param name="end">删除的结束时间</param>
        /// <returns></returns>
        public static bool DeleteDirectory(string path, DateTime start, DateTime end)
        {
            int count = 0;
            int days = end.Subtract(start).Days;
            DateTime curr = start;
            for (int i = 0; i < days; i++)
            {
                string yyyyMMdd = curr.Year.ToString().PadLeft(4, '0') + curr.Month.ToString().PadLeft(2, '0') + curr.Day.ToString().PadLeft(2, '0');
                string[] dirs = Directory.GetDirectories(path, yyyyMMdd + "*");
                foreach (string dir in dirs)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    string thePath = path + "\\" + di.Name;
                    try
                    {
                        Directory.Delete(thePath, true);
                        count++;
                    }
                    catch (Exception e)
                    {
                        //Logs.Error("DeleteDirectory[" + thePath + "]：" + e.Message);
                    }
                }
                curr.AddDays(1);
            }
            return count > 0;
        }

        public static bool ShrinkDB()
        {
            try
            {
                SqlHelper SqlHelper = new SqlHelper("connString");
                string sql = "DBCC SHRINKDATABASE('" + SqlHelper.Conn.Database + "')";
                SqlHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception e)
            {
                //Logs.Error("ShrinkDB：" + e.Message);
                return false;
            }
        }

        public static DataTable GetBackupHistory(string DB)
        {
            SqlHelper SqlHelper = new SqlHelper("connStringBackup");
            string sql = "select * from Record where DB='" + DB + "' order by ID desc";
            DataTable dt = SqlHelper.ExecuteQueryDataTable(sql);
            return dt;
        }
    }
}
