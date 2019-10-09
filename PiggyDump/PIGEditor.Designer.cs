namespace Descent2Workshop
{
    partial class PIGEditor
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
            this.SaveMenu = new System.Windows.Forms.MenuItem();
            this.SaveAsMenu = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.CloseMenu = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.InsertMenu = new System.Windows.Forms.MenuItem();
            this.ImportMenu = new System.Windows.Forms.MenuItem();
            this.DeleteMenu = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.ExportMenu = new System.Windows.Forms.MenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.TransparentCheck = new System.Windows.Forms.CheckBox();
            this.SupertransparentCheck = new System.Windows.Forms.CheckBox();
            this.NoLightingCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ColorPreview = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem6});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SaveMenu,
            this.SaveAsMenu,
            this.menuItem4,
            this.CloseMenu});
            this.menuItem1.Text = "File";
            // 
            // SaveMenu
            // 
            this.SaveMenu.Index = 0;
            this.SaveMenu.Text = "Save";
            // 
            // SaveAsMenu
            // 
            this.SaveAsMenu.Index = 1;
            this.SaveAsMenu.Text = "Save As...";
            this.SaveAsMenu.Click += new System.EventHandler(this.SaveAsMenu_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // CloseMenu
            // 
            this.CloseMenu.Index = 3;
            this.CloseMenu.Text = "Close Window";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.InsertMenu,
            this.ImportMenu,
            this.DeleteMenu,
            this.menuItem10,
            this.ExportMenu});
            this.menuItem6.Text = "Edit";
            // 
            // InsertMenu
            // 
            this.InsertMenu.Index = 0;
            this.InsertMenu.Text = "Insert...";
            // 
            // ImportMenu
            // 
            this.ImportMenu.Index = 1;
            this.ImportMenu.Text = "Import Over...";
            // 
            // DeleteMenu
            // 
            this.DeleteMenu.Index = 2;
            this.DeleteMenu.Text = "Delete";
            this.DeleteMenu.Click += new System.EventHandler(this.DeleteMenu_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 3;
            this.menuItem10.Text = "-";
            // 
            // ExportMenu
            // 
            this.ExportMenu.Index = 4;
            this.ExportMenu.Text = "Export...";
            this.ExportMenu.Click += new System.EventHandler(this.ExportMenu_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(314, 515);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listView1_KeyPress);
            // 
            // columnHeader1
            // 
            this.columnHeader1.DisplayIndex = 1;
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 2;
            this.columnHeader2.Text = "Size";
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 3;
            this.columnHeader3.Text = "Animation";
            this.columnHeader3.Width = 90;
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 0;
            this.columnHeader4.Text = "#";
            this.columnHeader4.Width = 40;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(553, 478);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "PIG Files|*.pig";
            // 
            // TransparentCheck
            // 
            this.TransparentCheck.AutoSize = true;
            this.TransparentCheck.Location = new System.Drawing.Point(3, 12);
            this.TransparentCheck.Name = "TransparentCheck";
            this.TransparentCheck.Size = new System.Drawing.Size(83, 17);
            this.TransparentCheck.TabIndex = 6;
            this.TransparentCheck.Text = "Transparent";
            this.TransparentCheck.UseVisualStyleBackColor = true;
            // 
            // SupertransparentCheck
            // 
            this.SupertransparentCheck.AutoSize = true;
            this.SupertransparentCheck.Location = new System.Drawing.Point(92, 12);
            this.SupertransparentCheck.Name = "SupertransparentCheck";
            this.SupertransparentCheck.Size = new System.Drawing.Size(107, 17);
            this.SupertransparentCheck.TabIndex = 7;
            this.SupertransparentCheck.Text = "Supertransparent";
            this.SupertransparentCheck.UseVisualStyleBackColor = true;
            // 
            // NoLightingCheck
            // 
            this.NoLightingCheck.AutoSize = true;
            this.NoLightingCheck.Location = new System.Drawing.Point(205, 12);
            this.NoLightingCheck.Name = "NoLightingCheck";
            this.NoLightingCheck.Size = new System.Drawing.Size(80, 17);
            this.NoLightingCheck.TabIndex = 8;
            this.NoLightingCheck.Text = "No Lighting";
            this.NoLightingCheck.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(291, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Average Color:";
            // 
            // ColorPreview
            // 
            this.ColorPreview.Location = new System.Drawing.Point(374, 13);
            this.ColorPreview.Name = "ColorPreview";
            this.ColorPreview.Size = new System.Drawing.Size(25, 14);
            this.ColorPreview.TabIndex = 10;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(405, 8);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Calculate";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.button4);
            this.splitContainer1.Panel2.Controls.Add(this.TransparentCheck);
            this.splitContainer1.Panel2.Controls.Add(this.ColorPreview);
            this.splitContainer1.Panel2.Controls.Add(this.SupertransparentCheck);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.NoLightingCheck);
            this.splitContainer1.Size = new System.Drawing.Size(871, 515);
            this.splitContainer1.SplitterDistance = 314;
            this.splitContainer1.TabIndex = 12;
            // 
            // PIGEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 515);
            this.Controls.Add(this.splitContainer1);
            this.Menu = this.mainMenu1;
            this.Name = "PIGEditor";
            this.Text = "PIGEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PIGEditor_FormClosing);
            this.Load += new System.EventHandler(this.PIGEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem SaveMenu;
        private System.Windows.Forms.MenuItem SaveAsMenu;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem CloseMenu;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox TransparentCheck;
        private System.Windows.Forms.CheckBox SupertransparentCheck;
        private System.Windows.Forms.CheckBox NoLightingCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ColorPreview;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem InsertMenu;
        private System.Windows.Forms.MenuItem ImportMenu;
        private System.Windows.Forms.MenuItem DeleteMenu;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem ExportMenu;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}