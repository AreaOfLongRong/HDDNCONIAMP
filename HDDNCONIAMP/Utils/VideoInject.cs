using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using log4net;

namespace HDDNCONIAMP.Utils
{
    public class VideoInject
    {

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(VideoInject));


        private const int SWP_NOOWNERZORDER = 0x200;
        private const int SWP_NOREDRAW = 0x8;
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int WS_EX_MDICHILD = 0x40;
        private const int SWP_FRAMECHANGED = 0x20;
        private const int SWP_NOACTIVATE = 0x10;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;
        private const int SWP_NOMOVE = 0x2;
        private const int SWP_NOSIZE = 0x1;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_CLOSE = 0x10;
        private const int WS_CHILD = 0x40000000;

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true,
             CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        private static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        private static extern long GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        private static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);
        //private static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hwnd, uint Msg, long wParam, long lParam);

        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        /// <summary>
        /// 窗口句柄
        /// </summary>
        IntPtr appWin;

        /// <summary>
        /// 视频宽高比
        /// </summary>
        private double ratio = 1920.0 / 1080.0;

        /// <summary>
        /// 视频服务器IP地址
        /// </summary>
        private string mVideoServerIP;

        /// <summary>
        /// 视频服务器用户名
        /// </summary>
        private string mVideoServerUserName = "admin";

        /// <summary>
        /// 视频服务器用户密码
        /// </summary>
        private string mVideoServerPassword = "admin";

        /// <summary>
        /// 窗体进程
        /// </summary>
        private Process _windowProcess;

        /// <summary>
        /// 面板进程
        /// </summary>
        private Process _panelProcess;

        /// <summary>
        /// 查看视频EXE路径
        /// </summary>
        private string mVideoExePath;

        public VideoInject(string videoServerIP, string userName, string password)
        {
            appWin = IntPtr.Zero;
            mVideoServerIP = videoServerIP;
            mVideoServerUserName = userName;
            mVideoServerPassword = password;
            mVideoExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "SamplePlayClient\\SamplePlayClient.exe");
        }

        /// <summary>
        /// 带边框视频窗口
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public Process injectWindow(string deviceID)
        {
            ProcessStartInfo psi = new ProcessStartInfo(mVideoExePath);
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            psi.Arguments = string.Format("{0} {1} {2} {3} 1 0 0 0 {4} {5} 0 0 {6} {7}",
                mVideoServerIP, mVideoServerUserName, mVideoServerUserName,
                deviceID, (int)(500 * ratio), 500, 
                screenWidth * ScreenUtils.ScaleX, 
                screenHeight * ScreenUtils.ScaleY);

            _windowProcess = new Process();
            _windowProcess.StartInfo = psi;
            _windowProcess.EnableRaisingEvents = true;
            _windowProcess.Exited += _process_Exited;
            logger.Info("打开带边框视频查看视频；参数：" + psi.Arguments);
            _windowProcess.Start();

            return _windowProcess;
        }

        /// <summary>
        /// 不带边框的视频窗口
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="fullScreenLocation"></param>
        /// <param name="fullScreenPanel"></param>
        /// <param name="deviceID"></param>
        /// <param name="isFullScreen"></param>
        public Process injectPanel(Panel panel, Point fullScreenLocation, Panel fullScreenPanel, string deviceID, string isFullScreen)
        {
            if (appWin != IntPtr.Zero)
                return null;
            ProcessStartInfo psi = new ProcessStartInfo(mVideoExePath);
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.Arguments = string.Format("{0} {1} {2} {3} 0 {4} 0 0 {5} {6} {7} {8} {9} {10}",
                mVideoServerIP, mVideoServerUserName, mVideoServerUserName,
                deviceID, isFullScreen,
                panel.Width, panel.Height,
                ScreenUtils.ScaleX * fullScreenLocation.X, 
                ScreenUtils.ScaleY * fullScreenLocation.Y,
                ScreenUtils.ScaleX * (fullScreenPanel.Width + 6), 
                ScreenUtils.ScaleY * (fullScreenPanel.Height + 8));

            _panelProcess = new Process();
            _panelProcess.StartInfo = psi;
            _panelProcess.EnableRaisingEvents = true;
            _panelProcess.Exited += _process_Exited;
            _panelProcess.Start();

            logger.Info("打开无边框视频查看视频；参数：" + psi.Arguments);

            if (_panelProcess.WaitForInputIdle())
            {

                while (_panelProcess.MainWindowHandle.ToInt32() == 0)
                {
                    _panelProcess.Refresh();//必须刷新状态才能重新获得TITLE
                    Thread.Sleep(100);
                }
                _panelProcess.StartInfo = psi;

                // Get the main handle
                appWin = _panelProcess.MainWindowHandle;

                // Put it into this form
                SetParent(appWin, panel.Handle);
                // Move the window to overlay it on this window
                //MoveWindow(appWin, 0, 0, _mainParent.dockPanel1.Width, _mainParent.dockPanel1.Height, true);
                MoveWindow(appWin, 0, 0, panel.Width, panel.Height, true);
            }

            return _panelProcess;
        }

        ~VideoInject()
        {
            if (_windowProcess != null && !_windowProcess.HasExited)
            {
                _windowProcess.Kill();
            }
            if (_panelProcess != null && !_panelProcess.HasExited)
            {
                _panelProcess.Kill();
            }
        }

        private void _process_Exited(object sender, EventArgs e)
        {
            if (_windowProcess != null && !_windowProcess.HasExited)
            {
                _windowProcess.Kill();
                _windowProcess.WaitForExit();
            }
            if (_panelProcess != null && !_panelProcess.HasExited)
            {
                _panelProcess.Kill();
                _panelProcess.WaitForExit();
            }
            appWin = IntPtr.Zero;
        }
    }
}
