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
            this.cbVClipSound = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtAnimLight = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtAnimTotalTime = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRemapVCFrame = new System.Windows.Forms.Button();
            this.nudAnimFrame = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAnimFrameNum = new System.Windows.Forms.TextBox();
            this.pbAnimFramePreview = new System.Windows.Forms.PictureBox();
            this.txtAnimFrameCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAnimFrameSpeed = new System.Windows.Forms.TextBox();
            this.VClipRodFlag = new System.Windows.Forms.CheckBox();
            this.AnimTimer = new System.Windows.Forms.Timer(this.components);
            this.PlayCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnimFrame)).BeginInit();
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
            // cbVClipSound
            // 
            this.cbVClipSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVClipSound.FormattingEnabled = true;
            this.cbVClipSound.Location = new System.Drawing.Point(83, 81);
            this.cbVClipSound.Name = "cbVClipSound";
            this.cbVClipSound.Size = new System.Drawing.Size(139, 21);
            this.cbVClipSound.TabIndex = 71;
            this.cbVClipSound.SelectedIndexChanged += new System.EventHandler(this.VClipSound_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 13);
            this.label13.TabIndex = 62;
            this.label13.Text = "Frame speed:";
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
            // txtAnimLight
            // 
            this.txtAnimLight.Location = new System.Drawing.Point(83, 55);
            this.txtAnimLight.Name = "txtAnimLight";
            this.txtAnimLight.Size = new System.Drawing.Size(76, 20);
            this.txtAnimLight.TabIndex = 67;
            this.txtAnimLight.Tag = "2";
            this.txtAnimLight.TextChanged += new System.EventHandler(this.VClipFixedProperty_TextChanged);
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
            // txtAnimTotalTime
            // 
            this.txtAnimTotalTime.Location = new System.Drawing.Point(83, 29);
            this.txtAnimTotalTime.Name = "txtAnimTotalTime";
            this.txtAnimTotalTime.Size = new System.Drawing.Size(76, 20);
            this.txtAnimTotalTime.TabIndex = 65;
            this.txtAnimTotalTime.Tag = "1";
            this.txtAnimTotalTime.TextChanged += new System.EventHandler(this.VClipFixedProperty_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PlayCheckbox);
            this.groupBox2.Controls.Add(this.btnRemapVCFrame);
            this.groupBox2.Controls.Add(this.nudAnimFrame);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtAnimFrameNum);
            this.groupBox2.Controls.Add(this.pbAnimFramePreview);
            this.groupBox2.Controls.Add(this.txtAnimFrameCount);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(9, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 306);
            this.groupBox2.TabIndex = 69;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Frames";
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
            // nudAnimFrame
            // 
            this.nudAnimFrame.Location = new System.Drawing.Point(269, 19);
            this.nudAnimFrame.Maximum = new decimal(new int[] {
            29,
            0,
            0,
            0});
            this.nudAnimFrame.Name = "nudAnimFrame";
            this.nudAnimFrame.Size = new System.Drawing.Size(61, 20);
            this.nudAnimFrame.TabIndex = 26;
            this.nudAnimFrame.ValueChanged += new System.EventHandler(this.nudAnimFrame_ValueChanged);
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
            // txtAnimFrameNum
            // 
            this.txtAnimFrameNum.Location = new System.Drawing.Point(83, 71);
            this.txtAnimFrameNum.Name = "txtAnimFrameNum";
            this.txtAnimFrameNum.Size = new System.Drawing.Size(100, 20);
            this.txtAnimFrameNum.TabIndex = 24;
            this.txtAnimFrameNum.Tag = "2";
            this.txtAnimFrameNum.TextChanged += new System.EventHandler(this.VClipProperty_TextChanged);
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
            // txtAnimFrameCount
            // 
            this.txtAnimFrameCount.Location = new System.Drawing.Point(56, 25);
            this.txtAnimFrameCount.Name = "txtAnimFrameCount";
            this.txtAnimFrameCount.Size = new System.Drawing.Size(77, 20);
            this.txtAnimFrameCount.TabIndex = 23;
            this.txtAnimFrameCount.Tag = "1";
            this.txtAnimFrameCount.TextChanged += new System.EventHandler(this.VClipProperty_TextChanged);
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
            // txtAnimFrameSpeed
            // 
            this.txtAnimFrameSpeed.Location = new System.Drawing.Point(83, 3);
            this.txtAnimFrameSpeed.Name = "txtAnimFrameSpeed";
            this.txtAnimFrameSpeed.ReadOnly = true;
            this.txtAnimFrameSpeed.Size = new System.Drawing.Size(76, 20);
            this.txtAnimFrameSpeed.TabIndex = 63;
            // 
            // VClipRodFlag
            // 
            this.VClipRodFlag.AutoSize = true;
            this.VClipRodFlag.Location = new System.Drawing.Point(165, 5);
            this.VClipRodFlag.Name = "VClipRodFlag";
            this.VClipRodFlag.Size = new System.Drawing.Size(83, 17);
            this.VClipRodFlag.TabIndex = 18;
            this.VClipRodFlag.Text = "Draw as rod";
            this.VClipRodFlag.UseVisualStyleBackColor = true;
            this.VClipRodFlag.CheckedChanged += new System.EventHandler(this.VClipRodFlag_CheckedChanged);
            // 
            // AnimTimer
            // 
            this.AnimTimer.Tick += new System.EventHandler(this.AnimTimer_Tick);
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
            // VClipPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VClipRodFlag);
            this.Controls.Add(this.RemapAnimationButton);
            this.Controls.Add(this.cbVClipSound);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtAnimLight);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtAnimTotalTime);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtAnimFrameSpeed);
            this.Name = "VClipPanel";
            this.Size = new System.Drawing.Size(358, 453);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnimFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnimFramePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RemapAnimationButton;
        private System.Windows.Forms.ComboBox cbVClipSound;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtAnimLight;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtAnimTotalTime;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemapVCFrame;
        private System.Windows.Forms.NumericUpDown nudAnimFrame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAnimFrameNum;
        private System.Windows.Forms.PictureBox pbAnimFramePreview;
        private System.Windows.Forms.TextBox txtAnimFrameCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAnimFrameSpeed;
        private System.Windows.Forms.CheckBox VClipRodFlag;
        private System.Windows.Forms.CheckBox PlayCheckbox;
        private System.Windows.Forms.Timer AnimTimer;
    }
}
