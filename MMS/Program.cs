using System;
using System.Windows.Forms;
using System.Threading;

namespace MMS
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Logs.Init();
            Application.Run(new MainForm());
        }
    }
}
