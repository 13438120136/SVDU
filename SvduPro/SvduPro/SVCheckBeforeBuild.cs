/*
 * 在图形组态编译成下装文件前进行的合法性检查
 * 
 * 主要从以下几个方面:
 * 1、检查当前页面的子控件是否超出页面显示范围
 * 2、页面中的多个控件是否存在重叠情况
 * 3、所有的ID是否唯一，不重复
 * 4、页面个数是否在合法范围内
 * 5、控件个数是否在合法范围内
 * 6、当前工程是否有启动页面
 * 7、是否有系统页面
 * 
 * 针对单个控件:
 * 1、ID号是否合法
 * 2、文本内容是否为空
 * 3、变量是否存在
 * 4、显示文本字体是否合法
 * 
 * */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SVControl;
using SVCore;

namespace SvduPro
{
    public class SVCheckBeforeBuild
    {
        //导航树
        SVTreeView _treeView;

        public SVCheckBeforeBuild()
        {
            SVInterfaceApplication app = SVApplication.Instance;
            _treeView = app.TreeProject as SVTreeView;
        }

        //检查是否有启动页面
        void checkStartPage()
        {
            if (SVPageWidget.MainPageWidget == null)
                throw new SVCheckValidException("当前工程中没有设置初始启动页面");
        }

        //检查ID是否重复
        void checkIDRepeat()
        {
            Dictionary<Int32, String> dict = new Dictionary<Int32, String>();

            foreach (var item in SVGlobalData.PageContainer)
            {
                SVPageWidget widget = item.Value;
                String pageText = widget.PageName;

                if (dict.ContainsKey(widget.Attrib.id))
                {
                    String msg = String.Format(dict[widget.Attrib.id] + "和页面{0}的ID重复", pageText);
                    throw new SVCheckValidException(msg);
                }

                dict.Add(widget.Attrib.id, String.Format("页面{0}", pageText));

                foreach (var contrl in widget.Controls)
                {
                    SVPanel panel = contrl as SVPanel;
                    if (dict.ContainsKey(panel.Id))
                    {
                        String msg = String.Format("{0}和页面[{1}]中的控件 ID重复, ID为{2}", dict[panel.Id], pageText, panel.Id);
                        throw new SVCheckValidException(msg);
                    }

                    String head = String.Format("页面[{0}]中的控件", pageText);
                    dict.Add(panel.Id, head);
                }
            }
        }

        //检查控件数量是否正确，并且包含检查每个控件本身
        void checkControlCount()
        {
            if (SVGlobalData.PageContainer.Count > SVLimit.MAX_PAGE_NUMBER)
            {
                String msg = String.Format("最大页面数为{0},当前页面数为{1}", SVLimit.MAX_PAGE_NUMBER, SVGlobalData.PageContainer.Count);
                throw new SVCheckValidException(msg);
            }

            foreach (var item in SVGlobalData.PageContainer)
            {
                SVPageWidget widget = item.Value;
                checkOverlapping(widget);
                checkOutOfRange(widget);

                //检查每个控件本身属性
                foreach (var contrl in widget.Controls)
                {
                    SVPanel panel = contrl as SVPanel;
                    panel.checkValid();
                }
            }
        }

       /*
        * 检查当前页面中，多个控件之间是否有重叠的情况 
        * 如果控件之间有重叠，将抛出异常 SVCheckValidException
        **/
        void checkOverlapping(SVPageWidget widget)
        {
            foreach (Control ctrl in widget.Controls)
            {
                foreach (Control child in widget.Controls)
                {
                    if (ctrl == child)
                        continue;

                    //排除直线控件的相交情况
                    if (ctrl is SVLine || child is SVLine)
                        continue;

                    SVPanel svCtrl = ctrl as SVPanel;
                    SVPanel svChild = child as SVPanel;

                    int left1 = ctrl.Location.X;
                    int right1 = ctrl.Location.X + ctrl.Width;
                    int top1 = ctrl.Location.Y;
                    int bottom1 = ctrl.Location.Y + ctrl.Height;

                    int left2 = child.Location.X;
                    int right2 = child.Location.X + child.Width;
                    int top2 = child.Location.Y;
                    int bottom2 = child.Location.Y + child.Height;

                    if (!(right1 < left2 || left1 > right2 || bottom1 < top2 || top1 > bottom2))
                    {
                        String msg = String.Format("控件为{0} 和 控件为{1} 产生重叠", svCtrl.Id, svChild.Id);
                        throw new SVCheckValidException(msg);
                    }
                }
            }
        }

        /*
         * 检查当前页面中的元素超出了页面的显示范围
         **/
        void checkOutOfRange(SVPageWidget widget)
        {
            foreach (Control ctrl in widget.Controls)
            {
                SVPanel svCtrl = ctrl as SVPanel;

                if (!widget.ClientRectangle.Contains(svCtrl.Bounds))
                {
                    String msg = String.Format("ID为:{0}的控件超出当前页面的显示范围", svCtrl.Id);
                    throw new SVCheckValidException(msg);
                }
            }
        }

        /// <summary>
        /// 编译前检查所有项
        /// </summary>
        public void checkAll()
        {
            checkStartPage();
            checkIDRepeat();
            checkControlCount();
        }
    }
}
