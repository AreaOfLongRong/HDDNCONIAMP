using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeTopology
{
    [Serializable]
    public class ARPItem
    {
        string internetaddress; //IP

        public string InternetAddress
        {
            get { return internetaddress; }
            set { internetaddress = value; }
        }
        string physicaladdress; //MAC

        public string PhysicalAddress
        {
            get { return physicaladdress; }
            set { physicaladdress = value; }
        }
        string type; //类型

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
