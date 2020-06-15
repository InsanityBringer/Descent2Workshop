namespace Descent2Workshop.EditorPanels
{
    partial class PolymodelPanel
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
            this.NoDepthCheckBox = new System.Windows.Forms.CheckBox();
            this.btnExportModel = new System.Windows.Forms.Button();
            this.GLControlStandin = new System.Windows.Forms.Panel();
            this.WireframeCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowNormCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowBBCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowRadiusCheckBox = new System.Windows.Forms.CheckBox();
            this.btnImportModel = new System.Windows.Forms.Button();
            this.cbModelDeadModel = new System.Windows.Forms.ComboBox();
            this.label150 = new System.Windows.Forms.Label();
            this.cbModelDyingModel = new System.Windows.Forms.ComboBox();
            this.label149 = new System.Windows.Forms.Label();
            this.cbModelLowDetail = new System.Windows.Forms.ComboBox();
            this.PitchTrackBar = new System.Windows.Forms.TrackBar();
            this.ZoomTrackBar = new System.Windows.Forms.TrackBar();
            this.AngleTrackBar = new System.Windows.Forms.TrackBar();
            this.label111 = new System.Windows.Forms.Label();
            this.label110 = new System.Windows.Forms.Label();
            this.txtModelMaxZ = new System.Windows.Forms.TextBox();
            this.txtModelMaxY = new System.Windows.Forms.TextBox();
            this.txtModelMaxX = new System.Windows.Forms.TextBox();
            this.txtModelMinZ = new System.Windows.Forms.TextBox();
            this.txtModelMinY = new System.Windows.Forms.TextBox();
            this.txtModelMinX = new System.Windows.Forms.TextBox();
            this.label108 = new System.Windows.Forms.Label();
            this.label106 = new System.Windows.Forms.Label();
            this.txtModelTextureCount = new System.Windows.Forms.TextBox();
            this.label105 = new System.Windows.Forms.Label();
            this.txtModelRadius = new System.Windows.Forms.TextBox();
            this.txtModelDataSize = new System.Windows.Forms.TextBox();
            this.label104 = new System.Windows.Forms.Label();
            this.txtModelNumModels = new System.Windows.Forms.TextBox();
            this.label103 = new System.Windows.Forms.Label();
            this.label102 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.TextureGroupBox = new System.Windows.Forms.GroupBox();
            this.FindPackButton = new System.Windows.Forms.Button();
            this.AnimatedWarningLabel = new System.Windows.Forms.Label();
            this.ModelBasePointerSpinner = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.ModelNumPointers = new System.Windows.Forms.TextBox();
            this.ModelNumTextures = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ModelBaseTextureSpinner = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.PitchTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleTrackBar)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.TextureGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelBasePointerSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelBaseTextureSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // NoDepthCheckBox
            // 
            this.NoDepthCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NoDepthCheckBox.AutoSize = true;
            this.NoDepthCheckBox.Location = new System.Drawing.Point(373, 384);
            this.NoDepthCheckBox.Name = "NoDepthCheckBox";
            this.NoDepthCheckBox.Size = new System.Drawing.Size(158, 17);
            this.NoDepthCheckBox.TabIndex = 74;
            this.NoDepthCheckBox.Text = "Emulate Software Overdraw";
            this.NoDepthCheckBox.UseVisualStyleBackColor = true;
            this.NoDepthCheckBox.CheckedChanged += new System.EventHandler(this.ModelViewProperty_Changed);
            // 
            // btnExportModel
            // 
            this.btnExportModel.Location = new System.Drawing.Point(201, 191);
            this.btnExportModel.Name = "btnExportModel";
            this.btnExportModel.Size = new System.Drawing.Size(100, 23);
            this.btnExportModel.TabIndex = 73;
            this.btnExportModel.Text = "Export Model...";
            this.btnExportModel.UseVisualStyleBackColor = true;
            this.btnExportModel.Click += new System.EventHandler(this.btnExportModel_Click);
            // 
            // GLControlStandin
            // 
            this.GLControlStandin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GLControlStandin.Location = new System.Drawing.Point(373, 25);
            this.GLControlStandin.Name = "GLControlStandin";
            this.GLControlStandin.Size = new System.Drawing.Size(256, 256);
            this.GLControlStandin.TabIndex = 72;
            // 
            // WireframeCheckBox
            // 
            this.WireframeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WireframeCheckBox.AutoSize = true;
            this.WireframeCheckBox.Location = new System.Drawing.Point(465, 361);
            this.WireframeCheckBox.Name = "WireframeCheckBox";
            this.WireframeCheckBox.Size = new System.Drawing.Size(74, 17);
            this.WireframeCheckBox.TabIndex = 71;
            this.WireframeCheckBox.Text = "Wireframe";
            this.WireframeCheckBox.UseVisualStyleBackColor = true;
            this.WireframeCheckBox.CheckedChanged += new System.EventHandler(this.ModelViewProperty_Changed);
            // 
            // ShowNormCheckBox
            // 
            this.ShowNormCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowNormCheckBox.AutoSize = true;
            this.ShowNormCheckBox.Location = new System.Drawing.Point(373, 361);
            this.ShowNormCheckBox.Name = "ShowNormCheckBox";
            this.ShowNormCheckBox.Size = new System.Drawing.Size(94, 17);
            this.ShowNormCheckBox.TabIndex = 70;
            this.ShowNormCheckBox.Text = "Show Normals";
            this.ShowNormCheckBox.UseVisualStyleBackColor = true;
            this.ShowNormCheckBox.CheckedChanged += new System.EventHandler(this.ModelViewProperty_Changed);
            // 
            // ShowBBCheckBox
            // 
            this.ShowBBCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowBBCheckBox.AutoSize = true;
            this.ShowBBCheckBox.Location = new System.Drawing.Point(465, 338);
            this.ShowBBCheckBox.Name = "ShowBBCheckBox";
            this.ShowBBCheckBox.Size = new System.Drawing.Size(133, 17);
            this.ShowBBCheckBox.TabIndex = 69;
            this.ShowBBCheckBox.Text = "Show Bounding Boxes";
            this.ShowBBCheckBox.UseVisualStyleBackColor = true;
            this.ShowBBCheckBox.CheckedChanged += new System.EventHandler(this.ModelViewProperty_Changed);
            // 
            // ShowRadiusCheckBox
            // 
            this.ShowRadiusCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowRadiusCheckBox.AutoSize = true;
            this.ShowRadiusCheckBox.Location = new System.Drawing.Point(373, 338);
            this.ShowRadiusCheckBox.Name = "ShowRadiusCheckBox";
            this.ShowRadiusCheckBox.Size = new System.Drawing.Size(89, 17);
            this.ShowRadiusCheckBox.TabIndex = 68;
            this.ShowRadiusCheckBox.Text = "Show Radius";
            this.ShowRadiusCheckBox.UseVisualStyleBackColor = true;
            this.ShowRadiusCheckBox.CheckedChanged += new System.EventHandler(this.ModelViewProperty_Changed);
            // 
            // btnImportModel
            // 
            this.btnImportModel.Location = new System.Drawing.Point(95, 191);
            this.btnImportModel.Name = "btnImportModel";
            this.btnImportModel.Size = new System.Drawing.Size(100, 23);
            this.btnImportModel.TabIndex = 67;
            this.btnImportModel.Text = "Import Model...";
            this.btnImportModel.UseVisualStyleBackColor = true;
            this.btnImportModel.Click += new System.EventHandler(this.btnImportModel_Click);
            // 
            // cbModelDeadModel
            // 
            this.cbModelDeadModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModelDeadModel.FormattingEnabled = true;
            this.cbModelDeadModel.Location = new System.Drawing.Point(95, 164);
            this.cbModelDeadModel.Name = "cbModelDeadModel";
            this.cbModelDeadModel.Size = new System.Drawing.Size(221, 21);
            this.cbModelDeadModel.TabIndex = 66;
            this.cbModelDeadModel.Tag = "DeadModelnum";
            this.cbModelDeadModel.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            // 
            // label150
            // 
            this.label150.AutoSize = true;
            this.label150.Location = new System.Drawing.Point(12, 167);
            this.label150.Name = "label150";
            this.label150.Size = new System.Drawing.Size(68, 13);
            this.label150.TabIndex = 65;
            this.label150.Text = "Dead Model:";
            // 
            // cbModelDyingModel
            // 
            this.cbModelDyingModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModelDyingModel.FormattingEnabled = true;
            this.cbModelDyingModel.Location = new System.Drawing.Point(95, 137);
            this.cbModelDyingModel.Name = "cbModelDyingModel";
            this.cbModelDyingModel.Size = new System.Drawing.Size(221, 21);
            this.cbModelDyingModel.TabIndex = 64;
            this.cbModelDyingModel.Tag = "DyingModelnum";
            this.cbModelDyingModel.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            // 
            // label149
            // 
            this.label149.AutoSize = true;
            this.label149.Location = new System.Drawing.Point(12, 140);
            this.label149.Name = "label149";
            this.label149.Size = new System.Drawing.Size(69, 13);
            this.label149.TabIndex = 63;
            this.label149.Text = "Dying Model:";
            // 
            // cbModelLowDetail
            // 
            this.cbModelLowDetail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModelLowDetail.FormattingEnabled = true;
            this.cbModelLowDetail.Location = new System.Drawing.Point(95, 110);
            this.cbModelLowDetail.Name = "cbModelLowDetail";
            this.cbModelLowDetail.Size = new System.Drawing.Size(221, 21);
            this.cbModelLowDetail.TabIndex = 62;
            this.cbModelLowDetail.Tag = "SimplerModels";
            this.cbModelLowDetail.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            // 
            // PitchTrackBar
            // 
            this.PitchTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PitchTrackBar.Location = new System.Drawing.Point(631, 25);
            this.PitchTrackBar.Maximum = 16;
            this.PitchTrackBar.Name = "PitchTrackBar";
            this.PitchTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.PitchTrackBar.Size = new System.Drawing.Size(45, 256);
            this.PitchTrackBar.TabIndex = 61;
            this.PitchTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.PitchTrackBar.Value = 8;
            this.PitchTrackBar.Scroll += new System.EventHandler(this.ModelViewProperty_Changed);
            // 
            // ZoomTrackBar
            // 
            this.ZoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ZoomTrackBar.Location = new System.Drawing.Point(322, 25);
            this.ZoomTrackBar.Maximum = 48;
            this.ZoomTrackBar.Name = "ZoomTrackBar";
            this.ZoomTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ZoomTrackBar.Size = new System.Drawing.Size(45, 256);
            this.ZoomTrackBar.TabIndex = 48;
            this.ZoomTrackBar.Value = 16;
            this.ZoomTrackBar.Scroll += new System.EventHandler(this.ModelViewProperty_Changed);
            // 
            // AngleTrackBar
            // 
            this.AngleTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AngleTrackBar.LargeChange = 15;
            this.AngleTrackBar.Location = new System.Drawing.Point(373, 287);
            this.AngleTrackBar.Maximum = 16;
            this.AngleTrackBar.Name = "AngleTrackBar";
            this.AngleTrackBar.Size = new System.Drawing.Size(256, 45);
            this.AngleTrackBar.TabIndex = 60;
            this.AngleTrackBar.Value = 8;
            this.AngleTrackBar.Scroll += new System.EventHandler(this.ModelViewProperty_Changed);
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.Location = new System.Drawing.Point(6, 45);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(27, 13);
            this.label111.TabIndex = 59;
            this.label111.Text = "Max";
            // 
            // label110
            // 
            this.label110.AutoSize = true;
            this.label110.Location = new System.Drawing.Point(6, 19);
            this.label110.Name = "label110";
            this.label110.Size = new System.Drawing.Size(24, 13);
            this.label110.TabIndex = 58;
            this.label110.Text = "Min";
            // 
            // txtModelMaxZ
            // 
            this.txtModelMaxZ.Location = new System.Drawing.Point(124, 42);
            this.txtModelMaxZ.Name = "txtModelMaxZ";
            this.txtModelMaxZ.ReadOnly = true;
            this.txtModelMaxZ.Size = new System.Drawing.Size(38, 20);
            this.txtModelMaxZ.TabIndex = 57;
            // 
            // txtModelMaxY
            // 
            this.txtModelMaxY.Location = new System.Drawing.Point(80, 42);
            this.txtModelMaxY.Name = "txtModelMaxY";
            this.txtModelMaxY.ReadOnly = true;
            this.txtModelMaxY.Size = new System.Drawing.Size(38, 20);
            this.txtModelMaxY.TabIndex = 56;
            // 
            // txtModelMaxX
            // 
            this.txtModelMaxX.Location = new System.Drawing.Point(36, 42);
            this.txtModelMaxX.Name = "txtModelMaxX";
            this.txtModelMaxX.ReadOnly = true;
            this.txtModelMaxX.Size = new System.Drawing.Size(38, 20);
            this.txtModelMaxX.TabIndex = 55;
            // 
            // txtModelMinZ
            // 
            this.txtModelMinZ.Location = new System.Drawing.Point(124, 16);
            this.txtModelMinZ.Name = "txtModelMinZ";
            this.txtModelMinZ.ReadOnly = true;
            this.txtModelMinZ.Size = new System.Drawing.Size(38, 20);
            this.txtModelMinZ.TabIndex = 54;
            // 
            // txtModelMinY
            // 
            this.txtModelMinY.Location = new System.Drawing.Point(80, 16);
            this.txtModelMinY.Name = "txtModelMinY";
            this.txtModelMinY.ReadOnly = true;
            this.txtModelMinY.Size = new System.Drawing.Size(38, 20);
            this.txtModelMinY.TabIndex = 53;
            // 
            // txtModelMinX
            // 
            this.txtModelMinX.Location = new System.Drawing.Point(36, 16);
            this.txtModelMinX.Name = "txtModelMinX";
            this.txtModelMinX.ReadOnly = true;
            this.txtModelMinX.Size = new System.Drawing.Size(38, 20);
            this.txtModelMinX.TabIndex = 52;
            // 
            // label108
            // 
            this.label108.AutoSize = true;
            this.label108.Location = new System.Drawing.Point(12, 113);
            this.label108.Name = "label108";
            this.label108.Size = new System.Drawing.Size(56, 13);
            this.label108.TabIndex = 50;
            this.label108.Text = "LD Model:";
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.Location = new System.Drawing.Point(12, 87);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(77, 13);
            this.label106.TabIndex = 49;
            this.label106.Text = "Texture Count:";
            // 
            // txtModelTextureCount
            // 
            this.txtModelTextureCount.Location = new System.Drawing.Point(95, 84);
            this.txtModelTextureCount.Name = "txtModelTextureCount";
            this.txtModelTextureCount.ReadOnly = true;
            this.txtModelTextureCount.Size = new System.Drawing.Size(100, 20);
            this.txtModelTextureCount.TabIndex = 47;
            // 
            // label105
            // 
            this.label105.AutoSize = true;
            this.label105.Location = new System.Drawing.Point(12, 61);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(43, 13);
            this.label105.TabIndex = 46;
            this.label105.Text = "Radius:";
            // 
            // txtModelRadius
            // 
            this.txtModelRadius.Location = new System.Drawing.Point(95, 58);
            this.txtModelRadius.Name = "txtModelRadius";
            this.txtModelRadius.ReadOnly = true;
            this.txtModelRadius.Size = new System.Drawing.Size(100, 20);
            this.txtModelRadius.TabIndex = 45;
            // 
            // txtModelDataSize
            // 
            this.txtModelDataSize.Location = new System.Drawing.Point(95, 32);
            this.txtModelDataSize.Name = "txtModelDataSize";
            this.txtModelDataSize.ReadOnly = true;
            this.txtModelDataSize.Size = new System.Drawing.Size(100, 20);
            this.txtModelDataSize.TabIndex = 44;
            // 
            // label104
            // 
            this.label104.AutoSize = true;
            this.label104.Location = new System.Drawing.Point(12, 35);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(68, 13);
            this.label104.TabIndex = 43;
            this.label104.Text = "Size of Data:";
            // 
            // txtModelNumModels
            // 
            this.txtModelNumModels.Location = new System.Drawing.Point(95, 6);
            this.txtModelNumModels.Name = "txtModelNumModels";
            this.txtModelNumModels.ReadOnly = true;
            this.txtModelNumModels.Size = new System.Drawing.Size(100, 20);
            this.txtModelNumModels.TabIndex = 42;
            // 
            // label103
            // 
            this.label103.AutoSize = true;
            this.label103.Location = new System.Drawing.Point(12, 9);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(62, 13);
            this.label103.TabIndex = 41;
            this.label103.Text = "Submodels:";
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.Location = new System.Drawing.Point(370, 9);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(48, 13);
            this.label102.TabIndex = 40;
            this.label102.Text = "Preview:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label110);
            this.groupBox1.Controls.Add(this.txtModelMinX);
            this.groupBox1.Controls.Add(this.txtModelMinY);
            this.groupBox1.Controls.Add(this.txtModelMinZ);
            this.groupBox1.Controls.Add(this.txtModelMaxX);
            this.groupBox1.Controls.Add(this.txtModelMaxY);
            this.groupBox1.Controls.Add(this.txtModelMaxZ);
            this.groupBox1.Controls.Add(this.label111);
            this.groupBox1.Location = new System.Drawing.Point(15, 220);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 69);
            this.groupBox1.TabIndex = 75;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Global Bounding Box";
            // 
            // TextureGroupBox
            // 
            this.TextureGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TextureGroupBox.Controls.Add(this.FindPackButton);
            this.TextureGroupBox.Controls.Add(this.AnimatedWarningLabel);
            this.TextureGroupBox.Controls.Add(this.ModelBasePointerSpinner);
            this.TextureGroupBox.Controls.Add(this.label9);
            this.TextureGroupBox.Controls.Add(this.ModelNumPointers);
            this.TextureGroupBox.Controls.Add(this.ModelNumTextures);
            this.TextureGroupBox.Controls.Add(this.label8);
            this.TextureGroupBox.Controls.Add(this.label6);
            this.TextureGroupBox.Controls.Add(this.ModelBaseTextureSpinner);
            this.TextureGroupBox.Controls.Add(this.label1);
            this.TextureGroupBox.Location = new System.Drawing.Point(15, 295);
            this.TextureGroupBox.Name = "TextureGroupBox";
            this.TextureGroupBox.Size = new System.Drawing.Size(352, 85);
            this.TextureGroupBox.TabIndex = 76;
            this.TextureGroupBox.TabStop = false;
            this.TextureGroupBox.Text = "Textures";
            this.TextureGroupBox.Visible = false;
            // 
            // FindPackButton
            // 
            this.FindPackButton.Location = new System.Drawing.Point(289, 11);
            this.FindPackButton.Name = "FindPackButton";
            this.FindPackButton.Size = new System.Drawing.Size(41, 23);
            this.FindPackButton.TabIndex = 37;
            this.FindPackButton.Text = "Find";
            this.FindPackButton.UseVisualStyleBackColor = true;
            this.FindPackButton.Click += new System.EventHandler(this.FindPackButton_Click);
            // 
            // AnimatedWarningLabel
            // 
            this.AnimatedWarningLabel.AutoSize = true;
            this.AnimatedWarningLabel.Location = new System.Drawing.Point(6, 36);
            this.AnimatedWarningLabel.Name = "AnimatedWarningLabel";
            this.AnimatedWarningLabel.Size = new System.Drawing.Size(266, 13);
            this.AnimatedWarningLabel.TabIndex = 36;
            this.AnimatedWarningLabel.Text = "Warning: This range conflicts with an animated texture.";
            this.AnimatedWarningLabel.Visible = false;
            // 
            // ModelBasePointerSpinner
            // 
            this.ModelBasePointerSpinner.Location = new System.Drawing.Point(208, 53);
            this.ModelBasePointerSpinner.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.ModelBasePointerSpinner.Name = "ModelBasePointerSpinner";
            this.ModelBasePointerSpinner.Size = new System.Drawing.Size(75, 20);
            this.ModelBasePointerSpinner.TabIndex = 34;
            this.ModelBasePointerSpinner.Tag = "FirstTexture";
            this.ModelBasePointerSpinner.ValueChanged += new System.EventHandler(this.ModelSpinner_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(153, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Insert At:";
            // 
            // ModelNumPointers
            // 
            this.ModelNumPointers.Location = new System.Drawing.Point(93, 53);
            this.ModelNumPointers.Name = "ModelNumPointers";
            this.ModelNumPointers.ReadOnly = true;
            this.ModelNumPointers.Size = new System.Drawing.Size(54, 20);
            this.ModelNumPointers.TabIndex = 33;
            // 
            // ModelNumTextures
            // 
            this.ModelNumTextures.Location = new System.Drawing.Point(93, 13);
            this.ModelNumTextures.Name = "ModelNumTextures";
            this.ModelNumTextures.ReadOnly = true;
            this.ModelNumTextures.Size = new System.Drawing.Size(54, 20);
            this.ModelNumTextures.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Pointers:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Object Bitmaps:";
            // 
            // ModelBaseTextureSpinner
            // 
            this.ModelBaseTextureSpinner.Location = new System.Drawing.Point(208, 13);
            this.ModelBaseTextureSpinner.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.ModelBaseTextureSpinner.Name = "ModelBaseTextureSpinner";
            this.ModelBaseTextureSpinner.Size = new System.Drawing.Size(75, 20);
            this.ModelBaseTextureSpinner.TabIndex = 29;
            this.ModelBaseTextureSpinner.Tag = "BaseTexture";
            this.ModelBaseTextureSpinner.ValueChanged += new System.EventHandler(this.ModelSpinner_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Insert At:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Parallax Object Files|*.pof";
            // 
            // PolymodelPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextureGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.NoDepthCheckBox);
            this.Controls.Add(this.btnExportModel);
            this.Controls.Add(this.GLControlStandin);
            this.Controls.Add(this.WireframeCheckBox);
            this.Controls.Add(this.ShowNormCheckBox);
            this.Controls.Add(this.ShowBBCheckBox);
            this.Controls.Add(this.ShowRadiusCheckBox);
            this.Controls.Add(this.btnImportModel);
            this.Controls.Add(this.cbModelDeadModel);
            this.Controls.Add(this.label150);
            this.Controls.Add(this.cbModelDyingModel);
            this.Controls.Add(this.label149);
            this.Controls.Add(this.cbModelLowDetail);
            this.Controls.Add(this.PitchTrackBar);
            this.Controls.Add(this.ZoomTrackBar);
            this.Controls.Add(this.AngleTrackBar);
            this.Controls.Add(this.label108);
            this.Controls.Add(this.label106);
            this.Controls.Add(this.txtModelTextureCount);
            this.Controls.Add(this.label105);
            this.Controls.Add(this.txtModelRadius);
            this.Controls.Add(this.txtModelDataSize);
            this.Controls.Add(this.label104);
            this.Controls.Add(this.txtModelNumModels);
            this.Controls.Add(this.label103);
            this.Controls.Add(this.label102);
            this.Name = "PolymodelPanel";
            this.Size = new System.Drawing.Size(682, 408);
            ((System.ComponentModel.ISupportInitialize)(this.PitchTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleTrackBar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TextureGroupBox.ResumeLayout(false);
            this.TextureGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelBasePointerSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelBaseTextureSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox NoDepthCheckBox;
        private System.Windows.Forms.Button btnExportModel;
        private System.Windows.Forms.Panel GLControlStandin;
        private System.Windows.Forms.CheckBox WireframeCheckBox;
        private System.Windows.Forms.CheckBox ShowNormCheckBox;
        private System.Windows.Forms.CheckBox ShowBBCheckBox;
        private System.Windows.Forms.CheckBox ShowRadiusCheckBox;
        private System.Windows.Forms.Button btnImportModel;
        private System.Windows.Forms.ComboBox cbModelDeadModel;
        private System.Windows.Forms.Label label150;
        private System.Windows.Forms.ComboBox cbModelDyingModel;
        private System.Windows.Forms.Label label149;
        private System.Windows.Forms.ComboBox cbModelLowDetail;
        private System.Windows.Forms.TrackBar PitchTrackBar;
        private System.Windows.Forms.TrackBar ZoomTrackBar;
        private System.Windows.Forms.TrackBar AngleTrackBar;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.Label label110;
        private System.Windows.Forms.TextBox txtModelMaxZ;
        private System.Windows.Forms.TextBox txtModelMaxY;
        private System.Windows.Forms.TextBox txtModelMaxX;
        private System.Windows.Forms.TextBox txtModelMinZ;
        private System.Windows.Forms.TextBox txtModelMinY;
        private System.Windows.Forms.TextBox txtModelMinX;
        private System.Windows.Forms.Label label108;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.TextBox txtModelTextureCount;
        private System.Windows.Forms.Label label105;
        private System.Windows.Forms.TextBox txtModelRadius;
        private System.Windows.Forms.TextBox txtModelDataSize;
        private System.Windows.Forms.Label label104;
        private System.Windows.Forms.TextBox txtModelNumModels;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.Label label102;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox TextureGroupBox;
        private System.Windows.Forms.Button FindPackButton;
        private System.Windows.Forms.Label AnimatedWarningLabel;
        private System.Windows.Forms.NumericUpDown ModelBasePointerSpinner;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ModelNumPointers;
        private System.Windows.Forms.TextBox ModelNumTextures;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown ModelBaseTextureSpinner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
