using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace SVCore
{
    /// <summary>
    /// SVWPFBitmapManagerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFBitmapManagerWindow : Window
    {
        //图片的分类及对应的图片名称列表
        Dictionary<String, List<String>> _iconData = null;
        SVPixmapElementManage _picManager = new SVPixmapElementManage();

        public SVWPFBitmapManagerWindow()
        {
            InitializeComponent();

            String file = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");
            _picManager.loadElementFromFile(file);
            _iconData = _picManager.getData();

            classlistView.ItemsSource = _iconData.Keys;
        }

        private void classlistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///分类名称
            String typeName = this.classlistView.SelectedValue as String;

            if (!_iconData.ContainsKey(typeName))
            {
                this.piclistView.ItemsSource = null;
                return;
            }
            
            List<Node> nodeList = new List<Node>();
            foreach (var item in _iconData[typeName])
            {
                Node node = new Node();
                node.Name = item;
                node.ShowName = String.Format("名称:{0}", item);

                String timeFile = _picManager.getFilePathFromName(item);
                String file = System.IO.Path.Combine(SVProData.IconPath, timeFile);                
                String fileCreateTime = File.GetCreationTime(file).ToString();
                node.File = String.Format("创建时间:{0}", fileCreateTime); 
                nodeList.Add(node);
            }

            this.piclistView.ItemsSource = nodeList;
        }
    }

    class Node
    {
        public String Name { get; set; }
        public String ShowName { get; set; }
        public String File { get; set; }
    }
}
