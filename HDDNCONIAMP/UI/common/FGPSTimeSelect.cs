using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace HDDNCONIAMP.UI.Common
{
    public partial class FGPSTimeSelect : DevComponents.DotNetBar.OfficeForm
    {
        public FGPSTimeSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取或设置开始时间
        /// </summary>
        public DateTime StartDateTime
        {
            get
            {
                return dateTimeInputStart.Value;
            }
            set {
                dateTimeInputStart.Value = value;
            }
        }

        /// <summary>
        /// 获取或设置结束时间
        /// </summary>
        public DateTime StopDateTime
        {
            get
            {
                return dateTimeInputStop.Value;
            }
            set
            {
                dateTimeInputStop.Value = value;
            }
        }

        private void buttonXLookGPSRoute_Click(object sender, EventArgs e)
        {

        }
    }
}