using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;
using System.ComponentModel;
using System.Drawing;

namespace SVControl
{
    public class SVBinaryVarTypeEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool IsDropDownResizable { get { return true; } }

        public override void PaintValue(PaintValueEventArgs e)
        {
            SVVarDefine var = e.Value as SVVarDefine;
            if (var == null)
                return;

            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            Rectangle rect = new Rectangle(1, 1, 19, 17);

            if (String.IsNullOrWhiteSpace(var.VarName))
            {
                e.Graphics.DrawString("-", new Font("宋体", 12, FontStyle.Bold), new SolidBrush(Color.Black), rect, strFormat);
                return;
            }

            switch (var.VarBlockType)
            {
                case 0:
                    e.Graphics.DrawString("I", new Font("宋体", 12, FontStyle.Bold), new SolidBrush(Color.Black), rect, strFormat);
                    break;
                case 1:
                    e.Graphics.DrawString("O", new Font("宋体", 12, FontStyle.Bold), new SolidBrush(Color.Black), rect, strFormat);
                    break;
                case 2:
                    e.Graphics.DrawString("S", new Font("宋体", 12, FontStyle.Bold), new SolidBrush(Color.Black), rect, strFormat);
                    break;
            }
        }

        /// <summary>
        /// 选择开关量变量
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
                SVVarDefine variable = value as SVVarDefine;
                if (variable == null)
                    return value;

                SVWpfControl variableDialog = new SVWpfControl();
                variableDialog.Width = 280;
                variableDialog.Height = 400;

                SVWPFVariableDialog dialog = new SVWPFVariableDialog();
                dialog.setFilter(new List<String> { "BOOL", "BOOL_S" });
                dialog.name.DataContext = variable.VarName;
                dialog.type.DataContext = variable.VarBlockType;
                variableDialog.addContent(dialog);

                edSvc.DropDownControl(variableDialog);

                variable.VarName = (String)dialog.name.DataContext;
                variable.VarBlockType = Convert.ToByte(dialog.type.DataContext);
            }

            return value;
        }
    }
}
