using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace HDDNCONIAMP.Utils
{
    /// <summary>
    /// 路径工具类
    /// </summary>
    public class PathUtils
    {

        /// <summary>
        /// 百度地图默认缓存路径
        /// </summary>
        public static string BDMAP_CACHE_DEFAULT_PATH = AppDomain.CurrentDomain.BaseDirectory + "BDMapCache";

        /// <summary>
        /// 视频数据默认缓存路径
        /// </summary>
        public static string VIDEO_DATA_DEFAULT_PATH = AppDomain.CurrentDomain.BaseDirectory + "Videos";

        /// <summary>
        /// 临时数据存储目录
        /// </summary>
        public static string TEMP_PATH = AppDomain.CurrentDomain.BaseDirectory + "Temp";

        /// <summary>
        /// 音视频临时数据存储目录
        /// </summary>
        public static string TEMP_AUDIOANDVIDEO_PATH = TEMP_PATH + "\\AudioAndVideo\\";

        /// <summary>
        /// 获取或设置百度地图缓存路径
        /// </summary>
        public string BDMapCachePath
        {
            get
            {
                if (Properties.Settings.Default.BDMAP_CACHE_PATH == "")
                {
                    Properties.Settings.Default.BDMAP_CACHE_PATH = BDMAP_CACHE_DEFAULT_PATH;
                    Properties.Settings.Default.Save();
                    if (!Directory.Exists(BDMAP_CACHE_DEFAULT_PATH))
                        Directory.CreateDirectory(BDMAP_CACHE_DEFAULT_PATH);
                }
                return Properties.Settings.Default.BDMAP_CACHE_PATH;
            }
            set
            {
                Properties.Settings.Default.BDMAP_CACHE_PATH = value;
                Properties.Settings.Default.Save();
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
            }
        }

        /// <summary>
        /// 获取或设置视频数据路径
        /// </summary>
        public string VideoDataPath
        {
            get
            {
                if (Properties.Settings.Default.VIDEO_DATA_PATH == "")
                {
                    Properties.Settings.Default.VIDEO_DATA_PATH = VIDEO_DATA_DEFAULT_PATH;
                    Properties.Settings.Default.Save();
                    if (!Directory.Exists(VIDEO_DATA_DEFAULT_PATH))
                        Directory.CreateDirectory(VIDEO_DATA_DEFAULT_PATH);
                }
                return Properties.Settings.Default.VIDEO_DATA_PATH;
            }
            set
            {
                Properties.Settings.Default.VIDEO_DATA_PATH = value;
                Properties.Settings.Default.Save();
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
            }
        }

        /// <summary>
        /// 获取数据存储临时路径
        /// </summary>
        public string TempPath
        {
            get
            {
                if (!Directory.Exists(TEMP_PATH))
                    Directory.CreateDirectory(TEMP_PATH);
                return TEMP_PATH;
            }
        }

        /// <summary>
        /// 获取音视频数据临时存储目录
        /// </summary>
        public string TempAudioAndVideoPath
        {
            get
            {
                if (!Directory.Exists(TEMP_AUDIOANDVIDEO_PATH))
                    Directory.CreateDirectory(TEMP_AUDIOANDVIDEO_PATH);
                return TEMP_AUDIOANDVIDEO_PATH;
            }
        }

        /// <summary>
        /// 临时日志文件目录
        /// </summary>
        public static string LOG_TEMP_DIRECTORY;

        /// <summary>
        /// 唯一实例
        /// </summary>
        public static PathUtils Instance = new PathUtils();

        /// <summary>
        /// 私有构造函数，防止被实例化
        /// </summary>
        private PathUtils()
        {

        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static PathUtils()
        {
            LOG_TEMP_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + "Temp\\Logs";
            if (!Directory.Exists(LOG_TEMP_DIRECTORY))
            {
                Directory.CreateDirectory(LOG_TEMP_DIRECTORY);
            }
        }

        /// <summary>
        /// 恢复默认缓存数据
        /// </summary>
        public void ResetDefaultCacheSetting()
        {
            BDMapCachePath = BDMAP_CACHE_DEFAULT_PATH;
            VideoDataPath = VIDEO_DATA_DEFAULT_PATH;
        }

    }
}
