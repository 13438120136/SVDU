using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Resources;

namespace SVCore
{
    public partial class SVBitmapManagerWindow : Form
    {
        ResourceManager res = new ResourceManager(typeof(Resource));
        SVPixmapElementManage _pixmapManage = new SVPixmapElementManage();

        SVBitmap _svBitMap = new SVBitmap();

        /// <summary>
        /// 定义当前选中的图片对象
        /// </summary>
        public SVBitmap SvBitMap
        {
            get { return _svBitMap; }
            set { _svBitMap = value; }
        }

        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public SVBitmapManagerWindow()
        {
            InitializeComponent();
            initalize();
            initClassfyContextMenu();
            initIconContextMenu();

            imageList.ImageSize = new Size(32, 32);
            listView.View = View.Details;
        }

        /// <summary>
        /// 判断当前图元管理文件是否存在
        /// 存在就加载具体的管理信息
        /// </summary>
        void loadIconInfo()
        {
            String file = Path.Combine(SVProData.IconPath, "icon.proj");
            if (!File.Exists(file))
                return;

            _pixmapManage.loadElementFromFile(file);
        }

        /// <summary>
        /// 保存当前图元管理信息到文件中
        /// </summary>
        void saveIconInfo()
        {
            String file = Path.Combine(SVProData.IconPath, "icon.proj");
            _pixmapManage.saveElementToFile(file);
        }

        void initalize()
        {
            loadIconInfo();

            ///获取信息加载到树中
            Dictionary<String, List<String>> vDict = _pixmapManage.getData();
            foreach (var item in vDict)
                treeView.Nodes.Add(item.Key);

            ///注册事件
            treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);

            listView.MouseDoubleClick += new MouseEventHandler(listView_MouseDoubleClick);
            listView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(listView_ItemSelectionChanged);
            //listView.Click += new EventHandler(listView_Click);
            listRadio.CheckedChanged += new EventHandler(listRadio_CheckedChanged);
            showRadio.CheckedChanged += new EventHandler(showRadio_CheckedChanged);
            treeView.AfterLabelEdit += new NodeLabelEditEventHandler(treeView_AfterLabelEdit);
            listView.AfterLabelEdit += new LabelEditEventHandler(listView_AfterLabelEdit);
        }

        /// <summary>
        /// 双击图片项事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView.SelectedItems[0];
            String timeFile = _pixmapManage.getFilePathFromName(item.Text);
            ///组织一个选中图元后的图片对象
            SvBitMap.ShowName = item.Text;
            SvBitMap.ImageFileName = timeFile;

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 根据当前选中节点，显示图元详细信息
        /// </summary>
        private void selectNodeShowInfo()
        {
            ListViewItem item = listView.SelectedItems[0];
            String timeFile = _pixmapManage.getFilePathFromName(item.Text);
            String file = Path.Combine(SVProData.IconPath, timeFile);

            ///组织一个选中图元后的图片对象
            SvBitMap.ShowName = item.Text;
            SvBitMap.ImageFileName = timeFile;

            //读文件数据
            SVPixmapFile pixmap = new SVPixmapFile();
            pixmap.readPixmapFile(file);
            Image srcImg = Image.FromStream(pixmap.Pixmap);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox1.Image = srcImg;

            String fileCreateTime = File.GetCreationTime(file).ToString();
            //显示信息
            String str = String.Format("显示名称: {0}\r\n\r\n文件: {1}\r\n\r\n宽度: {2}\r\n\r\n高度: {3}\r\n\r\n创建时间: {4}",
                item.Text, file, srcImg.Width, srcImg.Height, fileCreateTime);
            this.textBox1.Text = str;
        }

        /// <summary>
        /// 选中图片项事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            selectNodeShowInfo();
        }

        /// <summary>
        /// 添加图元右键菜单
        /// </summary>
        void initIconContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            MenuItem newItem = menu.MenuItems.Add(res.GetString("添加图元"));
            newItem.Click += new EventHandler(newItem_Click);

            MenuItem delItem = menu.MenuItems.Add(res.GetString("删除图元"));
            delItem.Click += new EventHandler(delItem_Click);

            MenuItem renameItem = menu.MenuItems.Add(res.GetString("重命名"));
            renameItem.Click += new EventHandler(renameItem_Click);

            listView.ContextMenu = menu;
        }

        /*
         * 重命名图标项
         * */
        void renameItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                listView.LabelEdit = true;
                item.BeginEdit();
            }
        }

        /*
         * 删除指定图标项
         * */
        void delItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                listView.Items.Remove(item);
                _pixmapManage.removeItem(treeView.SelectedNode.Text, item.Text);
                saveIconInfo();
            }
        }

        void newItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            String timeString = DateTime.Now.ToFileTime().ToString();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            ///可以选取多个文件
            openFileDialog.Multiselect = true;

            openFileDialog.Filter = "(JPG)|*.jpg|(BMP)|*.bmp|(PNG)|*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var itemName in openFileDialog.FileNames)
                    convertBitmap(itemName, timeString);
            }
        }

        //将文件转换为bmp 256色
        void convertBitmap(String fileName, String outFile)
        {
            Bitmap bitmap = new Bitmap(fileName);
            String outName = Path.GetFileNameWithoutExtension(fileName);
            Bitmap bitmapResult = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format8bppIndexed);
            String outFileName = Path.Combine(SVProData.IconPath, outName);

            MemoryStream outStream = new MemoryStream();
            bitmapResult.Save(outStream, ImageFormat.Bmp);

            //保存文件
            SVPixmapFile pixFile = new SVPixmapFile();
            pixFile.ShowName = outName;
            pixFile.Pixmap = outStream;

            imageList.Images.Add(outName, pixFile.getBitmapFromData());
            imageList1.Images.Add(outName, pixFile.getBitmapFromData());

            pixFile.writePixmapFile(outFileName);

            if (this.listRadio.Checked)
            {
                listView.View = View.Details;
                listView.SmallImageList = imageList;
                listView.LargeImageList = imageList1;
            }
            else
            {
                listView.View = View.LargeIcon;
                listView.SmallImageList = imageList1;
                listView.LargeImageList = imageList1;
            }

            ListViewItem viewItem = listView.Items.Add(outName);
            viewItem.ImageKey = outName;

            _pixmapManage.insertItemByClass(treeView.SelectedNode.Text, outName, outName);
            saveIconInfo();
        }

        void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            listView.LabelEdit = false;

            if (_pixmapManage.isItemExist(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            //执行具体的重命名
            ListViewItem tmpItem = listView.Items[e.Item];
            _pixmapManage.renameItem(treeView.SelectedNode.Text, tmpItem.Text, e.Label);
            saveIconInfo();
        }

        /*
         * 注册分类窗口中的右键事件
         * */
        void initClassfyContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            var newClassItem = menu.Items.Add(res.GetString("新建分类"));
            newClassItem.Click += new EventHandler(newClassItem_Click);

            var renameClassItem = menu.Items.Add(res.GetString("重命名"));
            renameClassItem.Click += new EventHandler(renameClassItem_Click);

            var removeClassItem = menu.Items.Add(res.GetString("移除分类"));
            removeClassItem.Click += new EventHandler(removeClassItem_Click);

            treeView.ContextMenuStrip = menu;
        }

        void removeClassItem_Click(object sender, EventArgs e)
        {
            treeView.Nodes.Remove(treeView.SelectedNode);
            _pixmapManage.removeClass(treeView.SelectedNode.Text);
            saveIconInfo();
        }

        void renameClassItem_Click(object sender, EventArgs e)
        {
            treeView.LabelEdit = true;

            var node = treeView.SelectedNode;
            node.BeginEdit();
        }

        void newClassItem_Click(object sender, EventArgs e)
        {
            int index = 1;
            String str = null;

            for (; ; )
            {
                str = String.Format("分类{0}", index);

                foreach (TreeNode item in treeView.Nodes)
                {
                    if (item.Text == str)
                    {
                        str = null;
                        break;
                    }
                }

                if (str == null)
                    index++;
                else
                    break;
            }

            var node = treeView.Nodes.Add(str);
            _pixmapManage.insertClass(str);
            saveIconInfo();
        }

        void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //获取分类名称
            String classText = treeView.SelectedNode.Text;

            listView.Items.Clear();
            textBox1.Clear();
            pictureBox1.Image = null;

            Dictionary<String, List<String>> vDict = _pixmapManage.getData();

            if (vDict.ContainsKey(classText))
            {
                foreach (var text in vDict[classText])
                {
                    //文件路径
                    String file = Path.Combine(SVProData.IconPath, _pixmapManage.getFilePathFromName(text));
                    if (!File.Exists(file))
                        return;

                    //读文件数据
                    SVPixmapFile pixmap = new SVPixmapFile();
                    pixmap.readPixmapFile(file);

                    imageList.Images.Add(text, pixmap.getBitmapFromData());
                    imageList1.Images.Add(text, pixmap.getBitmapFromData());

                    ListViewItem viewItem = listView.Items.Add(text);
                    viewItem.ImageKey = text;
                }
            }
        }

        void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            treeView.LabelEdit = false;

            Dictionary<String, List<String>> vDict = _pixmapManage.getData();

            if (e.Label != null)
            {
                if (vDict.ContainsKey(e.Label))
                {
                    e.CancelEdit = true;
                    return;
                }

                _pixmapManage.renameClass(e.Node.Text, e.Label);
                saveIconInfo();
            }
        }

        void listView_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView.SelectedItems[0];
            String timeFile = _pixmapManage.getFilePathFromName(item.Text);
            String file = Path.Combine(SVProData.IconPath, timeFile);

            ///组织一个选中图元后的图片对象
            SvBitMap.ShowName = item.Text;
            SvBitMap.ImageFileName = timeFile;

            //读文件数据
            SVPixmapFile pixmap = new SVPixmapFile();
            pixmap.readPixmapFile(file);
            Image srcImg = Image.FromStream(pixmap.Pixmap);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox1.Image = srcImg;

            String fileCreateTime = File.GetCreationTime(file).ToString();
            //显示信息
            String str = String.Format("显示名称: {0}\r\n\r\n文件: {1}\r\n\r\n宽度: {2}\r\n\r\n高度: {3}\r\n\r\n创建时间: {4}",
                item.Text, file, srcImg.Width, srcImg.Height, fileCreateTime);
            this.textBox1.Text = str;
        }

        void showRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (showRadio.Checked)
            {
                listView.View = View.LargeIcon;
                listView.SmallImageList = imageList1;
                listView.LargeImageList = imageList1;
            }
        }

        void listRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (this.listRadio.Checked)
            {
                listView.View = View.Details;
                listView.SmallImageList = imageList;
                listView.LargeImageList = imageList;
            }
        }

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
