namespace Descent2Workshop
{
    partial class HXMEditor
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
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.RobotTabPage = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.btnExportModel = new System.Windows.Forms.Button();
            this.btnImportModel = new System.Windows.Forms.Button();
            this.cbModelDeadModel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbModelDyingModel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbModelLowDetail = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
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
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.label50 = new System.Windows.Forms.Label();
            this.nudElementNum = new System.Windows.Forms.NumericUpDown();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.label48 = new System.Windows.Forms.Label();
            this.InsertButton = new System.Windows.Forms.Button();
            this.label49 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ReplacedElementComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelBasePointerSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelBaseTextureSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudElementNum)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem5});
            this.menuItem1.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Save";
            this.menuItem2.Click += new System.EventHandler(this.MenuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Save As...";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click_1);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "Close Window";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.RobotTabPage);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(856, 539);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // RobotTabPage
            // 
            this.RobotTabPage.Location = new System.Drawing.Point(4, 22);
            this.RobotTabPage.Name = "RobotTabPage";
            this.RobotTabPage.Size = new System.Drawing.Size(848, 513);
            this.RobotTabPage.TabIndex = 4;
            this.RobotTabPage.Text = "Robots";
            this.RobotTabPage.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.btnExportModel);
            this.tabPage7.Controls.Add(this.btnImportModel);
            this.tabPage7.Controls.Add(this.cbModelDeadModel);
            this.tabPage7.Controls.Add(this.label5);
            this.tabPage7.Controls.Add(this.cbModelDyingModel);
            this.tabPage7.Controls.Add(this.label4);
            this.tabPage7.Controls.Add(this.cbModelLowDetail);
            this.tabPage7.Controls.Add(this.groupBox1);
            this.tabPage7.Controls.Add(this.pictureBox3);
            this.tabPage7.Controls.Add(this.trackBar3);
            this.tabPage7.Controls.Add(this.trackBar2);
            this.tabPage7.Controls.Add(this.trackBar1);
            this.tabPage7.Controls.Add(this.label108);
            this.tabPage7.Controls.Add(this.label106);
            this.tabPage7.Controls.Add(this.txtModelTextureCount);
            this.tabPage7.Controls.Add(this.label105);
            this.tabPage7.Controls.Add(this.txtModelRadius);
            this.tabPage7.Controls.Add(this.txtModelDataSize);
            this.tabPage7.Controls.Add(this.label104);
            this.tabPage7.Controls.Add(this.txtModelNumModels);
            this.tabPage7.Controls.Add(this.label103);
            this.tabPage7.Controls.Add(this.label102);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(848, 513);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Models";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // btnExportModel
            // 
            this.btnExportModel.Location = new System.Drawing.Point(98, 223);
            this.btnExportModel.Name = "btnExportModel";
            this.btnExportModel.Size = new System.Drawing.Size(100, 23);
            this.btnExportModel.TabIndex = 37;
            this.btnExportModel.Text = "Export Model...";
            this.btnExportModel.UseVisualStyleBackColor = true;
            this.btnExportModel.Click += new System.EventHandler(this.btnExportModel_Click);
            // 
            // btnImportModel
            // 
            this.btnImportModel.Location = new System.Drawing.Point(98, 194);
            this.btnImportModel.Name = "btnImportModel";
            this.btnImportModel.Size = new System.Drawing.Size(100, 23);
            this.btnImportModel.TabIndex = 36;
            this.btnImportModel.Text = "Import Model...";
            this.btnImportModel.UseVisualStyleBackColor = true;
            this.btnImportModel.Click += new System.EventHandler(this.btnImportModel_Click);
            // 
            // cbModelDeadModel
            // 
            this.cbModelDeadModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModelDeadModel.FormattingEnabled = true;
            this.cbModelDeadModel.Location = new System.Drawing.Point(98, 167);
            this.cbModelDeadModel.Name = "cbModelDeadModel";
            this.cbModelDeadModel.Size = new System.Drawing.Size(234, 21);
            this.cbModelDeadModel.TabIndex = 35;
            this.cbModelDeadModel.Tag = "3";
            this.cbModelDeadModel.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Dead Model:";
            // 
            // cbModelDyingModel
            // 
            this.cbModelDyingModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModelDyingModel.FormattingEnabled = true;
            this.cbModelDyingModel.Location = new System.Drawing.Point(98, 140);
            this.cbModelDyingModel.Name = "cbModelDyingModel";
            this.cbModelDyingModel.Size = new System.Drawing.Size(234, 21);
            this.cbModelDyingModel.TabIndex = 33;
            this.cbModelDyingModel.Tag = "2";
            this.cbModelDyingModel.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Dying Model:";
            // 
            // cbModelLowDetail
            // 
            this.cbModelLowDetail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModelLowDetail.FormattingEnabled = true;
            this.cbModelLowDetail.Location = new System.Drawing.Point(98, 113);
            this.cbModelLowDetail.Name = "cbModelLowDetail";
            this.cbModelLowDetail.Size = new System.Drawing.Size(234, 21);
            this.cbModelLowDetail.TabIndex = 31;
            this.cbModelLowDetail.Tag = "1";
            this.cbModelLowDetail.SelectedIndexChanged += new System.EventHandler(this.ModelComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FindPackButton);
            this.groupBox1.Controls.Add(this.AnimatedWarningLabel);
            this.groupBox1.Controls.Add(this.ModelBasePointerSpinner);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.ModelNumPointers);
            this.groupBox1.Controls.Add(this.ModelNumTextures);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.ModelBaseTextureSpinner);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 360);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 150);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Textures";
            // 
            // FindPackButton
            // 
            this.FindPackButton.Location = new System.Drawing.Point(330, 11);
            this.FindPackButton.Name = "FindPackButton";
            this.FindPackButton.Size = new System.Drawing.Size(75, 23);
            this.FindPackButton.TabIndex = 37;
            this.FindPackButton.Text = "Find Pack";
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
            this.ModelBasePointerSpinner.Location = new System.Drawing.Point(249, 52);
            this.ModelBasePointerSpinner.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.ModelBasePointerSpinner.Name = "ModelBasePointerSpinner";
            this.ModelBasePointerSpinner.Size = new System.Drawing.Size(75, 20);
            this.ModelBasePointerSpinner.TabIndex = 34;
            this.ModelBasePointerSpinner.ValueChanged += new System.EventHandler(this.ModelBasePointerSpinner_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(194, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Insert At:";
            // 
            // ModelNumPointers
            // 
            this.ModelNumPointers.Location = new System.Drawing.Point(134, 52);
            this.ModelNumPointers.Name = "ModelNumPointers";
            this.ModelNumPointers.ReadOnly = true;
            this.ModelNumPointers.Size = new System.Drawing.Size(54, 20);
            this.ModelNumPointers.TabIndex = 33;
            // 
            // ModelNumTextures
            // 
            this.ModelNumTextures.Location = new System.Drawing.Point(134, 13);
            this.ModelNumTextures.Name = "ModelNumTextures";
            this.ModelNumTextures.ReadOnly = true;
            this.ModelNumTextures.Size = new System.Drawing.Size(54, 20);
            this.ModelNumTextures.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Needed Pointers:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Needed Object Bitmaps:";
            // 
            // ModelBaseTextureSpinner
            // 
            this.ModelBaseTextureSpinner.Location = new System.Drawing.Point(249, 13);
            this.ModelBaseTextureSpinner.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.ModelBaseTextureSpinner.Name = "ModelBaseTextureSpinner";
            this.ModelBaseTextureSpinner.Size = new System.Drawing.Size(75, 20);
            this.ModelBaseTextureSpinner.TabIndex = 29;
            this.ModelBaseTextureSpinner.ValueChanged += new System.EventHandler(this.ModelBaseTextureSpinner_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Insert At:";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(452, 61);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(256, 256);
            this.pictureBox3.TabIndex = 27;
            this.pictureBox3.TabStop = false;
            // 
            // trackBar3
            // 
            this.trackBar3.LargeChange = 4;
            this.trackBar3.Location = new System.Drawing.Point(710, 61);
            this.trackBar3.Maximum = 16;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar3.Size = new System.Drawing.Size(45, 256);
            this.trackBar3.TabIndex = 26;
            this.trackBar3.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar3.Value = 8;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(401, 61);
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar2.Size = new System.Drawing.Size(45, 256);
            this.trackBar2.TabIndex = 25;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar2.Value = 50;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 4;
            this.trackBar1.Location = new System.Drawing.Point(452, 323);
            this.trackBar1.Maximum = 16;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(256, 45);
            this.trackBar1.TabIndex = 24;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 8;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label108
            // 
            this.label108.AutoSize = true;
            this.label108.Location = new System.Drawing.Point(8, 116);
            this.label108.Name = "label108";
            this.label108.Size = new System.Drawing.Size(56, 13);
            this.label108.TabIndex = 12;
            this.label108.Text = "LD Model:";
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.Location = new System.Drawing.Point(8, 90);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(77, 13);
            this.label106.TabIndex = 9;
            this.label106.Text = "Texture Count:";
            // 
            // txtModelTextureCount
            // 
            this.txtModelTextureCount.Location = new System.Drawing.Point(98, 87);
            this.txtModelTextureCount.Name = "txtModelTextureCount";
            this.txtModelTextureCount.ReadOnly = true;
            this.txtModelTextureCount.Size = new System.Drawing.Size(100, 20);
            this.txtModelTextureCount.TabIndex = 8;
            // 
            // label105
            // 
            this.label105.AutoSize = true;
            this.label105.Location = new System.Drawing.Point(8, 64);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(43, 13);
            this.label105.TabIndex = 7;
            this.label105.Text = "Radius:";
            // 
            // txtModelRadius
            // 
            this.txtModelRadius.Location = new System.Drawing.Point(98, 61);
            this.txtModelRadius.Name = "txtModelRadius";
            this.txtModelRadius.ReadOnly = true;
            this.txtModelRadius.Size = new System.Drawing.Size(100, 20);
            this.txtModelRadius.TabIndex = 6;
            // 
            // txtModelDataSize
            // 
            this.txtModelDataSize.Location = new System.Drawing.Point(98, 33);
            this.txtModelDataSize.Name = "txtModelDataSize";
            this.txtModelDataSize.ReadOnly = true;
            this.txtModelDataSize.Size = new System.Drawing.Size(100, 20);
            this.txtModelDataSize.TabIndex = 5;
            // 
            // label104
            // 
            this.label104.AutoSize = true;
            this.label104.Location = new System.Drawing.Point(8, 36);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(68, 13);
            this.label104.TabIndex = 4;
            this.label104.Text = "Size of Data:";
            // 
            // txtModelNumModels
            // 
            this.txtModelNumModels.Location = new System.Drawing.Point(98, 7);
            this.txtModelNumModels.Name = "txtModelNumModels";
            this.txtModelNumModels.ReadOnly = true;
            this.txtModelNumModels.Size = new System.Drawing.Size(100, 20);
            this.txtModelNumModels.TabIndex = 3;
            // 
            // label103
            // 
            this.label103.AutoSize = true;
            this.label103.Location = new System.Drawing.Point(8, 10);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(62, 13);
            this.label103.TabIndex = 2;
            this.label103.Text = "Submodels:";
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.Location = new System.Drawing.Point(466, 45);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(48, 13);
            this.label102.TabIndex = 1;
            this.label102.Text = "Preview:";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 579);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(856, 22);
            this.statusBar1.TabIndex = 3;
            this.statusBar1.Text = "statusBar1";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(423, 15);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(99, 13);
            this.label50.TabIndex = 220;
            this.label50.Text = "Replacing Element:";
            // 
            // nudElementNum
            // 
            this.nudElementNum.Location = new System.Drawing.Point(102, 12);
            this.nudElementNum.Name = "nudElementNum";
            this.nudElementNum.Size = new System.Drawing.Size(63, 20);
            this.nudElementNum.TabIndex = 215;
            this.nudElementNum.ValueChanged += new System.EventHandler(this.nudElementNum_ValueChanged);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(342, 9);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 219;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(8, 14);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(88, 13);
            this.label48.TabIndex = 216;
            this.label48.Text = "Element Number:";
            // 
            // InsertButton
            // 
            this.InsertButton.Location = new System.Drawing.Point(261, 9);
            this.InsertButton.Name = "InsertButton";
            this.InsertButton.Size = new System.Drawing.Size(75, 23);
            this.InsertButton.TabIndex = 218;
            this.InsertButton.Text = "Insert";
            this.InsertButton.UseVisualStyleBackColor = true;
            this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(171, 15);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(84, 13);
            this.label49.TabIndex = 217;
            this.label49.Text = "Element Control:";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "HXM Files|*.HXM";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ReplacedElementComboBox
            // 
            this.ReplacedElementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReplacedElementComboBox.FormattingEnabled = true;
            this.ReplacedElementComboBox.Location = new System.Drawing.Point(528, 11);
            this.ReplacedElementComboBox.Name = "ReplacedElementComboBox";
            this.ReplacedElementComboBox.Size = new System.Drawing.Size(205, 21);
            this.ReplacedElementComboBox.TabIndex = 221;
            this.ReplacedElementComboBox.SelectedIndexChanged += new System.EventHandler(this.ReplacedElementComboBox_SelectedIndexChanged);
            // 
            // HXMEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 601);
            this.Controls.Add(this.ReplacedElementComboBox);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.nudElementNum);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.InsertButton);
            this.Controls.Add(this.label49);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Menu = this.mainMenu1;
            this.Name = "HXMEditor";
            this.Text = "HXMEditor";
            this.Load += new System.EventHandler(this.HXMEditor_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModelBasePointerSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelBaseTextureSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudElementNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage RobotTabPage;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.TrackBar trackBar1;
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
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.NumericUpDown nudElementNum;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Button InsertButton;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown ModelBaseTextureSpinner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbModelDeadModel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbModelDyingModel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbModelLowDetail;
        private System.Windows.Forms.Button btnExportModel;
        private System.Windows.Forms.Button btnImportModel;
        private System.Windows.Forms.NumericUpDown ModelBasePointerSpinner;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ModelNumPointers;
        private System.Windows.Forms.TextBox ModelNumTextures;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label AnimatedWarningLabel;
        private System.Windows.Forms.Button FindPackButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox ReplacedElementComboBox;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
    }
}