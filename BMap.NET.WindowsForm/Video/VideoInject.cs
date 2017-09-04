using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace BMap.NET.WindowsForm.Video
{
    public class VideoInject
    {
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
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

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

        IntPtr appWin;

        private double ratio = 1920.0 / 1080.0;

        public VideoInject()
        {
            appWin = IntPtr.Zero;
        }

        public void injectWindow()
        {
            ProcessStartInfo psi = new ProcessStartInfo("SamplePlayClient\\SamplePlayClient.exe");
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.Arguments = string.Format("1 0 0 {0} {1}", (int)(500 * ratio), 500);

            Process _process = new Process();
            _process.StartInfo = psi;
            _process.EnableRaisingEvents = true;
            _process.Start();
        }

        public void injectPanel(Panel panel)
        {
            if (appWin != IntPtr.Zero)
                return;
            ProcessStartInfo psi = new ProcessStartInfo("SamplePlayClient\\SamplePlayClient.exe");
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.Arguments = string.Format("0 0 0 {0} {1}", panel.Width, panel.Height);

            Process _process = new Process();
            _process.StartInfo = psi;
            _process.EnableRaisingEvents = true;
            _process.Exited += _process_Exited;
            _process.Start();

            if (_process.WaitForInputIdle())
            {

                while (_process.MainWindowHandle.ToInt32() == 0)
                {
                    Thread.Sleep(100);
                    _process.Refresh();//必须刷新状态才能重新获得TITLE
                }
                _process.StartInfo = psi;

                // Get the main handle
                appWin = _process.MainWindowHandle;

                // Put it into this form
                SetParent(appWin, panel.Handle);
                // Move the window to overlay it on this window
                //MoveWindow(appWin, 0, 0, _mainParent.dockPanel1.Width, _mainParent.dockPanel1.Height, true);
                MoveWindow(appWin, 0, 0, panel.Width, panel.Height, true);
            }
        }

        private void _process_Exited(object sender, EventArgs e)
        {
            appWin = IntPtr.Zero;
        }
    }
}
