using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVGifDataTypeEditor : UITypeEditor
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
        /// 重载EditValue,编辑动态图控件的弹出窗口
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
                //从当前对象中获取动态图控件对象
                SVGif gitInstance = context.Instance as SVGif;
                if (gitInstance == null)
                    return null;

                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    SVGifWindow win = new SVGifWindow(gitInstance);
                    edSvc.ShowDialog(win);

                    if (gitInstance.Attrib.VarName.Count == 0)
                    {
                        return "未设置变量";
                    }
                    else
                    {
                        String str = String.Join(",", gitInstance.Attrib.VarName);
                        return str;
                    }
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
