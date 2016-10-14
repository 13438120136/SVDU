using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SVControl;
using SVCore;
using System;

namespace SVSimulation
{
    public class SVSGif : SVGif
    {
        Timer _timer;

        public SVSGif(Timer timer)
        {
            _timer = timer;
            this.IsSimulation = true;
            this.IsMoved = false;
            this.BackColor = Color.White; 
        }

        public void fromBin(GifBin bin, byte[] picBuffer)
        {
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;

            List<Bitmap> bitList = new List<Bitmap>();
            for (int i = 0; i < (bin.type + 1); i++)
            {
                SVPixmapFile file = new SVPixmapFile();
                Bitmap bitmap = file.getFromFile(picBuffer, bin.imageOffset[i]);
                bitList.Add(bitmap);
            }

            Int32 index = 0;
            _timer.Tick += new EventHandler((sender, e) => 
            {
                this.BackgroundImage = bitList[index];
                index++;

                if (index == bin.type + 1)
                    index = 0;
            });
        }
    }
}
