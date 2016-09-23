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

        static public void addControlItem(Control control)
        {
            set.Add(control);
            List<Control> ctls = new List<Control>(set);
            SelectEvents(ctls);
        }

        static public void removeControlItem(Control control)
        {
            //如果不存在，不执行移除
            if (!set.Contains(control))
                return;

            set.Remove(control);
        }

        //执行删除
        static public void removeOperator()
        {
            List<Control> tmp = new List<Control>();
            tmp.AddRange(set);

            foreach (Control ctrl in tmp)
            {
                SVPanel sv = (SVPanel)ctrl;
                sv.delID();
                sv.Selected = false;
                ctrl.Parent.Controls.Remove(ctrl);
            }
            set.Clear();
        }

        //复制
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

        //剪切
        static public void cutOperator()
        {
            if (set.Count == 0)
                return;

            copyList.Clear();
            foreach (Control ctrl in set)
            {
                SVPanel svPanel = ctrl as SVPanel;
                copyList.Add(svPanel);
            }

            //删除当前选中项
            removeOperator();
        }

        //粘贴
        static public void pasteOperator()
        {
            ///一个临时列表
            List<Control> vTmp = new List<Control>();

            foreach (Control ctrl in copyList)
            {
                SVPanel svPanel = ctrl as SVPanel;
                vTmp.Add((Control)svPanel.cloneObject());
            }

            PasteEvent(vTmp);
        }

        //控件左对齐
        static public void leftAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
                sortList.Add(c.Location.X);

            int value = sortList.Min();
            foreach (Control c in set)
            {
                c.Location = new Point(value, c.Location.Y);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        //右对齐
        static public void rightAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
                sortList.Add(c.Location.X + c.Width);

            int value = sortList.Max();
            foreach (Control c in set)
            {
                c.Location = new Point(value - c.Width, c.Location.Y);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        //上对齐
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

        //下对齐
        static public void bottomAlign()
        {
            if (set.Count == 0)
                return;

            List<int> sortList = new List<int>();
            foreach (Control c in set)
                sortList.Add(c.Location.Y + c.Height);

            int value = sortList.Max();
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

        static public void up()
        {
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X, c.Location.Y - 1);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        static public void down()
        {
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X, c.Location.Y + 1);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

        static public void left()
        {
            foreach (Control c in set)
            {
                c.Location = new Point(c.Location.X - 1, c.Location.Y);
                SVPanel svPanel = c as SVPanel;
                svPanel.setStartPos(c.Location);
            }
        }

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
                return a.Location.Y < b.Location.Y ? 0 : 1;
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
                return a.Location.X < b.Location.X ? 0 : 1;
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
