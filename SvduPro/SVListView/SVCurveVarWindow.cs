using System;
using System.Drawing;
using System.Windows.Forms;

namespace SVControl
{
    public partial class SVCurveVarWindow : UserControl
    {
        SVCurve _curve;

        /// <summary>
        /// 趋势图构造函数
        /// </summary>
        public SVCurveVarWindow(SVCurve curve)
        {
            InitializeComponent();
            initControlEvent();

            _curve = curve;

            ///变量数组
            String[] varArray = _curve.Attrib.VarArray;

            ///设置颜色
            Color[] colorArray = _curve.Attrib.VarColorArray;
            varBtn1.BackColor = colorArray[0];
            varBtn2.BackColor = colorArray[1];
            varBtn3.BackColor = colorArray[2];
            varBtn4.BackColor = colorArray[3];

            ///设置使能
            Byte[] enabledArray = _curve.Attrib.LineEnabled;

            if (enabledArray[0] == 0)
                lineWidth1.SelectedIndex = 1;
            else
                lineWidth1.SelectedIndex = 0;

            if (enabledArray[1] == 0)
                lineWidth2.SelectedIndex = 1;
            else
                lineWidth2.SelectedIndex = 0;

            if (enabledArray[2] == 0)
                lineWidth3.SelectedIndex = 1;
            else
                lineWidth3.SelectedIndex = 0;

            if (enabledArray[3] == 0)
                lineWidth4.SelectedIndex = 1;
            else
                lineWidth4.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化事件触发函数
        /// </summary>
        void initControlEvent()
        {
            varBtn1.BackColorChanged += new EventHandler((sender, e) => 
            {
                _curve.Attrib.VarColorArray[0] = varBtn1.BackColor;
            });

            varBtn2.BackColorChanged += new EventHandler((sender, e) =>
            {
                _curve.Attrib.VarColorArray[1] = varBtn2.BackColor;
            });

            varBtn3.BackColorChanged += new EventHandler((sender, e) =>
            {
                _curve.Attrib.VarColorArray[2] = varBtn3.BackColor;
            });

            varBtn4.BackColorChanged += new EventHandler((sender, e) =>
            {
                _curve.Attrib.VarColorArray[3] = varBtn4.BackColor;
            });

            lineWidth1.SelectedIndexChanged += new EventHandler((sender, e)=>
            {
                if (lineWidth1.SelectedIndex == 0)
                    _curve.Attrib.LineEnabled[0] = 1;
                else
                    _curve.Attrib.LineEnabled[0] = 0;
            });

            lineWidth2.SelectedIndexChanged += new EventHandler((sender, e) =>
            {
                if (lineWidth2.SelectedIndex == 0)
                    _curve.Attrib.LineEnabled[1] = 1;
                else
                    _curve.Attrib.LineEnabled[1] = 0;
            });

            lineWidth3.SelectedIndexChanged += new EventHandler((sender, e) =>
            {
                if (lineWidth3.SelectedIndex == 0)
                    _curve.Attrib.LineEnabled[2] = 1;
                else
                    _curve.Attrib.LineEnabled[2] = 0;
            });

            lineWidth4.SelectedIndexChanged += new EventHandler((sender, e) =>
            {
                if (lineWidth4.SelectedIndex == 0)
                    _curve.Attrib.LineEnabled[3] = 1;
                else
                    _curve.Attrib.LineEnabled[3] = 0;
            });
        }
    }
}
