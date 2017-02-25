using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SVCore
{
    public class SVPixmapElementManage
    {
        /// <summary>
        /// 图标文件映射关系 Dictionary (名称, 文件名)
        /// </summary>
        Dictionary<String, String> _mapDict;

        /// <summary>
        /// 分类列表 Dictionary(分类名称, 列表名称)
        /// </summary>
        Dictionary<String, List<String>> _eleDict;

        public SVPixmapElementManage()
        {
            _mapDict = new Dictionary<String, String>();
            _eleDict = new Dictionary<String, List<String>>();
        }

        /// <summary>
        /// 判断当前分类节点包含的列表是否为空,
        /// 为空返回True,否则返回False
        /// </summary>
        /// <param oldName="oldName">当前分类名称</param>
        /// <returns></returns>
        public Boolean isEmptyClassfy(String name)
        {
            if (_eleDict.ContainsKey(name))
            {
                var list = _eleDict[name];
                return (list.Count == 0);
            }
            else
                return true;
        }

        /// <summary>
        /// 获取当前分类名称极其对应的所有列表元素
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, List<String>> getData()
        {
            return _eleDict;
        }

        /// <summary>
        /// 通过显示名称来获取对应的文件名
        /// </summary>
        /// <param oldName="oldName">显示名称字符串</param>
        /// <returns>文件名路径字符串</returns>
        public String getFilePathFromName(String name)
        {
            if (_mapDict.ContainsKey(name))
                return _mapDict[name];

            return null;
        }

        /// <summary>
        /// 从指定文件中加载图元管理信息
        /// </summary>
        /// <param oldName="File">图元管理文件</param>
        public void loadElementFromFile(String file)
        {
            _mapDict.Clear();
            _eleDict.Clear();

            XDocument rootNode = XDocument.Load(file);
            var result = from i in rootNode.Element("Root").Elements("Classify")
                     select new
                     {
                         Key = i.Attribute("Name").Value,
                         List = (from v in i.Elements("Item")
                                select v.Attribute("key").Value).ToList(),
                         Dict = (from v in i.Elements("Item")
                                  select new KeyValuePair<String, String>(v.Attribute("key").Value, v.Attribute("value").Value))
                     };

            foreach (var item in result)
            {
                _eleDict.Add(item.Key, item.List);

                foreach (var sItem in item.Dict)
                    _mapDict.Add(sItem.Key, sItem.Value);
            }
        }

        /// <summary>
        /// 保存当前图元管理内存信息到文件中
        /// </summary>
        /// <param oldName="File">要保存的文件名</param>
        public void saveElementToFile(String file)
        {
            XElement rootElement = new XElement("Root");
            XDocument docment = new XDocument(new XDeclaration("1.0", "gb2312", "yes"), rootElement);

            foreach (var item in _eleDict)
            {
                XElement Item = new XElement("Classify", new XAttribute("Name", item.Key));
                rootElement.Add(Item);

                var itemList = from value in item.Value
                               select new XElement("Item", new XAttribute("key", value), new XAttribute("value", _mapDict[value]));

                Item.Add(itemList);
            }

            //保存xml文件
            docment.Save(file);
        }

        //添加分类
        public void insertClass(String classfy)
        {
            if (!_eleDict.ContainsKey(classfy))
                _eleDict.Add(classfy, new List<String>());
        }

        //移除分类
        public void removeClass(String classfy)
        {
            if (_eleDict.ContainsKey(classfy))
                _eleDict.Remove(classfy);
        }

        //重新命名分类名称
        public void renameClass(String oldName, String newName)
        {
            if (oldName == newName)
                return;

            if (!_eleDict.ContainsKey(oldName))
                return;

            var value = _eleDict[oldName];
            _eleDict.Add(newName, value);
            _eleDict.Remove(oldName);
        }

        //添加指定分类的具体项
        public void insertItemByClass(String classfy, String item, String value)
        {
            if (_eleDict.ContainsKey(classfy))
            {
                _eleDict[classfy].Add(item);
            }
            else
            {
                List<String> strList = new List<String>();
                strList.Add(item);
                _eleDict.Add(classfy, strList);
            }

            _mapDict.Add(item, value);
        }

        //移除项
        public void removeItem(String classfy, String item)
        {
            if (_eleDict.ContainsKey(classfy))
                _eleDict[classfy].Remove(item);

            if (_mapDict.ContainsKey(item))
                _mapDict.Remove(item);
        }

        //重命名项
        public void renameItem(String classfy, String oldItem, String newItem)
        {
            if (_eleDict.ContainsKey(classfy))
            {
                _eleDict[classfy].Remove(oldItem);
                _eleDict[classfy].Add(newItem);
            }

            if (_mapDict.ContainsKey(oldItem))
            {
                String value = _mapDict[oldItem];
                _mapDict.Remove(oldItem);
                _mapDict.Add(newItem, value);
            }
        }

        /*
         * 判断图标项是否已经存在
         * 
         * true - 存在
         * false - 不存在
         * */
        public Boolean isItemExist(String item)
        {
            return _mapDict.ContainsKey(item);
        }
    }
}
