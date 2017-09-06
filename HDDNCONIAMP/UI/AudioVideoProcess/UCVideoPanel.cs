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
    public partial class UCVideoPanel : Panel
    {
        public UCVideoPanel()
        {
            InitializeComponent();

            BackgroundImage = Properties.Resources.video_no_signal_512;
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        public void ClearVideo()
        {
            
        }
    }
}
