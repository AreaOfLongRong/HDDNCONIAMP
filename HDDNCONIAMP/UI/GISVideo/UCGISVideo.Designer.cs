namespace HDDNCONIAMP.UI.GISVideo
{
    partial class UCGISVideo
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
            this.collapsibleSplitContainerMain = new DevComponents.DotNetBar.Controls.CollapsibleSplitContainer();
            
            this.bMapControl2Main = new BMap.NET.WindowsForm.BMapControl2();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainerMain)).BeginInit();
            this.collapsibleSplitContainerMain.Panel1.SuspendLayout();
            this.collapsibleSplitContainerMain.Panel2.SuspendLayout();
            this.collapsibleSplitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // collapsibleSplitContainerMain
            // 
            this.collapsibleSplitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsibleSplitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.collapsibleSplitContainerMain.Name = "collapsibleSplitContainerMain";
            // 
            // collapsibleSplitContainerMain.Panel2
            // 
            this.collapsibleSplitContainerMain.Panel2.Controls.Add(this.bMapControl2Main);
            this.collapsibleSplitContainerMain.Panel2MinSize = 5;
            this.collapsibleSplitContainerMain.Size = new System.Drawing.Size(600, 439);
            this.collapsibleSplitContainerMain.SplitterDistance = 150;
            this.collapsibleSplitContainerMain.SplitterWidth = 10;
            this.collapsibleSplitContainerMain.TabIndex = 0;
           
            // 
            // bMapControl2Main
            // 
            this.bMapControl2Main.BDirectionBoard = null;
            this.bMapControl2Main.BPlaceBox = null;
            this.bMapControl2Main.BPlacesBoard = null;
            this.bMapControl2Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bMapControl2Main.LoadMode = BMap.NET.LoadMapMode.Cache;
            this.bMapControl2Main.Location = new System.Drawing.Point(0, 0);
            this.bMapControl2Main.Mode = BMap.NET.MapMode.Normal;
            this.bMapControl2Main.Name = "bMapControl2Main";
            this.bMapControl2Main.Size = new System.Drawing.Size(440, 439);
            this.bMapControl2Main.TabIndex = 0;
            this.bMapControl2Main.Zoom = 12;
            // 
            // UCGISVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.collapsibleSplitContainerMain);
            this.Name = "UCGISVideo";
            this.Size = new System.Drawing.Size(600, 439);
            this.collapsibleSplitContainerMain.Panel1.ResumeLayout(false);
            this.collapsibleSplitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainerMain)).EndInit();
            this.collapsibleSplitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.CollapsibleSplitContainer collapsibleSplitContainerMain;
        
        private BMap.NET.WindowsForm.BMapControl2 bMapControl2Main;
    }
}
