using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVBinaryTypeTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 选择开关量类型
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl textDialog = new SVWpfControl();
                textDialog.Width = 100;
                textDialog.Height = 40;

                SVWPFBinaryTypeDialog dialog = new SVWPFBinaryTypeDialog();                
                textDialog.addContent(dialog);
                dialog.listView.DataContext = value;
                edSvc.DropDownControl(textDialog);

                if (dialog.listView.SelectedIndex != -1)
                    value = (Byte)dialog.listView.SelectedIndex;

                return value;
            }

            return value;
        }
    }
}
