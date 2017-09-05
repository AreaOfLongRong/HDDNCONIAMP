using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    public partial class UCGridVideo : UserControl
    {
        public UCGridVideo()
        {
            InitializeComponent();
        }

        public Panel GetFirstPanel()
        {
            return panelFirst;
        }
    }
}
