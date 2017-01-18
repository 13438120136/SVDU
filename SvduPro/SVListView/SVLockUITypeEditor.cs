using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;
using System;


namespace SVControl
{
    public class SVLockUITypeEditor : UITypeEditor
    {
                /// <summary>
        /// 选择图片的属性类
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl picDialog = new SVWpfControl();
                picDialog.Width = 40;
                picDialog.Height = 110;

                SVWPFLockDialog lockDialog = new SVWPFLockDialog();
                lockDialog.setChecked((Boolean)value);
                picDialog.addContent(lockDialog);
                edSvc.DropDownControl(picDialog);
                value = lockDialog.getChecked();

                return value;
            }

            return value;
        }
    }
}
