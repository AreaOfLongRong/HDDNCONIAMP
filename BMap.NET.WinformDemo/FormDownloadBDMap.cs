using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BMap.NET.HTTPService;
using BMap.NET.WindowsForm;

namespace BMap.NET.WinformDemo
{
    public partial class FormDownloadBDMap : Form
    {
        /// <summary>
        /// 日志输出最大行，超过之后清空文本框
        /// </summary>
        private static int MAX_LOG_ROW_COUNT = 100;
        /// <summary>
        /// 当前日志输出行数
        /// </summary>
        private int m_LogRowCount = 0;
        /// <summary>
        /// 地图数据服务
        /// </summary>
        MapService service = new MapService();

        public FormDownloadBDMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 界面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDownloadBDMap_Load(object sender, EventArgs e)
        {
            buttonDownload.Enabled = true;
            buttonPause.Enabled = false;
            buttonCancel.Enabled = false;
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDownload_Click(object sender, EventArgs e)
        {
            buttonDownload.Enabled = false;
            buttonCancel.Enabled = true;
            backgroundWorkerDownload.RunWorkerAsync();
        }

        /// <summary>
        /// 暂停下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPause_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 取消下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            backgroundWorkerDownload.CancelAsync();
            buttonDownload.Enabled = true;
            buttonCancel.Enabled = false;

            checkBoxWholeDownload.Enabled = false;
            numericUpDownWDStartZoom.Enabled = false;
            numericUpDownWDStopZoom.Enabled = false;
            checkBoxPartDownload.Enabled = false;
            numericUpDownPDStartZoom.Enabled = false;
            numericUpDownPDStopZoom.Enabled = false;
            numericUpDownNorth.Enabled = false;
            numericUpDownSouth.Enabled = false;
            numericUpDownEast.Enabled = false;
            numericUpDownWest.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            //下载指定图层的全部数据
            if (checkBoxWholeDownload.Checked)
            {
                for (int zoom = (int)numericUpDownWDStartZoom.Value; zoom <= (int)numericUpDownWDStopZoom.Value; zoom++)
                {
                    int half = (int)Math.Pow(2, zoom - 1);
                    for (int column = (-1) * half + 1; column < half; column++)
                    {
                        for (int row = (-1) * half + 1; row  < half; row++)
                        {
                            service.LoadMapTile(row, column, zoom, MapMode.Normal, LoadMapMode.Server);
                            textBoxLog.Invoke((Action)delegate ()
                            {
                                if (m_LogRowCount > MAX_LOG_ROW_COUNT)
                                {
                                    textBoxLog.Clear();
                                    m_LogRowCount = 0;
                                }
                                textBoxLog.AppendText("下载" + zoom + "_" + row + "_" + column + "完毕\n");
                                m_LogRowCount++;
                            });
                        }
                    }
                }
            }
            //下载指定图层的部分数据
            if (checkBoxPartDownload.Checked)
            {
                double west = (double)numericUpDownWest.Value;
                double north = (double)numericUpDownNorth.Value;
                double east = (double)numericUpDownEast.Value;
                double south = (double)numericUpDownSouth.Value;
                for (int zoom = (int)numericUpDownPDStartZoom.Value; zoom <= (int)numericUpDownPDStopZoom.Value; zoom++)
                {
                    PointF left_down = MapHelper.GetLocationByLatLng(new LatLngPoint(west, south), zoom); //左下角像素坐标
                    PointF right_up = MapHelper.GetLocationByLatLng(new LatLngPoint(east, north), zoom); //右上角像素坐标

                    int tile_left_down_x = (int)Math.Floor((left_down.X) / 256);  //左下角瓦片X坐标
                    int tile_left_down_y = (int)Math.Floor((left_down.Y) / 256);  //左下角瓦片Y坐标
                    int tile_right_up_x = (int)Math.Floor((right_up.X) / 256);    //右上角瓦片X坐标
                    int tile_right_up_y = (int)Math.Floor((right_up.Y) / 256);    //右上角瓦片Y坐标
                    for (int column = tile_left_down_x; column <= tile_right_up_x; ++column)
                    {
                        for (int row = tile_left_down_y; row <= tile_right_up_y; ++row)
                        {
                            service.LoadMapTile(column, row, zoom, MapMode.Normal, LoadMapMode.Server);
                            textBoxLog.Invoke((Action)delegate ()
                            {
                                if (m_LogRowCount > MAX_LOG_ROW_COUNT)
                                {
                                    textBoxLog.Clear();
                                    m_LogRowCount = 0;
                                }
                                textBoxLog.AppendText("下载" + zoom + "_" + row + "_" + column + "完毕\n");
                                m_LogRowCount++;
                            });
                        }
                    }
                }
            }
        }

        private void backgroundWorkerDownload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            //textBoxLog.AppendText(worker.)
        }

        private void backgroundWorkerDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonDownload.Enabled = true;
            buttonCancel.Enabled = false;

            checkBoxWholeDownload.Enabled = true;
            numericUpDownWDStartZoom.Enabled = checkBoxWholeDownload.Checked;
            numericUpDownWDStopZoom.Enabled = checkBoxWholeDownload.Checked;
            checkBoxPartDownload.Enabled = true;
            numericUpDownPDStartZoom.Enabled = checkBoxPartDownload.Checked;
            numericUpDownPDStopZoom.Enabled = checkBoxPartDownload.Checked;
            numericUpDownNorth.Enabled = checkBoxPartDownload.Checked;
            numericUpDownSouth.Enabled = checkBoxPartDownload.Checked;
            numericUpDownEast.Enabled = checkBoxPartDownload.Checked;
            numericUpDownWest.Enabled = checkBoxPartDownload.Checked;
        }

        /// <summary>
        /// 是否下载全部瓦片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxWholeDownload_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownWDStartZoom.Enabled = checkBoxWholeDownload.Checked;
            numericUpDownWDStopZoom.Enabled = checkBoxWholeDownload.Checked;
        }

        /// <summary>
        /// 是否下载部分瓦片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxPartDownload_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownPDStartZoom.Enabled = checkBoxPartDownload.Checked;
            numericUpDownPDStopZoom.Enabled = checkBoxPartDownload.Checked;
            numericUpDownNorth.Enabled = checkBoxPartDownload.Checked;
            numericUpDownSouth.Enabled = checkBoxPartDownload.Checked;
            numericUpDownEast.Enabled = checkBoxPartDownload.Checked;
            numericUpDownWest.Enabled = checkBoxPartDownload.Checked;
        }
    }
}
