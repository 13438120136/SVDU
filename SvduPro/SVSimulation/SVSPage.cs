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

        public void fromBin(PageBin bin)
        {
            this.BackColor = Color.FromArgb((Int32)bin.bgClr);
            this.ID = bin.id;
        }
    }
}
