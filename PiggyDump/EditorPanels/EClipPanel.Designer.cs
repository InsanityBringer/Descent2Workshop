namespace Descent2Workshop.EditorPanels
{
    partial class EClipPanel
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
            this.cbEClipMineCritical = new System.Windows.Forms.ComboBox();
            this.cbEClipBreakSound = new System.Windows.Forms.ComboBox();
            this.cbEClipBreakVClip = new System.Windows.Forms.ComboBox();
            this.cbEClipBreakEClip = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.txtEffectBrokenID = new System.Windows.Forms.TextBox();
            this.txtEffectExplodeSize = new System.Windows.Forms.TextBox();
            this.txtEffectLight = new System.Windows.Forms.TextBox();
            this.txtEffectTotalTime = new System.Windows.Forms.TextBox();
            this.txtEffectFrameSpeed = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbEffectOneShot = new System.Windows.Forms.CheckBox();
            this.cbEffectCritical = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnRemapECFrame = new System.Windows.Forms.Button();
            this.nudEffectFrame = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEffectFrameNum = new System.Windows.Forms.TextBox();
            this.pbEffectFramePreview = new System.Windows.Forms.PictureBox();
            this.txtEffectFrameCount = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.PlayCheckbox = new System.Windows.Forms.CheckBox();
            this.AnimTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEffectFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEffectFramePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // RemapAnimationButton
            // 
            this.RemapAnimationButton.Location = new System.Drawing.Point(83, 81);
            this.RemapAnimationButton.Name = "RemapAnimationButton";
            this.RemapAnimationButton.Size = new System.Drawing.Size(139, 23);
            this.RemapAnimationButton.TabIndex = 106;
            this.RemapAnimationButton.Tag = "2";
            this.RemapAnimationButton.Text = "RemapAnimationButton";
            this.RemapAnimationButton.UseVisualStyleBackColor = true;
            this.RemapAnimationButton.Click += new System.EventHandler(this.RemapMultiImage_Click);
            // 
            // cbEClipMineCritical
            // 
            this.cbEClipMineCritical.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEClipMineCritical.FormattingEnabled = true;
            this.cbEClipMineCritical.Location = new System.Drawing.Point(457, 138);
            this.cbEClipMineCritical.Name = "cbEClipMineCritical";
            this.cbEClipMineCritical.Size = new System.Drawing.Size(100, 21);
            this.cbEClipMineCritical.TabIndex = 105;
            this.cbEClipMineCritical.Tag = "4";
            this.cbEClipMineCritical.SelectedIndexChanged += new System.EventHandler(this.EClipComboBox_SelectedIndexChanged);
            // 
            // cbEClipBreakSound
            // 
            this.cbEClipBreakSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEClipBreakSound.FormattingEnabled = true;
            this.cbEClipBreakSound.Location = new System.Drawing.Point(457, 111);
            this.cbEClipBreakSound.Name = "cbEClipBreakSound";
            this.cbEClipBreakSound.Size = new System.Drawing.Size(100, 21);
            this.cbEClipBreakSound.TabIndex = 104;
            this.cbEClipBreakSound.Tag = "3";
            this.cbEClipBreakSound.SelectedIndexChanged += new System.EventHandler(this.EClipComboBox_SelectedIndexChanged);
            // 
            // cbEClipBreakVClip
            // 
            this.cbEClipBreakVClip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEClipBreakVClip.FormattingEnabled = true;
            this.cbEClipBreakVClip.Location = new System.Drawing.Point(457, 32);
            this.cbEClipBreakVClip.Name = "cbEClipBreakVClip";
            this.cbEClipBreakVClip.Size = new System.Drawing.Size(100, 21);
            this.cbEClipBreakVClip.TabIndex = 103;
            this.cbEClipBreakVClip.Tag = "2";
            this.cbEClipBreakVClip.SelectedIndexChanged += new System.EventHandler(this.EClipComboBox_SelectedIndexChanged);
            // 
            // cbEClipBreakEClip
            // 
            this.cbEClipBreakEClip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEClipBreakEClip.FormattingEnabled = true;
            this.cbEClipBreakEClip.Location = new System.Drawing.Point(457, 5);
            this.cbEClipBreakEClip.Name = "cbEClipBreakEClip";
            this.cbEClipBreakEClip.Size = new System.Drawing.Size(100, 21);
            this.cbEClipBreakEClip.TabIndex = 102;
            this.cbEClipBreakEClip.Tag = "1";
            this.cbEClipBreakEClip.SelectedIndexChanged += new System.EventHandler(this.EClipComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(355, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 101;
            this.label5.Text = "Mine critical effect:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(358, 88);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(93, 13);
            this.label33.TabIndex = 98;
            this.label33.Text = "Broken texture ID:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(371, 114);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(80, 13);
            this.label32.TabIndex = 97;
            this.label32.Text = "Ambient sound:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(362, 62);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(89, 13);
            this.label31.TabIndex = 100;
            this.label31.Text = "Size of explosion:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(371, 35);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(80, 13);
            this.label30.TabIndex = 99;
            this.label30.Text = "Break particles:";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(383, 8);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(68, 13);
            this.label29.TabIndex = 96;
            this.label29.Text = "Break effect:";
            // 
            // txtEffectBrokenID
            // 
            this.txtEffectBrokenID.Location = new System.Drawing.Point(457, 85);
            this.txtEffectBrokenID.Name = "txtEffectBrokenID";
            this.txtEffectBrokenID.Size = new System.Drawing.Size(100, 20);
            this.txtEffectBrokenID.TabIndex = 95;
            this.txtEffectBrokenID.Tag = "4";
            this.txtEffectBrokenID.TextChanged += new System.EventHandler(this.EClipProperty_TextChanged);
            // 
            // txtEffectExplodeSize
            // 
            this.txtEffectExplodeSize.Location = new System.Drawing.Point(457, 59);
            this.txtEffectExplodeSize.Name = "txtEffectExplodeSize";
            this.txtEffectExplodeSize.Size = new System.Drawing.Size(100, 20);
            this.txtEffectExplodeSize.TabIndex = 94;
            this.txtEffectExplodeSize.Tag = "3";
            this.txtEffectExplodeSize.TextChanged += new System.EventHandler(this.EClipFixedProperty_TextChanged);
            // 
            // txtEffectLight
            // 
            this.txtEffectLight.Location = new System.Drawing.Point(83, 55);
            this.txtEffectLight.Name = "txtEffectLight";
            this.txtEffectLight.Size = new System.Drawing.Size(76, 20);
            this.txtEffectLight.TabIndex = 91;
            this.txtEffectLight.Tag = "2";
            this.txtEffectLight.TextChanged += new System.EventHandler(this.EClipFixedProperty_TextChanged);
            // 
            // txtEffectTotalTime
            // 
            this.txtEffectTotalTime.Location = new System.Drawing.Point(83, 29);
            this.txtEffectTotalTime.Name = "txtEffectTotalTime";
            this.txtEffectTotalTime.Size = new System.Drawing.Size(76, 20);
            this.txtEffectTotalTime.TabIndex = 89;
            this.txtEffectTotalTime.Tag = "1";
            this.txtEffectTotalTime.TextChanged += new System.EventHandler(this.EClipFixedProperty_TextChanged);
            // 
            // txtEffectFrameSpeed
            // 
            this.txtEffectFrameSpeed.Location = new System.Drawing.Point(83, 3);
            this.txtEffectFrameSpeed.Name = "txtEffectFrameSpeed";
            this.txtEffectFrameSpeed.ReadOnly = true;
            this.txtEffectFrameSpeed.Size = new System.Drawing.Size(76, 20);
            this.txtEffectFrameSpeed.TabIndex = 87;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cbEffectOneShot);
            this.groupBox6.Controls.Add(this.cbEffectCritical);
            this.groupBox6.Location = new System.Drawing.Point(356, 165);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(201, 65);
            this.groupBox6.TabIndex = 93;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Useless EClip flags";
            // 
            // cbEffectOneShot
            // 
            this.cbEffectOneShot.AutoSize = true;
            this.cbEffectOneShot.Location = new System.Drawing.Point(6, 42);
            this.cbEffectOneShot.Name = "cbEffectOneShot";
            this.cbEffectOneShot.Size = new System.Drawing.Size(100, 17);
            this.cbEffectOneShot.TabIndex = 18;
            this.cbEffectOneShot.Text = "Plays only once";
            this.cbEffectOneShot.UseVisualStyleBackColor = true;
            // 
            // cbEffectCritical
            // 
            this.cbEffectCritical.AutoSize = true;
            this.cbEffectCritical.Location = new System.Drawing.Point(6, 19);
            this.cbEffectCritical.Name = "cbEffectCritical";
            this.cbEffectCritical.Size = new System.Drawing.Size(76, 17);
            this.cbEffectCritical.TabIndex = 18;
            this.cbEffectCritical.Text = "Critical clip";
            this.cbEffectCritical.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.PlayCheckbox);
            this.groupBox5.Controls.Add(this.btnRemapECFrame);
            this.groupBox5.Controls.Add(this.nudEffectFrame);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.txtEffectFrameNum);
            this.groupBox5.Controls.Add(this.pbEffectFramePreview);
            this.groupBox5.Controls.Add(this.txtEffectFrameCount);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Location = new System.Drawing.Point(9, 138);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(336, 302);
            this.groupBox5.TabIndex = 92;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Frames";
            // 
            // btnRemapECFrame
            // 
            this.btnRemapECFrame.Location = new System.Drawing.Point(189, 69);
            this.btnRemapECFrame.Name = "btnRemapECFrame";
            this.btnRemapECFrame.Size = new System.Drawing.Size(36, 23);
            this.btnRemapECFrame.TabIndex = 28;
            this.btnRemapECFrame.Tag = "3";
            this.btnRemapECFrame.Text = "...";
            this.btnRemapECFrame.UseVisualStyleBackColor = true;
            this.btnRemapECFrame.Click += new System.EventHandler(this.RemapSingleImage_Click);
            // 
            // nudEffectFrame
            // 
            this.nudEffectFrame.Location = new System.Drawing.Point(269, 19);
            this.nudEffectFrame.Maximum = new decimal(new int[] {
            29,
            0,
            0,
            0});
            this.nudEffectFrame.Name = "nudEffectFrame";
            this.nudEffectFrame.Size = new System.Drawing.Size(61, 20);
            this.nudEffectFrame.TabIndex = 27;
            this.nudEffectFrame.ValueChanged += new System.EventHandler(this.nudEffectFrame_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Bitmap Index:";
            // 
            // txtEffectFrameNum
            // 
            this.txtEffectFrameNum.Location = new System.Drawing.Point(83, 71);
            this.txtEffectFrameNum.Name = "txtEffectFrameNum";
            this.txtEffectFrameNum.Size = new System.Drawing.Size(100, 20);
            this.txtEffectFrameNum.TabIndex = 24;
            this.txtEffectFrameNum.Tag = "6";
            this.txtEffectFrameNum.TextChanged += new System.EventHandler(this.EClipProperty_TextChanged);
            // 
            // pbEffectFramePreview
            // 
            this.pbEffectFramePreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbEffectFramePreview.Location = new System.Drawing.Point(6, 97);
            this.pbEffectFramePreview.Name = "pbEffectFramePreview";
            this.pbEffectFramePreview.Size = new System.Drawing.Size(320, 200);
            this.pbEffectFramePreview.TabIndex = 19;
            this.pbEffectFramePreview.TabStop = false;
            // 
            // txtEffectFrameCount
            // 
            this.txtEffectFrameCount.Location = new System.Drawing.Point(56, 25);
            this.txtEffectFrameCount.Name = "txtEffectFrameCount";
            this.txtEffectFrameCount.Size = new System.Drawing.Size(77, 20);
            this.txtEffectFrameCount.TabIndex = 23;
            this.txtEffectFrameCount.Tag = "5";
            this.txtEffectFrameCount.TextChanged += new System.EventHandler(this.EClipProperty_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 28);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(44, 13);
            this.label17.TabIndex = 22;
            this.label17.Text = "Frames:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(15, 58);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(62, 13);
            this.label21.TabIndex = 90;
            this.label21.Text = "Light value:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(21, 32);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(56, 13);
            this.label22.TabIndex = 88;
            this.label22.Text = "Total time:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 6);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(71, 13);
            this.label23.TabIndex = 86;
            this.label23.Text = "Frame speed:";
            // 
            // PlayCheckbox
            // 
            this.PlayCheckbox.AutoSize = true;
            this.PlayCheckbox.Location = new System.Drawing.Point(284, 45);
            this.PlayCheckbox.Name = "PlayCheckbox";
            this.PlayCheckbox.Size = new System.Drawing.Size(46, 17);
            this.PlayCheckbox.TabIndex = 29;
            this.PlayCheckbox.Text = "Play";
            this.PlayCheckbox.UseVisualStyleBackColor = true;
            this.PlayCheckbox.CheckedChanged += new System.EventHandler(this.PlayCheckbox_CheckedChanged);
            // 
            // AnimTimer
            // 
            this.AnimTimer.Tick += new System.EventHandler(this.AnimTimer_Tick);
            // 
            // EClipPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RemapAnimationButton);
            this.Controls.Add(this.cbEClipMineCritical);
            this.Controls.Add(this.cbEClipBreakSound);
            this.Controls.Add(this.cbEClipBreakVClip);
            this.Controls.Add(this.cbEClipBreakEClip);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.txtEffectBrokenID);
            this.Controls.Add(this.txtEffectExplodeSize);
            this.Controls.Add(this.txtEffectLight);
            this.Controls.Add(this.txtEffectTotalTime);
            this.Controls.Add(this.txtEffectFrameSpeed);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label23);
            this.Name = "EClipPanel";
            this.Size = new System.Drawing.Size(570, 449);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEffectFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEffectFramePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RemapAnimationButton;
        private System.Windows.Forms.ComboBox cbEClipMineCritical;
        private System.Windows.Forms.ComboBox cbEClipBreakSound;
        private System.Windows.Forms.ComboBox cbEClipBreakVClip;
        private System.Windows.Forms.ComboBox cbEClipBreakEClip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtEffectBrokenID;
        private System.Windows.Forms.TextBox txtEffectExplodeSize;
        private System.Windows.Forms.TextBox txtEffectLight;
        private System.Windows.Forms.TextBox txtEffectTotalTime;
        private System.Windows.Forms.TextBox txtEffectFrameSpeed;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox cbEffectOneShot;
        private System.Windows.Forms.CheckBox cbEffectCritical;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnRemapECFrame;
        private System.Windows.Forms.NumericUpDown nudEffectFrame;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEffectFrameNum;
        private System.Windows.Forms.PictureBox pbEffectFramePreview;
        private System.Windows.Forms.TextBox txtEffectFrameCount;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.CheckBox PlayCheckbox;
        private System.Windows.Forms.Timer AnimTimer;
    }
}
