using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NodeTopology
{
    public class CommFnc
    {
        public static string FindFileToOpen()
        {
            string RetVal, FileName;
            RetVal = "";
            OpenFileDialog OpenDir = new OpenFileDialog();
            OpenDir.InitialDirectory = "c:\\";
            OpenDir.Filter = "Text Files|*.txt";
            OpenDir.Title = "Select a data file.";
            if (OpenDir.ShowDialog() == DialogResult.OK)
            {
                FileName = OpenDir.FileName;
                if (FileName != null)
                {
                    RetVal = FileName;
                }
            }
            return RetVal;
        }

        public static string AssignFileToSave()
        {
            string RetVal, FileName;
            RetVal = "";
            SaveFileDialog SaveDir = new SaveFileDialog();
            SaveDir.InitialDirectory = "c:\\";
            SaveDir.Filter = "Text Files|*.txt";
            SaveDir.Title = "Write a data file to disk.";
            if (SaveDir.ShowDialog() == DialogResult.OK)
            {
                FileName = SaveDir.FileName;
                if (FileName != null)
                {
                    RetVal = FileName;
                }
            }
            return RetVal;
        }

        public static void RowDecode(string pLine, ref string pVariable, ref string pValue)
        {
            int PosSep = 0;
            int PosEnd = pLine.Length;
            int nCar = 0;
            if (pLine == "")
            {
                pVariable = "null";
                pValue = "null"; ;
            }
            else
            {
                PosSep = pLine.IndexOf("=");
                pVariable = pLine.Substring(0, PosSep);
                PosEnd = pLine.LastIndexOf(";");
                nCar = PosEnd - PosSep - 1;
                pValue = pLine.Substring(PosSep + 1, nCar);
            }
        }

        public static double module(int x, int y)
        {
            return System.Math.Sqrt(x * x + y * y);
        }

        public static double distance(int x1, int y1, int x2, int y2)
        {
            return module((x1 - x2), (y1 - y2));
        }
    }
}
