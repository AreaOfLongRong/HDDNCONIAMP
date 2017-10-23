using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BMap.NET;
using DevComponents.DotNetBar;
using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Events;
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

        /// <summary>
        /// 获取或设置所有视频进程
        /// </summary>
        public List<Process> VideoProcesses { get; set; }

        /// <summary>
        /// 获取或设置所有窗口视频进程
        /// </summary>
        public List<Process> VideoWindowProcesses { get; set; }

        /// <summary>
        /// 获取或设置网路监听管理器
        /// </summary>
        public NetworkListenerManage NLM { get; set; }

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
        private DateTime DEADLINE = new DateTime(2017, 10, 28);

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
        private UCMeshManagement2 ucMeshManagement2;

        /// <summary>
        /// 用户配置管理控件
        /// </summary>
        private UCUserSettings ucUserSettings;

        /// <summary>
        /// 取消Token源
        /// </summary>
        private CancellationTokenSource mCTS;
        /// <summary>
        /// 取消Token
        /// </summary>
        private CancellationToken mCT;

        #endregion

        public FormMain()
        {
            InitializeComponent();
            //双缓冲设置，防止界面闪烁
            setTableLayoutPanelDoubleBufferd();

            labelXValidPeriod.Text = string.Format("测试版试用期截止时间：{0}", DEADLINE.ToShortDateString());

            VideoProcesses = new List<Process>();
            VideoWindowProcesses = new List<Process>();

            mCTS = new CancellationTokenSource();
            mCT = mCTS.Token;

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

            //杀死视频进程
            KillAllVideoProcess();

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
            logger.Info("关闭监听线程...");
            if (NLM != null)
                NLM.Stop();

            if (ucMeshManagement2 != null)
                ucMeshManagement2.StopTopology();
            //通知各线程关闭
            LifeTimeControl.closing = true;
            if (MeshTcpConfigManager.HasInstance())
            {
                MeshTcpConfigManager.GetInstance().CloseServer();
            }

            //杀死视频进程
            KillAllVideoProcess();

            //通知主窗体启动的线程
            if (mCTS != null)
                mCTS.Cancel();

            logger.Info("退出应用程序...");
        }

        #region 公共方法

        /// <summary>
        /// 触发用户登陆/登出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnRaiseUserLoginOroutEvent(object sender, UserLoginOrOutEventArgs e)
        {
            OnUserLoginOrOutEventHandler?.Invoke(sender, e);
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
            if (DateTime.Now >= DEADLINE.AddDays(1))
            {
                MessageBox.Show("演示版试用到期，请使用正式版软件！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                //启动ping设备列表任务
                Task.Factory.StartNew(() => PingMeshs(), mCT);

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

                ucGISVideo = null;
                ucAudioVideoProcess = null;
                ucMeshManagement2 = null;
                ucUserSettings = null;
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
                        //置顶主窗体
                        SetFormMainTop();
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
                        //置顶已全屏的视频面板
                        SetVideoPanelTop();
                    }
                    break;
                case OpenUCType.OpenMeshManagement:
                    if (CurrentUser == null)
                        updateSuperTabControlPanel(OpenUCType.OpenLogin);
                    else
                    {
                        if (ucMeshManagement2 == null)
                        {
                            ucMeshManagement2 = new UCMeshManagement2(this);
                            ucMeshManagement2.Dock = DockStyle.Fill;
                            ucMeshManagement2.OnMeshDeviceInfoModeified += UcMeshManagement2_OnMeshDeviceInfoModeified;
                            superTabControlPanelMeshManagement.Controls.Clear();  //清空所有控件
                            superTabControlPanelMeshManagement.Controls.Add(ucMeshManagement2);
                        }
                        //置顶主窗体
                        SetFormMainTop();
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
                        //置顶主窗体
                        SetFormMainTop();
                    }
                    break;
            }
        }

        /// <summary>
        /// 置顶主窗体
        /// </summary>
        private void SetFormMainTop()
        {
            // 获取查找窗体句柄(通过窗体标题名)
            IntPtr mainHandle = VideoInject.FindWindow(null, this.Text);
            logger.Info("主窗体句柄：" + mainHandle.ToString());
            if (mainHandle != IntPtr.Zero)
            {
                SetVideoPanelTop();
                //通过句柄设置当前窗体置顶
                VideoInject.SetForegroundWindow(mainHandle);
                logger.Info("主窗体置顶");
            }
        }



        /// <summary>
        /// 将视频面板置顶
        /// </summary>
        private void SetVideoPanelTop()
        {
            //如果存在已经打开的全屏视频，则继续恢复该全屏视频
            string fullScreenVideoProcessID = FileUtils.ReadFullScreenVideoProcessID();
            if (fullScreenVideoProcessID != null)
            {
                //通过句柄设置当前窗体置顶
                VideoInject.SetForegroundWindow(Process.GetProcessById(int.Parse(fullScreenVideoProcessID)).MainWindowHandle);
                logger.Info("九宫格置顶窗体：" + Process.GetProcessById(int.Parse(fullScreenVideoProcessID)).ProcessName);
            }
        }

        /// <summary>
        /// 更新Mesh设备信息事件
        /// </summary>
        /// <param name="mdi">Mesh设备信息</param>
        private void UcMeshManagement2_OnMeshDeviceInfoModeified(MeshDeviceInfo mdi)
        {
            if (ucGISVideo != null)
                ucGISVideo.UpdateMeshDeviceInfo(mdi);
        }

        /// <summary>
        /// 加载所有应用程序配置项
        /// </summary>
        private void loadAllApplicationSetting()
        {
            logger.Info("开始读取应用程序配置信息……");
            AllApplicationSetting = SQLiteHelper.GetInstance().ApplicationSettingAsDictionary();
            logger.Info("读取应用程序配置信息完毕！");

            if (AllApplicationSetting[ApplicationSettingKey.BDMapCachePath].Trim() == "")
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

        /// <summary>
        /// 杀死所有视频进程
        /// </summary>
        private void KillAllVideoProcess()
        {
            if (VideoProcesses != null)
            {
                foreach (Process p in VideoProcesses)
                {
                    logger.Info("关闭Panel进程“" + p.Id + "”");
                    if (!p.HasExited)
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }
            }
            if (VideoWindowProcesses != null)
            {
                foreach (Process p in VideoWindowProcesses)
                {
                    logger.Info("关闭窗体进程“" + p.Id + "”");
                    if (!p.HasExited)
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }
            }
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                if (p.ProcessName.Contains("SamplePlayClient"))
                {
                    logger.Info("关闭窗体进程“" + p.Id + "”");
                    if (!p.HasExited)
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }
            }
        }


        /// <summary>
        /// Ping所有Mesh列表
        /// </summary>
        private void PingMeshs()
        {
            //ping频率，至少10秒以上。
            int frequency = int.Parse(AllApplicationSetting[ApplicationSettingKey.MeshListRefreshFrequency]);
            frequency = frequency >= 10 * 1000 ? frequency : 10 * 1000;
            int timeOut = 1000;
            List<string> meshIPList = SQLiteHelper.GetInstance().MeshIPListQuery();
            //循环Ping Mesh设备IP地址
            while (true)
            {
                if (mCT.IsCancellationRequested)
                {
                    logger.Info("停止Ping扫描网络内Mesh设备。");
                    return;
                }
                foreach (string ip in meshIPList)
                {
                    Ping ping = new Ping();
                    PingOptions options = new PingOptions(64, true);
                    byte[] buffer = Encoding.ASCII.GetBytes(ip);
                    try
                    {
                        string isAsyncPing = AllApplicationSetting[ApplicationSettingKey.IsAsyncPing];
                        if (isAsyncPing != null && isAsyncPing.ToLower().Equals("true"))
                        {
                            //异步方式
                            ping.SendAsync(ip, timeOut, buffer, options, ip);
                            ping.PingCompleted += Ping_PingCompleted;
                        }
                        else
                        {
                            //默认采用，同步方式
                            PingReply reply = ping.Send(ip, timeOut, buffer, options);
                            logger.Info(string.Format("Ping\"{0}\":{1}", ip, reply.Status.ToString()));
                            string status = reply.Status == IPStatus.Success ? "在线" : "离线";
                            if (ucGISVideo != null)
                                ucGISVideo.UpdateMeshStatus(ip, status);
                            if (ucAudioVideoProcess != null)
                                ucAudioVideoProcess.UpdateMeshStatus(ip, status);
                        }
                    }
                    catch (PingException pe)
                    {
                        logger.Error(string.Format("Ping {0}过程中发生异常:{1}", ip, pe));
                    }
                    finally
                    {
                        ping.Dispose();
                    }
                }
                //Thread.Sleep(frequency);
            }
        }

        /// <summary>
        /// Ping结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            //触发Mesh设备列表进行更新
            string ip = (string)e.UserState;
            if (e.Reply.Status == IPStatus.Success)
                logger.Info(string.Format("Ping\"{0}\":{1}", ip, e.Reply.Status.ToString()));
            string status = e.Reply.Status == IPStatus.Success ? "在线" : "离线";
            if (ucGISVideo != null)
                ucGISVideo.UpdateMeshStatus(ip, status);
            if (ucAudioVideoProcess != null)
                ucAudioVideoProcess.UpdateMeshStatus(ip, status);
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
