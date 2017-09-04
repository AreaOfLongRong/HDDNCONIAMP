using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NodeTopology
{
    public class GObject
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


        public GObject()
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

        public GObject(string ObjName, string ObjType, int ObjX1, int ObjY1, string LNK1, int ObjX2, int ObjY2, string LNK2)
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

        public GObject(string ObjName, string ObjType, int ObjX1, int ObjY1, string LNK1, int ObjX2, int ObjY2, string LNK2,string IniAddInfo)
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

    public class GScenario
    {

        public int Nobj;
        public int CurrObjIndx;
        public GObject[] GObjects;
        private const double PrcLineDist = 0.01;

        public GScenario(int Nobjects)
        {
            CurrObjIndx = 0;
            Nobj = Nobjects;
            GObjects = new GObject[Nobjects];
            for (int i = 0; i < Nobjects; i++)
            {
                GObjects[i] = new GObject();
            }
        }

        public void Clear()
        {
            // Nobj remains unaffected !
            CurrObjIndx = 0;
            for (int i = 0; i < Nobj; i++)
            {
                GObjects[i].Clear();
            }
        }

        public int FindContainerObject(int X, int Y, ref GObject GContainer, bool NoLineFlag)
        {
            //此处应该禁止用线条来拖动!!!!
            
            GObject CurrObj;
            bool inside = false;
            int retval = -1;
            double m = 0;
            double q = 0;
            double d = 0;
            double dmax = 0;
            double e = 0;
            for (int i = 0; i < CurrObjIndx; i++)
            {
                CurrObj = GObjects[i];
                if (CurrObj.Type != "Line")
                {
                    //
                    //   module of (x2,y2) is always > (x1,y1)
                    //
                    inside = (X >= CurrObj.x1) && (Y >= CurrObj.y1);
                    inside = inside && (X <= CurrObj.x2);
                    inside = inside && (Y <= CurrObj.y2);

                    if (inside == true)
                    {
                        GContainer = CurrObj;
                        retval = i;

                        return retval;
                    }
                }
                else
                {
                    //
                    //   due to mobile links can be (x2,y2) > or < di (x1,y1)
                    //   in this case we don't consider the container
                    //   but only if we are next to the line
                    //

                    //m = (double)(CurrObj.y2 - CurrObj.y1) / (CurrObj.x2 - CurrObj.x1);
                    //q = (double)CurrObj.y1 - m * CurrObj.x1;
                    //d = System.Math.Abs(Y - m * X - q) / System.Math.Sqrt(1 + m * m);
                    //dmax = CommFnc.module(X, Y);
                    //e = d / dmax;
                    //inside = (e < PrcLineDist);
                    //if (NoLineFlag == true) inside = false;
                }

            }
            return retval;
        }

        public void AddGObject(string ObjName, string ObjType, int x1, int y1, int x2, int y2,string addinfo)
        {
            GObject ObjToAdd = new GObject();
            ObjToAdd.x1 = x1;
            ObjToAdd.y1 = y1;
            ObjToAdd.AddInfo = addinfo;
            if (ObjName.Length > 12)
            {
                ObjToAdd.Lnk1 = ObjName.Substring(0, 12);
            }
            else
            {
                ObjToAdd.Lnk1 = "";
            }
            
            ObjToAdd.x2 = x2;
            ObjToAdd.y2 = y2;
            if (ObjName.Length > 12)
            {
                ObjToAdd.Lnk2 = ObjName.Substring(12, 12);
            }
            else
            {
                ObjToAdd.Lnk2 = "";
            }
            
            ObjToAdd.Name = ObjName;
            ObjToAdd.Type = ObjType;

            GObjects[CurrObjIndx] = ObjToAdd;
            CurrObjIndx++;
        }

        public void ModifyGObject(GObject OldGObject, GObject NewGObject)
        {
            //
            //      Adjust all the references
            //
            for (int i = 0; i < Nobj; i++)
            {
                if (GObjects[i].Lnk1 == OldGObject.Name)
                {
                    GObjects[i].Lnk1 = NewGObject.Name;
                }
                if (GObjects[i].Lnk2 == OldGObject.Name)
                {
                    GObjects[i].Lnk2 = NewGObject.Name;
                }
            }
            //
            //      Adjust Object properties
            //
            OldGObject.x1 = NewGObject.x1;
            OldGObject.y1 = NewGObject.y1;
            OldGObject.Lnk1 = NewGObject.Lnk1;
            OldGObject.x2 = NewGObject.x2;
            OldGObject.y2 = NewGObject.y2;
            OldGObject.Lnk2 = NewGObject.Lnk2;
            OldGObject.Name = NewGObject.Name;
            OldGObject.Type = NewGObject.Type;
            OldGObject.AddInfo = NewGObject.AddInfo;

        }

        public void DeleteGObject(GObject GObjectToDelete)
        {
            if (CurrObjIndx == 0)
            {
                //
                //  There's no objects!
                //
            }
            else
            {
                //
                //      Find the index of the object to delete
                //
                int IndexToDelete = 0;
                FindGObjectIndxByName(GObjectToDelete.Name, ref IndexToDelete);
                //
                //      Adjust (nullify) all the references
                //
                for (int i = 0; i < Nobj; i++)
                {
                    if (GObjects[i].Lnk1 == GObjectToDelete.Name)
                    {
                        GObjects[i].Lnk1 = "";
                    }
                    if (GObjects[i].Lnk2 == GObjectToDelete.Name)
                    {
                        GObjects[i].Lnk2 = "";
                    }
                }
                //
                //      Nullify Object properties
                //
                GObjectToDelete.x1 = 0;
                GObjectToDelete.y1 = 0;
                GObjectToDelete.Lnk1 = "";
                GObjectToDelete.x2 = 0;
                GObjectToDelete.y2 = 0;
                GObjectToDelete.Lnk2 = "";
                GObjectToDelete.Name = "";
                GObjectToDelete.Type = "";
                GObjectToDelete.AddInfo = "";

                //
                //      Left Shift of the GObjects vector
                //
                int j = IndexToDelete;
                while (j < Nobj - 1)
                {
                    GObjects[j] = GObjects[j + 1];
                    j++;
                }
                CurrObjIndx--;
            }
        }

        public void FindGObjectIndxByName(string ObjName, ref int GObjIndx)
        {
            for (int i = 0; i < Nobj; i++)
            {
                if (GObjects[i].Name == ObjName)
                {
                    GObjIndx = i;
                }
            }
        }

        public void FindGObjectByName(string ObjLnkName, ref GObject GObj)
        {
           // for (int i = 0; i < Nobj; i++)
            for (int i = 0; i < CurrObjIndx; i++)
            {
                if (GObjects[i].Name == ObjLnkName)
                {
                    GObj = GObjects[i];
                }
            }
        }

        public void AdjustLinkedTo(string ObjLnkName)
        {
            GObject GObjLinked = new GObject();
            FindGObjectByName(ObjLnkName, ref GObjLinked);
            for (int i = 0; i < Nobj; i++)
            {
                if (GObjects[i].Type == "Line")
                {
                    if (GObjects[i].Lnk1 == ObjLnkName)
                    {
                        GObjects[i].x1 = (GObjLinked.x1 + GObjLinked.x2) / 2;
                        GObjects[i].y1 = (GObjLinked.y1 + GObjLinked.y2) / 2;
                    }
                    //
                    if (GObjects[i].Lnk2 == ObjLnkName)
                    {
                        GObjects[i].x2 = (GObjLinked.x1 + GObjLinked.x2) / 2;
                        GObjects[i].y2 = (GObjLinked.y1 + GObjLinked.y2) / 2;
                    }
                }
            }

        }

        public int LastIndexOfGObject(string Type)
        {
            int retval = 0;
            for (int i = 0; i < Nobj; i++)
            {
                if (GObjects[i].Type == Type)
                {
                    retval++;
                }
            }
            return retval;
        }

        public bool LoadFile(string FileFullPath, ref string sErrFileMsg)
        {
            string sVariable = "";
            string sValue = "";
            string cFirst = "";
            int i = 0;
            int iLine = 0;
            GObject NewGObj = new GObject();
            sErrFileMsg = "";
            try
            {
                using (StreamReader sr = new StreamReader(FileFullPath))
                {
                    String sLine = "";
                    while (sLine != "end network file.")
                    {
                        sLine = sr.ReadLine();
                        iLine++;
                        if (sLine == "end object.")
                        {
                            sLine = sr.ReadLine();
                            iLine++;
                        }
                        if (sLine == "")
                        {
                            cFirst = "*";
                        }
                        else
                        {
                            cFirst = sLine.Substring(0, 1);
                        }
                        if (cFirst == "*")
                        {
                            // null o *-beginning lines are like comments
                        }
                        else
                        {
                            while ((sLine != "end object.") && (sLine != "end network file."))
                            {
                                CommFnc.RowDecode(sLine, ref sVariable, ref sValue);
                                switch (sVariable)
                                {
                                    case "object":
                                        NewGObj.Name = sValue;
                                        break;
                                    case "type":
                                        NewGObj.Type = sValue;
                                        break;
                                    case "lnk1":
                                        NewGObj.Lnk1 = sValue;
                                        break;
                                    case "lnk2":
                                        NewGObj.Lnk2 = sValue;
                                        break;
                                    case "x1":
                                        NewGObj.x1 = System.Convert.ToInt32(sValue);
                                        break;
                                    case "x2":
                                        NewGObj.x2 = System.Convert.ToInt32(sValue);
                                        break;
                                    case "y1":
                                        NewGObj.y1 = System.Convert.ToInt32(sValue);
                                        break;
                                    case "y2":
                                        NewGObj.y2 = System.Convert.ToInt32(sValue);
                                        break;
                                    default:
                                        break;
                                }
                                sLine = sr.ReadLine();
                                iLine++;
                            }
                            if (sLine == "end object.")
                            {
                                GObjects[i] = new GObject(NewGObj.Name, NewGObj.Type,
                                    NewGObj.x1, NewGObj.y1, NewGObj.Lnk1, NewGObj.x2, NewGObj.y2, NewGObj.Lnk2);
                                i++;
                                CurrObjIndx = i;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                sErrFileMsg = "Error reading file : " + e.Message + " line = " + i.ToString() + "\n";
                return false;
            }
        }

        public bool SaveFile(string FileFullPath, ref string sErrFileMsg)
        {
            sErrFileMsg = "";
            try
            {
                using (StreamWriter sw = new StreamWriter(FileFullPath))
                {
                    String sLine;
                    for (int i = 0; i < Nobj; i++)
                    {
                        if (GObjects[i].Name != "")
                        {
                            sLine = "object=" + GObjects[i].Name + ";";
                            sw.WriteLine(sLine);
                            sLine = "type=" + GObjects[i].Type + ";";
                            sw.WriteLine(sLine);
                            sLine = "x1=" + GObjects[i].x1.ToString() + ";";
                            sw.WriteLine(sLine);
                            sLine = "y1=" + GObjects[i].y1.ToString() + ";";
                            sw.WriteLine(sLine);
                            sLine = "lnk1=" + GObjects[i].Lnk1 + ";";
                            sw.WriteLine(sLine);
                            sLine = "x2=" + GObjects[i].x2.ToString() + ";";
                            sw.WriteLine(sLine);
                            sLine = "y2=" + GObjects[i].y2.ToString() + ";";
                            sw.WriteLine(sLine);
                            sLine = "lnk2=" + GObjects[i].Lnk2 + ";";
                            sw.WriteLine(sLine);
                            sLine = "end object.";
                            sw.WriteLine(sLine);
                            sw.WriteLine();
                        }
                    }
                    sw.WriteLine("end network file.");
                }
                return true;
            }
            catch (Exception e)
            {
                sErrFileMsg = "Error writing file : " + e.Message + "\n";
                return false;
            }
        }

    }
}
