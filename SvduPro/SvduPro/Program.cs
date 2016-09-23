using System;
using System.Windows.Forms;
using SVCore;
using System.Threading;
using System.Globalization;

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
            ///加载软件配置信息
            SVConfig currInstance = SVConfig.instance();
            currInstance.loadConfig();
            ///设置语言
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(currInstance.Language);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SVDockMainWindow(args));
        }
    }
}
