using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SvduPro
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SVDockMainWindow(args));
        }
    }
}
