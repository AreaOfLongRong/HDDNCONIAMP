using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDDNCONIAMP.Mesh
{
    public class MeshGObject
    {
        public string Name;
        public string Type;
        public int x1;
        public int y1;
        public string Lnk1;
        public int x2;
        public int y2;
        public string Lnk2;
        //urinatedong 20170322 用来对对象信息进行显示
        public string AddInfo;


        public MeshGObject()
        {
            Name = "";
            Type = "";
            Lnk1 = "";
            Lnk2 = "";
            AddInfo = "";

            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
        }

        public MeshGObject(string ObjName, string ObjType, int ObjX1, int ObjY1, string LNK1, int ObjX2, int ObjY2, string LNK2)
        {
            Name = ObjName;
            Type = ObjType;
            Lnk1 = LNK1;
            Lnk2 = LNK2;
            x1 = ObjX1;
            y1 = ObjY1;
            x2 = ObjX2;
            y2 = ObjY2;
        }

        public MeshGObject(string ObjName, string ObjType, int ObjX1, int ObjY1, string LNK1, int ObjX2, int ObjY2, string LNK2, string IniAddInfo)
        {
            Name = ObjName;
            Type = ObjType;
            Lnk1 = LNK1;
            Lnk2 = LNK2;
            x1 = ObjX1;
            y1 = ObjY1;
            x2 = ObjX2;
            y2 = ObjY2;
            AddInfo = IniAddInfo;
        }

        public void Clear()
        {
            Name = "";
            Type = "";
            Lnk1 = "";
            Lnk2 = "";
            AddInfo = "";
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
        }
    }
}
