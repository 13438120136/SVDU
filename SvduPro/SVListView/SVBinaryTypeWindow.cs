using System;
using System.Windows.Forms;
using SVCore;
using System.IO;

namespace SVControl
{
    public partial class SVBinaryTypeWindow : Form
    {
        SVBinary _binary;

        public SVBinaryTypeWindow(SVBinary binary)
        {
            InitializeComponent();
            _binary = binary;

            this.customGroupBox.init();
            this.customGroupBox.setEnabledText("文本");

            this.picGroupBox.init();
            this.picGroupBox.setEnabledText("图片");

            if (binary.Attrib.Type == 0)
            {
                this.textBoxTrue.Text = binary.Attrib.CustomTrueText;
                this.textBoxFalse.Text = binary.Attrib.CustomFlaseText;
                customGroupBox.setChecked(true);
            }
            else
            {
                picGroupBox.setChecked(true);

                if (_binary.Attrib.CustomTrueText != null)
                {
                    String file = Path.Combine(SVProData.IconPath, _binary.Attrib.CustomTrueText);
                    if (File.Exists(file))
                        setButtonBackGd(truePic, file);
                }

                if (_binary.Attrib.CustomFlaseText != null)
                {
                    String file = Path.Combine(SVProData.IconPath, _binary.Attrib.CustomFlaseText);
                    if (File.Exists(file))
                        setButtonBackGd(falsePic, file);
                }
            }

            this.customGroupBox.EnabledChanged += new EventHandler(customGroupBox_EnabledChanged);
            this.picGroupBox.EnabledChanged += new EventHandler(picGroupBox_EnabledChanged);
            this.truePic.Click += new EventHandler(truePic_Click);
            this.falsePic.Click += new EventHandler(falsePic_Click);
            this.exPic.Click += new EventHandler(exPic_Click);
        }

        /// <summary>
        /// 选择图片为异常的情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void exPic_Click(object sender, EventArgs e)
        {
            //SVBitmapManagerWindow window = new SVBitmapManagerWindow();
            //if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            //{
            //    String file = Path.Combine(SVProData.IconPath, window.SvBitMap.ImageFileName);

            //    _binary.Attrib.CustomExceptionText = window.SvBitMap.ImageFileName;
            //    setButtonBackGd(exPic, file);
            //}
        }

        /// <summary>
        /// 选择图片为假
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void falsePic_Click(object sender, EventArgs e)
        {
            SVBitmapManagerWindow window = new SVBitmapManagerWindow();
            if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                String file = Path.Combine(SVProData.IconPath, window.SvBitMap.ImageFileName);

                _binary.Attrib.CustomFlaseText = window.SvBitMap.ImageFileName;
                setButtonBackGd(falsePic, file);
            }
        }

        /// <summary>
        /// 选择为真时的图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void truePic_Click(object sender, EventArgs e)
        {
            SVBitmapManagerWindow window = new SVBitmapManagerWindow();
            if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                String file = Path.Combine(SVProData.IconPath, window.SvBitMap.ImageFileName);

                _binary.Attrib.CustomTrueText = window.SvBitMap.ImageFileName;
                setButtonBackGd(truePic, file);
            }
        }

        /// <summary>
        /// 从当前图元文件获取背景图片数据设置按钮背景
        /// </summary>
        /// <param Name="button">要设置的按钮对象</param>
        /// <param Name="File">当前图片文件</param>
        private void setButtonBackGd(Button button, String file)
        {
            if (!File.Exists(file))
                return;

            SVPixmapFile pixmapFile = new SVPixmapFile();
            pixmapFile.readPixmapFile(file);
            button.BackgroundImageLayout = ImageLayout.Zoom;
            button.BackgroundImage = pixmapFile.getBitmapFromData();
        }

        /// <summary>
        /// 状态切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void picGroupBox_EnabledChanged(object sender, EventArgs e)
        {
            customGroupBox.setChecked(!picGroupBox.checkEnabled());
        }

        /// <summary>
        /// 状态切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void customGroupBox_EnabledChanged(object sender, EventArgs e)
        {
            picGroupBox.setChecked(!customGroupBox.checkEnabled());
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            if (customGroupBox.Enabled)
            {
                if (String.IsNullOrWhiteSpace(this.textBoxTrue.Text))
                {
                    MessageBox.Show("未设置为真字符串");
                    return;
                }

                if (String.IsNullOrWhiteSpace(this.textBoxFalse.Text))
                {
                    MessageBox.Show("未设置为假字符串");
                    return;
                }

                _binary.Attrib.Type = 0;
                _binary.Attrib.CustomTrueText = this.textBoxTrue.Text;
                _binary.Attrib.CustomFlaseText = this.textBoxFalse.Text;
            }
            else
            {
                _binary.Attrib.Type = 1;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
