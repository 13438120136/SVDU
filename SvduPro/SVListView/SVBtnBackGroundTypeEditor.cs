using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 设置按钮背景用户操作界面
    /// </summary>
    public class SVBtnBackGroundTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 重载GetEditStyle,设置窗口弹出行为
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// 重载EditValue,编辑按钮动作属性的弹出窗口
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            try
            {
                //从当前对象中获取按钮控件对象
                SVButton button = context.Instance as SVButton;
                if (button == null)
                    return null;

                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    SVBtnBackGroundWindow win = new SVBtnBackGroundWindow(button);
                    edSvc.ShowDialog(win);

                    if (button.Attrib.IsShowPic)
                        return "图片";
                    else
                        return "颜色";
                }
            }
            catch (Exception ex)
            {
                SVLog.TextLog.Exception(ex);
            }

            return value;
        }
    }
}
