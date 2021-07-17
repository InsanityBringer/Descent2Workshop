namespace Descent2Workshop
{
    partial class PIGEditor
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
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.SaveMenu = new System.Windows.Forms.MenuItem();
            this.SaveAsMenu = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.CloseMenu = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.CutMenuItem = new System.Windows.Forms.MenuItem();
            this.CopyMenuItem = new System.Windows.Forms.MenuItem();
            this.PasteMenuItem = new System.Windows.Forms.MenuItem();
            this.DeleteMenu = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.MoveUpMenuItem = new System.Windows.Forms.MenuItem();
            this.MoveDownMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.MakeAnimatedMenuItem = new System.Windows.Forms.MenuItem();
            this.ClearAnimationMenuItem = new System.Windows.Forms.MenuItem();
            this.ImportMenu = new System.Windows.Forms.MenuItem();
            this.InsertMenuItem = new System.Windows.Forms.MenuItem();
            this.ImportMenuItem = new System.Windows.Forms.MenuItem();
            this.ExportMenuItem = new System.Windows.Forms.MenuItem();
            this.DebugMenuSeparator = new System.Windows.Forms.MenuItem();
            this.ExportILBMMenuItem = new System.Windows.Forms.MenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.NameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IndexColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResolutionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FrameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.TransparentCheck = new System.Windows.Forms.CheckBox();
            this.SupertransparentCheck = new System.Windows.Forms.CheckBox();
            this.NoLightingCheck = new System.Windows.Forms.CheckBox();
            this.ColorPreview = new System.Windows.Forms.Label();
            this.CalculateAverageButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ZoomLabel = new System.Windows.Forms.Label();
            this.ZoomTrackBar = new System.Windows.Forms.TrackBar();
            this.CompressCheckBox = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem6,
            this.ImportMenu});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SaveMenu,
            this.SaveAsMenu,
            this.menuItem4,
            this.CloseMenu});
            this.menuItem1.Text = "File";
            // 
            // SaveMenu
            // 
            this.SaveMenu.Index = 0;
            this.SaveMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.SaveMenu.Text = "Save";
            // 
            // SaveAsMenu
            // 
            this.SaveAsMenu.Index = 1;
            this.SaveAsMenu.Text = "Save As...";
            this.SaveAsMenu.Click += new System.EventHandler(this.SaveAsMenu_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // CloseMenu
            // 
            this.CloseMenu.Index = 3;
            this.CloseMenu.Text = "Close Window";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.CutMenuItem,
            this.CopyMenuItem,
            this.PasteMenuItem,
            this.DeleteMenu,
            this.menuItem8,
            this.MoveUpMenuItem,
            this.MoveDownMenuItem,
            this.menuItem2,
            this.MakeAnimatedMenuItem,
            this.ClearAnimationMenuItem});
            this.menuItem6.Text = "Edit";
            // 
            // CutMenuItem
            // 
            this.CutMenuItem.Index = 0;
            this.CutMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.CutMenuItem.Text = "Cut";
            // 
            // CopyMenuItem
            // 
            this.CopyMenuItem.Index = 1;
            this.CopyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.CopyMenuItem.Text = "Copy";
            // 
            // PasteMenuItem
            // 
            this.PasteMenuItem.Index = 2;
            this.PasteMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.PasteMenuItem.Text = "Paste";
            // 
            // DeleteMenu
            // 
            this.DeleteMenu.Index = 3;
            this.DeleteMenu.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.DeleteMenu.Text = "Delete";
            this.DeleteMenu.Click += new System.EventHandler(this.DeleteMenu_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 4;
            this.menuItem8.Text = "-";
            // 
            // MoveUpMenuItem
            // 
            this.MoveUpMenuItem.Index = 5;
            this.MoveUpMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.MoveUpMenuItem.Text = "Move Up";
            this.MoveUpMenuItem.Click += new System.EventHandler(this.MoveUpMenuItem_Click);
            // 
            // MoveDownMenuItem
            // 
            this.MoveDownMenuItem.Index = 6;
            this.MoveDownMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.MoveDownMenuItem.Text = "Move Down";
            this.MoveDownMenuItem.Click += new System.EventHandler(this.MoveDownMenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 7;
            this.menuItem2.Text = "-";
            // 
            // MakeAnimatedMenuItem
            // 
            this.MakeAnimatedMenuItem.Index = 8;
            this.MakeAnimatedMenuItem.Text = "Make Range Animated";
            this.MakeAnimatedMenuItem.Click += new System.EventHandler(this.MakeAnimatedMenuItem_Click);
            // 
            // ClearAnimationMenuItem
            // 
            this.ClearAnimationMenuItem.Index = 9;
            this.ClearAnimationMenuItem.Text = "Clear Animation";
            this.ClearAnimationMenuItem.Click += new System.EventHandler(this.ClearAnimationMenuItem_Click);
            // 
            // ImportMenu
            // 
            this.ImportMenu.Index = 2;
            this.ImportMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.InsertMenuItem,
            this.ImportMenuItem,
            this.ExportMenuItem,
            this.DebugMenuSeparator,
            this.ExportILBMMenuItem});
            this.ImportMenu.Text = "Import";
            // 
            // InsertMenuItem
            // 
            this.InsertMenuItem.Index = 0;
            this.InsertMenuItem.Text = "Insert...";
            this.InsertMenuItem.Click += new System.EventHandler(this.InsertMenu_Click);
            // 
            // ImportMenuItem
            // 
            this.ImportMenuItem.Index = 1;
            this.ImportMenuItem.Text = "Import Over...";
            this.ImportMenuItem.Click += new System.EventHandler(this.ImportMenuItem_Click);
            // 
            // ExportMenuItem
            // 
            this.ExportMenuItem.Index = 2;
            this.ExportMenuItem.Text = "Export...";
            this.ExportMenuItem.Click += new System.EventHandler(this.ExportMenu_Click);
            // 
            // DebugMenuSeparator
            // 
            this.DebugMenuSeparator.Index = 3;
            this.DebugMenuSeparator.Text = "-";
            // 
            // ExportILBMMenuItem
            // 
            this.ExportILBMMenuItem.Index = 4;
            this.ExportILBMMenuItem.Text = "Export as ILBM...";
            this.ExportILBMMenuItem.Click += new System.EventHandler(this.ExportILBMMenu_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumnHeader,
            this.IndexColumnHeader,
            this.SizeColumnHeader,
            this.ResolutionColumnHeader,
            this.FrameColumnHeader});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(288, 540);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView1_AfterLabelEdit);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listView1_KeyPress);
            // 
            // NameColumnHeader
            // 
            this.NameColumnHeader.Text = "Name";
            this.NameColumnHeader.Width = 80;
            // 
            // IndexColumnHeader
            // 
            this.IndexColumnHeader.Text = "#";
            this.IndexColumnHeader.Width = 40;
            // 
            // SizeColumnHeader
            // 
            this.SizeColumnHeader.Text = "Size";
            this.SizeColumnHeader.Width = 50;
            // 
            // ResolutionColumnHeader
            // 
            this.ResolutionColumnHeader.Text = "Res";
            // 
            // FrameColumnHeader
            // 
            this.FrameColumnHeader.Text = "Frame";
            this.FrameColumnHeader.Width = 50;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(492, 473);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "PIG Files|*.pig";
            // 
            // TransparentCheck
            // 
            this.TransparentCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TransparentCheck.AutoSize = true;
            this.TransparentCheck.Location = new System.Drawing.Point(3, 479);
            this.TransparentCheck.Name = "TransparentCheck";
            this.TransparentCheck.Size = new System.Drawing.Size(83, 17);
            this.TransparentCheck.TabIndex = 6;
            this.TransparentCheck.Text = "Transparent";
            this.TransparentCheck.UseVisualStyleBackColor = true;
            this.TransparentCheck.CheckedChanged += new System.EventHandler(this.TransparentCheck_CheckedChanged);
            // 
            // SupertransparentCheck
            // 
            this.SupertransparentCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SupertransparentCheck.AutoSize = true;
            this.SupertransparentCheck.Location = new System.Drawing.Point(92, 479);
            this.SupertransparentCheck.Name = "SupertransparentCheck";
            this.SupertransparentCheck.Size = new System.Drawing.Size(107, 17);
            this.SupertransparentCheck.TabIndex = 7;
            this.SupertransparentCheck.Text = "Supertransparent";
            this.SupertransparentCheck.UseVisualStyleBackColor = true;
            this.SupertransparentCheck.CheckedChanged += new System.EventHandler(this.SupertransparentCheck_CheckedChanged);
            // 
            // NoLightingCheck
            // 
            this.NoLightingCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NoLightingCheck.AutoSize = true;
            this.NoLightingCheck.Location = new System.Drawing.Point(205, 479);
            this.NoLightingCheck.Name = "NoLightingCheck";
            this.NoLightingCheck.Size = new System.Drawing.Size(80, 17);
            this.NoLightingCheck.TabIndex = 8;
            this.NoLightingCheck.Text = "No Lighting";
            this.NoLightingCheck.UseVisualStyleBackColor = true;
            this.NoLightingCheck.CheckedChanged += new System.EventHandler(this.NoLightingCheck_CheckedChanged);
            // 
            // ColorPreview
            // 
            this.ColorPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ColorPreview.Location = new System.Drawing.Point(369, 480);
            this.ColorPreview.Name = "ColorPreview";
            this.ColorPreview.Size = new System.Drawing.Size(25, 14);
            this.ColorPreview.TabIndex = 10;
            // 
            // CalculateAverageButton
            // 
            this.CalculateAverageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CalculateAverageButton.Location = new System.Drawing.Point(400, 475);
            this.CalculateAverageButton.Name = "CalculateAverageButton";
            this.CalculateAverageButton.Size = new System.Drawing.Size(75, 23);
            this.CalculateAverageButton.TabIndex = 11;
            this.CalculateAverageButton.Text = "Calculate";
            this.CalculateAverageButton.UseVisualStyleBackColor = true;
            this.CalculateAverageButton.Click += new System.EventHandler(this.CalculateAverageButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ZoomLabel);
            this.splitContainer1.Panel2.Controls.Add(this.ZoomTrackBar);
            this.splitContainer1.Panel2.Controls.Add(this.CompressCheckBox);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.CalculateAverageButton);
            this.splitContainer1.Panel2.Controls.Add(this.TransparentCheck);
            this.splitContainer1.Panel2.Controls.Add(this.ColorPreview);
            this.splitContainer1.Panel2.Controls.Add(this.SupertransparentCheck);
            this.splitContainer1.Panel2.Controls.Add(this.NoLightingCheck);
            this.splitContainer1.Size = new System.Drawing.Size(784, 540);
            this.splitContainer1.SplitterDistance = 288;
            this.splitContainer1.TabIndex = 12;
            // 
            // ZoomLabel
            // 
            this.ZoomLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ZoomLabel.AutoSize = true;
            this.ZoomLabel.Location = new System.Drawing.Point(231, 505);
            this.ZoomLabel.Name = "ZoomLabel";
            this.ZoomLabel.Size = new System.Drawing.Size(66, 13);
            this.ZoomLabel.TabIndex = 14;
            this.ZoomLabel.Text = "Zoom: 100%";
            // 
            // ZoomTrackBar
            // 
            this.ZoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ZoomTrackBar.LargeChange = 1;
            this.ZoomTrackBar.Location = new System.Drawing.Point(3, 502);
            this.ZoomTrackBar.Maximum = 3;
            this.ZoomTrackBar.Name = "ZoomTrackBar";
            this.ZoomTrackBar.Size = new System.Drawing.Size(222, 45);
            this.ZoomTrackBar.TabIndex = 13;
            this.ZoomTrackBar.Scroll += new System.EventHandler(this.ZoomTrackBar_Scroll);
            // 
            // CompressCheckBox
            // 
            this.CompressCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CompressCheckBox.AutoSize = true;
            this.CompressCheckBox.Location = new System.Drawing.Point(291, 479);
            this.CompressCheckBox.Name = "CompressCheckBox";
            this.CompressCheckBox.Size = new System.Drawing.Size(72, 17);
            this.CompressCheckBox.TabIndex = 12;
            this.CompressCheckBox.Text = "Compress";
            this.CompressCheckBox.UseVisualStyleBackColor = true;
            this.CompressCheckBox.CheckedChanged += new System.EventHandler(this.CompressCheckBox_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // PIGEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 540);
            this.Controls.Add(this.splitContainer1);
            this.Menu = this.mainMenu1;
            this.Name = "PIGEditor";
            this.Text = "PIGEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PIGEditor_FormClosing);
            this.Load += new System.EventHandler(this.PIGEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ZoomTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem SaveMenu;
        private System.Windows.Forms.MenuItem SaveAsMenu;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem CloseMenu;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ColumnHeader NameColumnHeader;
        private System.Windows.Forms.ColumnHeader SizeColumnHeader;
        private System.Windows.Forms.ColumnHeader FrameColumnHeader;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox TransparentCheck;
        private System.Windows.Forms.CheckBox SupertransparentCheck;
        private System.Windows.Forms.CheckBox NoLightingCheck;
        private System.Windows.Forms.Label ColorPreview;
        private System.Windows.Forms.Button CalculateAverageButton;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem InsertMenuItem;
        private System.Windows.Forms.MenuItem ImportMenuItem;
        private System.Windows.Forms.MenuItem DeleteMenu;
        private System.Windows.Forms.MenuItem ExportMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ColumnHeader IndexColumnHeader;
        private System.Windows.Forms.CheckBox CompressCheckBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ColumnHeader ResolutionColumnHeader;
        private System.Windows.Forms.MenuItem CutMenuItem;
        private System.Windows.Forms.MenuItem CopyMenuItem;
        private System.Windows.Forms.MenuItem PasteMenuItem;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem MoveUpMenuItem;
        private System.Windows.Forms.MenuItem MoveDownMenuItem;
        private System.Windows.Forms.MenuItem ImportMenu;
        private System.Windows.Forms.MenuItem DebugMenuSeparator;
        private System.Windows.Forms.MenuItem ExportILBMMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem MakeAnimatedMenuItem;
        private System.Windows.Forms.MenuItem ClearAnimationMenuItem;
        private System.Windows.Forms.Label ZoomLabel;
        private System.Windows.Forms.TrackBar ZoomTrackBar;
    }
}