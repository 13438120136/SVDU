using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVPageUIEditer : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            //从当前对象中获取按钮控件对象
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            SVPageWidget widget = e.Context.Instance as SVPageWidget;
            if (widget.Attrib.BackGroundType == 0)
            {
                SolidBrush brush = new SolidBrush(widget.Attrib.BackColor);
                Rectangle rect = new Rectangle(1, 1, 19, 17);

                e.Graphics.FillRectangle(brush, rect);
            }
            else
            {
                Rectangle rect = new Rectangle(1, 1, 19, 17);
                e.Graphics.DrawImage(widget.Attrib.PicIconData.bitmap(), rect);
            }
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
    System.IServiceProvider provider, object value)
        {
            //从当前对象中获取按钮控件对象
            SVPageWidget page = context.Instance as SVPageWidget;
            if (page == null)
                return null;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl textDialog = new SVWpfControl();
                textDialog.Width = 200;
                textDialog.Height = 300;

                WPFUser edit = new WPFUser();
                edit.DataContext = page.Attrib;

                textDialog.addContent(edit);
                edSvc.DropDownControl(textDialog);

                page.refreshPropertyToPanel();
            }

            return value;
        }
    }
}
