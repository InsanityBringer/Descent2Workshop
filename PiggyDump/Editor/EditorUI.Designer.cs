namespace Descent2Workshop.Editor
{
    partial class EditorUI
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
            this.InfoStatusBar = new System.Windows.Forms.StatusBar();
            this.StatusTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // InfoStatusBar
            // 
            this.InfoStatusBar.Location = new System.Drawing.Point(0, 659);
            this.InfoStatusBar.Name = "InfoStatusBar";
            this.InfoStatusBar.Size = new System.Drawing.Size(1264, 22);
            this.InfoStatusBar.TabIndex = 0;
            this.InfoStatusBar.Text = "statusBar1";
            // 
            // StatusTimer
            // 
            this.StatusTimer.Interval = 5000;
            this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
            // 
            // EditorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.InfoStatusBar);
            this.Name = "EditorUI";
            this.Text = "EditorUI";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gl3DView_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusBar InfoStatusBar;
        private System.Windows.Forms.Timer StatusTimer;
    }
}