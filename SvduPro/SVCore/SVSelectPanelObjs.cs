using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace SVCore
{
    public delegate void PasteEvent(List<Control> list);
    //当控件被选中的时候，发出该信号
    public delegate void SelectControlEvents(List<Control> list);
    //当控件被复制的时候

    static public class SVSelectPanelObjs
    {
        static private HashSet<Control> copyList = new HashSet<Control>();
        static private HashSet<Control> set = new HashSet<Control>();
        static public Boolean _VK_Ctrl { get; set; }
        static public PasteEvent PasteEvent { get; set; }
        static public SelectControlEvents SelectEvents { get; set; }
        static public SelectControlEvents CopyEvents { get; set; }

        /// <summary>
        /// 将所有选中的控件进行反选
        /// </summary>
        static public void clearSelectControls()
        {
            List<Control> tmp = new List<Control>(set);
            foreach (var item in tmp)
            {
                SVPanel panel = item as SVPanel;
                if (panel == null)
                    continue;

                panel.Selected = false;
            }

            set.Clear();
        }

        /// <summary>
        /// 移动与当前控件都被选中的兄弟控件
        /// </summary>
        /// <param name="control">当前选中控件</param>
        /// <param name="x">在x方向上的偏离值</param>
        /// <param name="y">在y方向上的偏离值</param>
        static public void moveSelectControls(Control control, int x, int y)
        {
            foreach (var item in set)
            {
                if (control.Equals(item))
                    continue;

                SVPanel panel = item as SVPanel;
                if (panel == null)
                    continue;

                item.Location = new Point(item.Location.X + x, item.Location.Y + y);
                panel.setStartPos(item.Location);
            }
        }

        /// <summary>
        /// 获取在所有控件中被选中控件的个数
        /// </summary>
        /// <returns>得到选中控件的个数</returns>
        static public Int32 selectCount()
        {
            return set.Count;
        }

        /// <summary>
        /// 当控件被选中的时候执行添加操作，以便管理
        /// </summary>
        /// <param name="control">被选中的控件</param>
        static public void addControlItem(Control control)
        {
            set.Add(control);
            List<Control> ctls = new List<Control>(set);
            SelectEvents(ctls);
        }

        /// <summary>
        /// 当控件被反选的时候从全局管理中移除
        /// </summary>
        /// <param name="control">被反选的控件</param>
        static public void removeControlItem(Control control)
        {
            //如果不存在，不执行移除
            if (!set.Contains(control))
                return;

            set.Remove(control);
        }

        /// <summary>
        /// 对全局中的选中控件执行删除操作
        /// </summary>
        static public void removeOperator()
        {
            List<Control> tmp = new List<Control>();
            tmp.AddRange(set);

            ///删除控件并回收ID号
            foreach (Control ctrl in tmp)
            {
                SVPanel sv = (SVPanel)ctrl;
                sv.delID();
                sv.Selected = false;
                ctrl.Parent.Controls.Remove(ctrl);
            }

            ///清空选中状态
            set.Clear();
        }

        /// <summary>
        /// 将当前选中列表中的控件存放到复制列表中
        /// </summary>
        static public void copyOperator()
        {
            if (set.Count == 0)
                return;

            copyList.Clear();
            foreach (Control ctrl in set)
            {
                SVPanel svPanel = ctrl as SVPanel;
                copyList.Add((Control)svPanel.cloneObject());
            }
            CopyEvents(new List<Control>(set));
        }

        /// <summary>
        /// 将当前选中列表中的控件存放到复制列表中，并删除选中列表中的控件
        /// </summary>
        static public void cutOperator()
        {
            ///如果没有任何控件选中，就什么都不会执行
            if (set.Count == 0)
                return;

            ///存放到复制列表中
            copyList.Clear();
            foreach (Control ctrl in set)
            {
                SVPanel svPanel = ctrl as SVPanel;
                copyList.Add(svPanel);
            }

            //删除当前选中项
            removeOperator();
        }

        /// <summary>
        /// 将当前复制列表中的控件，放到指定的月诶安窗口中
        /// </summary>
        static public void pasteOperator()
        {
            ///一个临时列表
            List<Control> vTmp = new List<Control>();

            foreach (Control ctrl in copyList)
            {
                SVPanel svPanel = ctrl as SVPanel;
                vTmp.Add((Control)svPanel.cloneObject());
            }

            ///粘贴事件的发送
            PasteEvent(vTmp);
        }

        /// <summary>
        /// 将当前的选中控件，执行左对齐操作。
        /// 只是x坐标发生变化，y坐标保持不变
        /// </summary>
        static public void leftAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
                sortList.Add(c.Location.X);

            ///取x最小值
            int value = sortList.Min();

            ///遍历并且移动位置
            foreach (Control c in set)
            {
                c.Location = new Point(value, c.Location.Y);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        /// <summary>
        /// 将当前的选中控件，执行右对齐操作。
        /// 只是x坐标发生变化，y坐标保持不变
        /// </summary>
        static public void rightAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
                sortList.Add(c.Location.X + c.Width);

            ///取最右端的最大x值
            int value = sortList.Max();

            ///遍历并且移动位置
            foreach (Control c in set)
            {
                c.Location = new Point(value - c.Width, c.Location.Y);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        /// <summary>
        /// 将当前的选中控件，执行上对齐操作。
        /// 只是y坐标发生变化，x坐标保持不变
        /// </summary>
        static public void topAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
                sortList.Add(c.Location.Y);

            int value = sortList.Min();
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X, value);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        /// <summary>
        /// 将当前的选中控件，执行下对齐操作。
        /// 只是y坐标发生变化，x坐标保持不变
        /// </summary>
        static public void bottomAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
                sortList.Add(c.Location.Y + c.Height);

            ///取最大值
            int value = sortList.Max();
            ///遍历移动控件位置
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X, value - c.Height);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        //垂直居中
        static public void vCenterAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
            {
                sortList.Add(c.Location.X);
                sortList.Add(c.Location.X + c.Width);
            }

            int middle = (sortList.Min() + sortList.Max()) / 2;
            foreach (Control cc in set)
            {
                cc.Location = new Point(middle - cc.Width / 2, cc.Location.Y);
                SVPanel svPanel = cc as SVPanel;
                svPanel.setStartPos(cc.Location);
            }
        }

        //水平居中
        static public void hCenterAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
            {
                sortList.Add(c.Location.Y);
                sortList.Add(c.Location.Y + c.Height);
            }

            int middle = (sortList.Min() + sortList.Max()) / 2;
            foreach (Control cc in set)
            {
                cc.Location = new Point(cc.Location.X, middle - cc.Height / 2);
                SVPanel svPanel = cc as SVPanel;
                svPanel.setStartPos(cc.Location);
            }
        }

        /// <summary>
        /// 所有选中控件执行一个像素的上移操作
        /// </summary>
        static public void up()
        {
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X, c.Location.Y - 1);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        /// <summary>
        /// 所有选中控件执行一个像素的下移操作
        /// </summary>
        static public void down()
        {
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X, c.Location.Y + 1);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        /// <summary>
        /// 所有选中控件执行一个像素的左移操作
        /// </summary>
        static public void left()
        {
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X - 1, c.Location.Y);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        /// <summary>
        /// 所有选中控件执行一个像素的右移操作
        /// </summary>
        static public void right()
        {
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X + 1, c.Location.Y);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        //垂直等间距
        static public void vEqual()
        {
            if (set.Count <= 1)
                return;

            //将当前控件进行排序
            List<Control> ctlList = new List<Control>();
            ctlList.AddRange(set);
            ctlList.Sort((a, b) =>
            {
                return a.Location.Y.CompareTo(b.Location.Y);
            });

            List<int> sortList = new List<int>();
            int ctlDis = 0;
            foreach (Control c in ctlList)
            {
                sortList.Add(c.Location.Y);
                sortList.Add(c.Location.Y + c.Height);

                ctlDis += c.Height;
            }

            int distance = sortList.Max() - sortList.Min();
            int dis = distance - ctlDis;
            if (dis > 0)
                dis /= (ctlList.Count - 1);
            else
                dis = 0;

            int tmpH = sortList.Min();
            for (int i = 0; i < ctlList.Count; i++)
            {
                Control cc = ctlList[i];                
                cc.Location = new Point(cc.Location.X, tmpH);
                SVPanel svPanel = cc as SVPanel;
                svPanel.setStartPos(cc.Location);
                tmpH += cc.Height;
                tmpH += dis;
            }
        }

        //水平等间距
        static public void hEqual()
        {
            if (set.Count <= 1)
                return;

            //将当前控件进行排序
            List<Control> ctlList = new List<Control>();
            ctlList.AddRange(set);
            ctlList.Sort((a, b) =>
            {
                return a.Location.X.CompareTo(b.Location.X);
            });

            List<int> sortList = new List<int>();
            int ctlDis = 0;
            foreach (Control c in ctlList)
            {
                sortList.Add(c.Location.X);
                sortList.Add(c.Location.X + c.Width);
                ctlDis += c.Width;
            }

            int distance = sortList.Max() - sortList.Min();
            int dis = distance - ctlDis;
            if (dis > 0)
                dis /= (ctlList.Count - 1);
            else
                dis = 0;

            int tmpH = sortList.Min();
            for (int i = 0; i < ctlList.Count; i++)
            {
                Control cc = ctlList[i];
                cc.Location = new Point(tmpH, cc.Location.Y);
                SVPanel svPanel = cc as SVPanel;
                svPanel.setStartPos(cc.Location);
                tmpH += cc.Width;
                tmpH += dis;
            }
        }
    }
}
