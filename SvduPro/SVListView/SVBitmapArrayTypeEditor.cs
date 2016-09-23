using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVBitmapArrayTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 重载GetEditStyle
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 重载EditValue
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
                SVHeartbeat heart = context.Instance as SVHeartbeat;
                if (heart == null)
                    return null;

                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    SVBitmapArrayWindow window = new SVBitmapArrayWindow(heart.Attrib.BitMapArray);
                    window.ShowDialog();

                    return window.bitmapArray();
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
