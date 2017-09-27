namespace HDDNCONIAMP.UI.UserSettings
{
    partial class FSearchLog
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
            this.textBoxXSearchText = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonSearchDown = new System.Windows.Forms.RadioButton();
            this.radioButtonSearchUP = new System.Windows.Forms.RadioButton();
            this.buttonXLogSearch = new DevComponents.DotNetBar.ButtonX();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxXSearchText
            // 
            // 
            // 
            // 
            this.textBoxXSearchText.Border.Class = "TextBoxBorder";
            this.textBoxXSearchText.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXSearchText.Location = new System.Drawing.Point(12, 12);
            this.textBoxXSearchText.Name = "textBoxXSearchText";
            this.textBoxXSearchText.PreventEnterBeep = true;
            this.textBoxXSearchText.Size = new System.Drawing.Size(197, 21);
            this.textBoxXSearchText.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonSearchDown);
            this.groupBox1.Controls.Add(this.radioButtonSearchUP);
            this.groupBox1.Location = new System.Drawing.Point(12, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 58);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "方向";
            // 
            // radioButtonSearchDown
            // 
            this.radioButtonSearchDown.AutoSize = true;
            this.radioButtonSearchDown.Checked = true;
            this.radioButtonSearchDown.Location = new System.Drawing.Point(93, 20);
            this.radioButtonSearchDown.Name = "radioButtonSearchDown";
            this.radioButtonSearchDown.Size = new System.Drawing.Size(47, 16);
            this.radioButtonSearchDown.TabIndex = 1;
            this.radioButtonSearchDown.TabStop = true;
            this.radioButtonSearchDown.Text = "向下";
            this.radioButtonSearchDown.UseVisualStyleBackColor = true;
            // 
            // radioButtonSearchUP
            // 
            this.radioButtonSearchUP.AutoSize = true;
            this.radioButtonSearchUP.Location = new System.Drawing.Point(6, 20);
            this.radioButtonSearchUP.Name = "radioButtonSearchUP";
            this.radioButtonSearchUP.Size = new System.Drawing.Size(47, 16);
            this.radioButtonSearchUP.TabIndex = 0;
            this.radioButtonSearchUP.Text = "向上";
            this.radioButtonSearchUP.UseVisualStyleBackColor = true;
            // 
            // buttonXLogSearch
            // 
            this.buttonXLogSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXLogSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXLogSearch.Location = new System.Drawing.Point(225, 11);
            this.buttonXLogSearch.Name = "buttonXLogSearch";
            this.buttonXLogSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonXLogSearch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXLogSearch.TabIndex = 2;
            this.buttonXLogSearch.Text = "检  索";
            this.buttonXLogSearch.Click += new System.EventHandler(this.buttonXLogSearch_Click);
            // 
            // FSearchLog
            // 
            this.ClientSize = new System.Drawing.Size(310, 99);
            this.Controls.Add(this.buttonXLogSearch);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxXSearchText);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSearchLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "检索";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXSearchText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonSearchUP;
        private System.Windows.Forms.RadioButton radioButtonSearchDown;
        private DevComponents.DotNetBar.ButtonX buttonXLogSearch;
    }
}