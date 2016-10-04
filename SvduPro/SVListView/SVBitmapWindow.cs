using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using SVCore;

namespace SVControl
{
    public partial class SVBitmapWindow : Form
    {
        class SVListViewItem : ListViewItem
        {
            public String ListFile { get; set; }
        }

        private SVBitmap _bitmap = new SVBitmap();

        public SVBitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }        

        TreeNode _classNode;
        SVPixmapElementManage _pixmapManage;
        String _iconFile;
        Boolean _isRenameType;  //如果为true-新建分类 false-重命名分类
        SVPixmapFile _pixmapFile;

        public SVBitmapWindow()
        {
            InitializeComponent();

            _pixmapFile = new SVPixmapFile();

            //建立主节点
            _classNode = classTreeView.Nodes.Add("图标分类");
            _classNode.ExpandAll();
            _classNode.Nodes.Add("默认分类");

            _pixmapManage = new SVPixmapElementManage();
            _iconFile = Path.Combine(SVProData.IconPath, "icon.proj");
            if (File.Exists(_iconFile))
            {
                _pixmapManage.loadElementFromFile(_iconFile);
                loadData();
            }            

            //导入图片
            this.importPicBtn.Click += new EventHandler(importPicBtn_Click);
            this.VisibleChanged += new EventHandler(SVBitmapWindow_VisibleChanged);
            this.classTreeView.AfterSelect += new TreeViewEventHandler(classTreeView_Click);
            this.classTreeView.AfterLabelEdit += new NodeLabelEditEventHandler(classTreeView_AfterLabelEdit);
            this.listView.SelectedIndexChanged += new EventHandler(listView_TextChanged);
        }

        void classTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            foreach (TreeNode item in _classNode.Nodes)
            {
                if (e.Label == item.Text)
                {
                    e.CancelEdit = true;
                    break;
                }
            }

            classTreeView.LabelEdit = false;

            if (_isRenameType)
                _pixmapManage.insertClass(e.Node.Text);
            else
                _pixmapManage.renameClass(e.Node.Text, e.Label);
        }

        void listView_TextChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView.SelectedItems)
            {
                String fileName = _pixmapManage.getFilePathFromName(item.Text);
                String file = Path.Combine(SVProData.IconPath, fileName);
                if (!File.Exists(file))
                    continue;

                _pixmapFile.readPixmapFile(file);

                //显示图片
                Image srcImg = Image.FromStream(_pixmapFile.Pixmap);
                this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                this.pictureBox.Image = srcImg;
                
                //显示信息
                String str = String.Format("显示名称: {0}\r\n\r\n文件: {1}\r\n\r\n宽:{2}\t高:{3}\r\n\r\n创建时间: {4}",
                    item.Text, file, srcImg.Width, srcImg.Height, DateTime.FromFileTime(Int64.Parse(fileName)).ToString());
                this.textBox1.Text = str;

                ///记录名称
                Bitmap.ImageFileName = fileName;
                Bitmap.ShowName = item.Text;
            }
        }

        void classTreeView_Click(object sender, EventArgs e)
        {
            this.listView.Clear();            
            this.pictureBox.Image = null;
            this.textBox1.Text = null;

            TreeNode currNode = this.classTreeView.SelectedNode;
            if (currNode.Level != 1)
                return;

            Dictionary<String, List<String>> vDict = _pixmapManage.getData();
            if (vDict.ContainsKey(currNode.Text))
            {
                foreach (var item in vDict[currNode.Text])
                {
                    String inFile = Path.Combine(SVProData.IconPath, _pixmapManage.getFilePathFromName(item));
                    if (!File.Exists(inFile))
                        continue;

                    //_pixmapFile.readPixmapFile(inFile);

                    SVListViewItem listItem = new SVListViewItem();
                    listItem.Text = item;
                    this.listView.Items.Add(listItem);
                }
            }
        }

        void loadData()
        {
            Dictionary<String, List<String>> vDict = _pixmapManage.getData();
            foreach (var item in vDict)
            {
                if (item.Key == "默认分类")
                    continue;
                addRightMenu(item.Key);
            }
        }

        void SVBitmapWindow_VisibleChanged(object sender, EventArgs e)
        {
            _pixmapManage.saveElementToFile(_iconFile);
        }

        void importPicBtn_Click(object sender, EventArgs e)
        {
            TreeNode currNode = this.classTreeView.SelectedNode;
            if (currNode == null)
            {
                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content(Resource.提示, Resource.请先选中分类节点);
                msgBox.Show();

                return;
            }

            if (currNode.Level != 1)
            {
                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content(Resource.提示, Resource.请先选中分类节点);
                msgBox.Show();
                return;
            }

            if (this.textBoxName.Text == String.Empty)
            {
                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content(Resource.提示, Resource.输入名称);
                msgBox.Show();

                return;
            }

            String timeString = DateTime.Now.ToFileTime().ToString();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(JPG)|*.jpg|(BMP)|*.bmp|(PNG)|*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                convertBitmap(openFileDialog.FileName, timeString);
                _pixmapManage.insertItemByClass(currNode.Text, this.textBoxName.Text, timeString);

                ListViewItem listItem = new ListViewItem();
                listItem.Text = this.textBoxName.Text;
                this.listView.Items.Add(listItem);
            }
        }

        //将文件转换为bmp 256色
        void convertBitmap(String fileName, String outFile)
        {
            Bitmap bitmap = new Bitmap(fileName);
            String outName = outFile;// Path.GetFileNameWithoutExtension(fileName);
            Bitmap bitmapResult = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format8bppIndexed);
            String outFileName = Path.Combine(SVProData.IconPath, outFile);

            MemoryStream outStream = new MemoryStream();
            bitmapResult.Save(outStream, ImageFormat.Bmp);

            _pixmapFile.ShowName = this.textBoxName.Text;
            _pixmapFile.Pixmap = outStream;
            _pixmapFile.writePixmapFile(outFileName);
        }

        private void newClassBtn_Click(object sender, EventArgs e)
        {
            classTreeView.LabelEdit = true;

            int index = 1;

            for (; ; )
            {
                String name = String.Format("新分类{0}", index);

                bool isEnd = true;
                foreach (TreeNode item in _classNode.Nodes)
                {
                    if (name == item.Text)
                    {
                        isEnd = false;
                        break;
                    }
                }

                if (isEnd)
                {
                    addRightMenu(name);
                    return;
                }

                index++;
            }                     
        }

        void addRightMenu(String name)
        {
            TreeNode node = _classNode.Nodes.Add(name);
            classTreeView.LabelEdit = true;
            node.BeginEdit();

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripItem renameItem = contextMenu.Items.Add("重命名");
            renameItem.Click += new EventHandler((sender, e) =>
            {
                _isRenameType = false;
                classTreeView.LabelEdit = true;
                node.BeginEdit();
            });

            ToolStripItem delItem = contextMenu.Items.Add("删除");
            delItem.Click += new EventHandler((sender, e) =>
            {
                String text = String.Format("确定删除分类: {0} ?", node.Text);
                if (MessageBox.Show(text, "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    _classNode.Nodes.Remove(node);
                    _pixmapManage.removeClass(node.Text);
                }
            });

            _isRenameType = true;
            node.ContextMenuStrip = contextMenu;
        }
    }
}
