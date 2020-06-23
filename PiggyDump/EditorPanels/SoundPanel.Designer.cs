namespace Descent2Workshop.EditorPanels
{
    partial class SoundPanel
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
            this.SoundIDComboBox = new System.Windows.Forms.ComboBox();
            this.LowMemorySoundComboBox = new System.Windows.Forms.ComboBox();
            this.label144 = new System.Windows.Forms.Label();
            this.label143 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SoundIDComboBox
            // 
            this.SoundIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SoundIDComboBox.FormattingEnabled = true;
            this.SoundIDComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SoundIDComboBox.Location = new System.Drawing.Point(128, 3);
            this.SoundIDComboBox.Name = "SoundIDComboBox";
            this.SoundIDComboBox.Size = new System.Drawing.Size(196, 21);
            this.SoundIDComboBox.TabIndex = 8;
            this.SoundIDComboBox.Tag = "Sounds";
            this.SoundIDComboBox.SelectedIndexChanged += new System.EventHandler(this.SoundIDComboBox_SelectedIndexChanged);
            // 
            // LowMemorySoundComboBox
            // 
            this.LowMemorySoundComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LowMemorySoundComboBox.FormattingEnabled = true;
            this.LowMemorySoundComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LowMemorySoundComboBox.Location = new System.Drawing.Point(128, 30);
            this.LowMemorySoundComboBox.Name = "LowMemorySoundComboBox";
            this.LowMemorySoundComboBox.Size = new System.Drawing.Size(196, 21);
            this.LowMemorySoundComboBox.TabIndex = 7;
            this.LowMemorySoundComboBox.Tag = "AltSounds";
            this.LowMemorySoundComboBox.SelectedIndexChanged += new System.EventHandler(this.SoundIDComboBox_SelectedIndexChanged);
            // 
            // label144
            // 
            this.label144.AutoSize = true;
            this.label144.Location = new System.Drawing.Point(4, 33);
            this.label144.Name = "label144";
            this.label144.Size = new System.Drawing.Size(118, 13);
            this.label144.TabIndex = 6;
            this.label144.Text = "Low Memory Sound ID:";
            // 
            // label143
            // 
            this.label143.AutoSize = true;
            this.label143.Location = new System.Drawing.Point(4, 6);
            this.label143.Name = "label143";
            this.label143.Size = new System.Drawing.Size(55, 13);
            this.label143.TabIndex = 5;
            this.label143.Text = "Sound ID:";
            // 
            // SoundPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SoundIDComboBox);
            this.Controls.Add(this.LowMemorySoundComboBox);
            this.Controls.Add(this.label144);
            this.Controls.Add(this.label143);
            this.Name = "SoundPanel";
            this.Size = new System.Drawing.Size(327, 57);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SoundIDComboBox;
        private System.Windows.Forms.ComboBox LowMemorySoundComboBox;
        private System.Windows.Forms.Label label144;
        private System.Windows.Forms.Label label143;
    }
}
