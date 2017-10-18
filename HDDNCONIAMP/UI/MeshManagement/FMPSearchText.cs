using System;
using System.Windows.Forms;

namespace HDDNCONIAMP.UI.MeshManagement
{
    public partial class FMPSearchText : Form
    {
        public FMPSearchText()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取Mesh预案检索的文本
        /// </summary>
        public string MPSearchText
        {
            get
            {
                return textBoxXSearchText.Text.Trim();
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

        }
    }
}
