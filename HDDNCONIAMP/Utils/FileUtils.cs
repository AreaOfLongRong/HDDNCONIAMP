using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BMap.NET.WindowsForm;
using BMap.NET.WindowsForm.BMapElements;
using HDDNCONIAMP.Network;

namespace HDDNCONIAMP.Utils
{
    /// <summary>
    /// 文件操作工具类
    /// </summary>
    public class FileUtils
    {

        /// <summary>
        /// CHM帮助文档路径
        /// </summary>
        public static string HELP_CHM_PATH = AppDomain.CurrentDomain.BaseDirectory + "Help.chm";

        /// <summary>
        /// 用于处理视频全屏时切换处理时通信的进程ID文件路径
        /// </summary>
        public static string FILE_PROCESS_ID_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SamplePlayClient\\processId.txt");

        /// <summary>
        /// 数据库文件路径
        /// </summary>
        public static string FILE_DB_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB\\HDDNCONIAMP.db3");

        /// <summary>
        /// 视频转发服务EXE路径
        /// </summary>
        public static string VIDEO_TRANSFER_SERVER_EXE_PATH = AppDomain.CurrentDomain.BaseDirectory + "UDPBroadcastServer.exe";

        /// <summary>
        /// 私有构造函数，防止被实例化
        /// </summary>
        private FileUtils()
        {

        }

        /// <summary>
        /// 读取指定文件内容到字符串中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件内容字符串</returns>
        public static string ReadFileToString(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 追加GPS信息到文件中
        /// </summary>
        /// <param name="info">GPS信息</param>
        public static void AppendGPSInfoToFile(GPSInfo info)
        {
            string dir = Path.Combine(PathUtils.GPS_DATA_DEFAULT_PATH, info.ID);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string gpsFile = Path.Combine(dir, info.Time.Split(' ')[0].Replace('/', '-') + ".txt");
            using (StreamWriter sw = new StreamWriter(gpsFile, true))
            {
                sw.WriteLine(info.ToString());
            }
        }

        /// <summary>
        /// 生成音视频信息文件路径
        /// </summary>
        /// <returns>音视频信息文件路径</returns>
        public static string GenerateAudioAndVideoInfoFilePath()
        {
            return PathUtils.Instance.TempAudioAndVideoPath + Guid.NewGuid().ToString() + ".txt";
        }

        /// <summary>
        /// 读取音视频设备信息
        /// </summary>
        /// <param name="infoTxtPath">音视频设备信息文件路径</param>
        /// <returns></returns>
        public static string ReadAudioAndVideoInfo(string infoTxtPath)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(infoTxtPath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 从GPS日志文件中读取Mesh设备轨迹
        /// </summary>
        /// <param name="model265ID">265模块ID</param>
        /// <param name="startDT">开始时间</param>
        /// <param name="stopDT">结束时间</param>
        /// <returns></returns>
        public static BMeshRoute ReadMeshRouteFromGPSLogs(string model265ID, DateTime startDT, DateTime stopDT)
        {
            BMeshRoute bmr = new BMeshRoute();
            bmr.DeviceName = model265ID;
            bmr.RouteColor = bmr.getNextColor();
            bmr.DeviceLocationList = new List<LatLngPoint>();
            string dir = Path.Combine(PathUtils.GPS_DATA_DEFAULT_PATH, model265ID);
            DateTime tempDT = startDT;
            while (tempDT <= stopDT)
            {
                string gpsFile = Path.Combine(dir, tempDT.ToString("yyyy-M-d") + ".txt");
                if (File.Exists(gpsFile))
                {
                    using (StreamReader sr = new StreamReader(gpsFile))
                    {
                        //TODO:后续考虑增加GPS过滤，比如移动超过一定距离再画点。
                        string line = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] temp = line.Split(',');
                            bmr.DeviceLocationList.Add(
                                new LatLngPoint(double.Parse(temp[3].Replace("°","")), double.Parse(temp[2].Replace("°", ""))));
                        }
                    }
                }
                tempDT = tempDT.AddDays(1);
            }
            return bmr;
        }

        /// <summary>
        /// 从“processId.txt”文件中读取全屏视频的进程ID
        /// </summary>
        /// <returns>全屏视频的进程ID</returns>
        public static string ReadFullScreenVideoProcessID()
        {
            if (File.Exists(FILE_PROCESS_ID_PATH))
            {
                using(StreamReader sr = new StreamReader(FILE_PROCESS_ID_PATH))
                {
                    return sr.ReadLine();
                }
            }
            else
            {
                return null;
            }
        }

    }
}
