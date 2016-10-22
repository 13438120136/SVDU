using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using SVCore;

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

            ///初始化界面并且启动程序
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            ///创建主窗口对象
            SVDockMainWindow mainWindow = new SVDockMainWindow(args);
            ///捕获系统异常信息            
            Application.ThreadException += new ThreadExceptionEventHandler((sender, e) =>
            {
                mainWindow.captureExceptionAndSaveProject(e.Exception);
            });

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, e) =>
            {
                mainWindow.captureExceptionAndSaveProject(e.ExceptionObject);
            });

            Application.Run(mainWindow);
        }
    }
}
