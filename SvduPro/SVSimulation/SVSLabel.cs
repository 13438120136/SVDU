using System;
using System.Collections.Generic;
using System.Drawing;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSLabel : SVLabel
    {
        /// <summary>
        /// 仿真文本构造函数
        /// </summary>
        public SVSLabel()
        {
            this.IsSimulation = true;
            this.IsMoved = false;
        }

        /// <summary>
        /// 解析文本并仿真
        /// </summary>
        /// <param Name="bin">文本内存结构对象</param>
        public void fromBin(AreaBin bin)
        {
            Attrib.BackColorground = Color.FromArgb((Int32)bin.bgClr);

            this.ForeColor = Color.FromArgb((Int32)bin.fontClr);
            this.BTransparent = (bin.transparent == 1);
            if (BTransparent)
                this.BackColor = Color.Transparent;
            else
                this.BackColor = Color.FromArgb((Int32)bin.bgClr);

            Refresh();
            
            ///位置和尺寸
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;

            ///文本内容
            int len = 0;
            for (int i = 0; i < bin.text.Length; i += 2)
            {
                if (bin.text[i] == 0 && bin.text[i + 1] == 0)
                    break;

                len += 2;
            }
            this.Text = System.Text.Encoding.Unicode.GetString(bin.text, 0, len); 

            ///对齐方式
            Dictionary<Int32, ContentAlignment> dict = new Dictionary<Int32, ContentAlignment>() 
            {
                {0, ContentAlignment.TopLeft},
                {1, ContentAlignment.TopRight},
                {2, ContentAlignment.TopCenter},
                {3, ContentAlignment.MiddleCenter}
            };
            this.TextAlign = dict[bin.align];

            ///设置字体
            Dictionary<Byte, Font> FontConfig = new Dictionary<Byte, Font>()
            {
                {8, new Font("宋体", 8)},
                {12, new Font("宋体", 12)},
                {16, new Font("宋体", 16)}
            };
            this.Font = FontConfig[bin.font];
        }
    }
}
