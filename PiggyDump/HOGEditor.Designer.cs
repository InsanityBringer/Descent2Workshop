namespace Descent2Workshop
{
    partial class HOGEditor
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
            this.CloseWindow = new System.Windows.Forms.MenuItem();
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
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
            this.CloseWindow});
            this.menuItem1.Text = "File";
            // 
            // SaveMenu
            // 
            this.SaveMenu.Index = 0;
            this.SaveMenu.Text = "Save...";
            this.SaveMenu.Click += new System.EventHandler(this.SaveMenu_Click);
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
            // CloseWindow
            // 
            this.CloseWindow.Index = 3;
            this.CloseWindow.Text = "Close Window";
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
            this.InsertMenu.Click += new System.EventHandler(this.InsertMenu_Click);
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
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(352, 500);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 120;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "All Files|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "All Files|*.*";
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
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(862, 500);
            this.splitContainer1.SplitterDistance = 352;
            this.splitContainer1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Really Cool Element Previewer Feature Goes Here";
            // 
            // HOGEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 500);
            this.Controls.Add(this.splitContainer1);
            this.Menu = this.mainMenu1;
            this.Name = "HOGEditor";
            this.Text = "HOGEditor";
            this.Load += new System.EventHandler(this.HOGEditor_Load);
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
        private System.Windows.Forms.MenuItem CloseWindow;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem InsertMenu;
        private System.Windows.Forms.MenuItem ImportMenu;
        private System.Windows.Forms.MenuItem DeleteMenu;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem ExportMenu;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
    }
}