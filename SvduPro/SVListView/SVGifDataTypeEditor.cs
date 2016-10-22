using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SVControl
{
    public class SVGifDataTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 重载GetEditStyle,设置窗口弹出行为
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// 重载EditValue,编辑动态图控件的弹出窗口
        /// </summary>
        /// <param Name="context"></param>
        /// <param Name="provider"></param>
        /// <param Name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
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
                gitInstance.RedoUndo.operChanged();

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

            return value;
        }
    }
}
