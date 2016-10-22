using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVBitmapTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 选择图片的属性类
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVBitmapManagerWindow window = new SVBitmapManagerWindow();
                if (edSvc.ShowDialog(window) == System.Windows.Forms.DialogResult.Yes)
                    return window.SvBitMap;
                else
                    return null;
            }

            return value;
        }
    }
}
