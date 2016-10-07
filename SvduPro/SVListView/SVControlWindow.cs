//
// 控件窗口类
// 
// 存放当前控件的元素，是生成页面页面控件的主要入口。
//

using System.ComponentModel;
using System.Windows.Forms;
using SVCore;
using WeifenLuo.WinFormsUI.Docking;

namespace SVControl
{
    public class SVControlWindow : DockContent
    {
        Control _control = null;

        /// <summary>
        /// 设置和获取当前内部控件
        /// </summary>
        public Control CoreControl
        {
            get { return _control; }
            set { _control = value; }
        }

        /// <summary>
        /// 根据控件初始化窗口
        /// </summary>
        /// <param name="control"></param>
        public SVControlWindow(Control control)
        {
            //DockAreas = (DockAreas.Document | DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom);
            //ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            //HideOnClose = false;
            control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(control);

            this._control = control;
            this.AutoScroll = true;

            this.KeyPreview = true;

            ///注册快捷键方式,只有页面控件才有
            if (control is SVPageWidget)
                initShortKey();
        }

        /// <summary>
        /// 当前激活窗口的快捷键执行方式
        /// </summary>
        private void initShortKey()
        {
            ///一个隐藏的右键菜单
            ContextMenuStrip menu = new ContextMenuStrip();
            this.ContextMenuStrip = menu;

            ///添加Ctrl+C
            ToolStripMenuItem copyItem = new ToolStripMenuItem();
            copyItem.ShortcutKeys = Keys.Control | Keys.C;
            copyItem.Visible = false;
            menu.Items.Add(copyItem);

            ///添加Ctrl+X
            ToolStripMenuItem cutItem = new ToolStripMenuItem();
            cutItem.ShortcutKeys = Keys.Control | Keys.X;
            cutItem.Visible = false;
            menu.Items.Add(cutItem);
            
            ///添加Ctrl+V
            ToolStripMenuItem pasteItem = new ToolStripMenuItem();
            pasteItem.ShortcutKeys = Keys.Control | Keys.V;
            pasteItem.Visible = false;
            menu.Items.Add(pasteItem);

            ///添加Ctrl+A
            ToolStripMenuItem allItem = new ToolStripMenuItem();
            allItem.ShortcutKeys = Keys.Control | Keys.A;
            allItem.Visible = false;
            menu.Items.Add(allItem);

            ///添加Delete
            ToolStripMenuItem delItem = new ToolStripMenuItem();
            delItem.ShortcutKeys = Keys.Delete;
            delItem.Visible = false;
            menu.Items.Add(delItem);

            ///撤销
            ToolStripMenuItem undoItem = new ToolStripMenuItem();
            undoItem.ShortcutKeys = Keys.Control | Keys.Z;
            undoItem.Visible = false;
            menu.Items.Add(undoItem);

            ///恢复
            ToolStripMenuItem redoItem = new ToolStripMenuItem();
            redoItem.ShortcutKeys = Keys.Control | Keys.Y;
            redoItem.Visible = false;
            menu.Items.Add(redoItem);
            

            ///添加所有事件
            delItem.Click += new System.EventHandler(delItem_Click);
            copyItem.Click += new System.EventHandler(copyItem_Click);
            cutItem.Click += new System.EventHandler(cutItem_Click);
            pasteItem.Click += new System.EventHandler(pasteItem_Click);
            allItem.Click += new System.EventHandler(allItem_Click);
            undoItem.Click += new System.EventHandler(undoItem_Click);
            redoItem.Click += new System.EventHandler(redoItem_Click);
        }

        /// <summary>
        /// 执行恢复操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void redoItem_Click(object sender, System.EventArgs e)
        {
            redoMethod();
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void redoMethod()
        {
            SVPageWidget widget = this._control as SVPageWidget;
            if (widget != null)
                widget.RedoUndo.Redo();
        }

        /// <summary>
        /// 执行撤销操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void undoItem_Click(object sender, System.EventArgs e)
        {
            undoMethod();
        }

        public void undoMethod()
        {
            SVPageWidget widget = this._control as SVPageWidget;
            if (widget != null)
                widget.RedoUndo.Undo();
        }

        /// <summary>
        /// 全选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void allItem_Click(object sender, System.EventArgs e)
        {
            selectAllMethod();
        }

        public void selectAllMethod()
        {
            SVPageWidget widget = this._control as SVPageWidget;
            if (widget != null)
                widget.selectAll(true);
        }

        /// <summary>
        /// 粘贴事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pasteItem_Click(object sender, System.EventArgs e)
        {
            if (_control is SVPageWidget)
                SVSelectPanelObjs.pasteOperator();
        }

        /// <summary>
        /// 剪切事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cutItem_Click(object sender, System.EventArgs e)
        {
            if (_control is SVPageWidget)
                SVSelectPanelObjs.cutOperator();
        }

        /// <summary>
        /// 复制事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void copyItem_Click(object sender, System.EventArgs e)
        {
            if (_control is SVPageWidget)
                SVSelectPanelObjs.copyOperator();
        }

        /// <summary>
        /// 处理按键消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                    SVSelectPanelObjs.up();
                    break;
                case Keys.Down:
                    SVSelectPanelObjs.down();
                    break;
                case Keys.Left:
                    SVSelectPanelObjs.left();
                    break;
                case Keys.Right:
                    SVSelectPanelObjs.right();
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 按下删除键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delItem_Click(object sender, System.EventArgs e)
        {
            if (_control is SVPageWidget)
                SVSelectPanelObjs.removeOperator();
        }

        /// <summary>
        /// 设置当前控件
        /// </summary>
        /// <param name="control"></param>
        public void setControl(Control control)
        {
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
