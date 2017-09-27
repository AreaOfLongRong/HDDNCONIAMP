namespace HDDNCONIAMP.UI.MeshManagement
{
    partial class FMPSearchText
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
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // textBoxXSearchText
            // 
            // 
            // 
            // 
            this.textBoxXSearchText.Border.Class = "TextBoxBorder";
            this.textBoxXSearchText.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXSearchText.Location = new System.Drawing.Point(13, 13);
            this.textBoxXSearchText.Name = "textBoxXSearchText";
            this.textBoxXSearchText.PreventEnterBeep = true;
            this.textBoxXSearchText.Size = new System.Drawing.Size(206, 21);
            this.textBoxXSearchText.TabIndex = 0;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonX1.Location = new System.Drawing.Point(226, 12);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(65, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 1;
            this.buttonX1.Text = "检  索";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // FMPSearchText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 48);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.textBoxXSearchText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FMPSearchText";
            this.Text = "模糊检索条件选择";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXSearchText;
        private DevComponents.DotNetBar.ButtonX buttonX1;
    }
}