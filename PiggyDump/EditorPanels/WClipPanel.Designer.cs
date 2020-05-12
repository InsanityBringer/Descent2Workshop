namespace Descent2Workshop.EditorPanels
{
    partial class WClipPanel
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
            this.CloseSoundComboBox = new System.Windows.Forms.ComboBox();
            this.OpenSoundComboBox = new System.Windows.Forms.ComboBox();
            this.FilenameTextBox = new System.Windows.Forms.TextBox();
            this.TotalTimeTextBox = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.HiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.OnPrimaryTMapCheckBox = new System.Windows.Forms.CheckBox();
            this.ShootableCheckBox = new System.Windows.Forms.CheckBox();
            this.ExplodesCheckBox = new System.Windows.Forms.CheckBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.FrameSpinner = new System.Windows.Forms.NumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.FrameTextBox = new System.Windows.Forms.TextBox();
            this.FramePictureBox = new System.Windows.Forms.PictureBox();
            this.NumFramesTextBox = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FrameSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FramePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseSoundComboBox
            // 
            this.CloseSoundComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CloseSoundComboBox.FormattingEnabled = true;
            this.CloseSoundComboBox.Location = new System.Drawing.Point(83, 56);
            this.CloseSoundComboBox.Name = "CloseSoundComboBox";
            this.CloseSoundComboBox.Size = new System.Drawing.Size(150, 21);
            this.CloseSoundComboBox.TabIndex = 79;
            this.CloseSoundComboBox.Tag = "CloseSound";
            this.CloseSoundComboBox.SelectedIndexChanged += new System.EventHandler(this.SoundComboBox_SelectedIndexChanged);
            // 
            // OpenSoundComboBox
            // 
            this.OpenSoundComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OpenSoundComboBox.FormattingEnabled = true;
            this.OpenSoundComboBox.Location = new System.Drawing.Point(83, 29);
            this.OpenSoundComboBox.Name = "OpenSoundComboBox";
            this.OpenSoundComboBox.Size = new System.Drawing.Size(150, 21);
            this.OpenSoundComboBox.TabIndex = 78;
            this.OpenSoundComboBox.Tag = "OpenSound";
            this.OpenSoundComboBox.SelectedIndexChanged += new System.EventHandler(this.SoundComboBox_SelectedIndexChanged);
            // 
            // FilenameTextBox
            // 
            this.FilenameTextBox.Location = new System.Drawing.Point(83, 83);
            this.FilenameTextBox.Name = "FilenameTextBox";
            this.FilenameTextBox.Size = new System.Drawing.Size(150, 20);
            this.FilenameTextBox.TabIndex = 77;
            this.FilenameTextBox.Tag = "2";
            // 
            // TotalTimeTextBox
            // 
            this.TotalTimeTextBox.Location = new System.Drawing.Point(83, 3);
            this.TotalTimeTextBox.Name = "TotalTimeTextBox";
            this.TotalTimeTextBox.Size = new System.Drawing.Size(82, 20);
            this.TotalTimeTextBox.TabIndex = 74;
            this.TotalTimeTextBox.Tag = "PlayTime";
            this.TotalTimeTextBox.TextChanged += new System.EventHandler(this.FixPropertyTextBox_TextChanged);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(25, 86);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(52, 13);
            this.label40.TabIndex = 76;
            this.label40.Text = "Filename:";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.HiddenCheckBox);
            this.groupBox8.Controls.Add(this.OnPrimaryTMapCheckBox);
            this.groupBox8.Controls.Add(this.ShootableCheckBox);
            this.groupBox8.Controls.Add(this.ExplodesCheckBox);
            this.groupBox8.Location = new System.Drawing.Point(239, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(162, 126);
            this.groupBox8.TabIndex = 75;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Flags";
            // 
            // HiddenCheckBox
            // 
            this.HiddenCheckBox.AutoSize = true;
            this.HiddenCheckBox.Location = new System.Drawing.Point(6, 88);
            this.HiddenCheckBox.Name = "HiddenCheckBox";
            this.HiddenCheckBox.Size = new System.Drawing.Size(98, 17);
            this.HiddenCheckBox.TabIndex = 3;
            this.HiddenCheckBox.Tag = "SecretDoor";
            this.HiddenCheckBox.Text = "Hidden on map";
            this.HiddenCheckBox.UseVisualStyleBackColor = true;
            this.HiddenCheckBox.CheckedChanged += new System.EventHandler(this.FlagCheckBox_CheckedChanged);
            // 
            // OnPrimaryTMapCheckBox
            // 
            this.OnPrimaryTMapCheckBox.AutoSize = true;
            this.OnPrimaryTMapCheckBox.Location = new System.Drawing.Point(6, 65);
            this.OnPrimaryTMapCheckBox.Name = "OnPrimaryTMapCheckBox";
            this.OnPrimaryTMapCheckBox.Size = new System.Drawing.Size(106, 17);
            this.OnPrimaryTMapCheckBox.TabIndex = 2;
            this.OnPrimaryTMapCheckBox.Tag = "PrimaryTMap";
            this.OnPrimaryTMapCheckBox.Text = "On Base Texture";
            this.OnPrimaryTMapCheckBox.UseVisualStyleBackColor = true;
            this.OnPrimaryTMapCheckBox.CheckedChanged += new System.EventHandler(this.FlagCheckBox_CheckedChanged);
            // 
            // ShootableCheckBox
            // 
            this.ShootableCheckBox.AutoSize = true;
            this.ShootableCheckBox.Location = new System.Drawing.Point(6, 42);
            this.ShootableCheckBox.Name = "ShootableCheckBox";
            this.ShootableCheckBox.Size = new System.Drawing.Size(83, 17);
            this.ShootableCheckBox.TabIndex = 1;
            this.ShootableCheckBox.Tag = "Blastable";
            this.ShootableCheckBox.Text = "Is shootable";
            this.ShootableCheckBox.UseVisualStyleBackColor = true;
            this.ShootableCheckBox.CheckedChanged += new System.EventHandler(this.FlagCheckBox_CheckedChanged);
            // 
            // ExplodesCheckBox
            // 
            this.ExplodesCheckBox.AutoSize = true;
            this.ExplodesCheckBox.Location = new System.Drawing.Point(6, 19);
            this.ExplodesCheckBox.Name = "ExplodesCheckBox";
            this.ExplodesCheckBox.Size = new System.Drawing.Size(111, 17);
            this.ExplodesCheckBox.TabIndex = 0;
            this.ExplodesCheckBox.Tag = "Explodes";
            this.ExplodesCheckBox.Text = "Explodes on open";
            this.ExplodesCheckBox.UseVisualStyleBackColor = true;
            this.ExplodesCheckBox.CheckedChanged += new System.EventHandler(this.FlagCheckBox_CheckedChanged);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(9, 59);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(68, 13);
            this.label38.TabIndex = 71;
            this.label38.Text = "Close sound:";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(9, 32);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(68, 13);
            this.label37.TabIndex = 72;
            this.label37.Text = "Open sound:";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(21, 6);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(56, 13);
            this.label36.TabIndex = 73;
            this.label36.Text = "Total time:";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.FrameSpinner);
            this.groupBox7.Controls.Add(this.label26);
            this.groupBox7.Controls.Add(this.FrameTextBox);
            this.groupBox7.Controls.Add(this.FramePictureBox);
            this.groupBox7.Controls.Add(this.NumFramesTextBox);
            this.groupBox7.Controls.Add(this.label27);
            this.groupBox7.Location = new System.Drawing.Point(12, 137);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(336, 316);
            this.groupBox7.TabIndex = 70;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Frames";
            // 
            // FrameSpinner
            // 
            this.FrameSpinner.Location = new System.Drawing.Point(269, 19);
            this.FrameSpinner.Maximum = new decimal(new int[] {
            49,
            0,
            0,
            0});
            this.FrameSpinner.Name = "FrameSpinner";
            this.FrameSpinner.Size = new System.Drawing.Size(61, 20);
            this.FrameSpinner.TabIndex = 28;
            this.FrameSpinner.ValueChanged += new System.EventHandler(this.FrameSpinner_ValueChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(6, 74);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(75, 13);
            this.label26.TabIndex = 25;
            this.label26.Text = "Texture Index:";
            // 
            // FrameTextBox
            // 
            this.FrameTextBox.Location = new System.Drawing.Point(83, 71);
            this.FrameTextBox.Name = "FrameTextBox";
            this.FrameTextBox.Size = new System.Drawing.Size(100, 20);
            this.FrameTextBox.TabIndex = 24;
            this.FrameTextBox.TextChanged += new System.EventHandler(this.FrameTextBox_TextChanged);
            // 
            // FramePictureBox
            // 
            this.FramePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FramePictureBox.Location = new System.Drawing.Point(6, 97);
            this.FramePictureBox.Name = "FramePictureBox";
            this.FramePictureBox.Size = new System.Drawing.Size(320, 200);
            this.FramePictureBox.TabIndex = 19;
            this.FramePictureBox.TabStop = false;
            // 
            // NumFramesTextBox
            // 
            this.NumFramesTextBox.Location = new System.Drawing.Point(56, 25);
            this.NumFramesTextBox.Name = "NumFramesTextBox";
            this.NumFramesTextBox.Size = new System.Drawing.Size(77, 20);
            this.NumFramesTextBox.TabIndex = 23;
            this.NumFramesTextBox.Tag = "NumFrames";
            this.NumFramesTextBox.TextChanged += new System.EventHandler(this.NumFramesTextBox_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 28);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(44, 13);
            this.label27.TabIndex = 22;
            this.label27.Text = "Frames:";
            // 
            // WClipPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CloseSoundComboBox);
            this.Controls.Add(this.OpenSoundComboBox);
            this.Controls.Add(this.FilenameTextBox);
            this.Controls.Add(this.TotalTimeTextBox);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.groupBox7);
            this.Name = "WClipPanel";
            this.Size = new System.Drawing.Size(416, 461);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FrameSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FramePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CloseSoundComboBox;
        private System.Windows.Forms.ComboBox OpenSoundComboBox;
        private System.Windows.Forms.TextBox FilenameTextBox;
        private System.Windows.Forms.TextBox TotalTimeTextBox;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox HiddenCheckBox;
        private System.Windows.Forms.CheckBox OnPrimaryTMapCheckBox;
        private System.Windows.Forms.CheckBox ShootableCheckBox;
        private System.Windows.Forms.CheckBox ExplodesCheckBox;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.NumericUpDown FrameSpinner;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox FrameTextBox;
        private System.Windows.Forms.PictureBox FramePictureBox;
        private System.Windows.Forms.TextBox NumFramesTextBox;
        private System.Windows.Forms.Label label27;
    }
}
