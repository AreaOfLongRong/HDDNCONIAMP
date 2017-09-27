namespace HDDNCONIAMP.UI.Common
{
    partial class FGPSTimeSelect
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.dateTimeInputStart = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.dateTimeInputStop = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.buttonXLookGPSRoute = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputStop)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(13, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "起始时间：";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(13, 42);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "结束时间：";
            // 
            // dateTimeInputStart
            // 
            // 
            // 
            // 
            this.dateTimeInputStart.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimeInputStart.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStart.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTimeInputStart.ButtonDropDown.Visible = true;
            this.dateTimeInputStart.IsPopupCalendarOpen = false;
            this.dateTimeInputStart.Location = new System.Drawing.Point(94, 14);
            // 
            // 
            // 
            // 
            // 
            // 
            this.dateTimeInputStart.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStart.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateTimeInputStart.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStart.MonthCalendar.DisplayMonth = new System.DateTime(2017, 9, 1, 0, 0, 0, 0);
            this.dateTimeInputStart.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            // 
            // 
            // 
            this.dateTimeInputStart.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimeInputStart.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputStart.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimeInputStart.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStart.MonthCalendar.TodayButtonVisible = true;
            this.dateTimeInputStart.Name = "dateTimeInputStart";
            this.dateTimeInputStart.Size = new System.Drawing.Size(200, 21);
            this.dateTimeInputStart.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTimeInputStart.TabIndex = 2;
            // 
            // dateTimeInputStop
            // 
            // 
            // 
            // 
            this.dateTimeInputStop.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimeInputStop.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStop.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTimeInputStop.ButtonDropDown.Visible = true;
            this.dateTimeInputStop.IsPopupCalendarOpen = false;
            this.dateTimeInputStop.Location = new System.Drawing.Point(94, 43);
            // 
            // 
            // 
            // 
            // 
            // 
            this.dateTimeInputStop.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStop.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateTimeInputStop.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimeInputStop.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimeInputStop.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputStop.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimeInputStop.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimeInputStop.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimeInputStop.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimeInputStop.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStop.MonthCalendar.DisplayMonth = new System.DateTime(2017, 9, 1, 0, 0, 0, 0);
            this.dateTimeInputStop.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            // 
            // 
            // 
            this.dateTimeInputStop.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimeInputStop.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputStop.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimeInputStop.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStop.MonthCalendar.TodayButtonVisible = true;
            this.dateTimeInputStop.Name = "dateTimeInputStop";
            this.dateTimeInputStop.Size = new System.Drawing.Size(200, 21);
            this.dateTimeInputStop.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTimeInputStop.TabIndex = 3;
            // 
            // buttonXLookGPSRoute
            // 
            this.buttonXLookGPSRoute.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXLookGPSRoute.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXLookGPSRoute.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonXLookGPSRoute.Location = new System.Drawing.Point(221, 81);
            this.buttonXLookGPSRoute.Name = "buttonXLookGPSRoute";
            this.buttonXLookGPSRoute.Size = new System.Drawing.Size(73, 27);
            this.buttonXLookGPSRoute.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXLookGPSRoute.TabIndex = 4;
            this.buttonXLookGPSRoute.Text = "查  看";
            this.buttonXLookGPSRoute.Click += new System.EventHandler(this.buttonXLookGPSRoute_Click);
            // 
            // FGPSTimeSelect
            // 
            this.ClientSize = new System.Drawing.Size(311, 123);
            this.Controls.Add(this.buttonXLookGPSRoute);
            this.Controls.Add(this.dateTimeInputStop);
            this.Controls.Add(this.dateTimeInputStart);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FGPSTimeSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "历史轨迹时间选择";
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputStop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimeInputStart;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimeInputStop;
        private DevComponents.DotNetBar.ButtonX buttonXLookGPSRoute;
    }
}