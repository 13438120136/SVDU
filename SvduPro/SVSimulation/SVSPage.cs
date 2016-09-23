using System;
using System.Windows.Forms;
using System.Drawing;
using SVCore;

namespace SVSimulation
{
    public class SVSPage : Panel
    {
        public UInt16 ID { get; set; }

        public void fromBin(PageBin bin)
        {
            this.BackColor = Color.FromArgb((Int32)bin.bgClr);
            this.ID = bin.id;
        }
    }
}
