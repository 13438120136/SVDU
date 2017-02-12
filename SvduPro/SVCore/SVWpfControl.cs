using System.Windows.Forms;

namespace SVCore
{
    public partial class SVWpfControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SVWpfControl()
        {
            InitializeComponent();
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        /// <summary>
        /// 设置当前WPF对应的窗口到当前窗口中
        /// </summary>
        /// <param name="control"></param>
        public void addContent(System.Windows.Controls.UserControl control)
        {
            this.elementHost1.Child = control;
        }
    }
}
