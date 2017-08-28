using System.Data;
using Chloe.Entity;

namespace HDDNCONIAMP.DB.Model
{
    /// <summary>
    /// 用户表对象
    /// </summary>
    [TableAttribute("UserInfo")]
    public class User : IDEntity
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
        public string Name { get; set; }
        /// <summary>
        /// 密码，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string Password { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public EUserAuthority? Authority { get; set; }
    }
}
