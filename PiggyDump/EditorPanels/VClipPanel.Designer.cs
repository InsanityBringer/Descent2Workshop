namespace Descent2Workshop.EditorPanels
{
    partial class VClipPanel
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
            this.components = new System.ComponentModel.Container();
            this.RemapAnimationButton = new System.Windows.Forms.Button();
            this.SoundComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.LightValueTextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.TotalTimeTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PlayCheckbox = new System.Windows.Forms.CheckBox();
            this.btnRemapVCFrame = new System.Windows.Forms.Button();
            this.FrameSpinner = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.FrameNumTextBox = new System.Windows.Forms.TextBox();
            this.pbAnimFramePreview = new System.Windows.Forms.PictureBox();
            this.FrameCountTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FrameTimeTextBox = new System.Windows.Forms.TextBox();
            this.VClipRodFlag = new System.Windows.Forms.CheckBox();
            this.AnimTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FrameSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimFramePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // RemapAnimationButton
            // 
            this.RemapAnimationButton.Location = new System.Drawing.Point(83, 109);
            this.RemapAnimationButton.Name = "RemapAnimationButton";
            this.RemapAnimationButton.Size = new System.Drawing.Size(139, 23);
            this.RemapAnimationButton.TabIndex = 72;
            this.RemapAnimationButton.Tag = "1";
            this.RemapAnimationButton.Text = "Change Animation...";
            this.RemapAnimationButton.UseVisualStyleBackColor = true;
            this.RemapAnimationButton.Click += new System.EventHandler(this.RemapMultiImage_Click);
            // 
            // SoundComboBox
            // 
            this.SoundComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SoundComboBox.FormattingEnabled = true;
            this.SoundComboBox.Location = new System.Drawing.Point(83, 81);
            this.SoundComboBox.Name = "SoundComboBox";
            this.SoundComboBox.Size = new System.Drawing.Size(139, 21);
            this.SoundComboBox.TabIndex = 71;
            this.SoundComboBox.Tag = "SoundNum";
            this.SoundComboBox.SelectedIndexChanged += new System.EventHandler(this.VClipSound_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 13);
            this.label13.TabIndex = 62;
            this.label13.Text = "Frame time:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 32);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 13);
            this.label14.TabIndex = 64;
            this.label14.Text = "Total time:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 58);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 13);
            this.label15.TabIndex = 66;
            this.label15.Text = "Light value:";
            // 
            // LightValueTextBox
            // 
            this.LightValueTextBox.Location = new System.Drawing.Point(83, 55);
            this.LightValueTextBox.Name = "LightValueTextBox";
            this.LightValueTextBox.Size = new System.Drawing.Size(76, 20);
            this.LightValueTextBox.TabIndex = 67;
            this.LightValueTextBox.Tag = "LightValue";
            this.LightValueTextBox.TextChanged += new System.EventHandler(this.VClipFixedProperty_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(36, 84);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 13);
            this.label16.TabIndex = 68;
            this.label16.Text = "Sound:";
            // 
            // TotalTimeTextBox
            // 
            this.TotalTimeTextBox.Location = new System.Drawing.Point(83, 29);
            this.TotalTimeTextBox.Name = "TotalTimeTextBox";
            this.TotalTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.TotalTimeTextBox.TabIndex = 65;
            this.TotalTimeTextBox.Tag = "PlayTime";
            this.TotalTimeTextBox.TextChanged += new System.EventHandler(this.VClipFixedProperty_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PlayCheckbox);
            this.groupBox2.Controls.Add(this.btnRemapVCFrame);
            this.groupBox2.Controls.Add(this.FrameSpinner);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.FrameNumTextBox);
            this.groupBox2.Controls.Add(this.pbAnimFramePreview);
            this.groupBox2.Controls.Add(this.FrameCountTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(9, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 306);
            this.groupBox2.TabIndex = 69;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Frames";
            // 
            // PlayCheckbox
            // 
            this.PlayCheckbox.AutoSize = true;
            this.PlayCheckbox.Location = new System.Drawing.Point(284, 45);
            this.PlayCheckbox.Name = "PlayCheckbox";
            this.PlayCheckbox.Size = new System.Drawing.Size(46, 17);
            this.PlayCheckbox.TabIndex = 73;
            this.PlayCheckbox.Text = "Play";
            this.PlayCheckbox.UseVisualStyleBackColor = true;
            this.PlayCheckbox.CheckedChanged += new System.EventHandler(this.PlayCheckbox_CheckedChanged);
            // 
            // btnRemapVCFrame
            // 
            this.btnRemapVCFrame.Location = new System.Drawing.Point(189, 69);
            this.btnRemapVCFrame.Name = "btnRemapVCFrame";
            this.btnRemapVCFrame.Size = new System.Drawing.Size(36, 23);
            this.btnRemapVCFrame.TabIndex = 27;
            this.btnRemapVCFrame.Tag = "2";
            this.btnRemapVCFrame.Text = "...";
            this.btnRemapVCFrame.UseVisualStyleBackColor = true;
            this.btnRemapVCFrame.Click += new System.EventHandler(this.RemapSingleImage_Click);
            // 
            // FrameSpinner
            // 
            this.FrameSpinner.Location = new System.Drawing.Point(269, 19);
            this.FrameSpinner.Maximum = new decimal(new int[] {
            29,
            0,
            0,
            0});
            this.FrameSpinner.Name = "FrameSpinner";
            this.FrameSpinner.Size = new System.Drawing.Size(61, 20);
            this.FrameSpinner.TabIndex = 26;
            this.FrameSpinner.ValueChanged += new System.EventHandler(this.nudAnimFrame_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Bitmap Index:";
            // 
            // FrameNumTextBox
            // 
            this.FrameNumTextBox.Location = new System.Drawing.Point(83, 71);
            this.FrameNumTextBox.Name = "FrameNumTextBox";
            this.FrameNumTextBox.Size = new System.Drawing.Size(100, 20);
            this.FrameNumTextBox.TabIndex = 24;
            this.FrameNumTextBox.Tag = "2";
            this.FrameNumTextBox.TextChanged += new System.EventHandler(this.FrameNumTextBox_TextChanged);
            // 
            // pbAnimFramePreview
            // 
            this.pbAnimFramePreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbAnimFramePreview.Location = new System.Drawing.Point(6, 97);
            this.pbAnimFramePreview.Name = "pbAnimFramePreview";
            this.pbAnimFramePreview.Size = new System.Drawing.Size(320, 200);
            this.pbAnimFramePreview.TabIndex = 19;
            this.pbAnimFramePreview.TabStop = false;
            // 
            // FrameCountTextBox
            // 
            this.FrameCountTextBox.Location = new System.Drawing.Point(56, 25);
            this.FrameCountTextBox.Name = "FrameCountTextBox";
            this.FrameCountTextBox.Size = new System.Drawing.Size(77, 20);
            this.FrameCountTextBox.TabIndex = 23;
            this.FrameCountTextBox.Tag = "NumFrames";
            this.FrameCountTextBox.TextChanged += new System.EventHandler(this.VClipProperty_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Frames:";
            // 
            // FrameTimeTextBox
            // 
            this.FrameTimeTextBox.Location = new System.Drawing.Point(83, 3);
            this.FrameTimeTextBox.Name = "FrameTimeTextBox";
            this.FrameTimeTextBox.ReadOnly = true;
            this.FrameTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.FrameTimeTextBox.TabIndex = 63;
            // 
            // VClipRodFlag
            // 
            this.VClipRodFlag.AutoSize = true;
            this.VClipRodFlag.Location = new System.Drawing.Point(165, 5);
            this.VClipRodFlag.Name = "VClipRodFlag";
            this.VClipRodFlag.Size = new System.Drawing.Size(83, 17);
            this.VClipRodFlag.TabIndex = 18;
            this.VClipRodFlag.Tag = "DrawAsRod";
            this.VClipRodFlag.Text = "Draw as rod";
            this.VClipRodFlag.UseVisualStyleBackColor = true;
            this.VClipRodFlag.CheckedChanged += new System.EventHandler(this.VClipRodFlag_CheckedChanged);
            // 
            // AnimTimer
            // 
            this.AnimTimer.Tick += new System.EventHandler(this.AnimTimer_Tick);
            // 
            // VClipPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VClipRodFlag);
            this.Controls.Add(this.RemapAnimationButton);
            this.Controls.Add(this.SoundComboBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.LightValueTextBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.TotalTimeTextBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.FrameTimeTextBox);
            this.Name = "VClipPanel";
            this.Size = new System.Drawing.Size(358, 453);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FrameSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimFramePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RemapAnimationButton;
        private System.Windows.Forms.ComboBox SoundComboBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox LightValueTextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox TotalTimeTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemapVCFrame;
        private System.Windows.Forms.NumericUpDown FrameSpinner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FrameNumTextBox;
        private System.Windows.Forms.PictureBox pbAnimFramePreview;
        private System.Windows.Forms.TextBox FrameCountTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FrameTimeTextBox;
        private System.Windows.Forms.CheckBox VClipRodFlag;
        private System.Windows.Forms.CheckBox PlayCheckbox;
        private System.Windows.Forms.Timer AnimTimer;
    }
}
