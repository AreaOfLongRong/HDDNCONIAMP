using System.Data;
using Chloe.Entity;

namespace HDDNCONIAMP.DB.Model
{
    /// <summary>
    /// 预案对象
    /// </summary>
    [TableAttribute("MeshPlanManage")]
    public class MeshPlanManage : IDEntity
    {
        /// <summary>
        /// 预案ID列，主键，自增
        /// </summary>
        [Column(IsPrimaryKey = true)]
        [AutoIncrement]
        public virtual int ID { get; set; }
        /// <summary>
        /// Mesh设备IP地址，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string MeshIP { get; set; }
        /// <summary>
        /// Mesh设备IP地址，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string Alias { get; set; }
        /// <summary>
        /// 音视频设备ID，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string AudioVideoID { get; set; }
        /// <summary>
        /// 265模块IP地址，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string Model265IP { get; set; }
        /// <summary>
        /// 海康云台球机设备IP地址，字符串格式
        /// </summary>
        [Column(DbType = DbType.String)]
        public string HKVideoIP { get; set; }

        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "预案ID-" + ID +
                "Mesh设备IP-" + MeshIP +
                "别名-" + Alias +
                "音视频ID-" + AudioVideoID +
                "265模块IP-" + Model265IP +
                (HKVideoIP == null ? "" : "海康球机IP-" + HKVideoIP);
        }
    }
}
