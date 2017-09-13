namespace HDDNCONIAMP.UI.Common
{
    partial class UCMeshList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMeshList));
            this.tableLayoutPanelDeviceList = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxXSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.advTreeMeshList = new DevComponents.AdvTree.AdvTree();
            this.imageListMesh = new System.Windows.Forms.ImageList(this.components);
            this.nodeDefaultGroup = new DevComponents.AdvTree.Node();
            this.nodeConnectorMain = new DevComponents.AdvTree.NodeConnector();
            this.elementStyleMain = new DevComponents.DotNetBar.ElementStyle();
            this.barDeviceList = new DevComponents.DotNetBar.Bar();
            this.buttonItemExpandAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemFoldAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemAddGroup = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemDeleteGroup = new DevComponents.DotNetBar.ButtonItem();
            this.buttonXSearch = new DevComponents.DotNetBar.ButtonX();
            this.tableLayoutPanelDeviceList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeMeshList)).BeginInit();
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
            this.tableLayoutPanelDeviceList.Controls.Add(this.advTreeMeshList, 0, 2);
            this.tableLayoutPanelDeviceList.Controls.Add(this.barDeviceList, 0, 1);
            this.tableLayoutPanelDeviceList.Controls.Add(this.buttonXSearch, 1, 0);
            this.tableLayoutPanelDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDeviceList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDeviceList.Name = "tableLayoutPanelDeviceList";
            this.tableLayoutPanelDeviceList.RowCount = 3;
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDeviceList.Size = new System.Drawing.Size(252, 411);
            this.tableLayoutPanelDeviceList.TabIndex = 2;
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
            this.textBoxXSearch.Size = new System.Drawing.Size(216, 21);
            this.textBoxXSearch.TabIndex = 0;
            this.textBoxXSearch.WatermarkText = "<i>键入搜索</i>";
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
            this.advTreeMeshList.CellEdit = true;
            this.tableLayoutPanelDeviceList.SetColumnSpan(this.advTreeMeshList, 2);
            this.advTreeMeshList.ColumnsVisible = false;
            this.advTreeMeshList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeMeshList.ExpandImage = global::HDDNCONIAMP.Properties.Resources.folder_expand_19;
            this.advTreeMeshList.ExpandImageCollapse = global::HDDNCONIAMP.Properties.Resources.folder_fold_17;
            this.advTreeMeshList.GridColumnLines = false;
            this.advTreeMeshList.HotTracking = true;
            this.advTreeMeshList.ImageList = this.imageListMesh;
            this.advTreeMeshList.Location = new System.Drawing.Point(3, 63);
            this.advTreeMeshList.Name = "advTreeMeshList";
            this.advTreeMeshList.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.nodeDefaultGroup});
            this.advTreeMeshList.NodesConnector = this.nodeConnectorMain;
            this.advTreeMeshList.NodeStyle = this.elementStyleMain;
            this.advTreeMeshList.PathSeparator = ";";
            this.advTreeMeshList.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.FullRowSelect;
            this.advTreeMeshList.Size = new System.Drawing.Size(246, 345);
            this.advTreeMeshList.Styles.Add(this.elementStyleMain);
            this.advTreeMeshList.TabIndex = 1;
            this.advTreeMeshList.Text = "advTree1";
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
            this.barDeviceList.Images = this.imageListMesh;
            this.barDeviceList.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemExpandAll,
            this.buttonItemFoldAll,
            this.buttonItemAddGroup,
            this.buttonItemDeleteGroup});
            this.barDeviceList.Location = new System.Drawing.Point(3, 33);
            this.barDeviceList.Name = "barDeviceList";
            this.barDeviceList.Size = new System.Drawing.Size(246, 25);
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
            // 
            // buttonItemFoldAll
            // 
            this.buttonItemFoldAll.ImageIndex = 2;
            this.buttonItemFoldAll.Name = "buttonItemFoldAll";
            this.buttonItemFoldAll.Text = "全部折叠";
            this.buttonItemFoldAll.Tooltip = "全部折叠";
            // 
            // buttonItemAddGroup
            // 
            this.buttonItemAddGroup.BeginGroup = true;
            this.buttonItemAddGroup.ImageIndex = 0;
            this.buttonItemAddGroup.Name = "buttonItemAddGroup";
            this.buttonItemAddGroup.Text = "添加分组";
            this.buttonItemAddGroup.Tooltip = "添加分组";
            // 
            // buttonItemDeleteGroup
            // 
            this.buttonItemDeleteGroup.ImageIndex = 1;
            this.buttonItemDeleteGroup.Name = "buttonItemDeleteGroup";
            this.buttonItemDeleteGroup.Text = "删除分组";
            this.buttonItemDeleteGroup.Tooltip = "删除分组";
            // 
            // buttonXSearch
            // 
            this.buttonXSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXSearch.Image = global::HDDNCONIAMP.Properties.Resources.search_32;
            this.buttonXSearch.Location = new System.Drawing.Point(225, 3);
            this.buttonXSearch.Name = "buttonXSearch";
            this.buttonXSearch.Size = new System.Drawing.Size(24, 23);
            this.buttonXSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXSearch.TabIndex = 4;
            // 
            // UCMeshList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelDeviceList);
            this.Name = "UCMeshList";
            this.Size = new System.Drawing.Size(252, 411);
            this.tableLayoutPanelDeviceList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeMeshList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDeviceList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDeviceList;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXSearch;
        private DevComponents.AdvTree.AdvTree advTreeMeshList;
        private System.Windows.Forms.ImageList imageListMesh;
        private DevComponents.AdvTree.Node nodeDefaultGroup;
        private DevComponents.AdvTree.NodeConnector nodeConnectorMain;
        private DevComponents.DotNetBar.ElementStyle elementStyleMain;
        private DevComponents.DotNetBar.Bar barDeviceList;
        private DevComponents.DotNetBar.ButtonItem buttonItemExpandAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemFoldAll;
        private DevComponents.DotNetBar.ButtonItem buttonItemAddGroup;
        private DevComponents.DotNetBar.ButtonItem buttonItemDeleteGroup;
        private DevComponents.DotNetBar.ButtonX buttonXSearch;
    }
}
