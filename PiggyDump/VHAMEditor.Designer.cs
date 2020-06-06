namespace Descent2Workshop
{
    partial class VHAMEditor
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.txtElemName = new System.Windows.Forms.TextBox();
            this.button15 = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.nudElementNum = new System.Windows.Forms.NumericUpDown();
            this.btnDeleteElem = new System.Windows.Forms.Button();
            this.label48 = new System.Windows.Forms.Label();
            this.btnInsertElem = new System.Windows.Forms.Button();
            this.label49 = new System.Windows.Forms.Label();
            this.TabPages = new System.Windows.Forms.TabControl();
            this.RobotTabPage = new System.Windows.Forms.TabPage();
            this.WeaponTabPage = new System.Windows.Forms.TabPage();
            this.ModelTabPage = new System.Windows.Forms.TabPage();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.nudElementNum)).BeginInit();
            this.TabPages.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem5});
            this.menuItem1.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Save";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Save As...";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "Exit";
            // 
            // txtElemName
            // 
            this.txtElemName.Location = new System.Drawing.Point(552, 7);
            this.txtElemName.Name = "txtElemName";
            this.txtElemName.Size = new System.Drawing.Size(164, 20);
            this.txtElemName.TabIndex = 222;
            this.txtElemName.Text = "<unnamed>";
            // 
            // button15
            // 
            this.button15.Enabled = false;
            this.button15.Location = new System.Drawing.Point(427, 4);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 221;
            this.button15.Text = "List";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(508, 10);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(38, 13);
            this.label50.TabIndex = 220;
            this.label50.Text = "Name:";
            // 
            // nudElementNum
            // 
            this.nudElementNum.Location = new System.Drawing.Point(106, 7);
            this.nudElementNum.Name = "nudElementNum";
            this.nudElementNum.Size = new System.Drawing.Size(63, 20);
            this.nudElementNum.TabIndex = 215;
            this.nudElementNum.ValueChanged += new System.EventHandler(this.nudElementNum_ValueChanged);
            // 
            // btnDeleteElem
            // 
            this.btnDeleteElem.Location = new System.Drawing.Point(346, 4);
            this.btnDeleteElem.Name = "btnDeleteElem";
            this.btnDeleteElem.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteElem.TabIndex = 219;
            this.btnDeleteElem.Text = "Delete";
            this.btnDeleteElem.UseVisualStyleBackColor = true;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(12, 9);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(88, 13);
            this.label48.TabIndex = 216;
            this.label48.Text = "Element Number:";
            // 
            // btnInsertElem
            // 
            this.btnInsertElem.Location = new System.Drawing.Point(265, 4);
            this.btnInsertElem.Name = "btnInsertElem";
            this.btnInsertElem.Size = new System.Drawing.Size(75, 23);
            this.btnInsertElem.TabIndex = 218;
            this.btnInsertElem.Text = "Insert";
            this.btnInsertElem.UseVisualStyleBackColor = true;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(175, 10);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(84, 13);
            this.label49.TabIndex = 217;
            this.label49.Text = "Element Control:";
            // 
            // TabPages
            // 
            this.TabPages.Controls.Add(this.RobotTabPage);
            this.TabPages.Controls.Add(this.WeaponTabPage);
            this.TabPages.Controls.Add(this.ModelTabPage);
            this.TabPages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TabPages.Location = new System.Drawing.Point(0, 40);
            this.TabPages.Name = "TabPages";
            this.TabPages.SelectedIndex = 0;
            this.TabPages.Size = new System.Drawing.Size(854, 539);
            this.TabPages.TabIndex = 223;
            this.TabPages.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // RobotTabPage
            // 
            this.RobotTabPage.Location = new System.Drawing.Point(4, 22);
            this.RobotTabPage.Name = "RobotTabPage";
            this.RobotTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RobotTabPage.Size = new System.Drawing.Size(846, 513);
            this.RobotTabPage.TabIndex = 0;
            this.RobotTabPage.Text = "Robots";
            this.RobotTabPage.UseVisualStyleBackColor = true;
            // 
            // WeaponTabPage
            // 
            this.WeaponTabPage.Location = new System.Drawing.Point(4, 22);
            this.WeaponTabPage.Name = "WeaponTabPage";
            this.WeaponTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.WeaponTabPage.Size = new System.Drawing.Size(846, 513);
            this.WeaponTabPage.TabIndex = 1;
            this.WeaponTabPage.Text = "Weapons";
            this.WeaponTabPage.UseVisualStyleBackColor = true;
            // 
            // ModelTabPage
            // 
            this.ModelTabPage.Location = new System.Drawing.Point(4, 22);
            this.ModelTabPage.Name = "ModelTabPage";
            this.ModelTabPage.Size = new System.Drawing.Size(846, 513);
            this.ModelTabPage.TabIndex = 2;
            this.ModelTabPage.Text = "Models";
            this.ModelTabPage.UseVisualStyleBackColor = true;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 579);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(854, 22);
            this.statusBar1.TabIndex = 224;
            this.statusBar1.Text = "statusBar1";
            // 
            // VHAMEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 601);
            this.Controls.Add(this.TabPages);
            this.Controls.Add(this.txtElemName);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.nudElementNum);
            this.Controls.Add(this.btnDeleteElem);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.btnInsertElem);
            this.Controls.Add(this.label49);
            this.Controls.Add(this.statusBar1);
            this.Menu = this.mainMenu1;
            this.Name = "VHAMEditor";
            this.Text = "VHAMEditor";
            this.Load += new System.EventHandler(this.VHAMEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudElementNum)).EndInit();
            this.TabPages.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.TextBox txtElemName;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.NumericUpDown nudElementNum;
        private System.Windows.Forms.Button btnDeleteElem;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Button btnInsertElem;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TabControl TabPages;
        private System.Windows.Forms.TabPage RobotTabPage;
        private System.Windows.Forms.TabPage WeaponTabPage;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.TabPage ModelTabPage;
    }
}