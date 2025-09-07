namespace ProcessingWork.InputData.BusOrder
{
    partial class Frm_BusOrder
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.GridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.PrintOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDept = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnExPort = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnOrderPrint = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
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
            gridLevelNode2.RelationName = "Level1";
            this.GridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.GridControl1.Location = new System.Drawing.Point(0, 57);
            this.GridControl1.MainView = this.GridView1;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.Size = new System.Drawing.Size(703, 369);
            this.GridControl1.TabIndex = 20;
            this.GridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDelete,
            this.PrintOrderToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(111, 48);
            // 
            // mnDelete
            // 
            this.mnDelete.Name = "mnDelete";
            this.mnDelete.Size = new System.Drawing.Size(110, 22);
            this.mnDelete.Text = "Xóa";
            this.mnDelete.Click += new System.EventHandler(this.mnDelete_Click);
            // 
            // PrintOrderToolStripMenuItem
            // 
            this.PrintOrderToolStripMenuItem.Name = "PrintOrderToolStripMenuItem";
            this.PrintOrderToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.PrintOrderToolStripMenuItem.Text = "In lệnh";
            this.PrintOrderToolStripMenuItem.Click += new System.EventHandler(this.PrintOrderToolStripMenuItem_Click);
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
            this.GridView1.ViewCaption = "Danh sách order";
            this.GridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GridView1_ValidateRow);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDept);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnExPort);
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Controls.Add(this.btnOrderPrint);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(703, 57);
            this.panel1.TabIndex = 100;
            // 
            // cbDept
            // 
            this.cbDept.FormattingEnabled = true;
            this.cbDept.Items.AddRange(new object[] {
            "",
            "Import order mới",
            "Thay đổi thời hạn",
            "Thay đổi số lượng",
            "Hủy đơn hàng",
            "Kết thúc đơn hàng",
            "Start đơn hàng",
            "Import order form mới",
            "Tình trạng thanh toán",
            "Import Status",
            "Import Order cắt"});
            this.cbDept.Location = new System.Drawing.Point(96, 5);
            this.cbDept.Name = "cbDept";
            this.cbDept.Size = new System.Drawing.Size(164, 21);
            this.cbDept.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Công việc:";
            // 
            // btnExPort
            // 
            this.btnExPort.Location = new System.Drawing.Point(347, 3);
            this.btnExPort.Name = "btnExPort";
            this.btnExPort.Size = new System.Drawing.Size(91, 23);
            this.btnExPort.TabIndex = 3;
            this.btnExPort.Text = "Export to Excel";
            this.btnExPort.UseVisualStyleBackColor = true;
            this.btnExPort.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(266, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import List";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnOrderPrint
            // 
            this.btnOrderPrint.Location = new System.Drawing.Point(347, 28);
            this.btnOrderPrint.Name = "btnOrderPrint";
            this.btnOrderPrint.Size = new System.Drawing.Size(91, 23);
            this.btnOrderPrint.TabIndex = 2;
            this.btnOrderPrint.Text = "In tờ lệnh";
            this.btnOrderPrint.UseVisualStyleBackColor = true;
            this.btnOrderPrint.Click += new System.EventHandler(this.btnOrderPrint_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(266, 28);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 2;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // Frm_BusOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 426);
            this.Controls.Add(this.GridControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Frm_BusOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order";
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
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExPort;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnDelete;
        private System.Windows.Forms.ComboBox cbDept;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOrderPrint;
        private System.Windows.Forms.ToolStripMenuItem PrintOrderToolStripMenuItem;
    }
}