namespace Descent2Workshop.EditorPanels
{
    partial class ReactorPanel
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
            this.cbReactorModel = new System.Windows.Forms.ComboBox();
            this.label78 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbReactorModel
            // 
            this.cbReactorModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReactorModel.FormattingEnabled = true;
            this.cbReactorModel.Location = new System.Drawing.Point(49, 3);
            this.cbReactorModel.Name = "cbReactorModel";
            this.cbReactorModel.Size = new System.Drawing.Size(185, 21);
            this.cbReactorModel.TabIndex = 3;
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Location = new System.Drawing.Point(4, 6);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(39, 13);
            this.label78.TabIndex = 2;
            this.label78.Text = "Model:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "The Pinnacle of UI Design™";
            // 
            // ReactorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbReactorModel);
            this.Controls.Add(this.label78);
            this.Name = "ReactorPanel";
            this.Size = new System.Drawing.Size(237, 49);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbReactorModel;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Label label1;
    }
}
