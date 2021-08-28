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
            this.CutMenuItem = new System.Windows.Forms.MenuItem();
            this.CopyMenuItem = new System.Windows.Forms.MenuItem();
            this.PasteMenuItem = new System.Windows.Forms.MenuItem();
            this.DeleteMenu = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.MoveUpMenuItem = new System.Windows.Forms.MenuItem();
            this.MoveDownMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.MakeAnimatedMenuItem = new System.Windows.Forms.MenuItem();
            this.ClearAnimationMenuItem = new System.Windows.Forms.MenuItem();
            this.ImportMenu = new System.Windows.Forms.MenuItem();
            this.InsertMenuItem = new System.Windows.Forms.MenuItem();
            this.ImportMenuItem = new System.Windows.Forms.MenuItem();
            this.ExportMenuItem = new System.Windows.Forms.MenuItem();
            this.DebugMenuSeparator = new System.Windows.Forms.MenuItem();
            this.ExportILBMMenuItem = new System.Windows.Forms.MenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem6,
            this.ImportMenu});
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
            this.SaveMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.SaveMenu.Text = "Save";
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
            // CloseMenu
            // 
            this.CloseMenu.Index = 3;
            this.CloseMenu.Text = "Close Window";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.CutMenuItem,
            this.CopyMenuItem,
            this.PasteMenuItem,
            this.DeleteMenu,
            this.menuItem8,
            this.MoveUpMenuItem,
            this.MoveDownMenuItem,
            this.menuItem2,
            this.MakeAnimatedMenuItem,
            this.ClearAnimationMenuItem});
            this.menuItem6.Text = "Edit";
            // 
            // CutMenuItem
            // 
            this.CutMenuItem.Index = 0;
            this.CutMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.CutMenuItem.Text = "Cut";
            // 
            // CopyMenuItem
            // 
            this.CopyMenuItem.Index = 1;
            this.CopyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.CopyMenuItem.Text = "Copy";
            // 
            // PasteMenuItem
            // 
            this.PasteMenuItem.Index = 2;
            this.PasteMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.PasteMenuItem.Text = "Paste";
            // 
            // DeleteMenu
            // 
            this.DeleteMenu.Index = 3;
            this.DeleteMenu.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.DeleteMenu.Text = "Delete";
            this.DeleteMenu.Click += new System.EventHandler(this.DeleteMenu_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 4;
            this.menuItem8.Text = "-";
            // 
            // MoveUpMenuItem
            // 
            this.MoveUpMenuItem.Index = 5;
            this.MoveUpMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.MoveUpMenuItem.Text = "Move Up";
            this.MoveUpMenuItem.Click += new System.EventHandler(this.MoveUpMenuItem_Click);
            // 
            // MoveDownMenuItem
            // 
            this.MoveDownMenuItem.Index = 6;
            this.MoveDownMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.MoveDownMenuItem.Text = "Move Down";
            this.MoveDownMenuItem.Click += new System.EventHandler(this.MoveDownMenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 7;
            this.menuItem2.Text = "-";
            // 
            // MakeAnimatedMenuItem
            // 
            this.MakeAnimatedMenuItem.Index = 8;
            this.MakeAnimatedMenuItem.Text = "Make Range Animated";
            this.MakeAnimatedMenuItem.Click += new System.EventHandler(this.MakeAnimatedMenuItem_Click);
            // 
            // ClearAnimationMenuItem
            // 
            this.ClearAnimationMenuItem.Index = 9;
            this.ClearAnimationMenuItem.Text = "Clear Animation";
            this.ClearAnimationMenuItem.Click += new System.EventHandler(this.ClearAnimationMenuItem_Click);
            // 
            // ImportMenu
            // 
            this.ImportMenu.Index = 2;
            this.ImportMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.InsertMenuItem,
            this.ImportMenuItem,
            this.ExportMenuItem,
            this.DebugMenuSeparator,
            this.ExportILBMMenuItem});
            this.ImportMenu.Text = "Import";
            // 
            // InsertMenuItem
            // 
            this.InsertMenuItem.Index = 0;
            this.InsertMenuItem.Text = "Insert...";
            this.InsertMenuItem.Click += new System.EventHandler(this.InsertMenu_Click);
            // 
            // ImportMenuItem
            // 
            this.ImportMenuItem.Index = 1;
            this.ImportMenuItem.Text = "Import Over...";
            this.ImportMenuItem.Click += new System.EventHandler(this.ImportMenuItem_Click);
            // 
            // ExportMenuItem
            // 
            this.ExportMenuItem.Index = 2;
            this.ExportMenuItem.Text = "Export...";
            this.ExportMenuItem.Click += new System.EventHandler(this.ExportMenu_Click);
            // 
            // DebugMenuSeparator
            // 
            this.DebugMenuSeparator.Index = 3;
            this.DebugMenuSeparator.Text = "-";
            // 
            // ExportILBMMenuItem
            // 
            this.ExportILBMMenuItem.Index = 4;
            this.ExportILBMMenuItem.Text = "Export as ILBM...";
            this.ExportILBMMenuItem.Click += new System.EventHandler(this.ExportILBMMenu_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "PIG Files|*.pig";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // PIGEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 540);
            this.Menu = this.mainMenu1;
            this.Name = "PIGEditor";
            this.Text = "PIGEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PIGEditor_FormClosing);
            this.Load += new System.EventHandler(this.PIGEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem SaveMenu;
        private System.Windows.Forms.MenuItem SaveAsMenu;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem CloseMenu;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem InsertMenuItem;
        private System.Windows.Forms.MenuItem ImportMenuItem;
        private System.Windows.Forms.MenuItem DeleteMenu;
        private System.Windows.Forms.MenuItem ExportMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuItem CutMenuItem;
        private System.Windows.Forms.MenuItem CopyMenuItem;
        private System.Windows.Forms.MenuItem PasteMenuItem;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem MoveUpMenuItem;
        private System.Windows.Forms.MenuItem MoveDownMenuItem;
        private System.Windows.Forms.MenuItem ImportMenu;
        private System.Windows.Forms.MenuItem DebugMenuSeparator;
        private System.Windows.Forms.MenuItem ExportILBMMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem MakeAnimatedMenuItem;
        private System.Windows.Forms.MenuItem ClearAnimationMenuItem;
    }
}