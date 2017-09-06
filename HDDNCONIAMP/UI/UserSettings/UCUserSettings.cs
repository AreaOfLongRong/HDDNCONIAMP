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
using System.Diagnostics;
using System.Net.NetworkInformation;

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

        /// <summary>
        /// 网卡接口数组
        /// </summary>
        private NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

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

            //设置“权限管理”标签页的可见性
            if (CurrentUser.Authority == EUserAuthority.GeneralUser.ToString())
            {
                //普通用户，隐藏“权限管理”标签页
                superTabItemAuthorityManage.Visible = false;
            }
            else
            {
                //管理员用户，显示“权限管理”标签页
                superTabItemAuthorityManage.Visible = true;
                initAdvTreeUsers();
            }

            initMeshBaseParamConfit();

            initCacheSettingsTextBoxX();
        }

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
            if (advTreeLogList.SelectedNodes.Count > 0)
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
                logger.Info("账户“" + CurrentUser.Name + "”密码修改成功！");
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

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemUserAdd_Click(object sender, EventArgs e)
        {
            //开启添加用户功能
            updateUAControlsEnableState(true, true);
            buttonXOK.Text = "添  加";
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemUserEdit_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemUserDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除选中的用户？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string userName = ((User)advTreeUsers.SelectedNode.Tag).Name;
                if (SQLiteHelper.GetInstance().UserDelete(userName) == 1)
                {
                    advTreeUsers.Nodes.Remove(advTreeUsers.SelectedNode);
                    logger.Info("删除用户“" + userName + "”");
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //TODO:后续添加异常捕获处理
            }
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXUAOK_Click(object sender, EventArgs e)
        {
            if (buttonXUAOK.Text.Equals("添  加"))
            {  //添加用户
                if (textBoxXUAUserName.Text.Trim() != "" && textBoxXUAUserPassword.Text.Trim() != null)
                {
                    User user = new User();
                    user.Name = textBoxXUAUserName.Text.Trim();
                    user.Password = textBoxXUAUserPassword.Text.Trim();
                    user.Authority = radioButtonUAAdministrator.Checked ? EUserAuthority.Administrator.ToString() : EUserAuthority.GeneralUser.ToString();
                    if (SQLiteHelper.GetInstance().UserRegister(user) > 0)
                    {
                        advTreeUsers.BeginUpdate();
                        user = SQLiteHelper.GetInstance().UserSearchByName(user.Name);
                        Node userNode = new Node();
                        userNode.Tag = user;
                        userNode.Text = user.ID.ToString();
                        userNode.Cells.Add(new Cell(user.Name));
                        userNode.Cells.Add(new Cell(user.Authority == EUserAuthority.Administrator.ToString() ? "管理员" : "普通用户"));
                        advTreeUsers.Nodes.Add(userNode);
                        advTreeUsers.EndUpdate();

                        logger.Info("添加用户：“" + user.Name + "”，权限：“" + user.Authority + "”。");
                        MessageBox.Show("添加用户成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updateUAControlsEnableState(false, true);
                    }
                }
                else
                {
                    MessageBox.Show("用户名或密码不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {//修改用户信息

            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXUACancel_Click(object sender, EventArgs e)
        {
            updateUAControlsEnableState(false, true);
        }

        /// <summary>
        /// 用户列表树节点单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeUsers_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            //在右侧详情面板中显示用户信息
            if (advTreeUsers.SelectedNode != null)
            {
                User user = (User)advTreeUsers.SelectedNode.Tag;
                textBoxXUAUserName.Text = user.Name;
                textBoxXUAUserPassword.Text = user.Password;
                if (user.Authority.Equals(EUserAuthority.Administrator.ToString()))
                {
                    radioButtonUAAdministrator.Checked = true;
                }
                else
                {
                    radioButtonUAGeneralUser.Checked = true;
                }
            }
        }

        #endregion

        #region 软件设置事件处理

        /// <summary>
        /// 打开帮助文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemHelp_Click(object sender, EventArgs e)
        {
            Process.Start(FileUtils.HELP_CHM_PATH);
        }

        /// <summary>
        /// 选择离线地图缓存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXOfflineBDMapCachePathSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogCache.ShowDialog() == DialogResult.OK)
            {
                textBoxXOfflineBDMapCachePath.Text = folderBrowserDialogCache.SelectedPath;
            }
        }

        /// <summary>
        /// 选择视频数据缓存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXVideoDataPathSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogCache.ShowDialog() == DialogResult.OK)
            {
                textBoxXVideoDataPath.Text = folderBrowserDialogCache.SelectedPath;
            }
        }

        /// <summary>
        /// 恢复默认配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXSSCSResetDefault_Click(object sender, EventArgs e)
        {
            PathUtils.Instance.ResetDefaultCacheSetting();
            initCacheSettingsTextBoxX();
        }

        /// <summary>
        /// 保存缓存路径配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXSSCSSave_Click(object sender, EventArgs e)
        {
            try
            {
                PathUtils.Instance.BDMapCachePath = textBoxXOfflineBDMapCachePath.Text;
                PathUtils.Instance.VideoDataPath = textBoxXVideoDataPath.Text;
                logger.Info("更改离线地图缓存配置为“" + PathUtils.Instance.BDMapCachePath + "”；视频缓存位置为“" + PathUtils.Instance.VideoDataPath + "”。");
                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error("缓存配置保存失败！", ex);
                MessageBox.Show("保存失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消缓存路径配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXSSCSCancel_Click(object sender, EventArgs e)
        {
            textBoxXOfflineBDMapCachePath.Text = PathUtils.Instance.BDMapCachePath;
            textBoxXVideoDataPath.Text = PathUtils.Instance.VideoDataPath;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 启用TableLayoutPanel双缓冲，防止界面闪烁
        /// </summary>
        private void setTableLayoutPanelDoubleBufferd()
        {
            tableLayoutPanelLogManage.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelLogManage, true, null);
            tableLayoutPanelModifyPassword.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelModifyPassword, true, null);
            tableLayoutPanelAuthorityManage.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelAuthorityManage, true, null);
            tableLayoutPanelMeshBasicSetting.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshBasicSetting, true, null);
            tableLayoutPanelMeshLocalhostSetting.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshLocalhostSetting, true, null);
            tableLayoutPanelMeshTCP.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshTCP, true, null);
            tableLayoutPanelSSCacheSettings.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelSSCacheSettings, true, null);
            tableLayoutPanelSSAbout.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelSSAbout, true, null);
            tableLayoutPanelSoftSettings.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelSoftSettings, true, null);
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
        /// 初始化用户列表树
        /// </summary>
        private void initAdvTreeUsers()
        {
            List<User> users = SQLiteHelper.GetInstance().UserAllQuery();
            advTreeUsers.BeginUpdate();
            advTreeUsers.Nodes.Clear();
            foreach (User u in users)
            {
                if (u.Name == "admin")
                    continue;  //管理员账户自身不需要管理
                Node userNode = new Node();
                userNode.Tag = u;
                userNode.Text = u.ID.ToString();
                userNode.Cells.Add(new Cell(u.Name));
                userNode.Cells.Add(new Cell(u.Authority == EUserAuthority.Administrator.ToString() ? "管理员" : "普通用户"));
                advTreeUsers.Nodes.Add(userNode);
            }
            advTreeUsers.EndUpdate();
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

        /// <summary>
        /// 更新用户权限管理界面控件状态
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="clearContent"></param>
        private void updateUAControlsEnableState(bool enable, bool clearContent)
        {
            //空间可用性控制
            textBoxXUAUserName.Enabled = enable;
            textBoxXUAUserPassword.Enabled = enable;
            radioButtonUAAdministrator.Enabled = enable;
            radioButtonUAGeneralUser.Enabled = enable;
            buttonXUAOK.Enabled = enable;
            buttonXUACancel.Enabled = enable;
            //清空内容
            if (clearContent)
            {
                textBoxXUAUserName.Text = "";
                textBoxXUAUserPassword.Text = "";
                radioButtonUAGeneralUser.Checked = true;
            }
        }

        /// <summary>
        /// 初始化“Mesh基本参数配置”界面控件
        /// </summary>
        private void initMeshBaseParamConfit()
        {
            //判断是否为以太网卡
            //Ethernet              以太网卡  
            //Wireless80211         无线网卡
            NetworkInterface[] ethernets = nics.Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet).ToArray();
            foreach (NetworkInterface adapter in ethernets)
            {
                this.comboBoxExLocalhostNetwordCard.Items.Add(adapter.Name);
            }
            this.comboBoxExLocalhostNetwordCard.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化缓存设置控件的内容
        /// </summary>
        private void initCacheSettingsTextBoxX()
        {
            textBoxXOfflineBDMapCachePath.Text = PathUtils.Instance.BDMapCachePath;
            textBoxXVideoDataPath.Text = PathUtils.Instance.VideoDataPath;
        }

        #endregion

    }
}
