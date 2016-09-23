using System.Drawing;
using System.Windows.Forms;

namespace SVCore
{
    /// <summary>
    /// 定义一个特殊类，用来包装当前控件
    /// 同时用来在页面窗口中确定类型。
    /// </summary>
    public class SVBackGroundControl : Panel
    {
        /// <summary>
        /// 用来记录当前子控件
        /// </summary>
        public SVBasePanel BaseControl { get; set; }
    }
}
