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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCGISVideo));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelDeviceList = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxXSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.advTreeDeviceList = new DevComponents.AdvTree.AdvTree();
            this.nodeConnectorMain = new DevComponents.AdvTree.NodeConnector();
            this.elementStyleMain = new DevComponents.DotNetBar.ElementStyle();
            this.barDeviceList = new DevComponents.DotNetBar.Bar();
            this.tableLayoutPanelBDMap = new System.Windows.Forms.TableLayoutPanel();
            this.barToolsBDMap = new DevComponents.DotNetBar.Bar();
            this.buttonItemBMapDrag = new DevComponents.DotNetBar.ButtonItem();
            this.bMapControlMain = new BMap.NET.WindowsForm.BMapControl();
            this.nodeSearchResult = new DevComponents.AdvTree.Node();
            this.imageListGISVideo = new System.Windows.Forms.ImageList(this.components);
            this.nodeDefaultGroup = new DevComponents.AdvTree.Node();
            this.pictureBoxSearch = new System.Windows.Forms.PictureBox();
            this.buttonItemExpandAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemFoldAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemAddGroup = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemDeleteGroup = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemBMapZoomIn = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemBMapZoomout = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemBMapMeasure = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemBMapLocate = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelDeviceList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeDeviceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDeviceList)).BeginInit();
            this.tableLayoutPanelBDMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barToolsBDMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanelDeviceList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelBDMap);
            this.splitContainerMain.Size = new System.Drawing.Size(555, 361);
            this.splitContainerMain.SplitterDistance = 150;
            this.splitContainerMain.SplitterWidth = 2;
            this.splitContainerMain.TabIndex = 1;
            // 
            // tableLayoutPanelDeviceList
            // 
            this.tableLayoutPanelDeviceList.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelDeviceList.ColumnCount = 2;
            this.tableLayoutPanelDeviceList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDeviceList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDeviceList.Controls.Add(this.textBoxXSearch, 0, 0);
            this.tableLayoutPanelDeviceList.Controls.Add(this.advTreeDeviceList, 0, 2);
            this.tableLayoutPanelDeviceList.Controls.Add(this.pictureBoxSearch, 1, 0);
            this.tableLayoutPanelDeviceList.Controls.Add(this.barDeviceList, 0, 1);
            this.tableLayoutPanelDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDeviceList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDeviceList.Name = "tableLayoutPanelDeviceList";
            this.tableLayoutPanelDeviceList.RowCount = 3;
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDeviceList.Size = new System.Drawing.Size(150, 361);
            this.tableLayoutPanelDeviceList.TabIndex = 0;
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
            this.textBoxXSearch.Size = new System.Drawing.Size(114, 21);
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
            this.advTreeDeviceList.GridColumnLines = false;
            this.advTreeDeviceList.HotTracking = true;
            this.advTreeDeviceList.ImageList = this.imageListGISVideo;
            this.advTreeDeviceList.Location = new System.Drawing.Point(3, 63);
            this.advTreeDeviceList.Name = "advTreeDeviceList";
            this.advTreeDeviceList.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.nodeDefaultGroup,
            this.nodeSearchResult});
            this.advTreeDeviceList.NodesConnector = this.nodeConnectorMain;
            this.advTreeDeviceList.NodeStyle = this.elementStyleMain;
            this.advTreeDeviceList.PathSeparator = ";";
            this.advTreeDeviceList.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.FullRowSelect;
            this.advTreeDeviceList.Size = new System.Drawing.Size(144, 295);
            this.advTreeDeviceList.Styles.Add(this.elementStyleMain);
            this.advTreeDeviceList.TabIndex = 1;
            this.advTreeDeviceList.Text = "advTree1";
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
            this.barDeviceList.Size = new System.Drawing.Size(144, 24);
            this.barDeviceList.Stretch = true;
            this.barDeviceList.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.barDeviceList.TabIndex = 3;
            this.barDeviceList.TabStop = false;
            this.barDeviceList.Text = "bar1";
            // 
            // tableLayoutPanelBDMap
            // 
            this.tableLayoutPanelBDMap.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelBDMap.ColumnCount = 1;
            this.tableLayoutPanelBDMap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBDMap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBDMap.Controls.Add(this.barToolsBDMap, 0, 0);
            this.tableLayoutPanelBDMap.Controls.Add(this.bMapControlMain, 0, 1);
            this.tableLayoutPanelBDMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBDMap.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBDMap.Name = "tableLayoutPanelBDMap";
            this.tableLayoutPanelBDMap.RowCount = 2;
            this.tableLayoutPanelBDMap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelBDMap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBDMap.Size = new System.Drawing.Size(403, 361);
            this.tableLayoutPanelBDMap.TabIndex = 0;
            // 
            // barToolsBDMap
            // 
            this.barToolsBDMap.AntiAlias = true;
            this.barToolsBDMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barToolsBDMap.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.barToolsBDMap.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.barToolsBDMap.Images = this.imageListGISVideo;
            this.barToolsBDMap.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemBMapDrag,
            this.buttonItemBMapZoomIn,
            this.buttonItemBMapZoomout,
            this.buttonItemBMapMeasure,
            this.buttonItemBMapLocate});
            this.barToolsBDMap.Location = new System.Drawing.Point(3, 3);
            this.barToolsBDMap.Name = "barToolsBDMap";
            this.barToolsBDMap.Size = new System.Drawing.Size(397, 34);
            this.barToolsBDMap.Stretch = true;
            this.barToolsBDMap.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.barToolsBDMap.TabIndex = 0;
            this.barToolsBDMap.TabStop = false;
            this.barToolsBDMap.Text = "bar1";
            // 
            // buttonItemBMapDrag
            // 
            this.buttonItemBMapDrag.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemBMapDrag.ImageIndex = 10;
            this.buttonItemBMapDrag.Name = "buttonItemBMapDrag";
            this.buttonItemBMapDrag.Text = "平移";
            this.buttonItemBMapDrag.Click += new System.EventHandler(this.buttonItemBMapDrag_Click);
            // 
            // bMapControlMain
            // 
            this.bMapControlMain.BDirectionBoard = null;
            this.bMapControlMain.BPlaceBox = null;
            this.bMapControlMain.BPlacesBoard = null;
            this.bMapControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bMapControlMain.LoadMode = BMap.NET.LoadMapMode.Cache;
            this.bMapControlMain.Location = new System.Drawing.Point(3, 43);
            this.bMapControlMain.Mode = BMap.NET.MapMode.Normal;
            this.bMapControlMain.Name = "bMapControlMain";
            this.bMapControlMain.Size = new System.Drawing.Size(397, 315);
            this.bMapControlMain.TabIndex = 1;
            this.bMapControlMain.Zoom = 12;
            // 
            // nodeSearchResult
            // 
            this.nodeSearchResult.Editable = false;
            this.nodeSearchResult.Expanded = true;
            this.nodeSearchResult.ImageExpandedIndex = 4;
            this.nodeSearchResult.ImageIndex = 5;
            this.nodeSearchResult.Name = "nodeSearchResult";
            this.nodeSearchResult.Text = "检索结果";
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
            this.nodeDefaultGroup.Text = "默认分组";
            // 
            // pictureBoxSearch
            // 
            this.pictureBoxSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxSearch.Image = global::HDDNCONIAMP.Properties.Resources.search_32;
            this.pictureBoxSearch.Location = new System.Drawing.Point(123, 3);
            this.pictureBoxSearch.Name = "pictureBoxSearch";
            this.pictureBoxSearch.Size = new System.Drawing.Size(24, 24);
            this.pictureBoxSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSearch.TabIndex = 2;
            this.pictureBoxSearch.TabStop = false;
            this.pictureBoxSearch.Click += new System.EventHandler(this.pictureBoxSearch_Click);
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
            this.buttonItemAddGroup.Image = global::HDDNCONIAMP.Properties.Resources.add_group_24;
            this.buttonItemAddGroup.Name = "buttonItemAddGroup";
            this.buttonItemAddGroup.Text = "添加分组";
            this.buttonItemAddGroup.Tooltip = "添加分组";
            this.buttonItemAddGroup.Click += new System.EventHandler(this.buttonItemAddGroup_Click);
            // 
            // buttonItemDeleteGroup
            // 
            this.buttonItemDeleteGroup.Image = global::HDDNCONIAMP.Properties.Resources.delete_group_24;
            this.buttonItemDeleteGroup.Name = "buttonItemDeleteGroup";
            this.buttonItemDeleteGroup.Text = "删除分组";
            this.buttonItemDeleteGroup.Tooltip = "删除分组";
            this.buttonItemDeleteGroup.Click += new System.EventHandler(this.buttonItemDeleteGroup_Click);
            // 
            // buttonItemBMapZoomIn
            // 
            this.buttonItemBMapZoomIn.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemBMapZoomIn.ImageIndex = 11;
            this.buttonItemBMapZoomIn.Name = "buttonItemBMapZoomIn";
            this.buttonItemBMapZoomIn.Text = "放大";
            this.buttonItemBMapZoomIn.Click += new System.EventHandler(this.buttonItemBMapZoomIn_Click);
            // 
            // buttonItemBMapZoomout
            // 
            this.buttonItemBMapZoomout.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemBMapZoomout.ImageIndex = 12;
            this.buttonItemBMapZoomout.Name = "buttonItemBMapZoomout";
            this.buttonItemBMapZoomout.Text = "缩小";
            this.buttonItemBMapZoomout.Click += new System.EventHandler(this.buttonItemBMapZoomout_Click);
            // 
            // buttonItemBMapMeasure
            // 
            this.buttonItemBMapMeasure.BeginGroup = true;
            this.buttonItemBMapMeasure.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemBMapMeasure.ImageIndex = 14;
            this.buttonItemBMapMeasure.Name = "buttonItemBMapMeasure";
            this.buttonItemBMapMeasure.Text = "测量";
            // 
            // buttonItemBMapLocate
            // 
            this.buttonItemBMapLocate.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemBMapLocate.ImageIndex = 13;
            this.buttonItemBMapLocate.Name = "buttonItemBMapLocate";
            this.buttonItemBMapLocate.Text = "定位";
            // 
            // UCGISVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Name = "UCGISVideo";
            this.Size = new System.Drawing.Size(555, 361);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelDeviceList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeDeviceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDeviceList)).EndInit();
            this.tableLayoutPanelBDMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barToolsBDMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDeviceList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBDMap;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXSearch;
        private DevComponents.AdvTree.AdvTree advTreeDeviceList;
        private DevComponents.AdvTree.NodeConnector nodeConnectorMain;
        private DevComponents.DotNetBar.ElementStyle elementStyleMain;
        private DevComponents.DotNetBar.Bar barToolsBDMap;
        private DevComponents.DotNetBar.ButtonItem buttonItemBMapDrag;
        private BMap.NET.WindowsForm.BMapControl bMapControlMain;
        private DevComponents.AdvTree.Node nodeDefaultGroup;
        private System.Windows.Forms.PictureBox pictureBoxSearch;
        private DevComponents.DotNetBar.Bar barDeviceList;
        private DevComponents.DotNetBar.ButtonItem buttonItemExpandAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemFoldAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemAddGroup;
        private DevComponents.DotNetBar.ButtonItem buttonItemDeleteGroup;
        private DevComponents.AdvTree.Node nodeSearchResult;
        private System.Windows.Forms.ImageList imageListGISVideo;
        private DevComponents.DotNetBar.ButtonItem buttonItemBMapZoomIn;
        private DevComponents.DotNetBar.ButtonItem buttonItemBMapZoomout;
        private DevComponents.DotNetBar.ButtonItem buttonItemBMapMeasure;
        private DevComponents.DotNetBar.ButtonItem buttonItemBMapLocate;
    }
}
