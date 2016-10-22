using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

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
            varLabel1.Text = varArray[0];
            varLabel2.Text = varArray[1];
            varLabel3.Text = varArray[2];
            varLabel4.Text = varArray[3];

            ///设置颜色
            Color[] colorArray = _curve.Attrib.VarColorArray;
            varBtn1.BackColor = colorArray[0];
            varBtn2.BackColor = colorArray[1];
            varBtn3.BackColor = colorArray[2];
            varBtn4.BackColor = colorArray[3];

            ///设置使能
            Byte[] enabledArray = _curve.Attrib.LineEnabled;
            lineWidth1.SelectedIndex = Convert.ToInt32(enabledArray[0]);
            lineWidth2.SelectedIndex = Convert.ToInt32(enabledArray[1]);
            lineWidth3.SelectedIndex = Convert.ToInt32(enabledArray[2]);
            lineWidth4.SelectedIndex = Convert.ToInt32(enabledArray[3]);
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
                _curve.Attrib.LineEnabled[0] = Convert.ToByte(lineWidth1.SelectedIndex);
            });

            lineWidth2.SelectedIndexChanged += new EventHandler((sender, e) =>
            {
                _curve.Attrib.LineEnabled[1] = Convert.ToByte(lineWidth2.SelectedIndex);
            });

            lineWidth3.SelectedIndexChanged += new EventHandler((sender, e) =>
            {
                _curve.Attrib.LineEnabled[2] = Convert.ToByte(lineWidth3.SelectedIndex);
            });

            lineWidth4.SelectedIndexChanged += new EventHandler((sender, e) =>
            {
                _curve.Attrib.LineEnabled[3] = Convert.ToByte(lineWidth4.SelectedIndex);
            });

            this.btn1.Click += new EventHandler((sender, e)=>
            {
                SVVarWindow window = new SVVarWindow();
                window.setFilter(new List<String> { "SHORT_INT", "SHORTINT_VAR", "INT", "INT_VAR", "REAL", "REAL_VAR" });
                if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    this.varLabel1.Text = window.varText();                    
                    _curve.Attrib.VarArray[0] = this.varLabel1.Text;
                    _curve.Attrib.VarArrayType[0] = window.getVarType();
                }
            });

            this.btn2.Click += new EventHandler((sender, e) =>
            {
                SVVarWindow window = new SVVarWindow();
                window.setFilter(new List<String> { "SHORT_INT", "SHORTINT_VAR", "INT", "INT_VAR", "REAL", "REAL_VAR" });
                if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    this.varLabel2.Text = window.varText();
                    _curve.Attrib.VarArray[1] = this.varLabel2.Text;
                    _curve.Attrib.VarArrayType[1] = window.getVarType();
                }
            });

            this.btn3.Click += new EventHandler((sender, e) =>
            {
                SVVarWindow window = new SVVarWindow();
                window.setFilter(new List<String> { "SHORT_INT", "SHORTINT_VAR", "INT", "INT_VAR", "REAL", "REAL_VAR" });
                if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    this.varLabel3.Text = window.varText();
                    _curve.Attrib.VarArray[2] = this.varLabel3.Text;
                    _curve.Attrib.VarArrayType[2] = window.getVarType();
                }
            });

            this.btn4.Click += new EventHandler((sender, e) =>
            {
                SVVarWindow window = new SVVarWindow();
                window.setFilter(new List<String> { "SHORT_INT", "SHORTINT_VAR", "INT", "INT_VAR", "REAL", "REAL_VAR" });
                if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    this.varLabel4.Text = window.varText();
                    _curve.Attrib.VarArray[3] = this.varLabel4.Text;
                    _curve.Attrib.VarArrayType[3] = window.getVarType();
                }
            });
        }
    }
}
