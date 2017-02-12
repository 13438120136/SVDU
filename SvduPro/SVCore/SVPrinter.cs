/*
 * 打印机类
 * **/
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

//[Browsable(false)]

namespace SVCore
{
    public class SVPrinter
    {
        /// <summary>
        /// 打印指定图片对象
        /// </summary>
        /// <param name="bitmap">图片对象</param>
        public void printBmp(Bitmap bitmap)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", bitmap.Width, bitmap.Height);
            printDocument.PrintPage += new PrintPageEventHandler((sender, e) =>
            {
                 e.Graphics.DrawImage(bitmap, 0, 0);
            });

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            DialogResult result = printPreviewDialog.ShowDialog();
            if (result == DialogResult.OK)
                printDocument.Print();
        }
    }
}
