namespace BMap.NET.WinformDemo
{
    partial class FormDownloadBDMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxPartDownload = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownWest = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownEast = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownSouth = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownNorth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxPartDownload = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownPDStartZoom = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPDStopZoom = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.groupBoxWholeDownload = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownWDStopZoom = new System.Windows.Forms.NumericUpDown();
            this.checkBoxWholeDownload = new System.Windows.Forms.CheckBox();
            this.labelStartZoom = new System.Windows.Forms.Label();
            this.labelStopZoom = new System.Windows.Forms.Label();
            this.numericUpDownWDStartZoom = new System.Windows.Forms.NumericUpDown();
            this.backgroundWorkerDownload = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxPartDownload.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWest)).BeginInit();
            this.tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEast)).BeginInit();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSouth)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNorth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDStartZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDStopZoom)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxWholeDownload.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWDStopZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWDStartZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxPartDownload, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxLog, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxWholeDownload, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(609, 498);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBoxPartDownload
            // 
            this.groupBoxPartDownload.Controls.Add(this.tableLayoutPanel4);
            this.groupBoxPartDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPartDownload.Location = new System.Drawing.Point(3, 53);
            this.groupBoxPartDownload.Name = "groupBoxPartDownload";
            this.groupBoxPartDownload.Size = new System.Drawing.Size(603, 194);
            this.groupBoxPartDownload.TabIndex = 3;
            this.groupBoxPartDownload.TabStop = false;
            this.groupBoxPartDownload.Text = "部分下载";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel8, 4, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel7, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxPartDownload, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.numericUpDownPDStartZoom, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.numericUpDownPDStopZoom, 1, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(597, 174);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.numericUpDownWest, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label5, 1, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(133, 61);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(149, 52);
            this.tableLayoutPanel5.TabIndex = 10;
            // 
            // numericUpDownWest
            // 
            this.numericUpDownWest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownWest.DecimalPlaces = 4;
            this.numericUpDownWest.Enabled = false;
            this.numericUpDownWest.Location = new System.Drawing.Point(3, 29);
            this.numericUpDownWest.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownWest.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownWest.Name = "numericUpDownWest";
            this.numericUpDownWest.Size = new System.Drawing.Size(123, 21);
            this.numericUpDownWest.TabIndex = 5;
            this.numericUpDownWest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "西经：";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(132, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "°";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Controls.Add(this.numericUpDownEast, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label7, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(443, 61);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(151, 52);
            this.tableLayoutPanel8.TabIndex = 9;
            // 
            // numericUpDownEast
            // 
            this.numericUpDownEast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownEast.DecimalPlaces = 4;
            this.numericUpDownEast.Enabled = false;
            this.numericUpDownEast.Location = new System.Drawing.Point(3, 29);
            this.numericUpDownEast.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownEast.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownEast.Name = "numericUpDownEast";
            this.numericUpDownEast.Size = new System.Drawing.Size(125, 21);
            this.numericUpDownEast.TabIndex = 9;
            this.numericUpDownEast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "°";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(125, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "东经：";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Controls.Add(this.numericUpDownSouth, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.label8, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(288, 119);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(149, 52);
            this.tableLayoutPanel7.TabIndex = 8;
            // 
            // numericUpDownSouth
            // 
            this.numericUpDownSouth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSouth.DecimalPlaces = 4;
            this.numericUpDownSouth.Enabled = false;
            this.numericUpDownSouth.Location = new System.Drawing.Point(3, 29);
            this.numericUpDownSouth.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownSouth.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.numericUpDownSouth.Name = "numericUpDownSouth";
            this.numericUpDownSouth.Size = new System.Drawing.Size(123, 21);
            this.numericUpDownSouth.TabIndex = 9;
            this.numericUpDownSouth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(132, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "°";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 12);
            this.label9.TabIndex = 8;
            this.label9.Text = "南纬：";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label6, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.numericUpDownNorth, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(288, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(149, 52);
            this.tableLayoutPanel6.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "北纬：";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(132, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "°";
            // 
            // numericUpDownNorth
            // 
            this.numericUpDownNorth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownNorth.DecimalPlaces = 4;
            this.numericUpDownNorth.Enabled = false;
            this.numericUpDownNorth.Location = new System.Drawing.Point(3, 29);
            this.numericUpDownNorth.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownNorth.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.numericUpDownNorth.Name = "numericUpDownNorth";
            this.numericUpDownNorth.Size = new System.Drawing.Size(123, 21);
            this.numericUpDownNorth.TabIndex = 8;
            this.numericUpDownNorth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "结束层级：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxPartDownload
            // 
            this.checkBoxPartDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxPartDownload.AutoSize = true;
            this.checkBoxPartDownload.Location = new System.Drawing.Point(3, 22);
            this.checkBoxPartDownload.Name = "checkBoxPartDownload";
            this.checkBoxPartDownload.Size = new System.Drawing.Size(74, 14);
            this.checkBoxPartDownload.TabIndex = 1;
            this.checkBoxPartDownload.UseVisualStyleBackColor = true;
            this.checkBoxPartDownload.CheckedChanged += new System.EventHandler(this.checkBoxPartDownload_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "起始层级：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownPDStartZoom
            // 
            this.numericUpDownPDStartZoom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDownPDStartZoom.Enabled = false;
            this.numericUpDownPDStartZoom.Location = new System.Drawing.Point(83, 76);
            this.numericUpDownPDStartZoom.Maximum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.numericUpDownPDStartZoom.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownPDStartZoom.Name = "numericUpDownPDStartZoom";
            this.numericUpDownPDStartZoom.Size = new System.Drawing.Size(44, 21);
            this.numericUpDownPDStartZoom.TabIndex = 4;
            this.numericUpDownPDStartZoom.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // numericUpDownPDStopZoom
            // 
            this.numericUpDownPDStopZoom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDownPDStopZoom.Enabled = false;
            this.numericUpDownPDStopZoom.Location = new System.Drawing.Point(83, 134);
            this.numericUpDownPDStopZoom.Maximum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.numericUpDownPDStopZoom.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownPDStopZoom.Name = "numericUpDownPDStopZoom";
            this.numericUpDownPDStopZoom.Size = new System.Drawing.Size(44, 21);
            this.numericUpDownPDStopZoom.TabIndex = 5;
            this.numericUpDownPDStopZoom.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.buttonPause, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonDownload, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 253);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(603, 34);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // buttonPause
            // 
            this.buttonPause.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonPause.Location = new System.Drawing.Point(216, 5);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(170, 23);
            this.buttonPause.TabIndex = 2;
            this.buttonPause.Text = "暂停";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonDownload.Location = new System.Drawing.Point(19, 5);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(163, 23);
            this.buttonDownload.TabIndex = 0;
            this.buttonDownload.Text = "下载";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.Location = new System.Drawing.Point(417, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(170, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLog.Location = new System.Drawing.Point(3, 293);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(603, 202);
            this.textBoxLog.TabIndex = 1;
            // 
            // groupBoxWholeDownload
            // 
            this.groupBoxWholeDownload.Controls.Add(this.tableLayoutPanel3);
            this.groupBoxWholeDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxWholeDownload.Location = new System.Drawing.Point(3, 3);
            this.groupBoxWholeDownload.Name = "groupBoxWholeDownload";
            this.groupBoxWholeDownload.Size = new System.Drawing.Size(603, 44);
            this.groupBoxWholeDownload.TabIndex = 2;
            this.groupBoxWholeDownload.TabStop = false;
            this.groupBoxWholeDownload.Text = "全部下载";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.numericUpDownWDStopZoom, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxWholeDownload, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelStartZoom, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelStopZoom, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.numericUpDownWDStartZoom, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(597, 24);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // numericUpDownWDStopZoom
            // 
            this.numericUpDownWDStopZoom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDownWDStopZoom.Location = new System.Drawing.Point(456, 3);
            this.numericUpDownWDStopZoom.Maximum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.numericUpDownWDStopZoom.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownWDStopZoom.Name = "numericUpDownWDStopZoom";
            this.numericUpDownWDStopZoom.Size = new System.Drawing.Size(49, 21);
            this.numericUpDownWDStopZoom.TabIndex = 4;
            this.numericUpDownWDStopZoom.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // checkBoxWholeDownload
            // 
            this.checkBoxWholeDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxWholeDownload.AutoSize = true;
            this.checkBoxWholeDownload.Checked = true;
            this.checkBoxWholeDownload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWholeDownload.Location = new System.Drawing.Point(3, 5);
            this.checkBoxWholeDownload.Name = "checkBoxWholeDownload";
            this.checkBoxWholeDownload.Size = new System.Drawing.Size(24, 14);
            this.checkBoxWholeDownload.TabIndex = 0;
            this.checkBoxWholeDownload.UseVisualStyleBackColor = true;
            this.checkBoxWholeDownload.CheckedChanged += new System.EventHandler(this.checkBoxWholeDownload_CheckedChanged);
            // 
            // labelStartZoom
            // 
            this.labelStartZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStartZoom.AutoSize = true;
            this.labelStartZoom.Location = new System.Drawing.Point(33, 6);
            this.labelStartZoom.Name = "labelStartZoom";
            this.labelStartZoom.Size = new System.Drawing.Size(135, 12);
            this.labelStartZoom.TabIndex = 1;
            this.labelStartZoom.Text = "起始层级：";
            this.labelStartZoom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelStopZoom
            // 
            this.labelStopZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStopZoom.AutoSize = true;
            this.labelStopZoom.Location = new System.Drawing.Point(315, 6);
            this.labelStopZoom.Name = "labelStopZoom";
            this.labelStopZoom.Size = new System.Drawing.Size(135, 12);
            this.labelStopZoom.TabIndex = 2;
            this.labelStopZoom.Text = "结束层级：";
            this.labelStopZoom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownWDStartZoom
            // 
            this.numericUpDownWDStartZoom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDownWDStartZoom.Location = new System.Drawing.Point(174, 3);
            this.numericUpDownWDStartZoom.Maximum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.numericUpDownWDStartZoom.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownWDStartZoom.Name = "numericUpDownWDStartZoom";
            this.numericUpDownWDStartZoom.Size = new System.Drawing.Size(49, 21);
            this.numericUpDownWDStartZoom.TabIndex = 3;
            this.numericUpDownWDStartZoom.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // backgroundWorkerDownload
            // 
            this.backgroundWorkerDownload.WorkerSupportsCancellation = true;
            this.backgroundWorkerDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerDownload_DoWork);
            this.backgroundWorkerDownload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerDownload_ProgressChanged);
            this.backgroundWorkerDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerDownload_RunWorkerCompleted);
            // 
            // FormDownloadBDMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 510);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormDownloadBDMap";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "下载百度地图缓存";
            this.Load += new System.EventHandler(this.FormDownloadBDMap_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxPartDownload.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWest)).EndInit();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEast)).EndInit();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSouth)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNorth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDStartZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPDStopZoom)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBoxWholeDownload.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWDStopZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWDStartZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.ComponentModel.BackgroundWorker backgroundWorkerDownload;
        private System.Windows.Forms.GroupBox groupBoxPartDownload;
        private System.Windows.Forms.GroupBox groupBoxWholeDownload;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox checkBoxWholeDownload;
        private System.Windows.Forms.Label labelStartZoom;
        private System.Windows.Forms.Label labelStopZoom;
        private System.Windows.Forms.NumericUpDown numericUpDownWDStartZoom;
        private System.Windows.Forms.NumericUpDown numericUpDownWDStopZoom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxPartDownload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownPDStartZoom;
        private System.Windows.Forms.NumericUpDown numericUpDownPDStopZoom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.NumericUpDown numericUpDownWest;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownEast;
        private System.Windows.Forms.NumericUpDown numericUpDownSouth;
        private System.Windows.Forms.NumericUpDown numericUpDownNorth;
    }
}