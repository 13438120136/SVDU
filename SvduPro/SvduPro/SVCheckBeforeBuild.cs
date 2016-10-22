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
 * 5、控件个数是否超出最大个数值
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
            ///用来保存当前工程中已经遍历的页面和控件的ID及名称
            Dictionary<Int32, String> dict = new Dictionary<Int32, String>();

            foreach (var item in SVGlobalData.PageContainer)
            {
                SVPageWidget widget = item.Value;
                String pageText = widget.PageName;

                ///检查页面ID是否发生重复
                if (dict.ContainsKey(widget.Attrib.id))
                {
                    String msg = String.Format(dict[widget.Attrib.id] + "和页面{0}的ID重复", pageText);
                    throw new SVCheckValidException(msg);
                }

                ///保存页面ID
                dict.Add(widget.Attrib.id, String.Format("页面{0}", pageText));

                ///遍历页面中的所有子控件
                foreach (var contrl in widget.Controls)
                {
                    ///过滤不是定义的控件
                    SVPanel panel = contrl as SVPanel;
                    if (panel == null)
                        continue;

                    ///当前控件ID是否存在
                    if (dict.ContainsKey(panel.Id))
                    {
                        String msg = String.Format("{0}和页面[{1}]中的控件 ID重复, ID为{2}", dict[panel.Id], pageText, panel.Id);
                        throw new SVCheckValidException(msg);
                    }

                    String controlMsg = String.Format("页面[{0}]中的控件", pageText);
                    dict.Add(panel.Id, controlMsg);
                }
            }
        }

        //检查控件数量是否正确，并且包含检查每个控件本身
        void checkControlCount()
        {
            ///判断当前页面数是否超出最大范围
            if (SVGlobalData.PageContainer.Count > SVLimit.MAX_PAGE_NUMBER)
            {
                String msg = String.Format("最大页面数为{0},当前总页面数为{1}", SVLimit.MAX_PAGE_NUMBER, SVGlobalData.PageContainer.Count);
                throw new SVCheckValidException(msg);
            }

            ///循环判断每一页面
            foreach (var item in SVGlobalData.PageContainer)
            {
                SVPageWidget widget = item.Value;
                checkOverlapping(widget);
                checkOutOfRange(widget);

                ///所有计数都为0
                Int32 btnCount = 0;
                Int32 labelCount = 0;
                Int32 iconCount = 0;
                Int32 gifCount = 0;
                Int32 lineCount = 0;
                Int32 analogCount = 0;
                Int32 binaryCount = 0;
                Int32 trendCount = 0;
                Int32 heartCount = 0;

                //检查每个控件本身属性
                foreach (var contrl in widget.Controls)
                {
                    SVPanel panel = contrl as SVPanel;
                    if (panel == null)
                        continue;

                    ///对每一种控件进行计数
                    if (panel is SVButton)
                        btnCount++;
                    else if (panel is SVLabel)
                        labelCount++;
                    else if (panel is SVLine)
                        lineCount++;
                    else if (panel is SVIcon)
                        iconCount++;
                    else if (panel is SVGif)
                        gifCount++;
                    else if (panel is SVAnalog)
                        analogCount++;
                    else if (panel is SVBinary)
                        binaryCount++;
                    else if (panel is SVHeartbeat)
                        heartCount++;
                    else if (panel is SVCurve)
                        trendCount++;

                    ///判断当前控件个数是否合法
                    if (btnCount > SVLimit.PAGE_BTN_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的按钮个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    if (labelCount > SVLimit.PAGE_AREA_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的文本个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    if (lineCount > SVLimit.PAGE_LINE_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的线条个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    if (iconCount > SVLimit.PAGE_ICON_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的静态图个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    if (gifCount > SVLimit.PAGE_GIF_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的静态图个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    if (analogCount > SVLimit.PAGE_ANA_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的模拟量个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    if (binaryCount > SVLimit.PAGE_BOOL_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的开关量个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    if (trendCount > SVLimit.PAGE_TCHART_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的趋势图个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    if (heartCount > SVLimit.PAGE_TICKGIF_MAXNUM)
                    {
                        String msg = String.Format("页面{0}中的心跳控件个数超出最大值！", widget.PageName);
                        throw new SVCheckValidException(msg);
                    }

                    ///单个控件判断
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
                    ///如果是同一个控件，就排除
                    if (ctrl == child)
                        continue;

                    //排除直线控件的相交情况
                    if (ctrl is SVLine || child is SVLine)
                        continue;

                    SVPanel svCtrl = ctrl as SVPanel;
                    SVPanel svChild = child as SVPanel;

                    ///过滤控件类型
                    if (svCtrl == null || svChild == null)
                        continue;

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
                        String msg = String.Format("{0}页面中的,控件{0} 和 控件{1} 产生重叠", widget.PageName, svCtrl.Id, svChild.Id);
                        throw new SVCheckValidException(msg);
                    }
                }
            }
        }

        /// <summary>
        /// 检查当前页面中的元素超出了页面的显示范围
        /// </summary>
        /// <param Name="widget">页面对象</param>
        void checkOutOfRange(SVPageWidget widget)
        {
            foreach (Control ctrl in widget.Controls)
            {
                SVPanel svCtrl = ctrl as SVPanel;
                if (svCtrl == null)
                    continue;

                if (!widget.ClientRectangle.Contains(svCtrl.Bounds))
                {
                    String msg = String.Format("ID为{0}的控件超出当前页面{1}的显示范围", svCtrl.Id, widget.PageName);
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
