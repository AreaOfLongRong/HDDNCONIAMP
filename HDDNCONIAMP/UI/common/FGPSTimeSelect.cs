using System;

namespace HDDNCONIAMP.UI.Common
{
    public partial class FGPSTimeSelect : DevComponents.DotNetBar.OfficeForm
    {
        public FGPSTimeSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��ȡ�����ÿ�ʼʱ��
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
        /// ��ȡ�����ý���ʱ��
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