namespace BMap.NET.WindowsForm
{
    partial class UCVideosControl
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
            this.buttonX265Video = new DevComponents.DotNetBar.ButtonX();
            this.pictureBoxClose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonX265Video
            // 
            this.buttonX265Video.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX265Video.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX265Video.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonX265Video.Location = new System.Drawing.Point(3, 3);
            this.buttonX265Video.Name = "buttonX265Video";
            this.buttonX265Video.Size = new System.Drawing.Size(64, 64);
            this.buttonX265Video.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX265Video.TabIndex = 0;
            this.buttonX265Video.Click += new System.EventHandler(this.buttonX265Video_Click);
            // 
            // pictureBoxClose
            // 
            this.pictureBoxClose.Image = global::BMap.NET.WindowsForm.Properties.Resources.ico_screenshotCancel;
            this.pictureBoxClose.Location = new System.Drawing.Point(73, 3);
            this.pictureBoxClose.Name = "pictureBoxClose";
            this.pictureBoxClose.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxClose.TabIndex = 2;
            this.pictureBoxClose.TabStop = false;
            this.pictureBoxClose.Click += new System.EventHandler(this.pictureBoxClose_Click);
            // 
            // UCVideosControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBoxClose);
            this.Controls.Add(this.buttonX265Video);
            this.Name = "UCVideosControl";
            this.Size = new System.Drawing.Size(94, 71);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonX265Video;
        private System.Windows.Forms.PictureBox pictureBoxClose;
    }
}
