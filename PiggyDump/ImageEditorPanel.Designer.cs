namespace Descent2Workshop
{
    partial class ImageEditorPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ImageListView = new System.Windows.Forms.ListView();
            this.NameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IndexColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResolutionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FrameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ZoomLabel = new System.Windows.Forms.Label();
            this.ZoomTrackBar = new System.Windows.Forms.TrackBar();
            this.CompressCheckBox = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CalculateAverageButton = new System.Windows.Forms.Button();
            this.TransparentCheck = new System.Windows.Forms.CheckBox();
            this.ColorPreview = new System.Windows.Forms.Label();
            this.SupertransparentCheck = new System.Windows.Forms.CheckBox();
            this.NoLightingCheck = new System.Windows.Forms.CheckBox();
            this.ReplacamentPanel = new System.Windows.Forms.Panel();
            this.PaletteComboBox = new System.Windows.Forms.ComboBox();
            this.ChooseReplacementButton = new System.Windows.Forms.Button();
            this.ReplacementSpinner = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.ReplacamentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReplacementSpinner)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.ImageListView);
            this.splitContainer1.Panel1.Controls.Add(this.ReplacamentPanel);
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
            this.splitContainer1.Size = new System.Drawing.Size(1080, 731);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 13;
            // 
            // ImageListView
            // 
            this.ImageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumnHeader,
            this.IndexColumnHeader,
            this.SizeColumnHeader,
            this.ResolutionColumnHeader,
            this.FrameColumnHeader});
            this.ImageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageListView.FullRowSelect = true;
            this.ImageListView.HideSelection = false;
            this.ImageListView.LabelEdit = true;
            this.ImageListView.Location = new System.Drawing.Point(0, 0);
            this.ImageListView.Name = "ImageListView";
            this.ImageListView.Size = new System.Drawing.Size(305, 666);
            this.ImageListView.TabIndex = 3;
            this.ImageListView.UseCompatibleStateImageBehavior = false;
            this.ImageListView.View = System.Windows.Forms.View.Details;
            this.ImageListView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView1_AfterLabelEdit);
            this.ImageListView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
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
            // ZoomLabel
            // 
            this.ZoomLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ZoomLabel.AutoSize = true;
            this.ZoomLabel.Location = new System.Drawing.Point(231, 696);
            this.ZoomLabel.Name = "ZoomLabel";
            this.ZoomLabel.Size = new System.Drawing.Size(66, 13);
            this.ZoomLabel.TabIndex = 14;
            this.ZoomLabel.Text = "Zoom: 100%";
            // 
            // ZoomTrackBar
            // 
            this.ZoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ZoomTrackBar.LargeChange = 1;
            this.ZoomTrackBar.Location = new System.Drawing.Point(3, 693);
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
            this.CompressCheckBox.Location = new System.Drawing.Point(291, 670);
            this.CompressCheckBox.Name = "CompressCheckBox";
            this.CompressCheckBox.Size = new System.Drawing.Size(72, 17);
            this.CompressCheckBox.TabIndex = 12;
            this.CompressCheckBox.Text = "Compress";
            this.CompressCheckBox.UseVisualStyleBackColor = true;
            this.CompressCheckBox.CheckedChanged += new System.EventHandler(this.CompressCheckBox_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(771, 664);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // CalculateAverageButton
            // 
            this.CalculateAverageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CalculateAverageButton.Location = new System.Drawing.Point(400, 666);
            this.CalculateAverageButton.Name = "CalculateAverageButton";
            this.CalculateAverageButton.Size = new System.Drawing.Size(75, 23);
            this.CalculateAverageButton.TabIndex = 11;
            this.CalculateAverageButton.Text = "Calculate";
            this.CalculateAverageButton.UseVisualStyleBackColor = true;
            this.CalculateAverageButton.Click += new System.EventHandler(this.CalculateAverageButton_Click);
            // 
            // TransparentCheck
            // 
            this.TransparentCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TransparentCheck.AutoSize = true;
            this.TransparentCheck.Location = new System.Drawing.Point(3, 670);
            this.TransparentCheck.Name = "TransparentCheck";
            this.TransparentCheck.Size = new System.Drawing.Size(83, 17);
            this.TransparentCheck.TabIndex = 6;
            this.TransparentCheck.Text = "Transparent";
            this.TransparentCheck.UseVisualStyleBackColor = true;
            this.TransparentCheck.CheckedChanged += new System.EventHandler(this.TransparentCheck_CheckedChanged);
            // 
            // ColorPreview
            // 
            this.ColorPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ColorPreview.Location = new System.Drawing.Point(369, 671);
            this.ColorPreview.Name = "ColorPreview";
            this.ColorPreview.Size = new System.Drawing.Size(25, 14);
            this.ColorPreview.TabIndex = 10;
            // 
            // SupertransparentCheck
            // 
            this.SupertransparentCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SupertransparentCheck.AutoSize = true;
            this.SupertransparentCheck.Location = new System.Drawing.Point(92, 670);
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
            this.NoLightingCheck.Location = new System.Drawing.Point(205, 670);
            this.NoLightingCheck.Name = "NoLightingCheck";
            this.NoLightingCheck.Size = new System.Drawing.Size(80, 17);
            this.NoLightingCheck.TabIndex = 8;
            this.NoLightingCheck.Text = "No Lighting";
            this.NoLightingCheck.UseVisualStyleBackColor = true;
            this.NoLightingCheck.CheckedChanged += new System.EventHandler(this.NoLightingCheck_CheckedChanged);
            // 
            // ReplacamentPanel
            // 
            this.ReplacamentPanel.Controls.Add(this.PaletteComboBox);
            this.ReplacamentPanel.Controls.Add(this.ChooseReplacementButton);
            this.ReplacamentPanel.Controls.Add(this.ReplacementSpinner);
            this.ReplacamentPanel.Controls.Add(this.label2);
            this.ReplacamentPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ReplacamentPanel.Location = new System.Drawing.Point(0, 666);
            this.ReplacamentPanel.Name = "ReplacamentPanel";
            this.ReplacamentPanel.Size = new System.Drawing.Size(305, 65);
            this.ReplacamentPanel.TabIndex = 4;
            // 
            // PaletteComboBox
            // 
            this.PaletteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PaletteComboBox.FormattingEnabled = true;
            this.PaletteComboBox.Items.AddRange(new object[] {
            "groupa.256",
            "water.256",
            "fire.256",
            "ice.256",
            "alien1.256",
            "alien2.256"});
            this.PaletteComboBox.Location = new System.Drawing.Point(3, 34);
            this.PaletteComboBox.Name = "PaletteComboBox";
            this.PaletteComboBox.Size = new System.Drawing.Size(121, 21);
            this.PaletteComboBox.TabIndex = 11;
            // 
            // ChooseReplacementButton
            // 
            this.ChooseReplacementButton.Location = new System.Drawing.Point(117, 5);
            this.ChooseReplacementButton.Name = "ChooseReplacementButton";
            this.ChooseReplacementButton.Size = new System.Drawing.Size(75, 23);
            this.ChooseReplacementButton.TabIndex = 10;
            this.ChooseReplacementButton.Text = "Choose...";
            this.ChooseReplacementButton.UseVisualStyleBackColor = true;
            // 
            // ReplacementSpinner
            // 
            this.ReplacementSpinner.Location = new System.Drawing.Point(67, 8);
            this.ReplacementSpinner.Maximum = new decimal(new int[] {
            2620,
            0,
            0,
            0});
            this.ReplacementSpinner.Name = "ReplacementSpinner";
            this.ReplacementSpinner.Size = new System.Drawing.Size(44, 20);
            this.ReplacementSpinner.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Replacing:";
            // 
            // ImageEditorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ImageEditorPanel";
            this.Size = new System.Drawing.Size(1080, 731);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ZoomTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ReplacamentPanel.ResumeLayout(false);
            this.ReplacamentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReplacementSpinner)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView ImageListView;
        private System.Windows.Forms.ColumnHeader NameColumnHeader;
        private System.Windows.Forms.ColumnHeader IndexColumnHeader;
        private System.Windows.Forms.ColumnHeader SizeColumnHeader;
        private System.Windows.Forms.ColumnHeader ResolutionColumnHeader;
        private System.Windows.Forms.ColumnHeader FrameColumnHeader;
        private System.Windows.Forms.Panel ReplacamentPanel;
        private System.Windows.Forms.Label ZoomLabel;
        private System.Windows.Forms.TrackBar ZoomTrackBar;
        private System.Windows.Forms.CheckBox CompressCheckBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button CalculateAverageButton;
        private System.Windows.Forms.CheckBox TransparentCheck;
        private System.Windows.Forms.Label ColorPreview;
        private System.Windows.Forms.CheckBox SupertransparentCheck;
        private System.Windows.Forms.CheckBox NoLightingCheck;
        private System.Windows.Forms.ComboBox PaletteComboBox;
        private System.Windows.Forms.Button ChooseReplacementButton;
        private System.Windows.Forms.NumericUpDown ReplacementSpinner;
        private System.Windows.Forms.Label label2;
    }
}
