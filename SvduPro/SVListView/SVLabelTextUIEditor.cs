﻿using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVLabelTextUIEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            SVLabel svLabel = context.Instance as SVLabel;
            if (svLabel == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl textDialog = new SVWpfControl();
                textDialog.Width = 200;
                textDialog.Height = 120;

                SVWPFLabelTextEdit edit = new SVWPFLabelTextEdit();
                edit.textBox.DataContext = value;
                textDialog.addContent(edit);
                edSvc.DropDownControl(textDialog);
                value = edit.textBox.Text;

                return value;
            }

            return value;
        }
    }
}
