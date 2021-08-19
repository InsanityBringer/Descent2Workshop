namespace Descent2Workshop
{
    partial class POGEditor
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
            this.SaveMenuItem = new System.Windows.Forms.MenuItem();
            this.SaveAsMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.InsertMenuItem = new System.Windows.Forms.MenuItem();
            this.ImportMenuItem = new System.Windows.Forms.MenuItem();
            this.DeleteMenuItem = new System.Windows.Forms.MenuItem();
            this.ExportMenuItem = new System.Windows.Forms.MenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.MoveUpMenuItem = new System.Windows.Forms.MenuItem();
            this.MoveDownMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem6});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SaveMenuItem,
            this.SaveAsMenuItem,
            this.menuItem4,
            this.menuItem5});
            this.menuItem1.Text = "File";
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Index = 0;
            this.SaveMenuItem.Text = "Save";
            this.SaveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // SaveAsMenuItem
            // 
            this.SaveAsMenuItem.Index = 1;
            this.SaveAsMenuItem.Text = "Save As...";
            this.SaveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
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
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.InsertMenuItem,
            this.ImportMenuItem,
            this.ExportMenuItem});
            this.menuItem6.Text = "Import";
            // 
            // InsertMenuItem
            // 
            this.InsertMenuItem.Index = 0;
            this.InsertMenuItem.Text = "Insert...";
            this.InsertMenuItem.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // ImportMenuItem
            // 
            this.ImportMenuItem.Index = 1;
            this.ImportMenuItem.Text = "Import Over...";
            this.ImportMenuItem.Click += new System.EventHandler(this.ImportMenuItem_Click);
            // 
            // DeleteMenuItem
            // 
            this.DeleteMenuItem.Index = 0;
            this.DeleteMenuItem.Text = "Delete";
            // 
            // ExportMenuItem
            // 
            this.ExportMenuItem.Index = 2;
            this.ExportMenuItem.Text = "Export...";
            this.ExportMenuItem.Click += new System.EventHandler(this.ExportMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = ".PNG files|*.png";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "POG Files|*.pog";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.DeleteMenuItem,
            this.menuItem3,
            this.MoveUpMenuItem,
            this.MoveDownMenuItem});
            this.menuItem2.Text = "Edit";
            // 
            // MoveUpMenuItem
            // 
            this.MoveUpMenuItem.Index = 2;
            this.MoveUpMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.MoveUpMenuItem.Text = "Move Up";
            this.MoveUpMenuItem.Click += new System.EventHandler(this.MoveUpMenuItem_Click);
            // 
            // MoveDownMenuItem
            // 
            this.MoveDownMenuItem.Index = 3;
            this.MoveDownMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.MoveDownMenuItem.Text = "Move Down";
            this.MoveDownMenuItem.Click += new System.EventHandler(this.MoveDownMenuItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // POGEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 536);
            this.Menu = this.mainMenu1;
            this.Name = "POGEditor";
            this.Text = "POGEditor";
            this.Load += new System.EventHandler(this.POGEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem SaveMenuItem;
        private System.Windows.Forms.MenuItem SaveAsMenuItem;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem InsertMenuItem;
        private System.Windows.Forms.MenuItem ImportMenuItem;
        private System.Windows.Forms.MenuItem DeleteMenuItem;
        private System.Windows.Forms.MenuItem ExportMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem MoveUpMenuItem;
        private System.Windows.Forms.MenuItem MoveDownMenuItem;
        private System.Windows.Forms.MenuItem menuItem3;
    }
}