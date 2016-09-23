using System;
using SVControl;
using System.Drawing;
using SVCore;

namespace SVSimulation
{
    public class SVSLine : SVLine
    {
        public void fromBin(LineBin bin)
        {
            this.BackColor = Color.FromArgb((Int32)bin.color);
            //this.FlatAppearance.MouseDownBackColor = BackColor;
            //this.FlatAppearance.MouseOverBackColor = BackColor;
            this.Location = new Point(bin.x1, bin.y1);
            this.Width = (bin.x2 - bin.x1);
            this.Height = (bin.y2 - bin.y1);
        }
    }
}
