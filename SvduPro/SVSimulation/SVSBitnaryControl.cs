using System;
using System.Windows.Forms;

namespace SVSimulation
{
    public partial class SVSBitnaryControl : UserControl
    {
        /// <summary>
        /// 用来控制控件是否播放
        /// </summary>
        public CheckBox AutoPlay
        {
            get { return this.checkBox; }
        }

        /// <summary>
        /// 获取当前的值
        /// </summary>
        public CheckBox ValueTrue
        {
            get { return this.checkBoxTrue; }
        }

        public SVSBitnaryControl()
        {
            InitializeComponent();

            ValueTrue.CheckedChanged += new EventHandler((sender, e) =>
            {
                if (ValueTrue.Checked)
                    checkBoxTrue.Text = "真";
                else
                    checkBoxTrue.Text = "假";
            });
        }
    }
}
