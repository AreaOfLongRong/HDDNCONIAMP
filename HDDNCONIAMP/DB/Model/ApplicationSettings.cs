using System.Data;
using Chloe.Entity;

namespace HDDNCONIAMP.DB.Model
{
    /// <summary>
    /// 应用程序配置表
    /// </summary>
    [TableAttribute("ApplicationSettings")]
    public class ApplicationSettings
    {
        /// <summary>
        /// 配置项关键字，字符串格式
        /// </summary>
        [Column(DbType = DbType.String, IsPrimaryKey = true)]
        public string Key { get; set; }
        /// <summary>
        /// 配置项的值，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string Value { get; set; }
        /// <summary>
        /// 描述信息，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string Description { get; set; }
    }
}
