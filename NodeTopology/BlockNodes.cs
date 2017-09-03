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

        List<node> nodelist = new List<node>();

        public List<node> Nodelist
        {
            get { return nodelist; }
            set { nodelist = value; }
        }




        List<relation> relationlist = new List<relation>();

        public List<relation> Relationlist
        {
            get { return relationlist; }
            set { relationlist = value; }
        }





    }
}
