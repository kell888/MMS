using System;
using System.Collections.Generic;
using System.Text;

namespace MMS
{
    public class Record
    {
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        DateTime time;

        public DateTime BackupTime
        {
            get { return time; }
            set { time = value; }
        }
        string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public Record(int id, string path, DateTime time, string remark = null)
        {
            this.ID = id;
            this.Path = path;
            this.BackupTime = time;
            this.Remark = remark;
        }
    }
}
