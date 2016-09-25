using System.Drawing;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSGif : SVGif
    {
        public SVSGif()
        {
            this.IsSimulation = true;
            this.IsMoved = false;
            this.BackColor = Color.White; 
        }

        public void fromBin(GifBin bin, byte[] picBuffer)
        {
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;

            if (picBuffer.Length > bin.iamgeOffsetErr)
            {
                SVPixmapFile file = new SVPixmapFile();
                this.BackgroundImage = file.getFromFile(picBuffer, bin.iamgeOffsetErr);
            }
        }
    }
}
