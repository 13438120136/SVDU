using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;
using System.Drawing;

namespace SVControl
{
    public class SVColorTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            SolidBrush brush = new SolidBrush((Color)e.Value);
            Rectangle rect = new Rectangle(1, 1, 19, 17);

            e.Graphics.FillRectangle(brush, rect);
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl colorDialog = new SVWpfControl();
                colorDialog.Width = 170;
                colorDialog.Height = 160+200;

                SVWpfColorDialog dialog = new SVWpfColorDialog();
                colorDialog.addContent(dialog);

                var clr = (System.Drawing.Color)value;
                dialog.setDataContext(System.Windows.Media.Color.FromArgb(clr.A, clr.R, clr.G, clr.B));

                edSvc.DropDownControl(colorDialog);
                var rgb = dialog.SelectColor;
                value = System.Drawing.Color.FromArgb(rgb.A, rgb.R, rgb.G, rgb.B);

                return value;
            }

            return value;
        }
    }
}
