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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCUserSettings));
            this.superTabControlUserSettings = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel3 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.buttonX4 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.superTabItemAuthorityManage = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItemLogManage = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel4 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItemSoftwareSetting = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItemPasswordModify = new DevComponents.DotNetBar.SuperTabItem();
            this.tableLayoutPanelLogManage = new System.Windows.Forms.TableLayoutPanel();
            this.advTreeLogList = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.barLog = new DevComponents.DotNetBar.Bar();
            this.buttonItemExpandAll = new DevComponents.DotNetBar.ButtonItem();
            this.superTabControlLogs = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel5 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.imageListUserSettings = new System.Windows.Forms.ImageList(this.components);
            this.buttonItemFoldAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemRefresh = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemExportLogs = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControlUserSettings)).BeginInit();
            this.superTabControlUserSettings.SuspendLayout();
            this.superTabControlPanel3.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
            this.tableLayoutPanelLogManage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLogList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControlLogs)).BeginInit();
            this.superTabControlLogs.SuspendLayout();
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
            this.superTabControlUserSettings.Controls.Add(this.superTabControlPanel1);
            this.superTabControlUserSettings.Controls.Add(this.superTabControlPanel3);
            this.superTabControlUserSettings.Controls.Add(this.superTabControlPanel4);
            this.superTabControlUserSettings.Controls.Add(this.superTabControlPanel2);
            this.superTabControlUserSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlUserSettings.Location = new System.Drawing.Point(0, 0);
            this.superTabControlUserSettings.Name = "superTabControlUserSettings";
            this.superTabControlUserSettings.ReorderTabsEnabled = true;
            this.superTabControlUserSettings.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControlUserSettings.SelectedTabIndex = 0;
            this.superTabControlUserSettings.Size = new System.Drawing.Size(567, 430);
            this.superTabControlUserSettings.TabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.superTabControlUserSettings.TabIndex = 0;
            this.superTabControlUserSettings.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItemLogManage,
            this.superTabItemPasswordModify,
            this.superTabItemAuthorityManage,
            this.superTabItemSoftwareSetting});
            // 
            // superTabControlPanel3
            // 
            this.superTabControlPanel3.Controls.Add(this.buttonX4);
            this.superTabControlPanel3.Controls.Add(this.buttonX3);
            this.superTabControlPanel3.Controls.Add(this.buttonX2);
            this.superTabControlPanel3.Controls.Add(this.buttonX1);
            this.superTabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel3.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel3.Name = "superTabControlPanel3";
            this.superTabControlPanel3.Size = new System.Drawing.Size(567, 402);
            this.superTabControlPanel3.TabIndex = 0;
            this.superTabControlPanel3.TabItem = this.superTabItemAuthorityManage;
            // 
            // buttonX4
            // 
            this.buttonX4.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX4.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX4.Location = new System.Drawing.Point(246, 190);
            this.buttonX4.Name = "buttonX4";
            this.buttonX4.Size = new System.Drawing.Size(75, 23);
            this.buttonX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX4.TabIndex = 3;
            this.buttonX4.Text = "查询用户";
            this.buttonX4.Click += new System.EventHandler(this.buttonX4_Click);
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX3.Location = new System.Drawing.Point(113, 184);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(75, 23);
            this.buttonX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX3.TabIndex = 2;
            this.buttonX3.Text = "删除用户";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2.Location = new System.Drawing.Point(113, 135);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(75, 23);
            this.buttonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX2.TabIndex = 1;
            this.buttonX2.Text = "修改密码";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(113, 78);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 0;
            this.buttonX1.Text = "插入用户";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // superTabItemAuthorityManage
            // 
            this.superTabItemAuthorityManage.AttachedControl = this.superTabControlPanel3;
            this.superTabItemAuthorityManage.GlobalItem = false;
            this.superTabItemAuthorityManage.Name = "superTabItemAuthorityManage";
            this.superTabItemAuthorityManage.Text = "权限管理";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.tableLayoutPanelLogManage);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(567, 402);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItemLogManage;
            // 
            // superTabItemLogManage
            // 
            this.superTabItemLogManage.AttachedControl = this.superTabControlPanel1;
            this.superTabItemLogManage.GlobalItem = false;
            this.superTabItemLogManage.Name = "superTabItemLogManage";
            this.superTabItemLogManage.Text = "日志管理";
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
            // superTabItemSoftwareSetting
            // 
            this.superTabItemSoftwareSetting.AttachedControl = this.superTabControlPanel4;
            this.superTabItemSoftwareSetting.GlobalItem = false;
            this.superTabItemSoftwareSetting.Name = "superTabItemSoftwareSetting";
            this.superTabItemSoftwareSetting.Text = "软件设置";
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
            // superTabItemPasswordModify
            // 
            this.superTabItemPasswordModify.AttachedControl = this.superTabControlPanel2;
            this.superTabItemPasswordModify.GlobalItem = false;
            this.superTabItemPasswordModify.Name = "superTabItemPasswordModify";
            this.superTabItemPasswordModify.Text = "密码修改";
            // 
            // tableLayoutPanelLogManage
            // 
            this.tableLayoutPanelLogManage.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelLogManage.ColumnCount = 2;
            this.tableLayoutPanelLogManage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelLogManage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanelLogManage.Controls.Add(this.advTreeLogList, 0, 1);
            this.tableLayoutPanelLogManage.Controls.Add(this.barLog, 0, 0);
            this.tableLayoutPanelLogManage.Controls.Add(this.superTabControlLogs, 1, 1);
            this.tableLayoutPanelLogManage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLogManage.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLogManage.Name = "tableLayoutPanelLogManage";
            this.tableLayoutPanelLogManage.RowCount = 2;
            this.tableLayoutPanelLogManage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelLogManage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLogManage.Size = new System.Drawing.Size(567, 402);
            this.tableLayoutPanelLogManage.TabIndex = 0;
            // 
            // advTreeLogList
            // 
            this.advTreeLogList.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeLogList.AllowDrop = true;
            this.advTreeLogList.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeLogList.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeLogList.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTreeLogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeLogList.Location = new System.Drawing.Point(3, 33);
            this.advTreeLogList.MultiSelect = true;
            this.advTreeLogList.Name = "advTreeLogList";
            this.advTreeLogList.NodesConnector = this.nodeConnector1;
            this.advTreeLogList.NodeStyle = this.elementStyle1;
            this.advTreeLogList.PathSeparator = ";";
            this.advTreeLogList.Size = new System.Drawing.Size(135, 366);
            this.advTreeLogList.Styles.Add(this.elementStyle1);
            this.advTreeLogList.TabIndex = 0;
            this.advTreeLogList.Text = "advTree1";
            this.advTreeLogList.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeLogList_NodeDoubleClick);
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // barLog
            // 
            this.barLog.AntiAlias = true;
            this.tableLayoutPanelLogManage.SetColumnSpan(this.barLog, 2);
            this.barLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barLog.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.barLog.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.barLog.Images = this.imageListUserSettings;
            this.barLog.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemExpandAll,
            this.buttonItemFoldAll,
            this.buttonItemRefresh,
            this.buttonItemExportLogs});
            this.barLog.Location = new System.Drawing.Point(3, 3);
            this.barLog.Name = "barLog";
            this.barLog.Size = new System.Drawing.Size(561, 24);
            this.barLog.Stretch = true;
            this.barLog.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.barLog.TabIndex = 2;
            this.barLog.TabStop = false;
            // 
            // buttonItemExpandAll
            // 
            this.buttonItemExpandAll.ImageIndex = 1;
            this.buttonItemExpandAll.Name = "buttonItemExpandAll";
            this.buttonItemExpandAll.Text = "展开所有";
            this.buttonItemExpandAll.Tooltip = "展开所有";
            this.buttonItemExpandAll.Click += new System.EventHandler(this.buttonItemExpandAll_Click);
            // 
            // superTabControlLogs
            // 
            this.superTabControlLogs.CloseButtonOnTabsVisible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControlLogs.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControlLogs.ControlBox.MenuBox.Name = "";
            this.superTabControlLogs.ControlBox.Name = "";
            this.superTabControlLogs.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControlLogs.ControlBox.MenuBox,
            this.superTabControlLogs.ControlBox.CloseBox});
            this.superTabControlLogs.Controls.Add(this.superTabControlPanel5);
            this.superTabControlLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlLogs.Location = new System.Drawing.Point(144, 33);
            this.superTabControlLogs.Name = "superTabControlLogs";
            this.superTabControlLogs.ReorderTabsEnabled = true;
            this.superTabControlLogs.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControlLogs.SelectedTabIndex = -1;
            this.superTabControlLogs.Size = new System.Drawing.Size(420, 366);
            this.superTabControlLogs.TabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.superTabControlLogs.TabIndex = 3;
            this.superTabControlLogs.Text = "superTabControl1";
            // 
            // superTabControlPanel5
            // 
            this.superTabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel5.Location = new System.Drawing.Point(0, 10);
            this.superTabControlPanel5.Name = "superTabControlPanel5";
            this.superTabControlPanel5.Size = new System.Drawing.Size(420, 356);
            this.superTabControlPanel5.TabIndex = 1;
            // 
            // imageListUserSettings
            // 
            this.imageListUserSettings.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListUserSettings.ImageStream")));
            this.imageListUserSettings.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListUserSettings.Images.SetKeyName(0, "expand_all_16.png");
            this.imageListUserSettings.Images.SetKeyName(1, "fold_all_16.png");
            this.imageListUserSettings.Images.SetKeyName(2, "folder_expand_19.png");
            this.imageListUserSettings.Images.SetKeyName(3, "folder_fold_17.png");
            this.imageListUserSettings.Images.SetKeyName(4, "refresh_16.png");
            // 
            // buttonItemFoldAll
            // 
            this.buttonItemFoldAll.ImageIndex = 0;
            this.buttonItemFoldAll.Name = "buttonItemFoldAll";
            this.buttonItemFoldAll.Tooltip = "折叠所有";
            this.buttonItemFoldAll.Click += new System.EventHandler(this.buttonItemFoldAll_Click);
            // 
            // buttonItemRefresh
            // 
            this.buttonItemRefresh.BeginGroup = true;
            this.buttonItemRefresh.ImageIndex = 4;
            this.buttonItemRefresh.Name = "buttonItemRefresh";
            this.buttonItemRefresh.Tooltip = "刷新日志列表";
            this.buttonItemRefresh.Click += new System.EventHandler(this.buttonItemRefresh_Click);
            // 
            // buttonItemExportLogs
            // 
            this.buttonItemExportLogs.BeginGroup = true;
            this.buttonItemExportLogs.Name = "buttonItemExportLogs";
            this.buttonItemExportLogs.Text = "导出日志";
            this.buttonItemExportLogs.Click += new System.EventHandler(this.buttonItemExportLogs_Click);
            // 
            // UCUserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.superTabControlUserSettings);
            this.Name = "UCUserSettings";
            this.Size = new System.Drawing.Size(567, 430);
            this.Load += new System.EventHandler(this.UCUserSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControlUserSettings)).EndInit();
            this.superTabControlUserSettings.ResumeLayout(false);
            this.superTabControlPanel3.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            this.tableLayoutPanelLogManage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLogList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControlLogs)).EndInit();
            this.superTabControlLogs.ResumeLayout(false);
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
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        private DevComponents.DotNetBar.ButtonX buttonX3;
        private DevComponents.DotNetBar.ButtonX buttonX4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLogManage;
        private DevComponents.AdvTree.AdvTree advTreeLogList;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.Bar barLog;
        private DevComponents.DotNetBar.ButtonItem buttonItemExpandAll;
        private DevComponents.DotNetBar.SuperTabControl superTabControlLogs;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel5;
        private System.Windows.Forms.ImageList imageListUserSettings;
        private DevComponents.DotNetBar.ButtonItem buttonItemFoldAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemRefresh;
        private DevComponents.DotNetBar.ButtonItem buttonItemExportLogs;
    }
}
