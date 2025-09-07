namespace ProcessingWork.WTS.InputWTS
{
    partial class Frm_WTS
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.GridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.OKWTSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbStaff = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbMachine = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbShift = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExPort = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btn_Import = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridControl1
            // 
            this.GridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.GridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.GridControl1.Location = new System.Drawing.Point(0, 82);
            this.GridControl1.MainView = this.GridView1;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.Size = new System.Drawing.Size(830, 419);
            this.GridControl1.TabIndex = 20;
            this.GridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDelete,
            this.OKWTSToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(106, 48);
            // 
            // mnDelete
            // 
            this.mnDelete.Name = "mnDelete";
            this.mnDelete.Size = new System.Drawing.Size(105, 22);
            this.mnDelete.Text = "Xóa";
            this.mnDelete.Click += new System.EventHandler(this.mnDelete_Click);
            // 
            // OKWTSToolStripMenuItem
            // 
            this.OKWTSToolStripMenuItem.Name = "OKWTSToolStripMenuItem";
            this.OKWTSToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.OKWTSToolStripMenuItem.Text = "Duyệt";
            this.OKWTSToolStripMenuItem.Click += new System.EventHandler(this.OKWTSToolStripMenuItem_Click);
            // 
            // GridView1
            // 
            this.GridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.GridView1.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.GridView1.Appearance.ViewCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.GridView1.Appearance.ViewCaption.Options.UseFont = true;
            this.GridView1.Appearance.ViewCaption.Options.UseForeColor = true;
            this.GridView1.GridControl = this.GridControl1;
            this.GridView1.Name = "GridView1";
            this.GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsSelection.MultiSelect = true;
            this.GridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GridView1.OptionsView.ColumnAutoWidth = false;
            this.GridView1.OptionsView.ShowAutoFilterRow = true;
            this.GridView1.OptionsView.ShowFooter = true;
            this.GridView1.OptionsView.ShowGroupPanel = false;
            this.GridView1.OptionsView.ShowViewCaption = true;
            this.GridView1.ViewCaption = "Chi tiết WTS nhân viên";
            this.GridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.GridView1_RowCellStyle);
            this.GridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GridView1_ValidateRow);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbStaff);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cbMachine);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbShift);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dateTo);
            this.panel1.Controls.Add(this.dateFrom);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btn_Import);
            this.panel1.Controls.Add(this.btnExPort);
            this.panel1.Controls.Add(this.btnConfirm);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 82);
            this.panel1.TabIndex = 100;
            // 
            // cbStaff
            // 
            this.cbStaff.FormattingEnabled = true;
            this.cbStaff.Location = new System.Drawing.Point(467, 12);
            this.cbStaff.Name = "cbStaff";
            this.cbStaff.Size = new System.Drawing.Size(121, 21);
            this.cbStaff.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(405, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Nhân viên";
            // 
            // cbMachine
            // 
            this.cbMachine.FormattingEnabled = true;
            this.cbMachine.Location = new System.Drawing.Point(275, 44);
            this.cbMachine.Name = "cbMachine";
            this.cbMachine.Size = new System.Drawing.Size(121, 21);
            this.cbMachine.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(246, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Máy:";
            // 
            // cbShift
            // 
            this.cbShift.FormattingEnabled = true;
            this.cbShift.Items.AddRange(new object[] {
            "T0",
            "T1",
            "T2",
            "T3"});
            this.cbShift.Location = new System.Drawing.Point(275, 13);
            this.cbShift.Name = "cbShift";
            this.cbShift.Size = new System.Drawing.Size(121, 21);
            this.cbShift.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Ca:";
            // 
            // dateTo
            // 
            this.dateTo.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTo.Location = new System.Drawing.Point(75, 44);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(163, 20);
            this.dateTo.TabIndex = 6;
            // 
            // dateFrom
            // 
            this.dateFrom.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateFrom.Location = new System.Drawing.Point(75, 12);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(163, 20);
            this.dateFrom.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Đến ngày:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Từ ngày:";
            // 
            // btnExPort
            // 
            this.btnExPort.Location = new System.Drawing.Point(727, 11);
            this.btnExPort.Name = "btnExPort";
            this.btnExPort.Size = new System.Drawing.Size(91, 23);
            this.btnExPort.TabIndex = 3;
            this.btnExPort.Text = "Export to Excel";
            this.btnExPort.UseVisualStyleBackColor = true;
            this.btnExPort.Click += new System.EventHandler(this.btnExPort_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(626, 42);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(91, 23);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "Duyệt";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(626, 10);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(91, 23);
            this.btnView.TabIndex = 2;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btn_Import
            // 
            this.btn_Import.Location = new System.Drawing.Point(727, 42);
            this.btn_Import.Name = "btn_Import";
            this.btn_Import.Size = new System.Drawing.Size(91, 23);
            this.btn_Import.TabIndex = 3;
            this.btn_Import.Text = "Import WTS";
            this.btn_Import.UseVisualStyleBackColor = true;
            this.btn_Import.Click += new System.EventHandler(this.btn_Import_Click);
            // 
            // Frm_WTS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 501);
            this.Controls.Add(this.GridControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Frm_WTS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WTS nhan vien";
            this.Load += new System.EventHandler(this.Frm_WTS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        //////
        private DevExpress.XtraGrid.GridControl GridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView1;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnExPort;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnDelete;
        private System.Windows.Forms.DateTimePicker dateTo;
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbMachine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbShift;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStaff;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem OKWTSToolStripMenuItem;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btn_Import;
    }
}