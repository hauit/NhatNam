namespace ProcessingWork.Planning
{
    partial class Frm_CreateOrderNull
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
            this.mnFinishUsingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnChooseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOderNullSave = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOderNull = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFinish = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPartNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeadline = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.txtNumberOrder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCreateJigOrder = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnUpdateOrder = new System.Windows.Forms.Button();
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
            gridLevelNode1.RelationName = "Level1";
            this.GridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.GridControl1.Location = new System.Drawing.Point(0, 57);
            this.GridControl1.MainView = this.GridView1;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.Size = new System.Drawing.Size(934, 496);
            this.GridControl1.TabIndex = 0;
            this.GridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnFinishUsingToolStripMenuItem,
            this.mnChooseToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 48);
            // 
            // mnFinishUsingToolStripMenuItem
            // 
            this.mnFinishUsingToolStripMenuItem.Name = "mnFinishUsingToolStripMenuItem";
            this.mnFinishUsingToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.mnFinishUsingToolStripMenuItem.Text = "Kết thúc";
            this.mnFinishUsingToolStripMenuItem.Click += new System.EventHandler(this.mnFinishUsingToolStripMenuItem_Click);
            // 
            // mnChooseToolStripMenuItem
            // 
            this.mnChooseToolStripMenuItem.Name = "mnChooseToolStripMenuItem";
            this.mnChooseToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.mnChooseToolStripMenuItem.Text = "Tao order mới";
            this.mnChooseToolStripMenuItem.Visible = false;
            this.mnChooseToolStripMenuItem.Click += new System.EventHandler(this.mnChooseToolStripMenuItem_Click);
            // 
            // GridView1
            // 
            this.GridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.GridView1.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.GridView1.Appearance.ViewCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.GridView1.Appearance.ViewCaption.Options.UseFont = true;
            this.GridView1.Appearance.ViewCaption.Options.UseForeColor = true;
            this.GridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colOderNullSave,
            this.colOderNull,
            this.colOrder,
            this.colStatus,
            this.colFinish,
            this.colPartNo,
            this.colQty,
            this.colDeadline});
            this.GridView1.GridControl = this.GridControl1;
            this.GridView1.Name = "GridView1";
            this.GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsSelection.MultiSelect = true;
            this.GridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GridView1.OptionsView.ColumnAutoWidth = false;
            this.GridView1.OptionsView.ShowAutoFilterRow = true;
            this.GridView1.OptionsView.ShowFooter = true;
            this.GridView1.OptionsView.ShowGroupPanel = false;
            this.GridView1.OptionsView.ShowViewCaption = true;
            this.GridView1.ViewCaption = "Danh sách lệnh gấp";
            this.GridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GridView1_ValidateRow);
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.OptionsColumn.ReadOnly = true;
            this.colID.Visible = true;
            this.colID.VisibleIndex = 0;
            // 
            // colOderNullSave
            // 
            this.colOderNullSave.Caption = "Order ảo";
            this.colOderNullSave.FieldName = "OrderNullSave";
            this.colOderNullSave.Name = "colOderNullSave";
            this.colOderNullSave.OptionsColumn.ReadOnly = true;
            this.colOderNullSave.Visible = true;
            this.colOderNullSave.VisibleIndex = 1;
            // 
            // colOderNull
            // 
            this.colOderNull.Caption = "Order ảo";
            this.colOderNull.FieldName = "OrderNull";
            this.colOderNull.Name = "colOderNull";
            this.colOderNull.OptionsColumn.ReadOnly = true;
            // 
            // colOrder
            // 
            this.colOrder.Caption = "Order thực";
            this.colOrder.FieldName = "Order";
            this.colOrder.Name = "colOrder";
            this.colOrder.Visible = true;
            this.colOrder.VisibleIndex = 2;
            // 
            // colStatus
            // 
            this.colStatus.Caption = "Tình trạng";
            this.colStatus.FieldName = "status";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 3;
            // 
            // colFinish
            // 
            this.colFinish.Caption = "Kết thúc";
            this.colFinish.FieldName = "Finished";
            this.colFinish.Name = "colFinish";
            this.colFinish.Visible = true;
            this.colFinish.VisibleIndex = 4;
            // 
            // colPartNo
            // 
            this.colPartNo.Caption = "Tên chi tiết";
            this.colPartNo.FieldName = "PartNo";
            this.colPartNo.Name = "colPartNo";
            this.colPartNo.Visible = true;
            this.colPartNo.VisibleIndex = 5;
            // 
            // colQty
            // 
            this.colQty.Caption = "Số lượng";
            this.colQty.FieldName = "Qty";
            this.colQty.Name = "colQty";
            this.colQty.Visible = true;
            this.colQty.VisibleIndex = 6;
            // 
            // colDeadline
            // 
            this.colDeadline.Caption = "Thời hạn";
            this.colDeadline.FieldName = "Deadline";
            this.colDeadline.Name = "colDeadline";
            this.colDeadline.Visible = true;
            this.colDeadline.VisibleIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbType);
            this.panel1.Controls.Add(this.txtNumberOrder);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Controls.Add(this.btnCreateJigOrder);
            this.panel1.Controls.Add(this.btnCreate);
            this.panel1.Controls.Add(this.btnUpdateOrder);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(934, 57);
            this.panel1.TabIndex = 0;
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "",
            "Jig",
            "Null",
            "All"});
            this.cbType.Location = new System.Drawing.Point(81, 30);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(132, 21);
            this.cbType.TabIndex = 9;
            // 
            // txtNumberOrder
            // 
            this.txtNumberOrder.Location = new System.Drawing.Point(81, 5);
            this.txtNumberOrder.Name = "txtNumberOrder";
            this.txtNumberOrder.Size = new System.Drawing.Size(132, 20);
            this.txtNumberOrder.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Kiểu lệnh:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Số lệnh:";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(235, 29);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(411, 29);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCreateJigOrder
            // 
            this.btnCreateJigOrder.Location = new System.Drawing.Point(316, 3);
            this.btnCreateJigOrder.Name = "btnCreateJigOrder";
            this.btnCreateJigOrder.Size = new System.Drawing.Size(89, 23);
            this.btnCreateJigOrder.TabIndex = 1;
            this.btnCreateJigOrder.Text = "Phát lệnh JIG";
            this.btnCreateJigOrder.UseVisualStyleBackColor = true;
            this.btnCreateJigOrder.Click += new System.EventHandler(this.btnCreateJigOrder_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(235, 3);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Phát lệnh";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnUpdateOrder
            // 
            this.btnUpdateOrder.Location = new System.Drawing.Point(734, 3);
            this.btnUpdateOrder.Name = "btnUpdateOrder";
            this.btnUpdateOrder.Size = new System.Drawing.Size(124, 23);
            this.btnUpdateOrder.TabIndex = 1;
            this.btnUpdateOrder.Text = "Update Order Null";
            this.btnUpdateOrder.UseVisualStyleBackColor = true;
            this.btnUpdateOrder.Visible = false;
            this.btnUpdateOrder.Click += new System.EventHandler(this.btnUpdateOrder_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(411, 3);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "Xem";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // Frm_CreateOrderNull
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 553);
            this.Controls.Add(this.GridControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Frm_CreateOrderNull";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kế hoạch gia công";
            this.Load += new System.EventHandler(this.Frm_CreateOrderNull_Load);
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
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colOderNull;
        private DevExpress.XtraGrid.Columns.GridColumn colOderNullSave;
        private DevExpress.XtraGrid.Columns.GridColumn colOrder;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colFinish;
        private DevExpress.XtraGrid.Columns.GridColumn colPartNo;
        private DevExpress.XtraGrid.Columns.GridColumn colQty;
        private DevExpress.XtraGrid.Columns.GridColumn colDeadline;

        private DevExpress.XtraGrid.GridControl GridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.TextBox txtNumberOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCreateJigOrder;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdateOrder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnFinishUsingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnChooseToolStripMenuItem;
    }
}