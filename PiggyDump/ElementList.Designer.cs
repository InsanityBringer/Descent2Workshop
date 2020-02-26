namespace Descent2Workshop
{
    partial class ElementList
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
            this.ElementListBox = new System.Windows.Forms.ListBox();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.SelectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ElementListBox
            // 
            this.ElementListBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ElementListBox.FormattingEnabled = true;
            this.ElementListBox.Location = new System.Drawing.Point(0, 0);
            this.ElementListBox.Name = "ElementListBox";
            this.ElementListBox.Size = new System.Drawing.Size(505, 524);
            this.ElementListBox.TabIndex = 1;
            this.ElementListBox.DoubleClick += new System.EventHandler(this.ElementListBox_DoubleClick);
            // 
            // CancelDialogButton
            // 
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(418, 530);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(75, 23);
            this.CancelDialogButton.TabIndex = 2;
            this.CancelDialogButton.Text = "Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // SelectButton
            // 
            this.SelectButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SelectButton.Location = new System.Drawing.Point(337, 530);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(75, 23);
            this.SelectButton.TabIndex = 3;
            this.SelectButton.Text = "Select";
            this.SelectButton.UseVisualStyleBackColor = true;
            // 
            // ElementList
            // 
            this.AcceptButton = this.SelectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 561);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.CancelDialogButton);
            this.Controls.Add(this.ElementListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ElementList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Element";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ElementListBox;
        private System.Windows.Forms.Button CancelDialogButton;
        private System.Windows.Forms.Button SelectButton;
    }
}