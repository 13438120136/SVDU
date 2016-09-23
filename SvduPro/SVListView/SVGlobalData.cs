using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVControl
{
    static public class SVGlobalData
    {
        //记录当前的页面<页面名称, 页面对象>
        static Dictionary<String, SVPageWidget> _pageContainer = new Dictionary<String, SVPageWidget>();
        //记录当前的分类<页面分类, 页面列表>
        static Dictionary<String, HashSet<String>> _classContainer = new Dictionary<String, HashSet<String>>();

        static public Dictionary<String, SVPageWidget> PageContainer
        {
            get { return _pageContainer; }
        }

        static public void addClass(String className, String pageName)
        {
            if (_classContainer.ContainsKey(className))
                _classContainer[className].Add(pageName);
            else
                _classContainer.Add(className, new HashSet<String> { pageName });
        }

        static public void addPage(String pageName, SVPageWidget obj)
        {
            if (!_pageContainer.ContainsKey(pageName))
                _pageContainer.Add(pageName, obj);
        }

        static public void removePage(String pageName)
        {
            foreach (var item in _classContainer)
            {
                item.Value.Remove(pageName);
            }

            _pageContainer.Remove(pageName);
        }
    }
}
