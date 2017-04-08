using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SVCore;

namespace SvduPro
{
    public delegate void CompeleteEventHandler(String o, String n);
    public partial class SVReplaceVarWindow : Form
    {        
        public CompeleteEventHandler CompeleteEventHandler;
        List<SVVarDefine> _list;

        public SVReplaceVarWindow()
        {
            InitializeComponent();

            originVarCombobox.SelectedIndexChanged += new EventHandler(originVarCombobox_SelectedIndexChanged);
        }

        void originVarCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            resultCombobox.Items.Clear();
            int index = originVarCombobox.SelectedIndex;

            if (String.IsNullOrWhiteSpace(_list[index].VarName))
                return;

            var varInstance = SVVaribleType.instance();
            String type = varInstance.strToTypeString(_list[index].VarName, _list[index].VarBlockType);
            var nameList = varInstance.varFromStringType(type);
            resultCombobox.Items.AddRange(nameList.ToArray());
        }

        public void setVarList(List<SVVarDefine> list)
        {
            _list = list;

            foreach (var item in list)
                originVarCombobox.Items.Add(item.VarName);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            String oldName = _list[originVarCombobox.SelectedIndex].VarName;
            String newName = resultCombobox.SelectedItem as String;
            if (String.IsNullOrWhiteSpace(oldName) || String.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("名称不能为空!");
                return;
            }

            if (CompeleteEventHandler != null)
                CompeleteEventHandler(oldName, newName);
            DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
