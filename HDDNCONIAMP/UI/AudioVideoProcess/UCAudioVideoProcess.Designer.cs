namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    partial class UCAudioVideoProcess
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.collapsibleSplitContainer1 = new DevComponents.DotNetBar.Controls.CollapsibleSplitContainer();
            this.ucGridVideoMain = new HDDNCONIAMP.UI.AudioVideoProcess.UCGridVideo();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).BeginInit();
            this.collapsibleSplitContainer1.Panel2.SuspendLayout();
            this.collapsibleSplitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // collapsibleSplitContainer1
            // 
            this.collapsibleSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsibleSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.collapsibleSplitContainer1.Name = "collapsibleSplitContainer1";
            // 
            // collapsibleSplitContainer1.Panel2
            // 
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.ucGridVideoMain);
            this.collapsibleSplitContainer1.Panel2MinSize = 5;
            this.collapsibleSplitContainer1.Size = new System.Drawing.Size(617, 420);
            this.collapsibleSplitContainer1.SplitterDistance = 150;
            this.collapsibleSplitContainer1.SplitterWidth = 10;
            this.collapsibleSplitContainer1.TabIndex = 0;
            // 
            // ucGridVideoMain
            // 
            this.ucGridVideoMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGridVideoMain.Location = new System.Drawing.Point(0, 0);
            this.ucGridVideoMain.Name = "ucGridVideoMain";
            this.ucGridVideoMain.Size = new System.Drawing.Size(457, 420);
            this.ucGridVideoMain.TabIndex = 0;
            // 
            // UCAudioVideoProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.collapsibleSplitContainer1);
            this.Name = "UCAudioVideoProcess";
            this.Size = new System.Drawing.Size(617, 420);
            this.collapsibleSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).EndInit();
            this.collapsibleSplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.CollapsibleSplitContainer collapsibleSplitContainer1;
        private UCGridVideo ucGridVideoMain;
    }
}
