using System;
using System.Windows.Forms;
using System.Drawing;
using SVCore;
using SVControl;

namespace SVSimulation
{
    public class SVSPage : SVPageWidget
    {
        public UInt16 ID { get; set; }

        public void fromBin(PageBin bin, byte[] picBuffer)
        {
            if (bin.bgSet == 0)
            {
                ///背景颜色显示
                this.BackColor = Color.FromArgb((Int32)bin.bgClr);
            }
            else
            {
                ///背景图片显示
                SVPixmapFile file = new SVPixmapFile();
                this.BackgroundImage = file.getFromFile(picBuffer, bin.bgClr);
            }

            this.ID = bin.id;
        }
    }
}
