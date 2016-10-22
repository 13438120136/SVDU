using System.Drawing;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSIcon : SVIcon
    {
        /// <summary>
        /// 仿真静态图
        /// </summary>
        public SVSIcon()
        {
            this.IsSimulation = true;
            this.IsMoved = false;
        }

        /// <summary>
        /// 解析静态图数据并显示
        /// </summary>
        /// <param Name="bin">静态图结构</param>
        /// <param Name="picBuffer">图片数据</param>
        public void fromBin(IconBin bin, byte[] picBuffer)
        {
            ///设置位置和尺寸
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;

            ///设置图片
            SVPixmapFile file = new SVPixmapFile();
            this.BackgroundImage = file.getFromFile(picBuffer, bin.imageOffset);
        }
    }
}
