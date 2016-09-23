/*
 * 定义一个当前对话框的实例接口,目的是保持全局唯一的一个实例对象。
 * 方便对全局数据和全局界面的整体管理，方便后续功能扩展。
 * */
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVCore
{
    /// <summary>
    /// 定义应用程序接口
    /// </summary>
    public interface SVInterfaceApplication
    {
        /// <summary>
        /// 当前工程中的导航树窗口对象
        /// </summary>
        Control TreeProject { get; }

        /// <summary>
        /// 查找窗口
        /// </summary>
        Control FindWindow { get; }

        /// <summary>
        /// 数据库操作对象
        /// </summary>
        SVSqlDataBase DataBase { get; }
    }

    /// <summary>
    /// 记录当前实例对象
    /// </summary>
    public class SVApplication
    {
        public static SVInterfaceApplication Instance { get; set; }
    }
}
