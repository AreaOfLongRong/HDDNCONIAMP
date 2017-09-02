using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Chloe.Entity;

namespace HDDNCONIAMP.DB.Model
{
    /// <summary>
    /// 用户表对象
    /// </summary>
    [TableAttribute("SoftwareSettings")]
    public class SoftwareSettings : IDEntity
    {
        /// <summary>
        /// ID列，主键，自增
        /// </summary>
        [Column(IsPrimaryKey = true)]
        [AutoIncrement]
        public virtual int ID { get; set; }
        /// <summary>
        /// 配置项关键字
        /// </summary>
        [Column(DbType = DbType.String)]
        public string SettingKey { get; set; }
        /// <summary>
        /// 配置项的值
        /// </summary>
        [Column(DbType = DbType.String)]
        public string SettingValue { get; set; }
    }
}
