using System;
using System.IO;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using System.Diagnostics;

namespace HDDNCONIAMP
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process currentProcess = Process.GetCurrentProcess();
            string strProcessName = currentProcess.ProcessName;
            ////获取版本号 
            //CommonData.VersionNumber = Application.ProductVersion; 
            //检查进程是否已经启动，已经启动则显示报错信息退出程序。 
            Process[] processList = Process.GetProcessesByName(strProcessName);
            foreach (Process process in processList)
            {
                if (currentProcess.Id != process.Id)
                {
                    process.Kill();
                }
            }
           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //加载log4net配置
            FileInfo configFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(configFile);

            //创建logger
            var logger = LogManager.GetLogger(typeof(Program));
            try
            {
                FileInfo dbFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB\\HDDNCONIAMP.db3"));
                if(dbFile.Exists)
                {
                    if(dbFile.Length > 0)
                    {
                        logger.Info("启动程序...");
                        Application.Run(new FormMain());
                    }
                    else
                    {
                        string str = "数据库文件已损坏，无法正常启动程序！";
                        logger.Info(str);
                        MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
                else
                {
                    string str = "数据库文件缺失，无法正常启动程序！";
                    logger.Info(str);
                    MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            catch(AccessViolationException ave) {
                logger.Error(" 尝试读取或写入受保护的内存", ave);
            }
            catch (Exception ex)
            {
                logger.Error("程序出现异常：" + ex.ToString());
            }
        }
    }
}
