namespace Descent2Workshop
{
    partial class PolymodelPreviewer
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
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.chkRadius = new System.Windows.Forms.CheckBox();
            this.chkNorm = new System.Windows.Forms.CheckBox();
            this.chkShowBBs = new System.Windows.Forms.CheckBox();
            this.chkWireframe = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.chkAnimation = new System.Windows.Forms.CheckBox();
            this.chkSoftwareOverdraw = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 4;
            this.trackBar1.Location = new System.Drawing.Point(63, 402);
            this.trackBar1.Maximum = 16;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(384, 45);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 8;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.LargeChange = 4;
            this.trackBar2.Location = new System.Drawing.Point(453, 12);
            this.trackBar2.Maximum = 16;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar2.Size = new System.Drawing.Size(45, 384);
            this.trackBar2.TabIndex = 1;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar2.Value = 8;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // trackBar3
            // 
            this.trackBar3.Location = new System.Drawing.Point(12, 12);
            this.trackBar3.Maximum = 64;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar3.Size = new System.Drawing.Size(45, 384);
            this.trackBar3.TabIndex = 2;
            this.trackBar3.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar3.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // chkRadius
            // 
            this.chkRadius.AutoSize = true;
            this.chkRadius.Location = new System.Drawing.Point(63, 430);
            this.chkRadius.Name = "chkRadius";
            this.chkRadius.Size = new System.Drawing.Size(89, 17);
            this.chkRadius.TabIndex = 5;
            this.chkRadius.Text = "Show Radius";
            this.chkRadius.UseVisualStyleBackColor = true;
            this.chkRadius.CheckedChanged += new System.EventHandler(this.chkRadius_CheckedChanged);
            // 
            // chkNorm
            // 
            this.chkNorm.AutoSize = true;
            this.chkNorm.Location = new System.Drawing.Point(63, 453);
            this.chkNorm.Name = "chkNorm";
            this.chkNorm.Size = new System.Drawing.Size(94, 17);
            this.chkNorm.TabIndex = 6;
            this.chkNorm.Text = "Show Normals";
            this.chkNorm.UseVisualStyleBackColor = true;
            this.chkNorm.CheckedChanged += new System.EventHandler(this.chkRadius_CheckedChanged);
            // 
            // chkShowBBs
            // 
            this.chkShowBBs.AutoSize = true;
            this.chkShowBBs.Location = new System.Drawing.Point(158, 430);
            this.chkShowBBs.Name = "chkShowBBs";
            this.chkShowBBs.Size = new System.Drawing.Size(133, 17);
            this.chkShowBBs.TabIndex = 7;
            this.chkShowBBs.Text = "Show Bounding Boxes";
            this.chkShowBBs.UseVisualStyleBackColor = true;
            this.chkShowBBs.CheckedChanged += new System.EventHandler(this.chkRadius_CheckedChanged);
            // 
            // chkWireframe
            // 
            this.chkWireframe.AutoSize = true;
            this.chkWireframe.Location = new System.Drawing.Point(158, 453);
            this.chkWireframe.Name = "chkWireframe";
            this.chkWireframe.Size = new System.Drawing.Size(74, 17);
            this.chkWireframe.TabIndex = 8;
            this.chkWireframe.Text = "Wireframe";
            this.chkWireframe.UseVisualStyleBackColor = true;
            this.chkWireframe.CheckedChanged += new System.EventHandler(this.chkRadius_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(297, 431);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Frame:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(342, 429);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown1.TabIndex = 10;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // chkAnimation
            // 
            this.chkAnimation.AutoSize = true;
            this.chkAnimation.Location = new System.Drawing.Point(300, 453);
            this.chkAnimation.Name = "chkAnimation";
            this.chkAnimation.Size = new System.Drawing.Size(130, 17);
            this.chkAnimation.TabIndex = 11;
            this.chkAnimation.Text = "Show Animation State";
            this.chkAnimation.UseVisualStyleBackColor = true;
            this.chkAnimation.CheckedChanged += new System.EventHandler(this.chkRadius_CheckedChanged);
            // 
            // chkSoftwareOverdraw
            // 
            this.chkSoftwareOverdraw.AutoSize = true;
            this.chkSoftwareOverdraw.Location = new System.Drawing.Point(63, 476);
            this.chkSoftwareOverdraw.Name = "chkSoftwareOverdraw";
            this.chkSoftwareOverdraw.Size = new System.Drawing.Size(158, 17);
            this.chkSoftwareOverdraw.TabIndex = 12;
            this.chkSoftwareOverdraw.Text = "Emulate Software Overdraw";
            this.chkSoftwareOverdraw.UseVisualStyleBackColor = true;
            this.chkSoftwareOverdraw.CheckedChanged += new System.EventHandler(this.chkRadius_CheckedChanged);
            // 
            // PolymodelPreviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 497);
            this.Controls.Add(this.chkSoftwareOverdraw);
            this.Controls.Add(this.chkAnimation);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkWireframe);
            this.Controls.Add(this.chkShowBBs);
            this.Controls.Add(this.chkNorm);
            this.Controls.Add(this.chkRadius);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.trackBar2);
            this.Name = "PolymodelPreviewer";
            this.Text = "PolymodelPreviewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PolymodelPreviewer_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.CheckBox chkRadius;
        private System.Windows.Forms.CheckBox chkNorm;
        private System.Windows.Forms.CheckBox chkShowBBs;
        private System.Windows.Forms.CheckBox chkWireframe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox chkAnimation;
        private System.Windows.Forms.CheckBox chkSoftwareOverdraw;
    }
}