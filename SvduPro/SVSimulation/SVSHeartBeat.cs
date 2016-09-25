using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SVControl;
using SVCore;

namespace SVSimulation
{
    public class SVSHeartBeat : SVHeartbeat
    {
        SVSHeartBeatControl _dataControl = new SVSHeartBeatControl();
        Timer _timer;

        /// <summary>
        /// 获取当前数据控制组件
        /// </summary>
        public Control DataControl
        {
            get { return _dataControl; }
        }

        /// <summary>
        /// 仿真心跳控件
        /// 构造函数
        /// </summary>
        /// <param name="timer">定时器对象，用来定时播放图片</param>
        public SVSHeartBeat(Timer timer)
        {
            _timer = timer;
            this.IsSimulation = true;
            this.IsMoved = false;
        }

        /// <summary>
        /// 从二进制数据中解析心跳控件数据
        /// 并在界面上设置响应的值和事件
        /// </summary>
        /// <param name="tickBin">心跳控件内存数据</param>
        /// <param name="picBuffer">图片数据</param>
        internal void fromBin(TickBin tickBin, byte[] picBuffer)
        {
            ///设置控件位置和大小
            this.Location = new Point(tickBin.rect.sX, tickBin.rect.sY);
            this.Width = tickBin.rect.eX - tickBin.rect.sX;
            this.Height = tickBin.rect.eY - tickBin.rect.sY;

            ///保存当前心跳控件中的所有图片到一个队列中
            Int32 count = tickBin.count;
            List<Bitmap> bitmapList = new List<Bitmap>();
            for (int i = 0; i < count; i++)
            {
                SVPixmapFile file = new SVPixmapFile();
                Bitmap bitmap = file.getFromFile(picBuffer, tickBin.imageOffsetList[i]);
                bitmapList.Add(bitmap);
            }

            ///根据用户输入来显示对应图片
            _dataControl.Num.Minimum = 1;
            _dataControl.Num.Maximum = count;
            _dataControl.Num.Increment = 1;
            _dataControl.Num.ValueChanged += new EventHandler((sender, e) => 
            {
                Int32 nIndex = (Int32)_dataControl.Num.Value - 1;
                this.BackgroundImage = bitmapList[nIndex];
            });

            ///按照定时器的时间循环播放图片
            Int32 index = 0;
            _timer.Tick += new EventHandler((sender, e)=>
            {
                if (!_dataControl.AutoPlay.Checked)
                    return;

                this.BackgroundImage = bitmapList[index];

                index++;
                _dataControl.Num.Value = index;

                if (index == count)
                    index = 0;
            });
        }
    }
}
