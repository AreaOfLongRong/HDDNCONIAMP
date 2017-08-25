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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelDeviceList = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelBDMap = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxXSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.advTreeDeviceList = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.barTools = new DevComponents.DotNetBar.Bar();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.bMapControlMain = new BMap.NET.WindowsForm.BMapControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelDeviceList.SuspendLayout();
            this.tableLayoutPanelBDMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeDeviceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTools)).BeginInit();
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
            this.tableLayoutPanelDeviceList.ColumnCount = 1;
            this.tableLayoutPanelDeviceList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDeviceList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDeviceList.Controls.Add(this.textBoxXSearch, 0, 0);
            this.tableLayoutPanelDeviceList.Controls.Add(this.advTreeDeviceList, 0, 1);
            this.tableLayoutPanelDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDeviceList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDeviceList.Name = "tableLayoutPanelDeviceList";
            this.tableLayoutPanelDeviceList.RowCount = 2;
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelDeviceList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDeviceList.Size = new System.Drawing.Size(150, 361);
            this.tableLayoutPanelDeviceList.TabIndex = 0;
            // 
            // tableLayoutPanelBDMap
            // 
            this.tableLayoutPanelBDMap.ColumnCount = 1;
            this.tableLayoutPanelBDMap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBDMap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBDMap.Controls.Add(this.barTools, 0, 0);
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
            // textBoxXSearch
            // 
            this.textBoxXSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.textBoxXSearch.Border.Class = "TextBoxBorder";
            this.textBoxXSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXSearch.Location = new System.Drawing.Point(3, 9);
            this.textBoxXSearch.Name = "textBoxXSearch";
            this.textBoxXSearch.PreventEnterBeep = true;
            this.textBoxXSearch.Size = new System.Drawing.Size(144, 21);
            this.textBoxXSearch.TabIndex = 0;
            this.textBoxXSearch.WatermarkText = "<i>键入搜索</i>";
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
            this.advTreeDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeDeviceList.Location = new System.Drawing.Point(3, 43);
            this.advTreeDeviceList.Name = "advTreeDeviceList";
            this.advTreeDeviceList.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.advTreeDeviceList.NodesConnector = this.nodeConnector1;
            this.advTreeDeviceList.NodeStyle = this.elementStyle1;
            this.advTreeDeviceList.PathSeparator = ";";
            this.advTreeDeviceList.Size = new System.Drawing.Size(144, 315);
            this.advTreeDeviceList.Styles.Add(this.elementStyle1);
            this.advTreeDeviceList.TabIndex = 1;
            this.advTreeDeviceList.Text = "advTree1";
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
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
            // barTools
            // 
            this.barTools.AntiAlias = true;
            this.barTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barTools.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.barTools.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.barTools.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.barTools.Location = new System.Drawing.Point(3, 3);
            this.barTools.Name = "barTools";
            this.barTools.Size = new System.Drawing.Size(397, 26);
            this.barTools.Stretch = true;
            this.barTools.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.barTools.TabIndex = 0;
            this.barTools.TabStop = false;
            this.barTools.Text = "bar1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "buttonItem1";
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
            this.tableLayoutPanelBDMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeDeviceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTools)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDeviceList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBDMap;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXSearch;
        private DevComponents.AdvTree.AdvTree advTreeDeviceList;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.Bar barTools;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private BMap.NET.WindowsForm.BMapControl bMapControlMain;
    }
}
