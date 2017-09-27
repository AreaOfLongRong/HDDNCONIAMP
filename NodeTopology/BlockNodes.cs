using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeTopology
{
    public class BlockNodes
    {
        public BlockNodes()
        {

        }

        List<MeshNode> nodelist = new List<MeshNode>();

        public List<MeshNode> Nodelist
        {
            get { return nodelist; }
            set { nodelist = value; }
        }




        List<MeshRelation> relationlist = new List<MeshRelation>();

        public List<MeshRelation> Relationlist
        {
            get { return relationlist; }
            set { relationlist = value; }
        }





    }
}
