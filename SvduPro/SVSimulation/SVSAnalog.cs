using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSAnalog : SVAnalog
    {
        Int32 _nDecimalNum;
        Int32 _min;
        Int32 _max;
        static Random random = new Random();

        /// <summary>
        /// 仿真模拟量
        /// </summary>
        /// <param name="timer">定时器对象</param>
        public SVSAnalog(Timer timer)
        {
            this.IsMoved = false;
            this._nDecimalNum = 1;

            ///定时随机一个值来显示
            timer.Tick += new EventHandler((sender, e)=>
            {
                showValue();
            });
        }

        /// <summary>
        /// 解析模拟量数据并显示
        /// </summary>
        /// <param name="bin">模拟量内存结构</param>
        public void fromBin(AnalogBin bin)
        {
            ///设置正常的背景和前景颜色
            this.BackColor = Color.FromArgb((Int32)bin.normalBgClr);
            this.ForeColor = Color.FromArgb((Int32)bin.normalClr);

            ///设置位置
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;


            _nDecimalNum = bin.nDecimalNum;
            _min = (Int32)bin.vMin;
            _max = (Int32)bin.vMax;

            if (bin.enExponent == 1)
            {
                double num = 0.0;
                this.Text = num.ToString(String.Format("e{0}", bin.nDecimalNum), CultureInfo.InvariantCulture);
            }
            else
            {
                double num = 0.0;
                this.Text = num.ToString(String.Format("f{0}", bin.nDecimalNum));
            }

            Dictionary<Byte, Font> FontConfig = new Dictionary<Byte, Font>();
            FontConfig.Add(16, new Font("宋体", 16));
            FontConfig.Add(24, new Font("宋体", 24));
            FontConfig.Add(32, new Font("宋体", 32));
            this.Font = FontConfig[bin.font];
        }

        /// <summary>
        /// 设置并显示当前模拟量
        /// </summary>
        public void showValue()
        {
            //根据最大和最小值进行随机
            Int32 baseValue = random.Next(_min, _max - 1);
            double value = baseValue + random.NextDouble();
            this.Text = value.ToString(String.Format("f{0}", _nDecimalNum));
        }
    }
}
