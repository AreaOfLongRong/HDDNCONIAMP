using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace NodeTopology
{
    public partial class Form1 : Form
    {
        public GScenario GNetwork;
        private const int XTextPixelOffset = -10;
        private const int YTextPixelOffset = 30;//80;

        //这里是创建一个画布 

        private void ReDrawAll()
        {

            Bitmap bmp = new Bitmap(this.panel2.Width, this.panel2.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(this.panel2.BackColor);






            //Graphics g = this.CreateGraphics();
            //Graphics g = this.panel2.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            GObject CurrObj = new GObject();
            Rectangle Rct = new Rectangle();






            //Pen p = new Pen(Color.Blue);
            Image ObjImg;
            int xm = 0;
            int ym = 0;
            string IsLine = "";
            //for (int i = 0; i < GNetwork.Nobj; i++)
            //urinatedong 只显示需要显示的
            for (int i = 0; i < GNetwork.Nobj; i++)
            {
                CurrObj = GNetwork.GObjects[i];
                //
                if (CurrObj.Type == "") IsLine = "N/D";
                if (CurrObj.Type == "Line") IsLine = "Y";
                if ((CurrObj.Type != "Line") && (CurrObj.Type != "")) IsLine = "N";
                //
                switch (IsLine)
                {
                    case "Y":
                        // g.DrawLine(p, CurrObj.x1, CurrObj.y1, CurrObj.x2, CurrObj.y2);


                        AdjustableArrowCap lineCap = new AdjustableArrowCap(5, 6, true);

                        Pen p;

                        string[] MyRelationInfo = CurrObj.AddInfo.Split("\n".ToCharArray());

                        if (MyRelationInfo[6].IndexOf("2") > 0)
                        {
                            p = new Pen(Color.Blue, 3);
                        }
                        else
                        {
                            p = new Pen(Color.Orange, 3);
                            p.DashStyle = DashStyle.Custom;
                            p.DashPattern = new float[] { 6, 3 };
                        }

                        p.CustomEndCap = lineCap;
                        p.CustomStartCap = lineCap;

                        g.DrawLine(p, CurrObj.x1, CurrObj.y1, CurrObj.x2, CurrObj.y2);


                        xm = (CurrObj.x1 + CurrObj.x2) / 2;
                        ym = (CurrObj.y1 + CurrObj.y2) / 2;
                        //AddText(xm, ym, CurrObj.Name, false,g);

                        int x1 = (CurrObj.x1 + xm) / 2;
                        int y1 = (CurrObj.y1 + ym) / 2;

                        int x2 = (CurrObj.x2 + xm) / 2;
                        int y2 = (CurrObj.y2 + ym) / 2;

                        AddText(x1, y1, MyRelationInfo[0] + "\n" + MyRelationInfo[1] + "\n" + MyRelationInfo[2], false, g);

                        AddText(x2, y2, MyRelationInfo[3] + "\n" + MyRelationInfo[4] + "\n" + MyRelationInfo[5], false, g);

                        p.Dispose();
                        break;
                    case "N":

                        string[] MyNodeInfo = CurrObj.AddInfo.Split("\n".ToCharArray());

                        Rct.X = CurrObj.x1;
                        Rct.Y = CurrObj.y1;
                        Rct.Width = CurrObj.x2 - CurrObj.x1;
                        Rct.Height = CurrObj.y2 - CurrObj.y1;
                        if (CurrObj.Type != String.Empty)
                        {



                            if (double.Parse(MyNodeInfo[4].Replace("Battery", "")) > 0)
                            {
                                ObjImg = FindGObjectTypeImage("Router");
                            }
                            else
                            {
                                ObjImg = FindGObjectTypeImage("NotOnline");
                            }

                            g.DrawImage(ObjImg, Rct);

                            //使用IP地址显示
                            //AddText(CurrObj.x1, CurrObj.y1, CurrObj.Name, true,g);

                            AddText(CurrObj.x1, CurrObj.y1, MyNodeInfo[0], true, g);

                            GNetwork.AdjustLinkedTo(CurrObj.Name);
                        }
                        break;
                }
            }

            //g1.DrawEllipse(new Pen(System.Drawing.Color.Red), 10, 10, 100, 100); 
            //g1.DrawImage(Image.FromFile("E:/down.png"), x, 10);//这是在画布上绘制图形 
            this.panel2.CreateGraphics().DrawImage(bmp, 0, 0);//这句是将图形显示到窗口上

            bmp.Dispose();
            g.Dispose();


        }




        private void AddText(int Xbase, int Ybase, string Msg, bool UseOffset, Graphics UsingGraphics)
        {
            //Graphics g = this.CreateGraphics();
            //Graphics g = this.panel2.CreateGraphics();
            //g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.CompositingQuality = CompositingQuality.HighQuality;

            Font CurrFont = new Font("Arial", 8);
            int x = 0;
            int y = 0;
            if (UseOffset == true)
            {
                x = Xbase + XTextPixelOffset;
                y = Ybase + YTextPixelOffset;
            }
            else
            {
                x = Xbase;
                y = Ybase;
            }
            UsingGraphics.DrawString(Msg, CurrFont, new SolidBrush(Color.Black), x, y);
        }
        private Image FindGObjectTypeImage(string ObjType)
        {
            Image RetImg = null;
            switch (ObjType)
            {
                case "Network":
                    RetImg = imageList1.Images[0];
                    break;
                case "Router":
                    RetImg = imageList1.Images[3];
                    break;
                case "Emitter":
                    RetImg = imageList1.Images[2];
                    break;
                case "Receiver":
                    RetImg = imageList1.Images[1];
                    break;
                case "NotOnline":
                    RetImg = imageList1.Images[4];
                    break;
            }
            return RetImg;
        }

        public void AddGobject(int x1, int y1, int x2, int y2, relation prelation)
        {

            #region 为了防止刷新，不在此时画线!!! urinatedong  20170322

            //Graphics g = this.panel2.CreateGraphics();//this.CreateGraphics();
            //g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.CompositingQuality = CompositingQuality.HighQuality;

            //AdjustableArrowCap lineCap = new AdjustableArrowCap(5, 6, true);

            //Pen p;

            //if (prelation.Findtimes == 2)
            //{
            //     p = new Pen(Color.Blue,3);
            //}
            //else
            //{
            //     p = new Pen(Color.Orange,3);
            //     p.DashStyle = DashStyle.Custom;
            //     p.DashPattern = new float[] { 6, 3 };
            //}

            //p.CustomEndCap = lineCap;

            //g.DrawLine(p, x1, y1, x2, y2);

            #endregion

            ///新需求中不再强调这些信息
            ///urinatedong 20170314
            //int xm = (x1 + x2) / 2;
            //int ym = (y1 + y2) / 2;
            string relationinfo = "Localport " + prelation.Localport +
                       "\nTxsnr" + prelation.Txsnr +
                       "\nTxspeed " + prelation.Txspeed +
                       "\nRemoteport " + prelation.Remoteport +
                       "\nRxsnr " + prelation.Rxsnr +
                       "\nRxspeed " + prelation.Rxspeed +
                       "\nFindTimes " + prelation.Findtimes;
            //AddText(xm, ym, relationinfo, false);

            //AddText(xm, ym, "Rx Tx", false);

            GObject TempGObject = new GObject();

            //在数组中查找是否已经有该关系了
            GNetwork.FindGObjectByName(prelation.Localnode.MacAddress + prelation.Remotenode.MacAddress, ref TempGObject);

            if (TempGObject.Type == "")
            {
                GNetwork.AddGObject(prelation.Localnode.MacAddress + prelation.Remotenode.MacAddress, "Line", x1, y1, x2, y2, relationinfo);
            }
            else
            {
                ///更新关系信息！！！
                TempGObject.AddInfo = relationinfo;
            }

        }

        public void AddGObject(int x1, int y1, node pnode)
        {


            #region 为了防止刷新，不在此时画图形!!! urinatedong 20170322

            //Graphics g = this.panel2.CreateGraphics();//

            ////Graphics g = this.graphBuffer.Graphics;


            ////Graphics g = Graphics.FromHwnd(_panel.Handle);

            //g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle ObjRct = new Rectangle();
            //Pen p = new Pen(Color.Blue);
            Image ObjImg;


            ///新需求中不在要求更多的节点信息

            string AddInfo = pnode.IpAddress +
            "\nTxPower " + pnode.TxPower +
            "\nFrequency " + pnode.Frequency +
            "\nBandWidth " + pnode.BandWidth +
            "\nBattery " + pnode.Battery;

            string ObjName = pnode.MacAddress;


            ObjImg = FindGObjectTypeImage("Router");
            ObjRct.X = x1;
            ObjRct.Y = y1;
            ObjRct.Height = ObjImg.Height;
            ObjRct.Width = ObjImg.Width;
            //g.DrawImage(ObjImg, ObjRct);
            //AddText(x1, y1, ObjName, true);
            //AddText(x1, y1, AddInfo, true);
            int x2 = x1 + ObjRct.Width;
            int y2 = y1 + ObjRct.Height;

            #endregion


            GObject TempGObject = new GObject();

            //在数组中查找是否已经有该子节点了！！！
            GNetwork.FindGObjectByName(ObjName, ref TempGObject);

            if (TempGObject.Type == "")
            {
                GNetwork.AddGObject(ObjName, "Router", x1, y1, x2, y2, AddInfo);
            }
            else
            {
                string[] NodeInfo = TempGObject.AddInfo.Split("\n".ToCharArray());
                if (!NodeInfo[0].Equals(pnode.IpAddress))
                {
                    LogHelper.WriteLog("MAC地址为:" + pnode.MacAddress + " 的终端 IP地址由:" + NodeInfo[0] + " 改变为:" + pnode.IpAddress);
                }

                TempGObject.AddInfo = AddInfo;
            }




            //string ObjName = ObjType + "_" + GNetwork.LastIndexOfGObject(ObjType).ToString();
            ////
            //if (ObjType == "Line")
            //{
            //    g.DrawLine(p, x1, y1, x2, y2);
            //    int xm = (x1 + x2) / 2;
            //    int ym = (y1 + y2) / 2;
            //    AddText(xm, ym, ObjName, false);
            //}
            //else
            //{
            //    ObjImg = FindGObjectTypeImage(ObjType);
            //    ObjRct.X = x1;
            //    ObjRct.Y = y1;
            //    ObjRct.Height = ObjImg.Height;
            //    ObjRct.Width = ObjImg.Width;
            //    g.DrawImage(ObjImg, ObjRct);
            //    AddText(x1, y1, ObjName, true);
            //    x2 = x1 + ObjRct.Width;
            //    y2 = y1 + ObjRct.Height;
            //}
            ////
            //GNetwork.AddGObject(ObjName, ObjType, x1, y1, x2, y2);

            //p.Dispose(); 
            //g.Dispose();





        }

        public void AddGObject(int x1, int y1, int x2, int y2, string ObjType, string addinfo)
        {

            Graphics g = this.panel2.CreateGraphics();//this.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle ObjRct = new Rectangle();


            Pen p = new Pen(Color.Blue);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;//恢复实线  
            p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;



            Image ObjImg;
            string ObjName = ObjType + "_" + GNetwork.LastIndexOfGObject(ObjType).ToString();
            //
            if (ObjType == "Line")
            {
                g.DrawLine(p, x1, y1, x2, y2);
                int xm = (x1 + x2) / 3;
                int ym = (y1 + y2) / 3;
                //AddText(xm, ym, ObjName, false,g);

                AddText(xm, ym, addinfo, false, g);
            }
            else
            {
                ObjImg = FindGObjectTypeImage(ObjType);
                ObjRct.X = x1;
                ObjRct.Y = y1;
                ObjRct.Height = ObjImg.Height;
                ObjRct.Width = ObjImg.Width;
                g.DrawImage(ObjImg, ObjRct);
                AddText(x1, y1, ObjName, true, g);
                x2 = x1 + ObjRct.Width;
                y2 = y1 + ObjRct.Height;
            }
            //
            GNetwork.AddGObject(ObjName, ObjType, x1, y1, x2, y2, addinfo);
            p.Dispose();
            g.Dispose();
        }


        public void UpdateFrequencyTelnetTelegram(string needupdateNodeIP, double newfrequency)
        {

            //需要在这里try catch finally 以关闭连接
            //必须关闭连接!!!!!! tn.close!!!!

            //LogHelper.WriteLog("开始获取MAC为:" + needInfoNode + " 的NODE的信息！！！ ");

            //node TheNode = MYBlockNodes.Nodelist.Where(n => n.MacAddress.Equals(needInfoNode)).ToList().First();


            if (!string.IsNullOrEmpty(needupdateNodeIP))
            {

                myTelnet tn = new myTelnet(needupdateNodeIP);

                try
                {

                    string recvStr = tn.recvDataWaitWord("help", 1);

                    LogHelper.WriteLog(recvStr);

                    Thread.Sleep(1);
                    string sendnewfrequency = "frequency " + newfrequency.ToString();

                    LogHelper.WriteLog(recvStr);

                    tn.sendData(sendnewfrequency);
                    Thread.Sleep(1);
                    recvStr = tn.recvDataWaitWord("MHz", 1);

                    LogHelper.WriteLog(recvStr);

                    string receive = recvStr.Replace("OK\n#user@/>", "").Replace("MHz", "");

                    double result = double.Parse(receive);

                    if (newfrequency.Equals(result))
                    {
                        LogHelper.WriteLog("IP 为" + needupdateNodeIP + "的设备Frequency 修改为 " + result.ToString() + "成功!!!");
                    }
                    else
                    {
                        LogHelper.WriteLog("IP 为" + needupdateNodeIP + "的设备Frequency 修改失败!!!" + " newfrequency : " + newfrequency.ToString() + " result :" + result.ToString());
                    }

                    //Thread.Sleep(1);
                    //tn.sendData("frequency");
                    //recvStr = tn.recvDataWaitWord("MHz", 1);
                    //TheNode.Frequency = double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("MHz", ""));



                }

                catch (Exception ex)
                {
                    LogHelper.WriteLog("在修改频率时出现异常: " + ex.Message);
                    throw ex;
                }
                finally
                {
                    tn.close();
                }



            }//if end



        }

    }
}
