namespace PiggyDump
{
    partial class SXXEditor
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnPlay = new System.Windows.Forms.Button();
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
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.ExtractMenu = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 41);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(451, 512);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
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
            this.columnHeader3.Text = "Offset";
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 0;
            this.columnHeader4.Text = "#";
            this.columnHeader4.Width = 40;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 12);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 12;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
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
            this.menuItem9,
            this.ExtractMenu});
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
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 3;
            this.menuItem9.Text = "-";
            // 
            // ExtractMenu
            // 
            this.ExtractMenu.Index = 4;
            this.ExtractMenu.Text = "Extract";
            this.ExtractMenu.Click += new System.EventHandler(this.ExtractMenu_Click);
            // 
            // SXXEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 553);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.listView1);
            this.Menu = this.mainMenu1;
            this.Name = "SXXEditor";
            this.Text = "SXXEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SXXEditor_FormClosing);
            this.Load += new System.EventHandler(this.SXXEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem SaveMenu;
        private System.Windows.Forms.MenuItem SaveAsMenu;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem CloseMenu;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem InsertMenu;
        private System.Windows.Forms.MenuItem ImportMenu;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem ExtractMenu;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.MenuItem DeleteMenu;
    }
}