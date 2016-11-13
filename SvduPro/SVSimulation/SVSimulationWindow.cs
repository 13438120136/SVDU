using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SVCore;

namespace SVSimulation
{
    public partial class SVSimulationWindow : Form
    {
        private Dictionary<UInt16, SVSPage> pageDict = new Dictionary<UInt16, SVSPage>();
        private Timer _timer = new Timer();
        byte[] picBuffer = null;

        /// <summary>
        /// 仿真窗口构造函数
        /// </summary>
        public SVSimulationWindow()
        {
            InitializeComponent();

            _timer.Interval = 200;
            _timer.Start();
        }
        
        /// <summary>
        /// 从下装文件中加载数据
        /// </summary>
        /// <param Name="file">工程生成的下装文件</param>
        public void load(String file)
        {
            //从下装文件中读取实际数据
            SVBuildFile buildFile = new SVBuildFile(file);
            ///执行具体的读取过程
            buildFile.read();
            ///获取除去头部字节的实际数据
            byte[] fileBuffer = buildFile.getDataByteArray();

            PageArrayBin result = new PageArrayBin();
            SVBinData.byteArrayToStruct(fileBuffer, ref result);

            picBuffer = new byte[buildFile.getImageLength()];
            Array.Copy(fileBuffer, buildFile.getIamgePos() - 64, picBuffer, 0, buildFile.getImageLength());

            ///循环处理每一个页面
            for (int i = 0; i < result.pageCount; i++)
            {
                if (i == 0)
                    fromPageBin(result.pageArray[i], true);
                else
                    fromPageBin(result.pageArray[i], false);
            }
        }

        /// <summary>
        /// 解析数据将布局当前所有控件
        /// </summary>
        /// <param Name="bin">二进制数据</param>
        /// <param Name="isFirst">是否为其实页面 true表示起始页面</param>
        void fromPageBin(PageBin bin, Boolean isFirst)
        {
            SVSPage page = new SVSPage();
            page.fromBin(bin);

            ///标题栏高度
            Int32 titleHeight = this.Height - this.ClientRectangle.Height;
            ///设置窗口起始位置和宽高尺寸
            this.Size = new Size(page.Attrib.Width + 200, page.Attrib.Height + titleHeight);
            page.Size = this.Size;
            ///设置控件显示尺寸
            mainPanel.Location = new Point(0, 0);
            mainPanel.Size = new Size(page.Attrib.Width, page.Attrib.Height);
            ///设置数据显示
            dataPanel.Location = new Point(mainPanel.Location.X + mainPanel.Width, mainPanel.Location.Y);
            dataPanel.Size = new Size(200, mainPanel.Height);

            page.Paint += new PaintEventHandler((sender, e) =>
            {
                for (int i = 0; i < bin.lineNum; i++)
                {
                    LineBin lineBin = bin.m_line[i];
                    Graphics gh = e.Graphics;

                    Color lineColor = Color.FromArgb((Int32)lineBin.color);
                    SolidBrush brush = new SolidBrush(lineColor);
                    Pen pen = new Pen(brush, lineBin.width);
                    gh.DrawLine(pen, lineBin.x1, lineBin.y1, lineBin.x2, lineBin.y2);
                }
            });

            //添加ID和页面的映射关系
            pageDict.Add(page.ID, page);

            for (int i = 0; i < bin.btnNum; i++)
            {
                SVSButton button = new SVSButton();
                button.fromBin(bin.m_btn[i], picBuffer);
                page.Controls.Add(button);

                ButtonBin bbb = bin.m_btn[i];
                button.Click += new EventHandler((sender, e) =>
                {
                    UInt16 id = (UInt16)(bbb.param.pageId);
                    if (pageDict.ContainsKey(id))
                    {
                        SVSPage vPage = pageDict[id];
                        this.mainPanel.Controls.Clear();
                        this.mainPanel.Controls.Add(vPage);
                    }
                });
            }

            ///添加所有的模拟量控件
            for (int i = 0; i < bin.analog_num; i++)
            {
                SVSAnalog analog = new SVSAnalog(_timer);
                analog.fromBin(bin.m_analog[i]);
                page.Controls.Add(analog);

                analog.MouseDown += new MouseEventHandler((sender, e) =>
                {
                    this.dataPanel.Controls.Clear();
                    this.dataPanel.Controls.Add(analog.DataControl);
                });
            }

            ///添加所有开关量控件
            for (int i = 0; i < bin.binaryNum; i++)
            {
                SVSBinary binary = new SVSBinary(_timer);
                binary.fromBin(bin.m_binary[i], picBuffer);
                page.Controls.Add(binary);

                binary.MouseDown += new MouseEventHandler((sender, e) =>
                {
                    this.dataPanel.Controls.Clear();
                    this.dataPanel.Controls.Add(binary.DataControl);
                });
            }

            for (int i = 0; i < bin.trendChartNum; i++)
            {
                SVSCurve curve = new SVSCurve();
                curve.fromBin(bin.m_trendChart[i]);
                page.Controls.Add(curve);
            }

            for (int i = 0; i < bin.gif_num; i++)
            {
                SVSGif gif = new SVSGif(_timer);
                gif.fromBin(bin.m_gif[i], picBuffer);
                page.Controls.Add(gif);
            }


            for (int i = 0; i < bin.iconNum; i++)
            {
                SVSIcon icon = new SVSIcon();
                icon.fromBin(bin.m_icon[i], picBuffer);
                page.Controls.Add(icon);
            }

            for (int i = 0; i < bin.areaNum; i++)
            {
                SVSLabel label = new SVSLabel();
                label.fromBin(bin.m_area[i]);
                page.Controls.Add(label);
            }

            for (int i = 0; i < bin.tickNum; i++)
            {
                SVSHeartBeat heart = new SVSHeartBeat(_timer);
                heart.fromBin(bin.m_tick[i], picBuffer);
                page.Controls.Add(heart);

                heart.MouseDown += new MouseEventHandler((sender, e) => 
                {
                    this.dataPanel.Controls.Clear();
                    this.dataPanel.Controls.Add(heart.DataControl);
                });
            }

            if (isFirst)
            {
                this.mainPanel.Controls.Clear();
                this.mainPanel.Controls.Add(page);
            }
        }
    }
}
