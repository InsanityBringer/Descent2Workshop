namespace PiggyDump
{
    partial class ConfigDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkNoPMView = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.txtSndFilename = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.txtPigFilename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.txtHogFilename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbPofVer = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.txtTraceDir = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkTraces = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(320, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(239, 183);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(407, 177);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkNoPMView);
            this.tabPage1.Controls.Add(this.button5);
            this.tabPage1.Controls.Add(this.txtSndFilename);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.txtPigFilename);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.txtHogFilename);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(399, 151);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkNoPMView
            // 
            this.chkNoPMView.AutoSize = true;
            this.chkNoPMView.Location = new System.Drawing.Point(11, 123);
            this.chkNoPMView.Name = "chkNoPMView";
            this.chkNoPMView.Size = new System.Drawing.Size(204, 17);
            this.chkNoPMView.TabIndex = 9;
            this.chkNoPMView.Text = "Replicate original object bitmaps table";
            this.chkNoPMView.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(356, 95);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(35, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = ",,,";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // txtSndFilename
            // 
            this.txtSndFilename.Location = new System.Drawing.Point(11, 97);
            this.txtSndFilename.Name = "txtSndFilename";
            this.txtSndFilename.Size = new System.Drawing.Size(339, 20);
            this.txtSndFilename.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Default SND File:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(356, 56);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(35, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = ",,,";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // txtPigFilename
            // 
            this.txtPigFilename.Location = new System.Drawing.Point(11, 58);
            this.txtPigFilename.Name = "txtPigFilename";
            this.txtPigFilename.Size = new System.Drawing.Size(339, 20);
            this.txtPigFilename.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Default PIG File:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(356, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = ",,,";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // txtHogFilename
            // 
            this.txtHogFilename.Location = new System.Drawing.Point(11, 19);
            this.txtHogFilename.Name = "txtHogFilename";
            this.txtHogFilename.Size = new System.Drawing.Size(339, 20);
            this.txtHogFilename.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default HOG File:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbPofVer);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.button6);
            this.tabPage2.Controls.Add(this.txtTraceDir);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.chkTraces);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(399, 151);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbPofVer
            // 
            this.cbPofVer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPofVer.FormattingEnabled = true;
            this.cbPofVer.Items.AddRange(new object[] {
            "Version 7",
            "Version 8",
            "Version 9"});
            this.cbPofVer.Location = new System.Drawing.Point(8, 81);
            this.cbPofVer.Name = "cbPofVer";
            this.cbPofVer.Size = new System.Drawing.Size(121, 21);
            this.cbPofVer.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Export POF models as version:";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(354, 40);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(37, 23);
            this.button6.TabIndex = 3;
            this.button6.Text = "...";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // txtTraceDir
            // 
            this.txtTraceDir.Location = new System.Drawing.Point(8, 42);
            this.txtTraceDir.Name = "txtTraceDir";
            this.txtTraceDir.Size = new System.Drawing.Size(340, 20);
            this.txtTraceDir.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Save Polymodel traces in:";
            // 
            // chkTraces
            // 
            this.chkTraces.AutoSize = true;
            this.chkTraces.Location = new System.Drawing.Point(8, 6);
            this.chkTraces.Name = "chkTraces";
            this.chkTraces.Size = new System.Drawing.Size(142, 17);
            this.chkTraces.TabIndex = 0;
            this.chkTraces.Text = "Enable Polymodel traces";
            this.chkTraces.UseVisualStyleBackColor = true;
            // 
            // ConfigDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(407, 218);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.ConfigDialog_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox chkNoPMView;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox txtSndFilename;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtPigFilename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtHogFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cbPofVer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtTraceDir;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkTraces;
    }
}