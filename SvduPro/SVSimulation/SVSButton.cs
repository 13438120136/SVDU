using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSButton : SVButton
    {
        public UInt16 GotoID { get; set; }

        public SVSButton()
        {
            ///按钮不能移动
            this.IsMoved = false;
        }

        public void fromBin(ButtonBin bin, byte[] picBuffer)
        {
            this.MouseDown += new MouseEventHandler((sender, e) =>
            {
                ///如果为0，表示以颜色显示按钮背景
                if (bin.bgUpFlag == 0 && bin.bgDownFlag == 0)
                {
                    Attrib.BackColorgroundDown = Color.FromArgb((Int32)bin.bgDownColor);
                    _paintEvent = drawButtonDown;
                    Refresh();
                }
                else
                {
                    SVPixmapFile file = new SVPixmapFile();
                    this.BackgroundImage = file.getFromFile(picBuffer, bin.bgDownColor);
                }
            });

            this.MouseUp += new MouseEventHandler((sender, e) =>
            {
                ///如果为0，表示以颜色显示按钮背景
                if (bin.bgUpFlag == 0 && bin.bgDownFlag == 0)
                {
                    Attrib.BackColorground = Color.FromArgb((Int32)bin.bgUpColor);
                    _paintEvent = drawButtonNormal;
                    Refresh();
                }
                else
                {
                    SVPixmapFile file = new SVPixmapFile();
                    this.BackgroundImage = file.getFromFile(picBuffer, bin.bgUpColor);
                }
            });


            ///如果为0，表示以颜色显示按钮背景
            if (bin.bgUpFlag == 0 && bin.bgDownFlag == 0)
            {
                Attrib.BackColorground = Color.FromArgb((Int32)bin.bgUpColor);
                _paintEvent = drawButtonNormal;
                Refresh();
            }
            else
            {
                SVPixmapFile file = new SVPixmapFile();
                this.BackgroundImage = file.getFromFile(picBuffer, bin.bgUpColor);
            }

            ///按钮位置和尺寸
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;            

            ///字体
            Dictionary<Byte, Font> FontConfig = new Dictionary<Byte, Font>();
            FontConfig.Add(16, new Font("宋体", 16));
            FontConfig.Add(24, new Font("宋体", 24));
            FontConfig.Add(32, new Font("宋体", 32));
            this.Font = FontConfig[bin.font];

            int len = 0;
            for (int i = 0; i < bin.text.Length; i += 2)
            {
                if (bin.text[i] == 0 && bin.text[i+1] == 0)
                    break;

                len += 2;
            }

            ///显示字体颜色
            this.ForeColor = Color.FromArgb((Int32)bin.fontClr);
            ///显示字体
            this.Text = System.Text.Encoding.Unicode.GetString(bin.text, 0, len); 
        }
    }
}
