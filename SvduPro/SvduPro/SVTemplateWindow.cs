using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SVCore;

namespace SvduPro
{
    public partial class SVTemplateWindow : Form
    {
        public SVTemplateWindow()
        {
            InitializeComponent();

            initListView();

            this.listView.Click += new EventHandler(listView_Click);
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
            DirectoryInfo TheFolder = new DirectoryInfo(SVProData.TemplatePath);
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                String pix = Path.GetExtension(NextFile.Name);
                if (Path.GetExtension(NextFile.Name) == ".jpg")
                    continue;

                ListViewItem item = this.listView.Items.Add(NextFile.Name);
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
