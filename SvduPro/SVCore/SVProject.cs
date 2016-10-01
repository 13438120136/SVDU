/*
 * 工程管理的主类
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SVCore
{
    public class SVProject
    {
        /// <summary>
        /// 用来保存页面文件信息<类别, <名称,页面路径文件>>
        /// </summary>
        private Dictionary<String, Dictionary<String, String>> _pageDic;

        /// <summary>
        /// 操作mysql数据库对象
        /// </summary>
        private SVSqlDataBase _sqlDataBase;

        public SVProject() : base()
        {
            _pageDic = new Dictionary<String, Dictionary<String, String>>();
            _sqlDataBase = new SVSqlDataBase();
        }

        //通过linq特性，从xml文件中读取页面相关信息
        void linqFromFile(String file)
        {
            _pageDic.Clear();

            XDocument rootNode = XDocument.Load(file);
            var result = from i in rootNode.Element("Root").Elements("Classify")
                         select new
                         {
                             Key = i.Attribute("Text").Value,
                             Value = (from v in i.Elements("Node") select v)
                         };

            var otherResult = from v in result
                     select new
                     {
                         Key = v.Key,
                         Value = from x in v.Value
                                 select new
                                 {
                                     Key = x.Attribute("Name").Value,
                                     Value = x.Attribute("File").Value
                                 }
                     };

            foreach (var onePair in otherResult)
            {
                Dictionary<String, String> xTmp = new Dictionary<String, String>();
                foreach (var item in onePair.Value)
                {
                    xTmp.Add(item.Key, item.Value);
                }
                _pageDic.Add(onePair.Key, xTmp);
            }
        }

        //通过linq特性，保存页面的相关信息
        void linqSaveFile(String file)
        {
            if (file == null)
                return;

            XElement rootElement = new XElement("Root");
            XDocument docment = new XDocument(new XDeclaration("1.0", "gb2312", "yes"), rootElement);

            foreach (var item in _pageDic)
            {
                XElement Item = new XElement("Classify", new XAttribute("Text", item.Key));
                rootElement.Add(Item);

                var itemList = from value in item.Value
                               select new XElement("Node", new XAttribute("Name", value.Key), new XAttribute("File", value.Value));

                Item.Add(itemList);
            }

            rootElement.Save(file);
        }

        //通过路径和工程名称来创建一个工程
        public void createProject(String path, String name)
        {
            //根据名称创建工程目录           
            String vPath = Path.Combine(path, name);
            String fileName = Path.Combine(vPath, name + ".svduproj");
            Directory.CreateDirectory(vPath);

            //创建模板目录
            String vTempPath = Path.Combine(vPath, "Template");
            Directory.CreateDirectory(vTempPath);

            //创建图标管理目录
            String vIconPath = Path.Combine(vPath, "Icon");
            Directory.CreateDirectory(vIconPath);

            //创建日志目录
            String vLogPath = Path.Combine(vPath, "Log");
            Directory.CreateDirectory(vLogPath);

            //写工程xml文件
            linqSaveFile(fileName);

            SVProData.ProName = name;
            SVProData.ProPath = vPath;
            SVProData.FullProPath = fileName;
            SVProData.TemplatePath = vTempPath;
            SVProData.IconPath = vIconPath;
            SVProData.LogPath = vLogPath;

            //配置文件
            SVUniqueID idObj = SVUniqueID.instance();
            idObj.UniqueFile = Path.Combine(vPath, "config");
            idObj.initUniqueID();
            idObj.saveFile();

            //打开工程
            openProject(path, name);
        }

        //通过文件名来打开一个工程
        public Boolean openProject(String path, String name)
        {
            String vPath = Path.Combine(path, name);
            String fileName = Path.Combine(vPath, name + ".svduproj");
            if (!File.Exists(fileName))
                return false;

            //获取绝对路径
            vPath = Path.GetFullPath(vPath);
            fileName = Path.GetFullPath(fileName);

            SVProData.FullProPath = fileName;
            SVProData.ProPath = vPath;
            SVProData.ProName = Path.GetFileNameWithoutExtension(fileName);
            SVProData.TemplatePath = Path.Combine(vPath, "Template");
            SVProData.IconPath = Path.Combine(vPath, "Icon");
            SVProData.LogPath = Path.Combine(vPath, "Log");

            SVUniqueID idObj = SVUniqueID.instance();
            idObj.UniqueFile = Path.Combine(SVProData.ProPath, "config");
            idObj.loadFile();

            ///初始化日志
            SVTextTraceListenter textListener = new SVTextTraceListenter(SVProData.LogPath);
            SVLog.TextLog.Listeners().Add(textListener);
            SVLog.WinLog.Listeners().Add(textListener);

            _sqlDataBase.connect("192.168.2.103", "safeware", "lixiaolong", "123456");
            //SVProData.DownLoadFile = _sqlDataBase.getProPath();
            ///先不从数据库中去下装文件生成的路径
            SVProData.DownLoadFile = Path.Combine(SVProData.ProPath, "Download");
            if (String.IsNullOrEmpty(SVProData.DownLoadFile))
                SVLog.WinLog.Warning("获取工程路径出错");

            //从工程文件中加载信息
            linqFromFile(fileName);

            SVConfig.instance().addRectFilesItem(fileName);

            return true;
        }

        //关闭工程
        public void closeProject()
        {
            _sqlDataBase.Close();
            saveProject();
        }

        //保存工程
        public void saveProject()
        {
            SVUniqueID idObj = SVUniqueID.instance();
            idObj.saveFile();

            //写入xml文件
            linqSaveFile(SVProData.FullProPath);
        }

        /// <summary>
        /// 通过页面名称来获取对应的页面相对路径
        /// 
        /// 返回 相对路径, 后缀为.page
        /// </summary>
        /// <param name="name">当前页面名称</param>
        /// <returns>页面相对路径</returns>
        public String name2Path(String name)
        {
            //得到页面保存文件的实际路径
            String tmpPath = Path.Combine(".", SVProData.ProName);
            String file = Path.Combine(tmpPath, name + ".page");

            return file;
        }

        /// <summary>
        /// 移除分类页面
        /// </summary>
        /// <param name="className">页面名称</param>
        public void removeClassName(String className)
        {
            if (_pageDic.ContainsKey(className))
                _pageDic.Remove(className);
        }

        /// <summary>
        /// 添加页面节点
        /// 
        /// className - 页面分类
        /// name-页面节点名称
        /// 返回值: true - 添加页面节点成功, false - 失败，有重名页面名称
        /// </summary>
        /// <param name="className">页面分类名称</param>
        /// <param name="name">页面节点名称</param>
        /// <returns>true或者false</returns>
        public Boolean addPageNode(String className, String name)
        {
            //判断该页面名称是否存在
            foreach (var parent in _pageDic)
                if (parent.Value.ContainsKey(name))
                    return false;

            String file = name2Path(name);
            //执行保存
            if (_pageDic.ContainsKey(className))
                _pageDic[className].Add(name, file);
            else
                _pageDic.Add(className, new Dictionary<String, String> { { name, file } });

            return true;
        }

        /// <summary>
        /// 对页面分类进行重新命名
        /// </summary>
        /// <param name="oldName">旧的分类名称</param>
        /// <param name="newName">新的分类名称</param>
        public void renamePageClassName(String oldName, String newName)
        {
            ///如果名称没有发生改变
            if (oldName == newName)
                return;

            ///执行具体的页面分类重命名
            if (_pageDic.ContainsKey(oldName))
            {
                var value = _pageDic[oldName];
                _pageDic.Remove(oldName);
                _pageDic.Add(newName, value);
            }
        }

        /// <summary>
        /// 对页面进行重名名
        /// </summary>
        /// <param name="className">分类名称</param>
        /// <param name="oldName">页面旧名称</param>
        /// <param name="newName">页面新名称</param>
        public void renamePageName(String className, String oldName, String newName)
        {
            ///如果名称没有发生改变
            if (oldName == newName)
                return;

            ///修改名称
            if (_pageDic.ContainsKey(className))
            {
                var value = _pageDic[className];
                if (value.ContainsKey(oldName))
                {
                    var pageValue = value[oldName];
                    value.Remove(oldName);
                    value.Add(newName, pageValue);
                }
            }
        }

        /**
         * 移除页面节点
         * name - 页面名称
         **/
        public void removePageNode(String name)
        {
            foreach (var item in _pageDic)
            {
                if (item.Value.ContainsKey(name))
                    item.Value.Remove(name);
            }
        }

        /*
         * 删除对应的页面文件
         * 输入参数：页面名称
         */
        public void removePageNodeFile(String name)
        {
            String file = pageFileFromName(name);
            if (file == null)
                return;

            if (!File.Exists(file))
                return;

            //删除文件
            File.Delete(file);
        }

        /**
         * 根据页面名称来获取对应的页面对象
         * 
         * 如果不存在返回 null
         **/
        public String pageFileFromName(String pageName)
        {
            foreach (var item in _pageDic)
            {
                if (item.Value.ContainsKey(pageName))
                    return item.Value[pageName];
            }

            return null;
        }

        /*
         * 根据页面名称来判断当前是否存在该页面对象
         * */
        public Boolean isExist(String pageName)
        {
            foreach (var item in _pageDic)
            {
                if (item.Value.ContainsKey(pageName))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 返回页面名称及页面文件的对应表
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Dictionary<String, String>> getDictData()
        {
            return _pageDic;
        }

        /// <summary>
        /// 返回当前数据库操作连接对象
        /// </summary>
        /// <returns>数据库对象</returns>
        public SVSqlDataBase sqlDataBase()
        {
            return _sqlDataBase;
        }
    }
}
