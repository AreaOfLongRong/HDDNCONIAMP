using System.Windows.Forms;

namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    public partial class FormVideoFullScreen : Form
    {
        public FormVideoFullScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��ȡ�����
        /// </summary>
        public Panel PanelMain {
            get
            {
                return panelMain;
            }
        }

        private void panelMain_DoubleClick(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}