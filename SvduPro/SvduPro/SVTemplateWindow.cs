using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SVCore;
using SVControl;

namespace SvduPro
{
    /// <summary>
    /// 模板管理主窗口
    /// </summary>
    public partial class SVTemplateWindow : Form
    {
        public SVTemplateWindow()
        {
            InitializeComponent();
            initListView();
            initMenuEvent();
            
            this.listView.Click += new EventHandler(listView_Click);
            this.listView.AfterLabelEdit += new LabelEditEventHandler(listView_AfterLabelEdit);
        }

        void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            listView.LabelEdit = false;

            //如果没有修改
            if (e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }

            //如果修改后有重名
            if (listView.Items.ContainsKey(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            ///将文件句柄释放，不然会被占用。无法改名。
            this.pictureBox.Image.Dispose();
            this.pictureBox.Image = null;
            GC.Collect();

            ListViewItem item = listView.SelectedItems[0];
            String picFile = Path.Combine(SVProData.TemplatePath, item.Text + ".jpg");
            String file = Path.Combine(SVProData.TemplatePath, item.Text);
            String newPicFile = Path.Combine(SVProData.TemplatePath, e.Label + ".jpg");
            String newFile = Path.Combine(SVProData.TemplatePath, e.Label);

            try
            {
                Directory.Move(picFile, newPicFile);
                Directory.Move(file, newFile);
            }
            catch
            {
                SVLog.WinLog.Info("文件已经打开，改名失败.关闭后重试!");
            }
        }

        /// <summary>
        /// 初始化菜单事件
        /// </summary>
        private void initMenuEvent()
        {
            this.listView.MouseDown += new MouseEventHandler(listView_MouseDown);
        }

        /// <summary>
        /// 处理鼠标的右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu menu = new ContextMenu();

            var addItem = menu.MenuItems.Add(Resource.添加模板);
            addItem.Click += new EventHandler(addItem_Click);

            if (listView.SelectedItems.Count > 0)
            {
                var renameItem = menu.MenuItems.Add(Resource.重命名);
                renameItem.Click += new EventHandler(renameItem_Click);

                ///执行删除模板操作
                var delItem = menu.MenuItems.Add(Resource.删除模板);
                delItem.Click += new EventHandler(delItem_Click); 
            }

            menu.Show(listView, new Point(e.X, e.Y));
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addItem_Click(object sender, EventArgs e)
        {
            SVDockMainWindow app = SVApplication.Instance as SVDockMainWindow;
            SVTreeView treeView = app.TreeProject as SVTreeView;

            SVAddTemplateWindow win = new SVAddTemplateWindow(treeView);
            if (win.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                var item = this.listView.Items.Add(treeView.SelectedNode.Text);
                item.ImageIndex = 0;
            }

            ///将树形窗口恢复
            app.showProjectWindow();
        }

        /// <summary>
        /// 重命名模板名字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void renameItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView.SelectedItems[0];
            this.listView.LabelEdit = true;
            item.BeginEdit();
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void delItem_Click(object sender, EventArgs e)
        {
            ///将文件句柄释放，不然会被占用。无法删除文件。
            if (this.pictureBox.Image != null)
            {
                this.pictureBox.Image.Dispose();
                this.pictureBox.Image = null;
                GC.Collect();
            }

            ///获取文件路径
            ListViewItem item = listView.SelectedItems[0];
            String picFile = Path.Combine(SVProData.TemplatePath, item.Text + ".jpg");
            String file = Path.Combine(SVProData.TemplatePath, item.Text);

            try
            {
                ///把文件从磁盘移除
                File.Delete(picFile);
                File.Delete(file);
                ///列表中删除
                listView.Items.Remove(item);
            }
            catch
            {
                SVLog.WinLog.Info("文件已经打开，删除不成功");
            }
        }

        /// <summary>
        /// 单击Listview控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listView_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView.SelectedItems)
            {
                ///缩略图不存在，就不显示
                String picFile = Path.Combine(SVProData.TemplatePath, item.Text + ".jpg");
                if (!File.Exists(picFile))
                    continue;

                Image srcImg = Image.FromFile(picFile);
                this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                this.pictureBox.Image = srcImg;
            }
        }

        /// <summary>
        /// 初始化列表控件
        /// </summary>
        void initListView()
        {
            ///给列表添加图标
            listView.View = View.List;
            ImageList imgList = new ImageList();
            imgList.Images.Add(Resource.page);
            listView.SmallImageList = imgList;

            ///循环遍历目录读取模板文件
            DirectoryInfo TheFolder = new DirectoryInfo(SVProData.TemplatePath);
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                String pix = Path.GetExtension(NextFile.Name);
                if (Path.GetExtension(NextFile.Name) == ".jpg")
                    continue;

                String fileName = NextFile.Name;
                ListViewItem item = this.listView.Items.Add(fileName);
                item.ImageIndex = 0;
            }
        }

        /// <summary>
        /// 单击"确定"按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
