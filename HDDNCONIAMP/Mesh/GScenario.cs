using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HDDNCONIAMP.Mesh
{
    public class GScenario
    {
        public int Nobj;
        public int CurrObjIndx;
        public MeshGObject[] MeshGObjects;
        private const double PrcLineDist = 0.01;

        public GScenario(int Nobjects)
        {
            CurrObjIndx = 0;
            Nobj = Nobjects;
            MeshGObjects = new MeshGObject[Nobjects];
            for (int i = 0; i < Nobjects; i++)
            {
                MeshGObjects[i] = new MeshGObject();
            }
        }

        public void Clear()
        {
            // Nobj remains unaffected !
            CurrObjIndx = 0;
            for (int i = 0; i < Nobj; i++)
            {
                MeshGObjects[i].Clear();
            }
        }

        public int FindContainerObject(int X, int Y, ref MeshGObject GContainer, bool NoLineFlag)
        {
            //此处应该禁止用线条来拖动!!!!

            MeshGObject CurrObj;
            bool inside = false;
            int retval = -1;
            double m = 0;
            double q = 0;
            double d = 0;
            double dmax = 0;
            double e = 0;
            for (int i = 0; i < CurrObjIndx; i++)
            {
                CurrObj = MeshGObjects[i];
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

        public void AddMeshGObject(string ObjName, string ObjType, int x1, int y1, int x2, int y2, string addinfo)
        {
            MeshGObject ObjToAdd = new MeshGObject();
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

            MeshGObjects[CurrObjIndx] = ObjToAdd;
            CurrObjIndx++;
        }

        public void ModifyMeshGObject(MeshGObject OldMeshGObject, MeshGObject NewMeshGObject)
        {
            //
            //      Adjust all the references
            //
            for (int i = 0; i < Nobj; i++)
            {
                if (MeshGObjects[i].Lnk1 == OldMeshGObject.Name)
                {
                    MeshGObjects[i].Lnk1 = NewMeshGObject.Name;
                }
                if (MeshGObjects[i].Lnk2 == OldMeshGObject.Name)
                {
                    MeshGObjects[i].Lnk2 = NewMeshGObject.Name;
                }
            }
            //
            //      Adjust Object properties
            //
            OldMeshGObject.x1 = NewMeshGObject.x1;
            OldMeshGObject.y1 = NewMeshGObject.y1;
            OldMeshGObject.Lnk1 = NewMeshGObject.Lnk1;
            OldMeshGObject.x2 = NewMeshGObject.x2;
            OldMeshGObject.y2 = NewMeshGObject.y2;
            OldMeshGObject.Lnk2 = NewMeshGObject.Lnk2;
            OldMeshGObject.Name = NewMeshGObject.Name;
            OldMeshGObject.Type = NewMeshGObject.Type;
            OldMeshGObject.AddInfo = NewMeshGObject.AddInfo;

        }

        public void DeleteMeshGObject(MeshGObject MeshGObjectToDelete)
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
                FindMeshGObjectIndxByName(MeshGObjectToDelete.Name, ref IndexToDelete);
                //
                //      Adjust (nullify) all the references
                //
                for (int i = 0; i < Nobj; i++)
                {
                    if (MeshGObjects[i].Lnk1 == MeshGObjectToDelete.Name)
                    {
                        MeshGObjects[i].Lnk1 = "";
                    }
                    if (MeshGObjects[i].Lnk2 == MeshGObjectToDelete.Name)
                    {
                        MeshGObjects[i].Lnk2 = "";
                    }
                }
                //
                //      Nullify Object properties
                //
                MeshGObjectToDelete.x1 = 0;
                MeshGObjectToDelete.y1 = 0;
                MeshGObjectToDelete.Lnk1 = "";
                MeshGObjectToDelete.x2 = 0;
                MeshGObjectToDelete.y2 = 0;
                MeshGObjectToDelete.Lnk2 = "";
                MeshGObjectToDelete.Name = "";
                MeshGObjectToDelete.Type = "";
                MeshGObjectToDelete.AddInfo = "";

                //
                //      Left Shift of the MeshGObjects vector
                //
                int j = IndexToDelete;
                while (j < Nobj - 1)
                {
                    MeshGObjects[j] = MeshGObjects[j + 1];
                    j++;
                }
                CurrObjIndx--;
            }
        }

        public void FindMeshGObjectIndxByName(string ObjName, ref int GObjIndx)
        {
            for (int i = 0; i < Nobj; i++)
            {
                if (MeshGObjects[i].Name == ObjName)
                {
                    GObjIndx = i;
                }
            }
        }

        public void FindMeshGObjectByName(string ObjLnkName, ref MeshGObject GObj)
        {
            // for (int i = 0; i < Nobj; i++)
            for (int i = 0; i < CurrObjIndx; i++)
            {
                if (MeshGObjects[i].Name == ObjLnkName)
                {
                    GObj = MeshGObjects[i];
                }
            }
        }

        public void AdjustLinkedTo(string ObjLnkName)
        {
            MeshGObject GObjLinked = new MeshGObject();
            FindMeshGObjectByName(ObjLnkName, ref GObjLinked);
            for (int i = 0; i < Nobj; i++)
            {
                if (MeshGObjects[i].Type == "Line")
                {
                    if (MeshGObjects[i].Lnk1 == ObjLnkName)
                    {
                        MeshGObjects[i].x1 = (GObjLinked.x1 + GObjLinked.x2) / 2;
                        MeshGObjects[i].y1 = (GObjLinked.y1 + GObjLinked.y2) / 2;
                    }
                    //
                    if (MeshGObjects[i].Lnk2 == ObjLnkName)
                    {
                        MeshGObjects[i].x2 = (GObjLinked.x1 + GObjLinked.x2) / 2;
                        MeshGObjects[i].y2 = (GObjLinked.y1 + GObjLinked.y2) / 2;
                    }
                }
            }

        }

        public int LastIndexOfMeshGObject(string Type)
        {
            int retval = 0;
            for (int i = 0; i < Nobj; i++)
            {
                if (MeshGObjects[i].Type == Type)
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
            MeshGObject NewGObj = new MeshGObject();
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
                                MeshCommon.RowDecode(sLine, ref sVariable, ref sValue);
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
                                MeshGObjects[i] = new MeshGObject(NewGObj.Name, NewGObj.Type,
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
                        if (MeshGObjects[i].Name != "")
                        {
                            sLine = "object=" + MeshGObjects[i].Name + ";";
                            sw.WriteLine(sLine);
                            sLine = "type=" + MeshGObjects[i].Type + ";";
                            sw.WriteLine(sLine);
                            sLine = "x1=" + MeshGObjects[i].x1.ToString() + ";";
                            sw.WriteLine(sLine);
                            sLine = "y1=" + MeshGObjects[i].y1.ToString() + ";";
                            sw.WriteLine(sLine);
                            sLine = "lnk1=" + MeshGObjects[i].Lnk1 + ";";
                            sw.WriteLine(sLine);
                            sLine = "x2=" + MeshGObjects[i].x2.ToString() + ";";
                            sw.WriteLine(sLine);
                            sLine = "y2=" + MeshGObjects[i].y2.ToString() + ";";
                            sw.WriteLine(sLine);
                            sLine = "lnk2=" + MeshGObjects[i].Lnk2 + ";";
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
