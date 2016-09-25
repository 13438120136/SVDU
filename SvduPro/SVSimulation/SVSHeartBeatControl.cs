using System.Windows.Forms;

namespace SVSimulation
{
    public partial class SVSHeartBeatControl : UserControl
    {
        /// <summary>
        /// 用来控制控件是否播放
        /// </summary>
        public CheckBox AutoPlay
        {
            get { return this.checkBox; }
        }

        /// <summary>
        /// 选择当前图片位置
        /// </summary>
        public NumericUpDown Num
        {
            get { return this.numControl; }
        }

        public SVSHeartBeatControl()
        {
            InitializeComponent();
        }
    }
}
