using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSBinary : SVBinary
    {
        SVSBitnaryControl _dataControl = new SVSBitnaryControl();

        private Byte _typeByte;
        static Random random = new Random();
        Color trueColor;
        Color trueBgColor;
        Color falseColor;
        Color falseBgColor;
        List<String> customList = new List<String>();
        List<Bitmap> customImageList = new List<Bitmap>();

        /// <summary>
        /// 获取当前数据控制组件
        /// </summary>
        public Control DataControl
        {
            get { return _dataControl; }
        }

        /// <summary>
        /// 仿真开关量构造函数
        /// </summary>
        /// <param Name="timer">定时器对象</param>
        public SVSBinary(Timer timer)
        {
            this.IsSimulation = true;
            this.IsMoved = false;
            this.Text = null;

            ///定时刷新值
            timer.Tick += new EventHandler((sender, e)=>
            {
                if (_dataControl.AutoPlay.Checked)
                {
                    Int32 boolValue = random.Next(0, 2);                    
                    _dataControl.ValueTrue.Checked = boolValue == 0 ? true : false;
                    showValue(boolValue);
                }
            });

            _dataControl.ValueTrue.CheckedChanged += new EventHandler((sender, e)=>
            {
                if (!_dataControl.AutoPlay.Checked)
                {
                    Int32 value = _dataControl.ValueTrue.Checked ? 0 : 1;
                    showValue(value);
                }
            });
        }

        /// <summary>
        /// 解析并显示开关量
        /// </summary>
        /// <param Name="bin">开关量内存结构</param>
        public void fromBin(BinaryBin bin, byte[] byteBuffer)
        {
            ///位置和尺寸
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;

            ///开关量类型
            _typeByte = bin.type;

            ///显示字体
            Dictionary<Byte, Font> FontConfig = new Dictionary<Byte, Font>();
            FontConfig.Add(8, new Font("宋体", 8));
            FontConfig.Add(12, new Font("宋体", 12));
            FontConfig.Add(16, new Font("宋体", 16));
            this.Font = FontConfig[bin.font];

            ///显示内容
            if (bin.type == 0)
            {
                this.Text = Encoding.Unicode.GetString(bin.trueText);
                customList.Add(Encoding.Unicode.GetString(bin.trueText));
                customList.Add(Encoding.Unicode.GetString(bin.falseText));

                trueColor = Color.FromArgb((Int32)bin.trueClr);
                trueBgColor = Color.FromArgb((Int32)bin.trueBgClr);
                falseColor = Color.FromArgb((Int32)bin.falseClr);
                falseBgColor = Color.FromArgb((Int32)bin.falseBgClr);
            }
            else
            {
                SVPixmapFile file = new SVPixmapFile();
                customImageList.Add(file.getFromFile(byteBuffer, bin.trueClr));
                customImageList.Add(file.getFromFile(byteBuffer, bin.falseClr));
            }
        }

        /// <summary>
        /// 显示值
        /// </summary>
        public void showValue(Int32 value)
        {
            if (_typeByte == 0)
            {
                //根据最大和最小值进行随机
                this.Text = customList[value];

                if (value == 0)
                {
                    this.ForeColor = trueColor;
                    this.BackColor = trueBgColor;
                }
                else
                {
                    this.ForeColor = falseColor;
                    this.BackColor = falseBgColor;
                }
            }
            else
            {
                this.BackgroundImage = customImageList[value];
            }
        }
    }
}
