using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// <param name="timer">定时器对象</param>
        public SVSBinary(Timer timer)
        {
            this.IsSimulation = true;
            this.IsMoved = false;

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
        /// <param name="bin">开关量内存结构</param>
        public void fromBin(BinaryBin bin)
        {
            ///位置和尺寸
            this.Location = new Point(bin.rect.sX, bin.rect.sY);
            this.Width = bin.rect.eX - bin.rect.sX;
            this.Height = bin.rect.eY - bin.rect.sY;

            ///开关量类型
            _typeByte = bin.type;

            Dictionary<Int32, String> trueConfig = new Dictionary<Int32, String>()
            {
                {0, "打开"},
                {1, "运行"},
                {2, "1"},
                {3, "是"},
                {4, "真"},
                {5, "正确"},
                {6, "开"}
            };
            this.Text = trueConfig[bin.type];

            Dictionary<Byte, Font> FontConfig = new Dictionary<Byte, Font>();
            FontConfig.Add(16, new Font("宋体", 16));
            FontConfig.Add(24, new Font("宋体", 24));
            FontConfig.Add(32, new Font("宋体", 32));
            this.Font = FontConfig[bin.font];

            this.ForeColor = Color.FromArgb((Int32)bin.trueClr);
            this.BackColor = Color.FromArgb((Int32)bin.trueBgClr);

            trueColor = Color.FromArgb((Int32)bin.trueClr);
            trueBgColor = Color.FromArgb((Int32)bin.trueBgClr);
            falseColor = Color.FromArgb((Int32)bin.falseClr);
            falseBgColor = Color.FromArgb((Int32)bin.falseBgClr);
        }

        /// <summary>
        /// 显示值
        /// </summary>
        public void showValue(Int32 value)
        {
            Dictionary<Byte, List<String>> trueConfig = new Dictionary<Byte, List<String>>()
            {
                {0, new List<String>{"打开", "关闭"}},
                {1, new List<String>{"运行", "停止"}},
                {2, new List<String>{"1", "0"}},
                {3, new List<String>{"是", "否"}},
                {4, new List<String>{"真", "假"}},
                {5, new List<String>{"正确", "错误"}},
                {6, new List<String>{"开", "关"}}
            };

            List<String> strList = trueConfig[_typeByte];

            //根据最大和最小值进行随机
            this.Text = strList[value];

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
    }
}
