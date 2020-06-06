namespace Descent2Workshop
{
    partial class HXMEditor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.RobotTabPage = new System.Windows.Forms.TabPage();
            this.ModelTabPage = new System.Windows.Forms.TabPage();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.label50 = new System.Windows.Forms.Label();
            this.nudElementNum = new System.Windows.Forms.NumericUpDown();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.label48 = new System.Windows.Forms.Label();
            this.InsertButton = new System.Windows.Forms.Button();
            this.label49 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ReplacedElementComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudElementNum)).BeginInit();
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
            this.menuItem2.Click += new System.EventHandler(this.MenuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Save As...";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click_1);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "Close Window";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.RobotTabPage);
            this.tabControl1.Controls.Add(this.ModelTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 40);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(856, 539);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // RobotTabPage
            // 
            this.RobotTabPage.Location = new System.Drawing.Point(4, 22);
            this.RobotTabPage.Name = "RobotTabPage";
            this.RobotTabPage.Size = new System.Drawing.Size(848, 513);
            this.RobotTabPage.TabIndex = 4;
            this.RobotTabPage.Text = "Robots";
            this.RobotTabPage.UseVisualStyleBackColor = true;
            // 
            // ModelTabPage
            // 
            this.ModelTabPage.Location = new System.Drawing.Point(4, 22);
            this.ModelTabPage.Name = "ModelTabPage";
            this.ModelTabPage.Size = new System.Drawing.Size(848, 513);
            this.ModelTabPage.TabIndex = 6;
            this.ModelTabPage.Text = "Models";
            this.ModelTabPage.UseVisualStyleBackColor = true;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 579);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(856, 22);
            this.statusBar1.TabIndex = 3;
            this.statusBar1.Text = "statusBar1";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(423, 15);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(99, 13);
            this.label50.TabIndex = 220;
            this.label50.Text = "Replacing Element:";
            // 
            // nudElementNum
            // 
            this.nudElementNum.Location = new System.Drawing.Point(102, 12);
            this.nudElementNum.Name = "nudElementNum";
            this.nudElementNum.Size = new System.Drawing.Size(63, 20);
            this.nudElementNum.TabIndex = 215;
            this.nudElementNum.ValueChanged += new System.EventHandler(this.nudElementNum_ValueChanged);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(342, 9);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 219;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(8, 14);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(88, 13);
            this.label48.TabIndex = 216;
            this.label48.Text = "Element Number:";
            // 
            // InsertButton
            // 
            this.InsertButton.Location = new System.Drawing.Point(261, 9);
            this.InsertButton.Name = "InsertButton";
            this.InsertButton.Size = new System.Drawing.Size(75, 23);
            this.InsertButton.TabIndex = 218;
            this.InsertButton.Text = "Insert";
            this.InsertButton.UseVisualStyleBackColor = true;
            this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(171, 15);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(84, 13);
            this.label49.TabIndex = 217;
            this.label49.Text = "Element Control:";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "HXM Files|*.HXM";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ReplacedElementComboBox
            // 
            this.ReplacedElementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReplacedElementComboBox.FormattingEnabled = true;
            this.ReplacedElementComboBox.Location = new System.Drawing.Point(528, 11);
            this.ReplacedElementComboBox.Name = "ReplacedElementComboBox";
            this.ReplacedElementComboBox.Size = new System.Drawing.Size(205, 21);
            this.ReplacedElementComboBox.TabIndex = 221;
            this.ReplacedElementComboBox.SelectedIndexChanged += new System.EventHandler(this.ReplacedElementComboBox_SelectedIndexChanged);
            // 
            // HXMEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 601);
            this.Controls.Add(this.ReplacedElementComboBox);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.nudElementNum);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.InsertButton);
            this.Controls.Add(this.label49);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Menu = this.mainMenu1;
            this.Name = "HXMEditor";
            this.Text = "HXMEditor";
            this.Load += new System.EventHandler(this.HXMEditor_Load);
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudElementNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage RobotTabPage;
        private System.Windows.Forms.TabPage ModelTabPage;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.NumericUpDown nudElementNum;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Button InsertButton;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox ReplacedElementComboBox;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
    }
}