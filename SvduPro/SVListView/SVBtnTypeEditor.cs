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
            //从当前对象中获取按钮控件对象
            SVButton button = context.Instance as SVButton;
            if (button == null)
                return null;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVBtnDoWindow win = new SVBtnDoWindow(button);
                edSvc.ShowDialog(win);
                return button.Attrib.BtnType;
            }

            return value;
        }
    }
}
