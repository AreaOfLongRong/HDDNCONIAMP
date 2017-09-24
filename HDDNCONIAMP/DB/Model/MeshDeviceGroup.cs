using System.Data;
using Chloe.Entity;

namespace HDDNCONIAMP.DB.Model
{
    /// <summary>
    /// Mesh设备分组表
    /// </summary>
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

        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GroupName;
        }
    }
}
