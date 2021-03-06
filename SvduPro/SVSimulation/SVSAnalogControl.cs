﻿using System.Windows.Forms;

namespace SVSimulation
{
    public partial class SVSAnalogControl : UserControl
    {
        /// <summary>
        /// 用来控制控件是否播放
        /// </summary>
        public CheckBox AutoPlay
        {
            get { return this.checkBox; }
        }

        public NumericUpDown Num
        {
            get { return this.numControl; }
        }

        public Label LabelInfo
        {
            get { return this.Info; }
        }

        public SVSAnalogControl()
        {
            InitializeComponent();
        }
    }
}
