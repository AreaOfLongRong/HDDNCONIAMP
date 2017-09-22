using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BMap.NET;
using DevComponents.DotNetBar;
using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Events;
using HDDNCONIAMP.Mesh;
using HDDNCONIAMP.Network;
using HDDNCONIAMP.UI.AudioVideoProcess;
using HDDNCONIAMP.UI.GISVideo;
using HDDNCONIAMP.UI.MeshManagement;
using HDDNCONIAMP.UI.UserSettings;
using HDDNCONIAMP.Utils;
using log4net;

namespace HDDNCONIAMP
{
    public partial class FormMain : Office2007Form
    {

        #region 公共变量、属性

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// 获取所有应用程序配置项
        /// </summary>
        public Dictionary<string, string> AllApplicationSetting { get; set; }

        /// <summary>
        /// Mesh预案管理字典
        /// </summary>
        public Dictionary<string, MeshPlanManage> MeshPlanManageDictionary
        {
            get;
            set;
        }
        
        #endregion

        #region 自定义事件

        /// <summary>
        /// 用户登陆/登出事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void UserLoginOrOutEventHandler(object sender, UserLoginOrOutEventArgs e);

        /// <summary>
        /// 用户登陆/登出事件处理器
        /// </summary>
        public event UserLoginOrOutEventHandler OnUserLoginOrOutEventHandler;

        #endregion

        #region 私有变量

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(FormMain));

        /// <summary>
        /// 程序截止日期
        /// </summary>
        private DateTime DEADLINE = new DateTime(2017, 9, 30);

        /// <summary>
        /// GIS定位关联视频控件
        /// </summary>
        private UCGISVideo ucGISVideo;

        /// <summary>
        /// 音视频综合处理控件
        /// </summary>
        private UCAudioVideoProcess ucAudioVideoProcess;

        /// <summary>
        /// Mesh设备管理控件
        /// </summary>
        private UCMeshManagement ucMeshManagement;

        /// <summary>
        /// Mesh设备管理控件
        /// </summary>
        private UCMeshManagement2 ucMeshManagement2;

        /// <summary>
        /// 用户配置管理控件
        /// </summary>
        private UCUserSettings ucUserSettings;

        #endregion

        /// <summary>
        /// 获取或设置网路监听管理器
        /// </summary>
        public NetworkListenerManage NLM { get; set; }

        public FormMain()
        {
            InitializeComponent();
            //双缓冲设置，防止界面闪烁
            setTableLayoutPanelDoubleBufferd();

            labelXValidPeriod.Text = string.Format("测试版试用期截止时间：{0}", DEADLINE.ToShortDateString());
            
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            logger.Info("加载主界面...");
            //默认设置账户文本框为焦点
            textBoxXUserName.Focus();

            logger.Info("启动更新系统时间计时器...");
            timerUpdateTime.Start();

            //更新界面
            updateSuperTabControlPanel(OpenUCType.OpenLogin);

            logger.Info("读取预案...");
            MeshPlanManageDictionary = SQLiteHelper.GetInstance().MeshPlanAllQueryAsDictionary();

            logger.Info("开启监听...");
            NLM = new NetworkListenerManage();
            NLM.Start();


            //注册用户登陆/登出事件处理
            OnUserLoginOrOutEventHandler += FormMain_OnUserLoginOrOutEventHandler;

        }

        /// <summary>
        /// 主界面关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Info("关闭主窗体...");
            logger.Info("停止更新系统时间计时器...");
            //停止计时器
            timerUpdateTime.Stop();
            //通知各线程关闭
            LifeTimeControl.closing = true;

        }

        #region 公共方法

        /// <summary>
        /// 触发用户登陆/登出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnRaiseUserLoginOroutEvent(object sender, UserLoginOrOutEventArgs e)
        {
            if (OnUserLoginOrOutEventHandler != null)
            {
                OnUserLoginOrOutEventHandler(sender, e);
            }
        }

        public Point GetVideoFullScreenLocation()
        {
            Point p = new Point();            
            p.X = superTabControlMain.Location.X + 
                superTabControlPanelAudioVideoProcess.Location.X + 
                ucAudioVideoProcess.GetGridLocationX();
            p.Y = superTabControlMain.Location.Y;
            return p;
        }

        #endregion

        #region 界面事件

        /// <summary>
        /// 退出应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            logger.Info("关闭监听线程...");
            NLM.Stop();
            logger.Info("退出应用程序...");
            Application.Exit();
        }

        /// <summary>
        /// 标签选中项更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superTabControlMain_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            switch (superTabControlMain.SelectedTabIndex)
            {
                case 0:
                    updateSuperTabControlPanel(OpenUCType.OpenGISVideo);
                    break;
                case 1:
                    updateSuperTabControlPanel(OpenUCType.OpenAudioVideoProcess);
                    break;
                case 2:
                    updateSuperTabControlPanel(OpenUCType.OpenMeshManagement);
                    break;
                case 3:
                    updateSuperTabControlPanel(OpenUCType.OpenUserSettings);
                    break;
            }
        }

        /// <summary>
        /// 密码框按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxXPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)Keys.Return))
                buttonLogin_Click(this, null);
        }

        /// <summary>
        /// 登陆按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (DateTime.Now >= DEADLINE)
            {
                MessageBox.Show("测试版使用到期，请使用正式版软件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (textBoxXUserName.Text.Trim().Equals("") || textBoxXPassword.Text.Trim().Equals(""))
            {
                MessageBox.Show("账号或密码不能为空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //账户登陆
            if (SQLiteHelper.GetInstance().UserLogin(textBoxXUserName.Text, textBoxXPassword.Text))
            {
                CurrentUser = SQLiteHelper.GetInstance().UserSearchByName(textBoxXUserName.Text);
                logger.Info("账户“" + textBoxXUserName.Text + "”登陆系统");

                loadAllApplicationSetting();

                //更新主界面
                OnRaiseUserLoginOroutEvent(this, new UserLoginOrOutEventArgs(CurrentUser, true));
            }
            else
            {
                MessageBox.Show("账户名或密码输入有误，请重新输入！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 登出系统事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelXLogOut_Click(object sender, EventArgs e)
        {
            OnRaiseUserLoginOroutEvent(this, new UserLoginOrOutEventArgs(CurrentUser, false));
        }

        /// <summary>
        /// 更新主界面中的当前时间信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUpdateTime_Tick(object sender, EventArgs e)
        {
            labelXSystemTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion

        #region 自定义事件处理

        /// <summary>
        /// 用户登陆/登出事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_OnUserLoginOrOutEventHandler(object sender, UserLoginOrOutEventArgs e)
        {
            if (e.IsLogin)
            {
                //用户登陆处理
                labelXUserName.Text = e.User.Name;
                this.CurrentUser = e.User;
                updateSuperTabControlPanel(OpenUCType.OpenGISVideo);
            }
            else
            {
                //用户登出处理
                labelXUserName.Text = "";
                this.CurrentUser = null;
                updateSuperTabControlPanel(OpenUCType.OpenLogin);
            }
        }


        #endregion

        #region 私有方法

        /// <summary>
        /// 启用TableLayoutPanel双缓冲，防止界面闪烁
        /// </summary>
        private void setTableLayoutPanelDoubleBufferd()
        {
            tableLayoutPanelMain.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMain, true, null);
            tableLayoutPanelTop.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelTop, true, null);
            tableLayoutPanelLogin.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelLogin, true, null);
            tableLayoutPanelBottom.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelBottom, true, null);
        }

        /// <summary>
        /// 更新标签面板中的内容
        /// </summary>
        /// <param name="type"></param>
        private void updateSuperTabControlPanel(OpenUCType type)
        {
            switch (type)
            {
                case OpenUCType.OpenLogin:
                    if (ucGISVideo != null && superTabControlPanelGISVideo.Contains(ucGISVideo))
                    {
                        ucGISVideo.Visible = false;
                    }
                    tableLayoutPanelLogin.Visible = true;  //显示
                    superTabControlMain.SelectedTabIndex = 0;
                    break;
                case OpenUCType.OpenGISVideo:
                    if (CurrentUser == null)
                        updateSuperTabControlPanel(OpenUCType.OpenLogin);
                    else
                    {
                        if (ucGISVideo == null)
                        {
                            ucGISVideo = new UCGISVideo(this);
                            ucGISVideo.Dock = DockStyle.Fill;
                            superTabControlPanelGISVideo.Controls.Add(ucGISVideo);
                        }
                        tableLayoutPanelLogin.Visible = false;  //隐藏登陆界面
                        ucGISVideo.Visible = true;  //显示GIS定位视频界面
                    }
                    break;
                case OpenUCType.OpenAudioVideoProcess:
                    if (CurrentUser == null)
                        updateSuperTabControlPanel(OpenUCType.OpenLogin);
                    else
                    {
                        if (ucAudioVideoProcess == null)
                        {
                            ucAudioVideoProcess = new UCAudioVideoProcess(this);
                            ucAudioVideoProcess.Dock = DockStyle.Fill;
                            superTabControlPanelAudioVideoProcess.Controls.Clear();  //清空所有控件
                            superTabControlPanelAudioVideoProcess.Controls.Add(ucAudioVideoProcess);
                        }
                    }
                    break;
                case OpenUCType.OpenMeshManagement:
                    if (CurrentUser == null)
                        updateSuperTabControlPanel(OpenUCType.OpenLogin);
                    else
                    {
                        //if (ucMeshManagement == null)
                        //{
                        //    ucMeshManagement = new UCMeshManagement();
                        //    ucMeshManagement.Dock = DockStyle.Fill;
                        //    superTabControlPanelMeshManagement.Controls.Clear();  //清空所有控件
                        //    superTabControlPanelMeshManagement.Controls.Add(ucMeshManagement);
                        //}
                        if (ucMeshManagement2 == null)
                        {
                            ucMeshManagement2 = new UCMeshManagement2(this);
                            ucMeshManagement2.Dock = DockStyle.Fill;
                            superTabControlPanelMeshManagement.Controls.Clear();  //清空所有控件
                            superTabControlPanelMeshManagement.Controls.Add(ucMeshManagement2);
                        }
                    }
                    break;
                case OpenUCType.OpenUserSettings:
                    if (CurrentUser == null)
                        updateSuperTabControlPanel(OpenUCType.OpenLogin);
                    else
                    {
                        if (ucUserSettings == null)
                        {
                            ucUserSettings = new UCUserSettings(this);
                            ucUserSettings.Dock = DockStyle.Fill;
                            ucUserSettings.CurrentUser = CurrentUser;
                            superTabControlPanelUserSettings.Controls.Clear();  //清空所有控件
                            superTabControlPanelUserSettings.Controls.Add(ucUserSettings);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 加载所有应用程序配置项
        /// </summary>
        private void loadAllApplicationSetting()
        {
            logger.Info("开始读取应用程序配置信息……");
            AllApplicationSetting = SQLiteHelper.GetInstance().ApplicationSettingAsDictionary();
            logger.Info("读取应用程序配置信息完毕！");

            if(AllApplicationSetting[ApplicationSettingKey.BDMapCachePath].Trim() == "")
            {
                AllApplicationSetting[ApplicationSettingKey.BDMapCachePath] =
                    PathUtils.BDMAP_CACHE_DEFAULT_PATH;
                SQLiteHelper.GetInstance().ApplicationSettingUpdate(ApplicationSettingKey.BDMapCachePath, PathUtils.BDMAP_CACHE_DEFAULT_PATH);
            }
            BMapConfiguration.MapCachePath = AllApplicationSetting[ApplicationSettingKey.BDMapCachePath];

            if (AllApplicationSetting[ApplicationSettingKey.VideoCachePath].Trim() == "")
            {
                AllApplicationSetting[ApplicationSettingKey.VideoCachePath] =
                    PathUtils.VIDEO_DATA_DEFAULT_PATH;
                SQLiteHelper.GetInstance().ApplicationSettingUpdate(ApplicationSettingKey.VideoCachePath, PathUtils.VIDEO_DATA_DEFAULT_PATH);
            }
        }

        #endregion

        #region 内部枚举

        /// <summary>
        /// 打开用户控件类型
        /// </summary>
        enum OpenUCType
        {
            /// <summary>
            /// 打开登陆界面
            /// </summary>
            OpenLogin,
            /// <summary>
            /// 打开GIS定位视频管理界面
            /// </summary>
            OpenGISVideo,
            /// <summary>
            /// 打开音视频综合处理界面
            /// </summary>
            OpenAudioVideoProcess,
            /// <summary>
            /// 打开Mesh设备管理界面
            /// </summary>
            OpenMeshManagement,
            /// <summary>
            /// 打开用户设置界面
            /// </summary>
            OpenUserSettings
        }

        #endregion

    }
}
