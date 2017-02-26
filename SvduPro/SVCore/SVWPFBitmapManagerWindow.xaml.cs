using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
            if (File.Exists(file))
                _picManager.loadElementFromFile(file);            

            _iconData = _picManager.getData();

            BindingList<String> obj = new BindingList<String>();
            foreach (var item in _iconData)
                obj.Add(item.Key);

            classlistView.ItemsSource = obj;
        }

        /// <summary>
        /// 分类节点进行选择的时候更新
        /// </summary>
        /// <param oldName="sender"></param>
        /// <param oldName="e"></param>
        private void classlistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iconRefresh();
        }

        /// <summary>
        /// 刷新当前图元子控件内容
        /// </summary>
        private void iconRefresh()
        {
            ///分类名称
            String typeName = this.classlistView.SelectedItem as String;
            if (typeName == null)
            {
                this.piclistView.ItemsSource = null;
                return;
            }

            ///名称不存在，则不显示图片
            if (!_iconData.ContainsKey(typeName))
            {
                this.piclistView.ItemsSource = null;
                return;
            }

            ///加载当前所有图片信息
            BindingList<Node> nodeList = new BindingList<Node>();
            foreach (var item in _iconData[typeName])
                nodeList.Add(createNodeFromName(item));

            ///显示
            this.piclistView.ItemsSource = nodeList; 
        }

        /// <summary>
        /// 通过名称来创建一个显示节点
        /// </summary>
        /// <param oldName="oldName"></param>
        /// <returns></returns>
        private Node createNodeFromName(String fileName)
        {
            String name = Path.GetFileNameWithoutExtension(fileName);

            Node node = new Node();
            node.Name = name;
            node.ShowName = String.Format("名称:{0}", name);

            String timeFile = _picManager.getFilePathFromName(name);
            String file = System.IO.Path.Combine(SVProData.IconPath, timeFile);
            String fileCreateTime = File.GetCreationTime(file).ToString();
            node.File = String.Format("创建时间:{0}", fileCreateTime);

            return node;
        }

        /// <summary>
        /// 添加图元到当前窗口中
        /// </summary>
        /// <param oldName="sender"></param>
        /// <param oldName="e"></param>
        private void importIcon_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "(JPG)|*.jpg|(BMP)|*.bmp|(PNG)|*.png";

            String nowTimeString = DateTime.Now.ToFileTime().ToString();

            BindingList<Node> nodeList = this.piclistView.ItemsSource as BindingList<Node>;

            ///弹出图片选择窗口
            if (openFileDialog.ShowDialog() == true)
            {
                Int32 index = 1;
                foreach (var fileName in openFileDialog.FileNames)
                {
                    ///根据路径获取名称
                    String itemName = Path.GetFileNameWithoutExtension(fileName);
                    ///如果已经存在，不执行该文件的导入
                    if (_picManager.isItemExist(itemName))
                    {
                        SVLog.WinLog.Warning(String.Format("图元名称 {0} 已经存在, 导入失败!", itemName));
                        continue;
                    }

                    ///保存图片
                    convertBitmap(fileName, nowTimeString + index.ToString());
                    index++;

                    ///界面显示
                    Node node = createNodeFromName(itemName);
                    nodeList.Add(node);
                    
                }

                ///保存图标信息到配置文件中
                saveIconInfo();
            }
        }

        /// <summary>
        /// 删除当前图片
        /// </summary>
        /// <param oldName="sender"></param>
        /// <param oldName="e"></param>
        private void delIcon_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否删除选中图元?", "提示", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            BindingList<Node> nodeList = this.piclistView.ItemsSource as BindingList<Node>;

            List<Node> tmp = new List<Node>();
            foreach (Node item in this.piclistView.SelectedItems)
                tmp.Add(item);

            foreach (Node item in tmp)
            {
                ///删除对应的文件
                String timeFile = _picManager.getFilePathFromName(item.Name);
                String file = Path.Combine(SVProData.IconPath, timeFile);
                File.Delete(file);

                ///删除列表
                nodeList.Remove(item);
                ///删除管理数据
                _picManager.removeItem(this.classlistView.SelectedValue as String, item.Name);
            }

            ///保存文件数据      
            saveIconInfo();
        }

        //将文件转换为bmp 256色
        void convertBitmap(String fileName, String outFile)
        {
            Bitmap bitmap = new Bitmap(fileName);
            Bitmap bitmapResult = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format8bppIndexed);

            String outName = Path.GetFileNameWithoutExtension(fileName);            
            String outFileName = Path.Combine(SVProData.IconPath, outFile);

            MemoryStream outStream = new MemoryStream();
            bitmapResult.Save(outStream, ImageFormat.Bmp);

            //保存文件
            SVPixmapFile pixFile = new SVPixmapFile();
            pixFile.ShowName = outName;
            pixFile.Pixmap = outStream;
            ///执行写文件过程
            pixFile.writePixmapFile(outFileName);
            _picManager.insertItemByClass(this.classlistView.SelectedValue as String, outName, outFile);
        }

        /// <summary>
        /// 保存当前图元管理信息到文件中
        /// </summary>
        void saveIconInfo()
        {
            String file = Path.Combine(SVProData.IconPath, "icon.proj");
            _picManager.saveElementToFile(file);
        }

        /// <summary>
        /// 新建图元分类
        /// </summary>
        /// <param oldName="sender"></param>
        /// <param oldName="e"></param>
        public void NewClassItem_Click(object sender, RoutedEventArgs e)
        {
            BindingList<String> list = this.classlistView.ItemsSource as BindingList<String>;

            WPFNewIconClassDialog dialog = new WPFNewIconClassDialog();
            dialog.setNameData(list);
            if (dialog.ShowDialog() == true)
            {
                String name = dialog.textBox.Text;
                _picManager.insertClass(name);
                list.Add(name);
                saveIconInfo();
            }
        }

        private void renClassItem_Click(object sender, RoutedEventArgs e)
        {
            BindingList<String> list = this.classlistView.ItemsSource as BindingList<String>;

            int index = classlistView.SelectedIndex;
            String oldName = list[index];

            SVWPFRenameIconDialog dialog = new SVWPFRenameIconDialog();
            dialog.setOldName(oldName);
            dialog.setNameData(list);
            if (dialog.ShowDialog() == true)
            {
                String newName = dialog.textBox.Text;
                _picManager.renameClass(oldName, newName);
                list[index] = newName;

                saveIconInfo();
                iconRefresh();
            }
        }

        private void classlistView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (classlistView.SelectedItems.Count == 0)
            {
                this.rename.IsEnabled = false;
                this.del.IsEnabled = false;
            }
            else
            {
                this.rename.IsEnabled = true;
                this.del.IsEnabled = true;
            }
        }

        private void delClass_Click(object sender, RoutedEventArgs e)
        {
            BindingList<String> list = this.classlistView.ItemsSource as BindingList<String>;

            String name = classlistView.SelectedValue as String;
            MessageBoxResult result = MessageBox.Show(String.Format("是否删除'{0}'分类", name), "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                ///循环删除当前图元文件
                foreach (String varName in _iconData[name])
                {
                    ///删除对应的文件
                    String timeFile = _picManager.getFilePathFromName(varName);
                    String file = Path.Combine(SVProData.IconPath, timeFile);
                    File.Delete(file);
                }

                _picManager.removeClass(name);
                list.Remove(name);

                ///保存文件数据 
                saveIconInfo();
                iconRefresh();
            }
        }

        private void piclistView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (classlistView.SelectedItems.Count == 0)
            {
                this.importIcon.IsEnabled = false;
                this.delIcon.IsEnabled = false;
            }
            else
            {
                this.importIcon.IsEnabled = true;
                this.delIcon.IsEnabled = true;
            }
        }
    }

    /// <summary>
    /// 便与显示
    /// </summary>
    class Node
    {
        public String Name { get; set; }
        public String ShowName { get; set; }
        public String File { get; set; }
    }
}
