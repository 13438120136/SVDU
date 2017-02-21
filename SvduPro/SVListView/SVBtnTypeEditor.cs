using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SVControl
{    
    public class SVBtnTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 重载GetEditStyle,设置窗口弹出行为
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        /// 
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// 重载EditValue,编辑按钮动作属性的弹出窗口
        /// </summary>
        /// <param Name="context"></param>
        /// <param Name="provider"></param>
        /// <param Name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            SVBtnChoicePage page = value as SVBtnChoicePage;
            if (page == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVPageSelectWindow win = new SVPageSelectWindow();
                if (win.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    page.PageText = win.getPageText();
                    page.PageID = win.getPageID();
                    return page;
                }
            }

            return value;
        }
    }
}
