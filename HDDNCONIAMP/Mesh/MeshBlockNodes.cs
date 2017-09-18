using System.Collections.Generic;

namespace HDDNCONIAMP.Mesh
{
    /// <summary>
    /// Mesh节点块，封装了节点及关系的列表属性
    /// </summary>
    public  class MeshBlockNodes
    {

        /// <summary>
        /// 获取或设置Mesh节点列表
        /// </summary>
        public List<MeshNode> MeshNodeList { get; set; }

        /// <summary>
        /// 获取或设置Mesh关系列表
        /// </summary>
        public List<MeshRelation> MeshRelationList { get; set; }

    }
}
