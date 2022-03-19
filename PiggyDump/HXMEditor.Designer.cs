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
            this.EditMenu = new System.Windows.Forms.MenuItem();
            this.UndoMenuItem = new System.Windows.Forms.MenuItem();
            this.RedoMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.AllocateReplacementsMenuItem = new System.Windows.Forms.MenuItem();
            this.EditorTabs = new System.Windows.Forms.TabControl();
            this.RobotTabPage = new System.Windows.Forms.TabPage();
            this.ModelTabPage = new System.Windows.Forms.TabPage();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.label50 = new System.Windows.Forms.Label();
            this.ElementSpinner = new System.Windows.Forms.NumericUpDown();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.label48 = new System.Windows.Forms.Label();
            this.InsertButton = new System.Windows.Forms.Button();
            this.label49 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ReplacedElementComboBox = new System.Windows.Forms.ComboBox();
            this.EditorTabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ElementSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.EditMenu,
            this.menuItem6});
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
            this.menuItem2.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuItem2.Text = "Save";
            this.menuItem2.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
            this.menuItem3.Text = "Save As...";
            this.menuItem3.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
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
            // EditMenu
            // 
            this.EditMenu.Index = 1;
            this.EditMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.UndoMenuItem,
            this.RedoMenuItem});
            this.EditMenu.Text = "Edit";
            // 
            // UndoMenuItem
            // 
            this.UndoMenuItem.Index = 0;
            this.UndoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.UndoMenuItem.Text = "Undo";
            this.UndoMenuItem.Click += new System.EventHandler(this.UndoMenuItem_Click);
            // 
            // RedoMenuItem
            // 
            this.RedoMenuItem.Index = 1;
            this.RedoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.RedoMenuItem.Text = "Redo";
            this.RedoMenuItem.Click += new System.EventHandler(this.RedoMenuItem_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.AllocateReplacementsMenuItem});
            this.menuItem6.Text = "Actions";
            // 
            // AllocateReplacementsMenuItem
            // 
            this.AllocateReplacementsMenuItem.Index = 0;
            this.AllocateReplacementsMenuItem.Text = "Allocate Replaced Bitmaps and Joints";
            this.AllocateReplacementsMenuItem.Click += new System.EventHandler(this.AllocateReplacementsMenuItem_Click);
            // 
            // EditorTabs
            // 
            this.EditorTabs.Controls.Add(this.RobotTabPage);
            this.EditorTabs.Controls.Add(this.ModelTabPage);
            this.EditorTabs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.EditorTabs.Location = new System.Drawing.Point(0, 62);
            this.EditorTabs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EditorTabs.Name = "EditorTabs";
            this.EditorTabs.SelectedIndex = 0;
            this.EditorTabs.Size = new System.Drawing.Size(1284, 829);
            this.EditorTabs.TabIndex = 2;
            this.EditorTabs.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // RobotTabPage
            // 
            this.RobotTabPage.Location = new System.Drawing.Point(4, 29);
            this.RobotTabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RobotTabPage.Name = "RobotTabPage";
            this.RobotTabPage.Size = new System.Drawing.Size(1276, 796);
            this.RobotTabPage.TabIndex = 4;
            this.RobotTabPage.Text = "Robots";
            this.RobotTabPage.UseVisualStyleBackColor = true;
            // 
            // ModelTabPage
            // 
            this.ModelTabPage.Location = new System.Drawing.Point(4, 29);
            this.ModelTabPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ModelTabPage.Name = "ModelTabPage";
            this.ModelTabPage.Size = new System.Drawing.Size(1276, 796);
            this.ModelTabPage.TabIndex = 6;
            this.ModelTabPage.Text = "Models";
            this.ModelTabPage.UseVisualStyleBackColor = true;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 891);
            this.statusBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(1284, 34);
            this.statusBar1.TabIndex = 3;
            this.statusBar1.Text = "statusBar1";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(634, 23);
            this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(147, 20);
            this.label50.TabIndex = 220;
            this.label50.Text = "Replacing Element:";
            // 
            // ElementSpinner
            // 
            this.ElementSpinner.Location = new System.Drawing.Point(153, 18);
            this.ElementSpinner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ElementSpinner.Name = "ElementSpinner";
            this.ElementSpinner.Size = new System.Drawing.Size(94, 26);
            this.ElementSpinner.TabIndex = 215;
            this.ElementSpinner.ValueChanged += new System.EventHandler(this.nudElementNum_ValueChanged);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(513, 14);
            this.DeleteButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(112, 35);
            this.DeleteButton.TabIndex = 219;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(12, 22);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(132, 20);
            this.label48.TabIndex = 216;
            this.label48.Text = "Element Number:";
            // 
            // InsertButton
            // 
            this.InsertButton.Location = new System.Drawing.Point(392, 14);
            this.InsertButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InsertButton.Name = "InsertButton";
            this.InsertButton.Size = new System.Drawing.Size(112, 35);
            this.InsertButton.TabIndex = 218;
            this.InsertButton.Text = "Insert";
            this.InsertButton.UseVisualStyleBackColor = true;
            this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(256, 23);
            this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(127, 20);
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
            this.ReplacedElementComboBox.Location = new System.Drawing.Point(792, 17);
            this.ReplacedElementComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReplacedElementComboBox.Name = "ReplacedElementComboBox";
            this.ReplacedElementComboBox.Size = new System.Drawing.Size(306, 28);
            this.ReplacedElementComboBox.TabIndex = 221;
            this.ReplacedElementComboBox.SelectedIndexChanged += new System.EventHandler(this.ReplacedElementComboBox_SelectedIndexChanged);
            // 
            // HXMEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 925);
            this.Controls.Add(this.ReplacedElementComboBox);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.ElementSpinner);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.InsertButton);
            this.Controls.Add(this.label49);
            this.Controls.Add(this.EditorTabs);
            this.Controls.Add(this.statusBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Menu = this.mainMenu1;
            this.Name = "HXMEditor";
            this.Text = "HXMEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HXMEditor_FormClosing);
            this.Load += new System.EventHandler(this.HXMEditor_Load);
            this.EditorTabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ElementSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.TabControl EditorTabs;
        private System.Windows.Forms.TabPage RobotTabPage;
        private System.Windows.Forms.TabPage ModelTabPage;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.NumericUpDown ElementSpinner;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Button InsertButton;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox ReplacedElementComboBox;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem EditMenu;
        private System.Windows.Forms.MenuItem UndoMenuItem;
        private System.Windows.Forms.MenuItem RedoMenuItem;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem AllocateReplacementsMenuItem;
    }
}