//
// 创建页面对话框
//

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace SVCore
{
    public partial class SVCreatePageForm : Form
    {
        //模板路径
        public String TemplatePath { get; set; }
        //模板名称
        public String TemplateName { get; set; }
        //页面名称
        public String PageName { get; set; }
        //是否为模板
        public Boolean IsTempate { get; set; }

        public SVCreatePageForm()
        {
            InitializeComponent();

            //设置模板名称最大长度为32个字符
            this.textBox.MaxLength = 32;
            TemplatePath = SVProData.TemplatePath;

            this.Shown += new EventHandler((sender, e) =>
            {
                ImageList imgList = new ImageList();
                //imgList.Images.Add(Resource.home);
                listView.SmallImageList = imgList;    

                DirectoryInfo TheFolder = new DirectoryInfo(TemplatePath);
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    String pix = Path.GetExtension(NextFile.Name);
                    if (Path.GetExtension(NextFile.Name) == ".jpg")
                        continue;

                    ListViewItem item = this.listView.Items.Add(NextFile.Name);
                    item.ImageIndex = 0;
                }
            });
            
            //this.textBox.KeyPress += new KeyPressEventHandler((sender, e)=>
            //{
            //    if (e.KeyChar == '\b')
            //        return;

            //    String str = this.textBox.Text + e.KeyChar;
            //    if (!Regex.IsMatch(str, "^[_a-zA-Z][_a-zA-Z0-9]*$"))
            //        e.Handled = true;
            //    else
            //        e.Handled = false;
            //});

            this.okBtn.Click += new EventHandler((sender, e) =>
            {
                PageName = this.textBox.Text;
                if (PageName == String.Empty)
                {
                    SVMessageBox msgBox = new SVMessageBox();
                    msgBox.content(" ", Resource.页面名称为空);
                    msgBox.Show();
                    return;
                }
                
                foreach (ListViewItem item in this.listView.SelectedItems) 
                {
                    TemplateName = item.Text;
                }

                if (IsTempate && TemplateName == null)
                {
                    SVMessageBox msgBox = new SVMessageBox();
                    msgBox.content(" ", Resource.需要选择模板);
                    msgBox.Show();
                    return;
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            });

            this.cancelBtn.Click += new EventHandler((sender, e) =>
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            });

            this.tempRadioBtn.CheckedChanged += new EventHandler((sender, e) =>
            {
                this.listView.Enabled = true;
                this.pictureBox.Show();
                IsTempate = true;
            });

            this.spaceRadioBtn.CheckedChanged += new EventHandler((sender, e) =>
            {
                this.listView.Enabled = false;
                this.pictureBox.Hide();
                IsTempate = false;
            });

            this.listView.SelectedIndexChanged += new EventHandler((sender, e) =>
            {
                foreach (ListViewItem item in this.listView.SelectedItems)
                {
                    String picFile = Path.Combine(TemplatePath, item.Text + ".jpg");
                    if (!File.Exists(picFile))
                        continue;

                    Image srcImg = Image.FromFile(picFile);
                    this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    this.pictureBox.Image = srcImg;

                    //Bitmap img = new Bitmap(this.pictureBox.Width, this.pictureBox.Height);
                    //img.SetResolution(img.HorizontalResolution, img.VerticalResolution);                    

                    //Graphics grPhoto = Graphics.FromImage(img);
                    //grPhoto.CompositingMode = CompositingMode.SourceCopy;
                    //grPhoto.CompositingQuality = CompositingQuality.HighQuality;
                    //grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //grPhoto.SmoothingMode = SmoothingMode.HighQuality;
                    //grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    //grPhoto.DrawImage(srcImg, new Rectangle(0, 0, pictureBox.Width, pictureBox.Height), new Rectangle(0, 0, srcImg.Width, srcImg.Height), GraphicsUnit.Pixel);
                }
            });
        }
    }
}
