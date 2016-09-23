using System;
using System.Drawing;
using System.Windows.Forms;

namespace SVControl
{
    public partial class SVCurveVarWindow : UserControl
    {
        String[] _varArray;        //变量数组
        Color[] _colorArray;      //颜色数组
        Byte[] _lineWidthArray;      //线宽数组

        public String[] VarArray
        {
            get 
            {
                return _varArray; 
            }
            set 
            {
                _varArray = value; 
            }
        }

        public Color[] ColorArray
        {
            get 
            {
                _colorArray[0] = varBtn1.BackColor;
                _colorArray[1] = varBtn2.BackColor;
                _colorArray[2] = varBtn3.BackColor;
                _colorArray[3] = varBtn4.BackColor;
                return _colorArray;
            }
            set 
            {
                varBtn1.BackColor = value[0];
                varBtn2.BackColor = value[1];
                varBtn3.BackColor = value[2];
                varBtn4.BackColor = value[3];
                _colorArray = value; 
            }
        }

        public Byte[] LineWidthArray
        {
            get 
            {
                _lineWidthArray[0] = Byte.Parse(lineWidth1.Text);
                _lineWidthArray[1] = Byte.Parse(lineWidth2.Text);
                _lineWidthArray[2] = Byte.Parse(lineWidth3.Text);
                _lineWidthArray[3] = Byte.Parse(lineWidth4.Text);
                return _lineWidthArray; 
            }
            set 
            {
                lineWidth1.Text = value[0].ToString();
                lineWidth2.Text = value[1].ToString();
                lineWidth3.Text = value[2].ToString();
                lineWidth4.Text = value[3].ToString();
                _lineWidthArray = value; 
            }
        }

        public SVCurveVarWindow()
        {
            InitializeComponent();
        }
    }
}
