namespace HDDNCONIAMP.UI.Common
{
    partial class UCMeshList2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMeshList2));
            this.tableLayoutPanelMeshDeviceList = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxXSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.advTreeMeshList = new DevComponents.AdvTree.AdvTree();
            this.columnHeaderGroup = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeaderState = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeaderGPS = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeaderVideo = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeaderGPSTrack = new DevComponents.AdvTree.ColumnHeader();
            this.imageListMesh = new System.Windows.Forms.ImageList(this.components);
            this.nodeConnectorMain = new DevComponents.AdvTree.NodeConnector();
            this.elementStyleMain = new DevComponents.DotNetBar.ElementStyle();
            this.barDeviceList = new DevComponents.DotNetBar.Bar();
            this.buttonItemExpandAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemFoldAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemAddGroup = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemDeleteGroup = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemRefreshTree = new DevComponents.DotNetBar.ButtonItem();
            this.buttonXSearch = new DevComponents.DotNetBar.ButtonX();
            this.tableLayoutPanelMeshDeviceList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeMeshList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDeviceList)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMeshDeviceList
            // 
            this.tableLayoutPanelMeshDeviceList.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelMeshDeviceList.ColumnCount = 2;
            this.tableLayoutPanelMeshDeviceList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMeshDeviceList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMeshDeviceList.Controls.Add(this.textBoxXSearch, 0, 0);
            this.tableLayoutPanelMeshDeviceList.Controls.Add(this.advTreeMeshList, 0, 2);
            this.tableLayoutPanelMeshDeviceList.Controls.Add(this.barDeviceList, 0, 1);
            this.tableLayoutPanelMeshDeviceList.Controls.Add(this.buttonXSearch, 1, 0);
            this.tableLayoutPanelMeshDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMeshDeviceList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMeshDeviceList.Name = "tableLayoutPanelMeshDeviceList";
            this.tableLayoutPanelMeshDeviceList.RowCount = 3;
            this.tableLayoutPanelMeshDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMeshDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMeshDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMeshDeviceList.Size = new System.Drawing.Size(257, 410);
            this.tableLayoutPanelMeshDeviceList.TabIndex = 3;
            // 
            // textBoxXSearch
            // 
            this.textBoxXSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXSearch.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxXSearch.Border.Class = "TextBoxBorder";
            this.textBoxXSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXSearch.DisabledBackColor = System.Drawing.Color.White;
            this.textBoxXSearch.ForeColor = System.Drawing.Color.Black;
            this.textBoxXSearch.Location = new System.Drawing.Point(3, 4);
            this.textBoxXSearch.Name = "textBoxXSearch";
            this.textBoxXSearch.PreventEnterBeep = true;
            this.textBoxXSearch.Size = new System.Drawing.Size(221, 21);
            this.textBoxXSearch.TabIndex = 0;
            this.textBoxXSearch.WatermarkText = "<i>键入搜索</i>";
            this.textBoxXSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxXSearch_KeyPress);
            // 
            // advTreeMeshList
            // 
            this.advTreeMeshList.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeMeshList.AllowDrop = true;
            this.advTreeMeshList.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeMeshList.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeMeshList.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTreeMeshList.Columns.Add(this.columnHeaderGroup);
            this.advTreeMeshList.Columns.Add(this.columnHeaderState);
            this.advTreeMeshList.Columns.Add(this.columnHeaderGPS);
            this.advTreeMeshList.Columns.Add(this.columnHeaderVideo);
            this.advTreeMeshList.Columns.Add(this.columnHeaderGPSTrack);
            this.tableLayoutPanelMeshDeviceList.SetColumnSpan(this.advTreeMeshList, 2);
            this.advTreeMeshList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeMeshList.ExpandImage = global::HDDNCONIAMP.Properties.Resources.folder_expand_19;
            this.advTreeMeshList.ExpandImageCollapse = global::HDDNCONIAMP.Properties.Resources.folder_fold_17;
            this.advTreeMeshList.GridRowLines = true;
            this.advTreeMeshList.HotTracking = true;
            this.advTreeMeshList.ImageList = this.imageListMesh;
            this.advTreeMeshList.Location = new System.Drawing.Point(3, 63);
            this.advTreeMeshList.Name = "advTreeMeshList";
            this.advTreeMeshList.NodesConnector = this.nodeConnectorMain;
            this.advTreeMeshList.NodeStyle = this.elementStyleMain;
            this.advTreeMeshList.PathSeparator = ";";
            this.advTreeMeshList.SelectionPerCell = true;
            this.advTreeMeshList.Size = new System.Drawing.Size(251, 344);
            this.advTreeMeshList.Styles.Add(this.elementStyleMain);
            this.advTreeMeshList.TabIndex = 1;
            this.advTreeMeshList.Text = "advTree1";
            this.advTreeMeshList.AfterCellEditComplete += new DevComponents.AdvTree.CellEditEventHandler(this.advTreeMeshList_AfterCellEditComplete);
            this.advTreeMeshList.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeMeshList_NodeClick);
            // 
            // columnHeaderGroup
            // 
            this.columnHeaderGroup.Name = "columnHeaderGroup";
            this.columnHeaderGroup.Width.Absolute = 120;
            this.columnHeaderGroup.Width.AutoSizeMinHeader = true;
            // 
            // columnHeaderState
            // 
            this.columnHeaderState.Name = "columnHeaderState";
            this.columnHeaderState.Text = "状态";
            this.columnHeaderState.Width.Absolute = 40;
            // 
            // columnHeaderGPS
            // 
            this.columnHeaderGPS.Name = "columnHeaderGPS";
            this.columnHeaderGPS.Text = "GPS";
            this.columnHeaderGPS.Width.Absolute = 30;
            // 
            // columnHeaderVideo
            // 
            this.columnHeaderVideo.Name = "columnHeaderVideo";
            this.columnHeaderVideo.Text = "视频";
            this.columnHeaderVideo.Width.Absolute = 35;
            // 
            // columnHeaderGPSTrack
            // 
            this.columnHeaderGPSTrack.Name = "columnHeaderGPSTrack";
            this.columnHeaderGPSTrack.Text = "轨迹";
            this.columnHeaderGPSTrack.Width.Absolute = 50;
            // 
            // imageListMesh
            // 
            this.imageListMesh.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMesh.ImageStream")));
            this.imageListMesh.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMesh.Images.SetKeyName(0, "add_group_24.png");
            this.imageListMesh.Images.SetKeyName(1, "delete_group_24.png");
            this.imageListMesh.Images.SetKeyName(2, "expand_all_16.png");
            this.imageListMesh.Images.SetKeyName(3, "fold_all_16.png");
            this.imageListMesh.Images.SetKeyName(4, "folder_expand_19.png");
            this.imageListMesh.Images.SetKeyName(5, "folder_fold_17.png");
            this.imageListMesh.Images.SetKeyName(6, "bmap_drag_16.png");
            this.imageListMesh.Images.SetKeyName(7, "refresh_16.png");
            this.imageListMesh.Images.SetKeyName(8, "mesh_person_offline_32.png");
            this.imageListMesh.Images.SetKeyName(9, "gps_online_16.png");
            this.imageListMesh.Images.SetKeyName(10, "video_camera_32.png");
            this.imageListMesh.Images.SetKeyName(11, "gps_track_32.png");
            // 
            // nodeConnectorMain
            // 
            this.nodeConnectorMain.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyleMain
            // 
            this.elementStyleMain.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.elementStyleMain.Name = "elementStyleMain";
            this.elementStyleMain.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // barDeviceList
            // 
            this.barDeviceList.AntiAlias = true;
            this.tableLayoutPanelMeshDeviceList.SetColumnSpan(this.barDeviceList, 2);
            this.barDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barDeviceList.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.barDeviceList.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.barDeviceList.Images = this.imageListMesh;
            this.barDeviceList.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemExpandAll,
            this.buttonItemFoldAll,
            this.buttonItemAddGroup,
            this.buttonItemDeleteGroup,
            this.buttonItemRefreshTree});
            this.barDeviceList.Location = new System.Drawing.Point(3, 33);
            this.barDeviceList.Name = "barDeviceList";
            this.barDeviceList.Size = new System.Drawing.Size(251, 25);
            this.barDeviceList.Stretch = true;
            this.barDeviceList.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.barDeviceList.TabIndex = 3;
            this.barDeviceList.TabStop = false;
            this.barDeviceList.Text = "bar1";
            // 
            // buttonItemExpandAll
            // 
            this.buttonItemExpandAll.ImageIndex = 3;
            this.buttonItemExpandAll.Name = "buttonItemExpandAll";
            this.buttonItemExpandAll.Text = "全部展开";
            this.buttonItemExpandAll.Tooltip = "全部展开";
            this.buttonItemExpandAll.Click += new System.EventHandler(this.buttonItemExpandAll_Click);
            // 
            // buttonItemFoldAll
            // 
            this.buttonItemFoldAll.ImageIndex = 2;
            this.buttonItemFoldAll.Name = "buttonItemFoldAll";
            this.buttonItemFoldAll.Text = "全部折叠";
            this.buttonItemFoldAll.Tooltip = "全部折叠";
            this.buttonItemFoldAll.Click += new System.EventHandler(this.buttonItemFoldAll_Click);
            // 
            // buttonItemAddGroup
            // 
            this.buttonItemAddGroup.BeginGroup = true;
            this.buttonItemAddGroup.ImageIndex = 0;
            this.buttonItemAddGroup.Name = "buttonItemAddGroup";
            this.buttonItemAddGroup.Text = "添加分组";
            this.buttonItemAddGroup.Tooltip = "添加分组";
            this.buttonItemAddGroup.Visible = false;
            this.buttonItemAddGroup.Click += new System.EventHandler(this.buttonItemAddGroup_Click);
            // 
            // buttonItemDeleteGroup
            // 
            this.buttonItemDeleteGroup.ImageIndex = 1;
            this.buttonItemDeleteGroup.Name = "buttonItemDeleteGroup";
            this.buttonItemDeleteGroup.Text = "删除分组";
            this.buttonItemDeleteGroup.Tooltip = "删除分组";
            this.buttonItemDeleteGroup.Visible = false;
            this.buttonItemDeleteGroup.Click += new System.EventHandler(this.buttonItemDeleteGroup_Click);
            // 
            // buttonItemRefreshTree
            // 
            this.buttonItemRefreshTree.BeginGroup = true;
            this.buttonItemRefreshTree.ImageIndex = 7;
            this.buttonItemRefreshTree.Name = "buttonItemRefreshTree";
            this.buttonItemRefreshTree.Click += new System.EventHandler(this.buttonItemRefreshTree_Click);
            // 
            // buttonXSearch
            // 
            this.buttonXSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXSearch.Image = global::HDDNCONIAMP.Properties.Resources.search_32;
            this.buttonXSearch.Location = new System.Drawing.Point(230, 3);
            this.buttonXSearch.Name = "buttonXSearch";
            this.buttonXSearch.Size = new System.Drawing.Size(24, 23);
            this.buttonXSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXSearch.TabIndex = 4;
            this.buttonXSearch.Click += new System.EventHandler(this.buttonXSearch_Click);
            // 
            // UCMeshList2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMeshDeviceList);
            this.Name = "UCMeshList2";
            this.Size = new System.Drawing.Size(257, 410);
            this.Load += new System.EventHandler(this.UCMeshList2_Load);
            this.tableLayoutPanelMeshDeviceList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeMeshList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDeviceList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMeshDeviceList;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXSearch;
        private DevComponents.AdvTree.AdvTree advTreeMeshList;
        private System.Windows.Forms.ImageList imageListMesh;
        private DevComponents.AdvTree.NodeConnector nodeConnectorMain;
        private DevComponents.DotNetBar.ElementStyle elementStyleMain;
        private DevComponents.DotNetBar.Bar barDeviceList;
        private DevComponents.DotNetBar.ButtonItem buttonItemExpandAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemFoldAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemAddGroup;
        private DevComponents.DotNetBar.ButtonItem buttonItemDeleteGroup;
        private DevComponents.DotNetBar.ButtonX buttonXSearch;
        private DevComponents.AdvTree.ColumnHeader columnHeaderGroup;
        private DevComponents.AdvTree.ColumnHeader columnHeaderGPS;
        private DevComponents.AdvTree.ColumnHeader columnHeaderVideo;
        private DevComponents.AdvTree.ColumnHeader columnHeaderState;
        private DevComponents.DotNetBar.ButtonItem buttonItemRefreshTree;
        private DevComponents.AdvTree.ColumnHeader columnHeaderGPSTrack;
    }
}
