namespace Descent2Workshop.EditorPanels
{
    partial class TMAPInfoPanel
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
            this.btnRemapTexture = new System.Windows.Forms.Button();
            this.cbTexEClip = new System.Windows.Forms.ComboBox();
            this.TextureDestroyedTextBox = new System.Windows.Forms.TextBox();
            this.label107 = new System.Windows.Forms.Label();
            this.txtTexSlideV = new System.Windows.Forms.TextBox();
            this.txtTexSlideU = new System.Windows.Forms.TextBox();
            this.txtTexDamage = new System.Windows.Forms.TextBox();
            this.txtTexLight = new System.Windows.Forms.TextBox();
            this.txtTexID = new System.Windows.Forms.TextBox();
            this.label130 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbTexHoardGoal = new System.Windows.Forms.CheckBox();
            this.cbTexRedGoal = new System.Windows.Forms.CheckBox();
            this.cbTexBlueGoal = new System.Windows.Forms.CheckBox();
            this.cbTexForcefield = new System.Windows.Forms.CheckBox();
            this.cbTexWater = new System.Windows.Forms.CheckBox();
            this.cbTexLava = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbTexPrev = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTexPrev)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRemapTexture
            // 
            this.btnRemapTexture.Location = new System.Drawing.Point(185, 1);
            this.btnRemapTexture.Name = "btnRemapTexture";
            this.btnRemapTexture.Size = new System.Drawing.Size(43, 23);
            this.btnRemapTexture.TabIndex = 64;
            this.btnRemapTexture.Tag = "1";
            this.btnRemapTexture.Text = "...";
            this.btnRemapTexture.UseVisualStyleBackColor = true;
            this.btnRemapTexture.Click += new System.EventHandler(this.RemapSingleImage_Click);
            // 
            // cbTexEClip
            // 
            this.cbTexEClip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTexEClip.FormattingEnabled = true;
            this.cbTexEClip.Location = new System.Drawing.Point(79, 81);
            this.cbTexEClip.Name = "cbTexEClip";
            this.cbTexEClip.Size = new System.Drawing.Size(149, 21);
            this.cbTexEClip.TabIndex = 63;
            this.cbTexEClip.Tag = "EClipNum";
            this.cbTexEClip.TextChanged += new System.EventHandler(this.TMAPInfoEClip_SelectedIndexChanged);
            // 
            // TextureDestroyedTextBox
            // 
            this.TextureDestroyedTextBox.Location = new System.Drawing.Point(79, 133);
            this.TextureDestroyedTextBox.Name = "TextureDestroyedTextBox";
            this.TextureDestroyedTextBox.Size = new System.Drawing.Size(100, 20);
            this.TextureDestroyedTextBox.TabIndex = 62;
            this.TextureDestroyedTextBox.Tag = "DestroyedID";
            this.TextureDestroyedTextBox.TextChanged += new System.EventHandler(this.TextureIntegerProperty_TextChanged);
            // 
            // label107
            // 
            this.label107.AutoSize = true;
            this.label107.Location = new System.Drawing.Point(1, 136);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(72, 13);
            this.label107.TabIndex = 61;
            this.label107.Text = "Destroyed ID:";
            // 
            // txtTexSlideV
            // 
            this.txtTexSlideV.Location = new System.Drawing.Point(234, 107);
            this.txtTexSlideV.Name = "txtTexSlideV";
            this.txtTexSlideV.Size = new System.Drawing.Size(100, 20);
            this.txtTexSlideV.TabIndex = 60;
            this.txtTexSlideV.Tag = "5";
            this.txtTexSlideV.TextChanged += new System.EventHandler(this.TextureProperty_TextChanged);
            // 
            // txtTexSlideU
            // 
            this.txtTexSlideU.Location = new System.Drawing.Point(79, 107);
            this.txtTexSlideU.Name = "txtTexSlideU";
            this.txtTexSlideU.Size = new System.Drawing.Size(100, 20);
            this.txtTexSlideU.TabIndex = 57;
            this.txtTexSlideU.Tag = "4";
            this.txtTexSlideU.TextChanged += new System.EventHandler(this.TextureProperty_TextChanged);
            // 
            // txtTexDamage
            // 
            this.txtTexDamage.Location = new System.Drawing.Point(79, 55);
            this.txtTexDamage.Name = "txtTexDamage";
            this.txtTexDamage.Size = new System.Drawing.Size(100, 20);
            this.txtTexDamage.TabIndex = 54;
            this.txtTexDamage.Tag = "Damage";
            this.txtTexDamage.TextChanged += new System.EventHandler(this.TextureProperty_TextChanged);
            // 
            // txtTexLight
            // 
            this.txtTexLight.Location = new System.Drawing.Point(79, 29);
            this.txtTexLight.Name = "txtTexLight";
            this.txtTexLight.Size = new System.Drawing.Size(100, 20);
            this.txtTexLight.TabIndex = 52;
            this.txtTexLight.Tag = "Lighting";
            this.txtTexLight.TextChanged += new System.EventHandler(this.TextureProperty_TextChanged);
            // 
            // txtTexID
            // 
            this.txtTexID.Location = new System.Drawing.Point(79, 3);
            this.txtTexID.Name = "txtTexID";
            this.txtTexID.Size = new System.Drawing.Size(100, 20);
            this.txtTexID.TabIndex = 50;
            this.txtTexID.Tag = "0";
            this.txtTexID.TextChanged += new System.EventHandler(this.TextureProperty_TextChanged);
            // 
            // label130
            // 
            this.label130.AutoSize = true;
            this.label130.Location = new System.Drawing.Point(185, 110);
            this.label130.Name = "label130";
            this.label130.Size = new System.Drawing.Size(43, 13);
            this.label130.TabIndex = 59;
            this.label130.Text = "Slide V:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbTexHoardGoal);
            this.groupBox1.Controls.Add(this.cbTexRedGoal);
            this.groupBox1.Controls.Add(this.cbTexBlueGoal);
            this.groupBox1.Controls.Add(this.cbTexForcefield);
            this.groupBox1.Controls.Add(this.cbTexWater);
            this.groupBox1.Controls.Add(this.cbTexLava);
            this.groupBox1.Location = new System.Drawing.Point(2, 164);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 162);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flags";
            // 
            // cbTexHoardGoal
            // 
            this.cbTexHoardGoal.AutoSize = true;
            this.cbTexHoardGoal.Location = new System.Drawing.Point(6, 134);
            this.cbTexHoardGoal.Name = "cbTexHoardGoal";
            this.cbTexHoardGoal.Size = new System.Drawing.Size(80, 17);
            this.cbTexHoardGoal.TabIndex = 23;
            this.cbTexHoardGoal.Tag = "HoardGoal";
            this.cbTexHoardGoal.Text = "Hoard Goal";
            this.cbTexHoardGoal.UseVisualStyleBackColor = true;
            this.cbTexHoardGoal.CheckedChanged += new System.EventHandler(this.TextureFlagCheckbox_CheckedChanged);
            // 
            // cbTexRedGoal
            // 
            this.cbTexRedGoal.AutoSize = true;
            this.cbTexRedGoal.Location = new System.Drawing.Point(6, 111);
            this.cbTexRedGoal.Name = "cbTexRedGoal";
            this.cbTexRedGoal.Size = new System.Drawing.Size(71, 17);
            this.cbTexRedGoal.TabIndex = 22;
            this.cbTexRedGoal.Tag = "RedGoal";
            this.cbTexRedGoal.Text = "Red Goal";
            this.cbTexRedGoal.UseVisualStyleBackColor = true;
            this.cbTexRedGoal.CheckedChanged += new System.EventHandler(this.TextureFlagCheckbox_CheckedChanged);
            // 
            // cbTexBlueGoal
            // 
            this.cbTexBlueGoal.AutoSize = true;
            this.cbTexBlueGoal.Location = new System.Drawing.Point(6, 88);
            this.cbTexBlueGoal.Name = "cbTexBlueGoal";
            this.cbTexBlueGoal.Size = new System.Drawing.Size(72, 17);
            this.cbTexBlueGoal.TabIndex = 21;
            this.cbTexBlueGoal.Tag = "BlueGoal";
            this.cbTexBlueGoal.Text = "Blue Goal";
            this.cbTexBlueGoal.UseVisualStyleBackColor = true;
            this.cbTexBlueGoal.CheckedChanged += new System.EventHandler(this.TextureFlagCheckbox_CheckedChanged);
            // 
            // cbTexForcefield
            // 
            this.cbTexForcefield.AutoSize = true;
            this.cbTexForcefield.Location = new System.Drawing.Point(6, 65);
            this.cbTexForcefield.Name = "cbTexForcefield";
            this.cbTexForcefield.Size = new System.Drawing.Size(78, 17);
            this.cbTexForcefield.TabIndex = 20;
            this.cbTexForcefield.Tag = "ForceField";
            this.cbTexForcefield.Text = "Force Field";
            this.cbTexForcefield.UseVisualStyleBackColor = true;
            this.cbTexForcefield.CheckedChanged += new System.EventHandler(this.TextureFlagCheckbox_CheckedChanged);
            // 
            // cbTexWater
            // 
            this.cbTexWater.AutoSize = true;
            this.cbTexWater.Location = new System.Drawing.Point(6, 42);
            this.cbTexWater.Name = "cbTexWater";
            this.cbTexWater.Size = new System.Drawing.Size(55, 17);
            this.cbTexWater.TabIndex = 19;
            this.cbTexWater.Tag = "Water";
            this.cbTexWater.Text = "Water";
            this.cbTexWater.UseVisualStyleBackColor = true;
            this.cbTexWater.CheckedChanged += new System.EventHandler(this.TextureFlagCheckbox_CheckedChanged);
            // 
            // cbTexLava
            // 
            this.cbTexLava.AutoSize = true;
            this.cbTexLava.Location = new System.Drawing.Point(6, 19);
            this.cbTexLava.Name = "cbTexLava";
            this.cbTexLava.Size = new System.Drawing.Size(108, 17);
            this.cbTexLava.TabIndex = 18;
            this.cbTexLava.Tag = "Volatile";
            this.cbTexLava.Text = "Volatile (like lava)";
            this.cbTexLava.UseVisualStyleBackColor = true;
            this.cbTexLava.CheckedChanged += new System.EventHandler(this.TextureFlagCheckbox_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 110);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 56;
            this.label12.Text = "Slide U:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 55;
            this.label11.Text = "Effect:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Damage:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 51;
            this.label9.Text = "Lighting:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Texture ID:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(348, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "Image Preview:";
            // 
            // pbTexPrev
            // 
            this.pbTexPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTexPrev.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbTexPrev.Location = new System.Drawing.Point(351, 22);
            this.pbTexPrev.Name = "pbTexPrev";
            this.pbTexPrev.Size = new System.Drawing.Size(320, 200);
            this.pbTexPrev.TabIndex = 47;
            this.pbTexPrev.TabStop = false;
            // 
            // TMAPInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRemapTexture);
            this.Controls.Add(this.cbTexEClip);
            this.Controls.Add(this.TextureDestroyedTextBox);
            this.Controls.Add(this.label107);
            this.Controls.Add(this.txtTexSlideV);
            this.Controls.Add(this.txtTexSlideU);
            this.Controls.Add(this.txtTexDamage);
            this.Controls.Add(this.txtTexLight);
            this.Controls.Add(this.txtTexID);
            this.Controls.Add(this.label130);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbTexPrev);
            this.Name = "TMAPInfoPanel";
            this.Size = new System.Drawing.Size(679, 342);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTexPrev)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRemapTexture;
        private System.Windows.Forms.ComboBox cbTexEClip;
        private System.Windows.Forms.TextBox TextureDestroyedTextBox;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.TextBox txtTexSlideV;
        private System.Windows.Forms.TextBox txtTexSlideU;
        private System.Windows.Forms.TextBox txtTexDamage;
        private System.Windows.Forms.TextBox txtTexLight;
        private System.Windows.Forms.TextBox txtTexID;
        private System.Windows.Forms.Label label130;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbTexHoardGoal;
        private System.Windows.Forms.CheckBox cbTexRedGoal;
        private System.Windows.Forms.CheckBox cbTexBlueGoal;
        private System.Windows.Forms.CheckBox cbTexForcefield;
        private System.Windows.Forms.CheckBox cbTexWater;
        private System.Windows.Forms.CheckBox cbTexLava;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbTexPrev;
    }
}
