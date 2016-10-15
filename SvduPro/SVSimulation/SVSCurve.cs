using System;
using System.Collections.Generic;
using System.Drawing;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSCurve : SVCurve
    {
        public SVSCurve()
        {
            this.IsSimulation = true;
            this.IsMoved = false;
        }

        public void fromBin(TrendChartBin bin)
        {
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;

            Attrib.Rect = new Rectangle(this.Location, new Size(this.Width, this.Height));

            this.BackColor = Color.FromArgb((Int32)bin.bgClr);
            this.ForeColor = Color.FromArgb((Int32)bin.scaleClr);
            Attrib.Interval = bin.maxTime;
            Attrib.Min = bin.yMin;
            Attrib.Max = bin.yMax;

            Dictionary<Byte, Font> FontConfig = new Dictionary<Byte, Font>();
            FontConfig.Add(8, new Font("宋体", 8));
            FontConfig.Add(12, new Font("宋体", 12));
            FontConfig.Add(16, new Font("宋体", 16));
            Attrib.Font = FontConfig[bin.font];
        }
    }
}
