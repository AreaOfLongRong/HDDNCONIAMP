using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.DB;
using System.Threading.Tasks;
using System.IO;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using HDDNCONIAMP.Utils;
using log4net;

namespace HDDNCONIAMP.UI.UserSettings
{
    /// <summary>
    /// 用户配置管理控件
    /// </summary>
    public partial class UCUserSettings : UserControl
    {

        #region 私有变量
        
        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(UCUserSettings));

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置当前用户
        /// </summary>
        public User CurrentUser { get; set; }

        #endregion

        public UCUserSettings()
        {
            InitializeComponent();
            //双缓冲设置，防止界面闪烁
            setTableLayoutPanelDoubleBufferd();
        }

        /// <summary>
        /// 界面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCUserSettings_Load(object sender, EventArgs e)
        {
            initAdvTreeLogManage();
        }

        private int currentID;

        #region 日志管理事件处理

        /// <summary>
        /// 节点双击事件，点击查看日志消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeLogList_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            Node selectNode = advTreeLogList.SelectedNode;
            if (selectNode.Level == 2)
            {
                if (!superTabControlLogs.Tabs.Contains(selectNode.Text))
                {
                    SuperTabItem logItem = new SuperTabItem();
                    SuperTabControlPanel logPanel = new SuperTabControlPanel();
                    TextBoxX logContet = new TextBoxX();

                    logItem.Name = selectNode.Text;
                    logItem.Text = selectNode.Text;
                    logItem.GlobalItem = false;
                    logItem.AttachedControl = logPanel;

                    logPanel.Dock = DockStyle.Fill;
                    logPanel.Location = new Point(0, 28);
                    logPanel.Size = new Size(300, 300);
                    logPanel.TabItem = logItem;

                    logContet.Border.Class = "TextBoxBorder";
                    logContet.Border.CornerType = eCornerType.Square;
                    logContet.Dock = DockStyle.Fill;
                    logContet.Location = new Point(0, 0);
                    logContet.Size = new Size(420, 340);
                    logContet.PreventEnterBeep = true;
                    logContet.Multiline = true;
                    logContet.ScrollBars = ScrollBars.Vertical;
                    logContet.Font = new Font("微软雅黑", 10);
                    if (selectNode.Text.Split('.')[0].Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        string tempLogFile = Path.Combine(PathUtils.LOG_TEMP_DIRECTORY, selectNode.Text + ".temp");
                        File.Copy(selectNode.Tag.ToString(), tempLogFile, true);
                        logContet.Text = FileUtils.ReadFileToString(tempLogFile);
                    }
                    else
                    {
                        logContet.Text = FileUtils.ReadFileToString(selectNode.Tag.ToString());
                    }
                    
                    logPanel.Controls.Add(logContet);

                    superTabControlLogs.Controls.Add(logPanel);
                    superTabControlLogs.Tabs.Add(logItem);
                    superTabControlLogs.SelectedTab = logItem;
                }
                else
                {
                    superTabControlLogs.SelectedTabIndex = superTabControlLogs.Tabs.IndexOf(selectNode.Text);
                }
            }
        }

        /// <summary>
        /// 展开所有节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemExpandAll_Click(object sender, EventArgs e)
        {
            advTreeLogList.ExpandAll();
        }

        /// <summary>
        /// 折叠所有节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemFoldAll_Click(object sender, EventArgs e)
        {
            advTreeLogList.CollapseAll();
        }

        /// <summary>
        /// 刷新日志节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemRefresh_Click(object sender, EventArgs e)
        {
            initAdvTreeLogManage();
        }

        /// <summary>
        /// 导出选中的日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemExportLogs_Click(object sender, EventArgs e)
        {
            if(advTreeLogList.SelectedNodes.Count > 0)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择日志文件导出的路径...";
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string exportDir = fbd.SelectedPath;
                    try
                    {
                        logger.Info("开始导出日志文件：");
                        foreach (Node item in advTreeLogList.SelectedNodes)
                        {
                            string destLog = Path.Combine(exportDir, item.Text);
                            File.Copy(item.Tag.ToString(), destLog, true);
                            logger.Info("导出日志“" + destLog + "”");
                        }
                        logger.Info("日志文件导出完毕！");
                        MessageBox.Show("成功导出日志文件到“" + exportDir + "”文件夹内！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("日志文件导出失败：", ex);
                    }
                    
                }                
            }
            else
            {
                MessageBox.Show("请选择要导出的日志!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region 密码修改事件处理

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            if (!textBoxXOriginalPassword.Text.Equals(CurrentUser.Password))
            {
                MessageBox.Show("原始密码错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clearPasswordTextBoxContent();
            }
            if (!textBoxXNewPassword.Text.Equals(textBoxXMakeSurePassword.Text))
            {
                MessageBox.Show("新密码前后输入不一致！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SQLiteHelper.GetInstance().UserModifyPassword(CurrentUser.Name, textBoxXNewPassword.Text);
                logger.Info("账户“"+CurrentUser.Name + "”密码修改成功！");
                MessageBox.Show("密码修改成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearPasswordTextBoxContent();
            }
        }

        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            clearPasswordTextBoxContent();
        }

        #endregion

        #region 权限管理事件处理


        private void buttonX1_Click(object sender, EventArgs e)
        {
            currentID = SQLiteHelper.GetInstance().UserRegister(DateTime.Now.ToString(), "123", EUserAuthority.GeneralUser.ToString());
            MessageBox.Show(currentID + "");
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            int row = SQLiteHelper.GetInstance().UserModifyPassword(currentID, "456");
            MessageBox.Show(row + "");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            int row = SQLiteHelper.GetInstance().UserDelete(currentID);
            MessageBox.Show(row + "");
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            List<User> users = SQLiteHelper.GetInstance().UserAllQuery();
        }


        #endregion

        #region 软件设置事件处理



        #endregion

        #region 私有方法

        /// <summary>
        /// 启用TableLayoutPanel双缓冲，防止界面闪烁
        /// </summary>
        private void setTableLayoutPanelDoubleBufferd()
        {
            tableLayoutPanelLogManage.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelLogManage, true, null);
            tableLayoutPanelModifyPassword.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelModifyPassword, true, null);
        }

        /// <summary>
        /// 初始化日志记录树
        /// </summary>
        private void initAdvTreeLogManage()
        {
            Task.Factory.StartNew(() =>
            {
                advTreeLogList.Nodes.Clear();
                string logRootDir = AppDomain.CurrentDomain.BaseDirectory + "Logs";
                foreach (string year in Directory.GetDirectories(logRootDir))
                {
                    Node yearNode = new Node(year.Split('\\').Last());
                    foreach (string month in Directory.GetDirectories(year))
                    {
                        Node monthNode = new Node(month.Split('\\').Last());
                        yearNode.Nodes.Add(monthNode);
                        foreach (string logFile in Directory.GetFiles(month))
                        {
                            Node logNode = new Node(logFile.Split('\\').Last());
                            logNode.Tag = logFile;
                            monthNode.Nodes.Add(logNode);
                        }
                    }
                    advTreeLogList.Nodes.Add(yearNode);
                }
            });
        }

        /// <summary>
        /// 清空密码文本框中的内容
        /// </summary>
        private void clearPasswordTextBoxContent()
        {
            textBoxXOriginalPassword.Clear();
            textBoxXNewPassword.Clear();
            textBoxXMakeSurePassword.Clear();
        }

        #endregion

    }
}
