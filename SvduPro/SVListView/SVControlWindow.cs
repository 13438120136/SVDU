//
// 控件窗口类
// 
// 存放当前控件的元素，是生成页面页面控件的主要入口。
//

using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using System.ComponentModel;

namespace SVControl
{
    public class SVControlWindow : DockContent
    {
        Control _control = null;

        public SVControlWindow(Control control)
        {
            //DockAreas = (DockAreas.Document | DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom);
            //ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            //HideOnClose = false;
            control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(control);

            this._control = control;
            this.AutoScroll = true;
        }

        /// <summary>
        /// 当窗口尺寸发生改变的时候
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            ///让子窗口相对于父窗口居中显示
            int x = (this.Width - _control.Width) / 2;
            int y = (this.Height - _control.Height) / 2;

            ///保证坐标不能出现负数
            if (x < 0)
                x = 0;

            if (y < 0)
                y = 0;

            _control.Location = new System.Drawing.Point(x, y);
        }

        /// <summary>
        /// 当窗口执行关闭的时候
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
