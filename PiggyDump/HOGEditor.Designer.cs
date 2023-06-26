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
            this.lvlPreview = new Descent2Workshop.LevelViewer();
            this.btnSave = new System.Windows.Forms.Button();
            this.picPreview = new Descent2Workshop.PictureBoxInterpolation();
            this.txtPreview = new System.Windows.Forms.TextBox();
            this.lblPreviewPlaceholder = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
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
            this.listView1.AllowDrop = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(352, 500);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            this.listView1.DragOver += new System.Windows.Forms.DragEventHandler(this.listView1_DragOver);
            this.listView1.DragLeave += new System.EventHandler(this.listView1_DragLeave);
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
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvlPreview);
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Panel2.Controls.Add(this.picPreview);
            this.splitContainer1.Panel2.Controls.Add(this.txtPreview);
            this.splitContainer1.Panel2.Controls.Add(this.lblPreviewPlaceholder);
            this.splitContainer1.Panel2.SizeChanged += new System.EventHandler(this.splitContainer1_Panel2_SizeChanged);
            this.splitContainer1.Size = new System.Drawing.Size(862, 500);
            this.splitContainer1.SplitterDistance = 352;
            this.splitContainer1.TabIndex = 4;
            // 
            // lvlPreview
            // 
            this.lvlPreview.BackColor = System.Drawing.Color.Black;
            this.lvlPreview.Host = null;
            this.lvlPreview.Level = null;
            this.lvlPreview.Location = new System.Drawing.Point(0, 0);
            this.lvlPreview.Name = "lvlPreview";
            this.lvlPreview.Size = new System.Drawing.Size(150, 150);
            this.lvlPreview.TabIndex = 4;
            this.lvlPreview.Visible = false;
            this.lvlPreview.VSync = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(0, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save in HOG";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.Black;
            this.picPreview.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.picPreview.Location = new System.Drawing.Point(0, 0);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(100, 50);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 2;
            this.picPreview.TabStop = false;
            this.picPreview.Visible = false;
            // 
            // txtPreview
            // 
            this.txtPreview.Location = new System.Drawing.Point(0, 29);
            this.txtPreview.MaxLength = 0;
            this.txtPreview.Multiline = true;
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPreview.Size = new System.Drawing.Size(181, 36);
            this.txtPreview.TabIndex = 1;
            this.txtPreview.Visible = false;
            this.txtPreview.ModifiedChanged += new System.EventHandler(this.txtPreview_ModifiedChanged);
            // 
            // lblPreviewPlaceholder
            // 
            this.lblPreviewPlaceholder.AutoSize = true;
            this.lblPreviewPlaceholder.Location = new System.Drawing.Point(135, 224);
            this.lblPreviewPlaceholder.Name = "lblPreviewPlaceholder";
            this.lblPreviewPlaceholder.Size = new System.Drawing.Size(244, 13);
            this.lblPreviewPlaceholder.TabIndex = 0;
            this.lblPreviewPlaceholder.Text = "Really Cool Element Previewer Feature Goes Here";
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HOGEditor_FormClosing);
            this.Load += new System.EventHandler(this.HOGEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
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
        private System.Windows.Forms.Label lblPreviewPlaceholder;
        private System.Windows.Forms.TextBox txtPreview;
        private System.Windows.Forms.Button btnSave;
        private PictureBoxInterpolation picPreview;
        private LevelViewer lvlPreview;
    }
}