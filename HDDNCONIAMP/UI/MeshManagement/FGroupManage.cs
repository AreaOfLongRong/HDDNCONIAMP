using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using log4net;

namespace HDDNCONIAMP.UI.MeshManagement
{
    public partial class FGroupManage : Form
    {

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(FGroupManage));

        /// <summary>
        /// 是否处于分组名称编辑状态
        /// </summary>
        private bool isGNEdite = false;

        private int mEditeID = -1;

        public FGroupManage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FGroupManage_Load(object sender, EventArgs e)
        {
            initListBoxGroupName();
        }

        private void initListBoxGroupName()
        {
            listBoxGroupName.BeginUpdate();
            listBoxGroupName.Items.Clear();
            listBoxGroupName.Items.AddRange(SQLiteHelper.GetInstance().MeshDeviceGroupAllQuery().ToArray());
            listBoxGroupName.EndUpdate();
        }

        private void buttonAddGroup_Click(object sender, EventArgs e)
        {
            textBoxGroupName.Text = "";
            textBoxGroupName.Enabled = true;
            isGNEdite = false;
            buttonAddOK.Text = "确定添加";
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBoxGroupName.SelectedItem != null)
            {
                DialogResult result = MessageBox.Show("确定删除该分组？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    MeshDeviceGroup mdg = (MeshDeviceGroup)listBoxGroupName.SelectedItem;
                    SQLiteHelper.GetInstance().MeshDeviceGroupDelete(listBoxGroupName.SelectedItem.ToString());
                    logger.Info("删除分组“" + textBoxGroupName.Text + "”");
                    listBoxGroupName.Items.Remove(listBoxGroupName.SelectedItem);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            
        }

        private void listBoxGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGroupName.SelectedItem != null)
            {
                MeshDeviceGroup mdg = (MeshDeviceGroup)listBoxGroupName.SelectedItem;
                textBoxGroupName.Text = mdg.ToString();
                buttonAddOK.Text = "修改名称";
                buttonAddOK.Enabled = true;
                textBoxGroupName.Enabled = true;
                isGNEdite = true;
                mEditeID = mdg.ID;
            }
        }

        private void buttonAddOK_Click(object sender, EventArgs e)
        {
            if (isGNEdite)
            {
                SQLiteHelper.GetInstance().MeshDeviceGroupUpdate(mEditeID, textBoxGroupName.Text);
                initListBoxGroupName();
                isGNEdite = false;
                buttonAddOK.Text = "确定添加";
                buttonAddOK.Enabled = false;
            }
            else
            {
                SQLiteHelper.GetInstance().MeshDeviceGroupInsert(textBoxGroupName.Text);
                logger.Info("插入新的分组“" + textBoxGroupName.Text + "”");
                listBoxGroupName.Items.Add(textBoxGroupName.Text);
                isGNEdite = false;
                buttonAddOK.Text = "确定添加";
                buttonAddOK.Enabled = false;
            }
            textBoxGroupName.Enabled = false;
        }
    }
}
