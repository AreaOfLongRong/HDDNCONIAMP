namespace HDDNCONIAMP.UI.UserSettings
{
    partial class UCUserSettings
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
            this.superTabControlUserSettings = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabItemLogManage = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItemPasswordModify = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItemAuthorityManage = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel3 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItemSoftwareSetting = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel4 = new DevComponents.DotNetBar.SuperTabControlPanel();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControlUserSettings)).BeginInit();
            this.superTabControlUserSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // superTabControlUserSettings
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControlUserSettings.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControlUserSettings.ControlBox.MenuBox.Name = "";
            this.superTabControlUserSettings.ControlBox.Name = "";
            this.superTabControlUserSettings.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControlUserSettings.ControlBox.MenuBox,
            this.superTabControlUserSettings.ControlBox.CloseBox});
            this.superTabControlUserSettings.Controls.Add(this.superTabControlPanel4);
            this.superTabControlUserSettings.Controls.Add(this.superTabControlPanel3);
            this.superTabControlUserSettings.Controls.Add(this.superTabControlPanel2);
            this.superTabControlUserSettings.Controls.Add(this.superTabControlPanel1);
            this.superTabControlUserSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlUserSettings.Location = new System.Drawing.Point(0, 0);
            this.superTabControlUserSettings.Name = "superTabControlUserSettings";
            this.superTabControlUserSettings.ReorderTabsEnabled = true;
            this.superTabControlUserSettings.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControlUserSettings.SelectedTabIndex = 3;
            this.superTabControlUserSettings.Size = new System.Drawing.Size(567, 430);
            this.superTabControlUserSettings.TabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.superTabControlUserSettings.TabIndex = 0;
            this.superTabControlUserSettings.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItemLogManage,
            this.superTabItemPasswordModify,
            this.superTabItemAuthorityManage,
            this.superTabItemSoftwareSetting});
            // 
            // superTabItemLogManage
            // 
            this.superTabItemLogManage.AttachedControl = this.superTabControlPanel1;
            this.superTabItemLogManage.GlobalItem = false;
            this.superTabItemLogManage.Name = "superTabItemLogManage";
            this.superTabItemLogManage.Text = "日志管理";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(567, 402);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItemLogManage;
            // 
            // superTabItemPasswordModify
            // 
            this.superTabItemPasswordModify.AttachedControl = this.superTabControlPanel2;
            this.superTabItemPasswordModify.GlobalItem = false;
            this.superTabItemPasswordModify.Name = "superTabItemPasswordModify";
            this.superTabItemPasswordModify.Text = "密码修改";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(567, 402);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.superTabItemPasswordModify;
            // 
            // superTabItemAuthorityManage
            // 
            this.superTabItemAuthorityManage.AttachedControl = this.superTabControlPanel3;
            this.superTabItemAuthorityManage.GlobalItem = false;
            this.superTabItemAuthorityManage.Name = "superTabItemAuthorityManage";
            this.superTabItemAuthorityManage.Text = "权限管理";
            // 
            // superTabControlPanel3
            // 
            this.superTabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel3.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel3.Name = "superTabControlPanel3";
            this.superTabControlPanel3.Size = new System.Drawing.Size(567, 402);
            this.superTabControlPanel3.TabIndex = 0;
            this.superTabControlPanel3.TabItem = this.superTabItemAuthorityManage;
            // 
            // superTabItemSoftwareSetting
            // 
            this.superTabItemSoftwareSetting.AttachedControl = this.superTabControlPanel4;
            this.superTabItemSoftwareSetting.GlobalItem = false;
            this.superTabItemSoftwareSetting.Name = "superTabItemSoftwareSetting";
            this.superTabItemSoftwareSetting.Text = "软件设置";
            // 
            // superTabControlPanel4
            // 
            this.superTabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel4.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel4.Name = "superTabControlPanel4";
            this.superTabControlPanel4.Size = new System.Drawing.Size(567, 402);
            this.superTabControlPanel4.TabIndex = 0;
            this.superTabControlPanel4.TabItem = this.superTabItemSoftwareSetting;
            // 
            // UCUserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.superTabControlUserSettings);
            this.Name = "UCUserSettings";
            this.Size = new System.Drawing.Size(567, 430);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControlUserSettings)).EndInit();
            this.superTabControlUserSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperTabControl superTabControlUserSettings;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel4;
        private DevComponents.DotNetBar.SuperTabItem superTabItemSoftwareSetting;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel3;
        private DevComponents.DotNetBar.SuperTabItem superTabItemAuthorityManage;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem superTabItemPasswordModify;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem superTabItemLogManage;
    }
}
