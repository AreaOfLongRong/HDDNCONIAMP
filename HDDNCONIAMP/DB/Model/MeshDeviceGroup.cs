using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Chloe.Entity;

namespace HDDNCONIAMP.DB.Model
{
    public class MeshDeviceGroup : IDEntity
    {
        /// <summary>
        /// 用户ID列，主键，自增
        /// </summary>
        [Column(IsPrimaryKey = true)]
        [AutoIncrement]
        public virtual int ID { get; set; }
        /// <summary>
        /// 用户名，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string GroupName { get; set; }
    }
}
