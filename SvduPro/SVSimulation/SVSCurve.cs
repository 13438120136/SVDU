using System;
using System.Drawing;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSCurve : SVCurve
    {
        public SVSCurve()
        {
            this.IsMoved = false;
        }

        public void fromBin(TrendChartBin bin)
        {
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;

            this.BackColor = Color.FromArgb((Int32)bin.bgClr);
            //this.FlatAppearance.MouseDownBackColor = BackColor;
            //this.FlatAppearance.MouseOverBackColor = BackColor; 
        }
    }
}
