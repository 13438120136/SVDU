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
        /// 颜色
        /// </summary>
        Color normal;
        Color normalBg;
        Color overMinClr;
        Color overMaxClr;
        Color overMinBgClr;
        Color overMaxBgClr;

        SVSAnalogControl _dataControl = new SVSAnalogControl();

        /// <summary>
        /// 获取当前数据控制组件
        /// </summary>
        public Control DataControl
        {
            get { return _dataControl; }
        }

        /// <summary>
        /// 仿真模拟量
        /// </summary>
        /// <param Name="timer">定时器对象</param>
        public SVSAnalog(Timer timer)
        {
            this.IsSimulation = true;
            this.IsMoved = false;
            this._nDecimalNum = 1;

            ///定时随机一个值来显示
            timer.Tick += new EventHandler((sender, e)=>
            {
                if (_dataControl.AutoPlay.Checked)
                {
                    //根据最大和最小值进行随机
                    Int32 baseValue = random.Next(_min - 20, _max + 20);
                    double value = baseValue + random.NextDouble();
                    _dataControl.Num.Value = new Decimal(value);
                    showValue(value);
                }
            });


            _dataControl.Num.ValueChanged += new EventHandler((sender, e)=>
            {
                double value = Decimal.ToDouble(_dataControl.Num.Value);
                showValue(value);
            });
        }

        /// <summary>
        /// 解析模拟量数据并显示
        /// </summary>
        /// <param Name="bin">模拟量内存结构</param>
        public void fromBin(AnalogBin bin)
        {
            ///设置正常的背景和前景颜色
            normalBg = Color.FromArgb((Int32)bin.normalBgClr);
            normal = Color.FromArgb((Int32)bin.normalClr);
            overMinClr = Color.FromArgb((Int32)bin.overMinClr);
            overMinBgClr = Color.FromArgb((Int32)bin.overMinBgClr);
            overMaxClr = Color.FromArgb((Int32)bin.overMaxClr);
            overMaxBgClr = Color.FromArgb((Int32)bin.overMaxBgClr);

            ///设置位置
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;


            _nDecimalNum = bin.nDecimalNum;
            _min = (Int32)bin.vMin;
            _max = (Int32)bin.vMax;

            _dataControl.LabelInfo.Text = String.Format("范围: {0}-{1}", _min, _max);
            _dataControl.Num.Minimum = _min - 20;
            _dataControl.Num.Maximum = _max + 20;

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
            FontConfig.Add(8, new Font("Courier New", 8));
            FontConfig.Add(12, new Font("Courier New", 12));
            FontConfig.Add(16, new Font("Courier New", 16));
            this.Font = FontConfig[bin.font];
        }

        /// <summary>
        /// 设置并显示当前模拟量
        /// </summary>
        public void showValue(double value)
        {
            if (value < _min)
            {
                this.BackColor = overMinBgClr;
                this.ForeColor = overMinClr;
            }
            else if (value >= _min && value <= _max)
            {
                this.BackColor = normalBg;
                this.ForeColor = normal;
            }
            else
            {
                this.BackColor = overMaxBgClr;
                this.ForeColor = overMaxClr;
            }

            this.Text = value.ToString(String.Format("f{0}", _nDecimalNum));
        }
    }
}
