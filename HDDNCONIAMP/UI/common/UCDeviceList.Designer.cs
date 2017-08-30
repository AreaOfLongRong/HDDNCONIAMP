namespace HDDNCONIAMP.UI.common
{
    partial class UCDeviceList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDeviceList));
            this.tableLayoutPanelDeviceList = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxXSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.advTreeDeviceList = new DevComponents.AdvTree.AdvTree();
            this.imageListGISVideo = new System.Windows.Forms.ImageList(this.components);
            this.nodeDefaultGroup = new DevComponents.AdvTree.Node();
            this.node1 = new DevComponents.AdvTree.Node();
            this.node2 = new DevComponents.AdvTree.Node();
            this.node3 = new DevComponents.AdvTree.Node();
            this.node4 = new DevComponents.AdvTree.Node();
            this.node5 = new DevComponents.AdvTree.Node();
            this.node6 = new DevComponents.AdvTree.Node();
            this.nodeConnectorMain = new DevComponents.AdvTree.NodeConnector();
            this.elementStyleMain = new DevComponents.DotNetBar.ElementStyle();
            this.barDeviceList = new DevComponents.DotNetBar.Bar();
            this.buttonItemExpandAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemFoldAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemAddGroup = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemDeleteGroup = new DevComponents.DotNetBar.ButtonItem();
            this.buttonXSearch = new DevComponents.DotNetBar.ButtonX();
            this.tableLayoutPanelDeviceList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeDeviceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDeviceList)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelDeviceList
            // 
            this.tableLayoutPanelDeviceList.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelDeviceList.ColumnCount = 2;
            this.tableLayoutPanelDeviceList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDeviceList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDeviceList.Controls.Add(this.textBoxXSearch, 0, 0);
            this.tableLayoutPanelDeviceList.Controls.Add(this.advTreeDeviceList, 0, 2);
            this.tableLayoutPanelDeviceList.Controls.Add(this.barDeviceList, 0, 1);
            this.tableLayoutPanelDeviceList.Controls.Add(this.buttonXSearch, 1, 0);
            this.tableLayoutPanelDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDeviceList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDeviceList.Name = "tableLayoutPanelDeviceList";
            this.tableLayoutPanelDeviceList.RowCount = 3;
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDeviceList.Size = new System.Drawing.Size(190, 391);
            this.tableLayoutPanelDeviceList.TabIndex = 1;
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
            this.textBoxXSearch.Size = new System.Drawing.Size(154, 21);
            this.textBoxXSearch.TabIndex = 0;
            this.textBoxXSearch.WatermarkText = "<i>键入搜索</i>";
            this.textBoxXSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxXSearch_KeyPress);
            // 
            // advTreeDeviceList
            // 
            this.advTreeDeviceList.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeDeviceList.AllowDrop = true;
            this.advTreeDeviceList.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeDeviceList.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeDeviceList.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTreeDeviceList.CellEdit = true;
            this.tableLayoutPanelDeviceList.SetColumnSpan(this.advTreeDeviceList, 2);
            this.advTreeDeviceList.ColumnsVisible = false;
            this.advTreeDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeDeviceList.ExpandImage = global::HDDNCONIAMP.Properties.Resources.folder_expand_19;
            this.advTreeDeviceList.ExpandImageCollapse = global::HDDNCONIAMP.Properties.Resources.folder_fold_17;
            this.advTreeDeviceList.GridColumnLines = false;
            this.advTreeDeviceList.HotTracking = true;
            this.advTreeDeviceList.ImageList = this.imageListGISVideo;
            this.advTreeDeviceList.Location = new System.Drawing.Point(3, 63);
            this.advTreeDeviceList.Name = "advTreeDeviceList";
            this.advTreeDeviceList.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.nodeDefaultGroup,
            this.node4});
            this.advTreeDeviceList.NodesConnector = this.nodeConnectorMain;
            this.advTreeDeviceList.NodeStyle = this.elementStyleMain;
            this.advTreeDeviceList.PathSeparator = ";";
            this.advTreeDeviceList.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.FullRowSelect;
            this.advTreeDeviceList.Size = new System.Drawing.Size(184, 325);
            this.advTreeDeviceList.Styles.Add(this.elementStyleMain);
            this.advTreeDeviceList.TabIndex = 1;
            this.advTreeDeviceList.Text = "advTree1";
            // 
            // imageListGISVideo
            // 
            this.imageListGISVideo.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListGISVideo.ImageStream")));
            this.imageListGISVideo.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListGISVideo.Images.SetKeyName(0, "add_group_24.png");
            this.imageListGISVideo.Images.SetKeyName(1, "delete_group_24.png");
            this.imageListGISVideo.Images.SetKeyName(2, "expand_all_16.png");
            this.imageListGISVideo.Images.SetKeyName(3, "fold_all_16.png");
            this.imageListGISVideo.Images.SetKeyName(4, "folder_expand_19.png");
            this.imageListGISVideo.Images.SetKeyName(5, "folder_fold_17.png");
            this.imageListGISVideo.Images.SetKeyName(6, "camera_offline_16.png");
            this.imageListGISVideo.Images.SetKeyName(7, "camera_offline_32.png");
            this.imageListGISVideo.Images.SetKeyName(8, "camera_online_16.png");
            this.imageListGISVideo.Images.SetKeyName(9, "camera_online_32.png");
            this.imageListGISVideo.Images.SetKeyName(10, "bmap_drag_16.png");
            this.imageListGISVideo.Images.SetKeyName(11, "bmap_zoom_in_16.png");
            this.imageListGISVideo.Images.SetKeyName(12, "bmap_zoom_out_16.png");
            this.imageListGISVideo.Images.SetKeyName(13, "bmap_locate_16.png");
            this.imageListGISVideo.Images.SetKeyName(14, "bmap_measure_16.png");
            // 
            // nodeDefaultGroup
            // 
            this.nodeDefaultGroup.Editable = false;
            this.nodeDefaultGroup.Expanded = true;
            this.nodeDefaultGroup.ImageExpandedIndex = 4;
            this.nodeDefaultGroup.ImageIndex = 5;
            this.nodeDefaultGroup.Name = "nodeDefaultGroup";
            this.nodeDefaultGroup.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1,
            this.node2,
            this.node3});
            this.nodeDefaultGroup.Text = "默认分组";
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // node2
            // 
            this.node2.Expanded = true;
            this.node2.Name = "node2";
            this.node2.Text = "node2";
            // 
            // node3
            // 
            this.node3.Expanded = true;
            this.node3.Name = "node3";
            this.node3.Text = "test3";
            // 
            // node4
            // 
            this.node4.Expanded = true;
            this.node4.Name = "node4";
            this.node4.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node5,
            this.node6});
            this.node4.Text = "node4";
            // 
            // node5
            // 
            this.node5.Expanded = true;
            this.node5.Name = "node5";
            this.node5.Text = "node5";
            // 
            // node6
            // 
            this.node6.Expanded = true;
            this.node6.Name = "node6";
            this.node6.Text = "test6";
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
            this.tableLayoutPanelDeviceList.SetColumnSpan(this.barDeviceList, 2);
            this.barDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barDeviceList.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.barDeviceList.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.barDeviceList.Images = this.imageListGISVideo;
            this.barDeviceList.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemExpandAll,
            this.buttonItemFoldAll,
            this.buttonItemAddGroup,
            this.buttonItemDeleteGroup});
            this.barDeviceList.Location = new System.Drawing.Point(3, 33);
            this.barDeviceList.Name = "barDeviceList";
            this.barDeviceList.Size = new System.Drawing.Size(184, 25);
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
            this.buttonItemAddGroup.Click += new System.EventHandler(this.buttonItemAddGroup_Click);
            // 
            // buttonItemDeleteGroup
            // 
            this.buttonItemDeleteGroup.ImageIndex = 1;
            this.buttonItemDeleteGroup.Name = "buttonItemDeleteGroup";
            this.buttonItemDeleteGroup.Text = "删除分组";
            this.buttonItemDeleteGroup.Tooltip = "删除分组";
            this.buttonItemDeleteGroup.Click += new System.EventHandler(this.buttonItemDeleteGroup_Click);
            // 
            // buttonXSearch
            // 
            this.buttonXSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXSearch.Image = global::HDDNCONIAMP.Properties.Resources.search_32;
            this.buttonXSearch.Location = new System.Drawing.Point(163, 3);
            this.buttonXSearch.Name = "buttonXSearch";
            this.buttonXSearch.Size = new System.Drawing.Size(24, 23);
            this.buttonXSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXSearch.TabIndex = 4;
            this.buttonXSearch.Click += new System.EventHandler(this.buttonXSearch_Click);
            // 
            // UCDeviceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelDeviceList);
            this.Name = "UCDeviceList";
            this.Size = new System.Drawing.Size(190, 391);
            this.Load += new System.EventHandler(this.UCDeviceList_Load);
            this.tableLayoutPanelDeviceList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeDeviceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDeviceList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDeviceList;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXSearch;
        private DevComponents.AdvTree.AdvTree advTreeDeviceList;
        private DevComponents.AdvTree.Node nodeDefaultGroup;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.Node node3;
        private DevComponents.AdvTree.Node node4;
        private DevComponents.AdvTree.Node node5;
        private DevComponents.AdvTree.Node node6;
        private DevComponents.AdvTree.NodeConnector nodeConnectorMain;
        private DevComponents.DotNetBar.ElementStyle elementStyleMain;
        private DevComponents.DotNetBar.Bar barDeviceList;
        private DevComponents.DotNetBar.ButtonItem buttonItemExpandAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemFoldAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemAddGroup;
        private DevComponents.DotNetBar.ButtonItem buttonItemDeleteGroup;
        private DevComponents.DotNetBar.ButtonX buttonXSearch;
        private System.Windows.Forms.ImageList imageListGISVideo;
    }
}
