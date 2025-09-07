namespace ProcessingWork.Planning
{
    partial class Frm_ImportKH
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
            this.colDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSoJig = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDKM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDept = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProcessInFreely = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShift = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.GridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colViaCheck2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colViaGhiChu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMachineID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSlg = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStart = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFinish = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTGGC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTGGL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKHCT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateToDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateFromDate = new System.Windows.Forms.DateTimePicker();
            this.btnView = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.cbShift = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDept = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colDate
            // 
            this.colDate.Caption = "Ngày nhập";
            this.colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            this.colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colDate.FieldName = "Date";
            this.colDate.Name = "colDate";
            this.colDate.Visible = true;
            this.colDate.VisibleIndex = 0;
            // 
            // colTT
            // 
            this.colTT.Caption = "Tình trạng phôi";
            this.colTT.FieldName = "TinhTrang";
            this.colTT.Name = "colTT";
            this.colTT.Visible = true;
            this.colTT.VisibleIndex = 16;
            // 
            // colTH
            // 
            this.colTH.Caption = "Thời hạn";
            this.colTH.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            this.colTH.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colTH.FieldName = "ThoiHan";
            this.colTH.Name = "colTH";
            this.colTH.Visible = true;
            this.colTH.VisibleIndex = 17;
            // 
            // colSoJig
            // 
            this.colSoJig.Caption = "Số jig";
            this.colSoJig.FieldName = "SoJig";
            this.colSoJig.Name = "colSoJig";
            this.colSoJig.Visible = true;
            this.colSoJig.VisibleIndex = 18;
            // 
            // colDKM
            // 
            this.colDKM.Caption = "Máy";
            this.colDKM.FieldName = "Jig";
            this.colDKM.Name = "colDKM";
            this.colDKM.Visible = true;
            this.colDKM.VisibleIndex = 19;
            // 
            // colDept
            // 
            this.colDept.Caption = "Số jig";
            this.colDept.FieldName = "Dept";
            this.colDept.Name = "colDept";
            this.colDept.Visible = true;
            this.colDept.VisibleIndex = 20;
            // 
            // colProcessInFreely
            // 
            this.colProcessInFreely.Caption = "Ko theo thứ tự";
            this.colProcessInFreely.FieldName = "ProcessInFreely";
            this.colProcessInFreely.Name = "colProcessInFreely";
            this.colProcessInFreely.Visible = true;
            this.colProcessInFreely.VisibleIndex = 21;
            // 
            // colShift
            // 
            this.colShift.Caption = "Ca";
            this.colShift.FieldName = "Shift";
            this.colShift.Name = "colShift";
            this.colShift.Visible = true;
            this.colShift.VisibleIndex = 1;
            // 
            // colK
            // 
            this.colK.Caption = "Thứ tự gia công";
            this.colK.FieldName = "K";
            this.colK.Name = "colK";
            this.colK.Visible = true;
            this.colK.VisibleIndex = 15;
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
            this.GridControl1.Size = new System.Drawing.Size(830, 444);
            this.GridControl1.TabIndex = 1;
            this.GridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // mnDelete
            // 
            this.mnDelete.Name = "mnDelete";
            this.mnDelete.Size = new System.Drawing.Size(94, 22);
            this.mnDelete.Text = "Xóa";
            this.mnDelete.Click += new System.EventHandler(this.mnDelete_Click);
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
            this.col10,
            this.col20,
            this.colViaCheck2,
            this.colViaGhiChu,
            this.colOder,
            this.colNC,
            this.colMachineID,
            this.colSlg,
            this.colStart,
            this.colFinish,
            this.colTGGC,
            this.colTGGL,
            this.colKHCT,
            this.colK,
            this.colDate,
            this.colShift,
            this.colTT,
            this.colTH,
            this.colSoJig,
            this.colDKM,
            this.colDept,
            this.colProcessInFreely});
            this.GridView1.GridControl = this.GridControl1;
            this.GridView1.Name = "GridView1";
            this.GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.GridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsSelection.MultiSelect = true;
            this.GridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GridView1.OptionsView.ColumnAutoWidth = false;
            this.GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.GridView1.OptionsView.ShowAutoFilterRow = true;
            this.GridView1.OptionsView.ShowFooter = true;
            this.GridView1.OptionsView.ShowGroupPanel = false;
            this.GridView1.OptionsView.ShowViewCaption = true;
            this.GridView1.ViewCaption = "Danh sách kế hoạch gia công";
            this.GridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GridView1_ValidateRow);
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // col10
            // 
            this.col10.Caption = "Trạng thái CB";
            this.col10.FieldName = "10%";
            this.col10.Name = "col10";
            this.col10.Visible = true;
            this.col10.VisibleIndex = 10;
            // 
            // col20
            // 
            this.col20.Caption = "Số lượng CB";
            this.col20.FieldName = "20%";
            this.col20.Name = "col20";
            this.col20.Visible = true;
            this.col20.VisibleIndex = 8;
            // 
            // colViaCheck2
            // 
            this.colViaCheck2.Caption = "Code chuẩn bị";
            this.colViaCheck2.FieldName = "Via_check2";
            this.colViaCheck2.Name = "colViaCheck2";
            this.colViaCheck2.Visible = true;
            this.colViaCheck2.VisibleIndex = 7;
            // 
            // colViaGhiChu
            // 
            this.colViaGhiChu.Caption = "TG chuẩn bị";
            this.colViaGhiChu.FieldName = "Via_ghi_chu";
            this.colViaGhiChu.Name = "colViaGhiChu";
            this.colViaGhiChu.Visible = true;
            this.colViaGhiChu.VisibleIndex = 9;
            // 
            // colOder
            // 
            this.colOder.Caption = "Số Order";
            this.colOder.FieldName = "Order";
            this.colOder.Name = "colOder";
            this.colOder.OptionsColumn.FixedWidth = true;
            this.colOder.Visible = true;
            this.colOder.VisibleIndex = 5;
            this.colOder.Width = 100;
            // 
            // colNC
            // 
            this.colNC.Caption = "Số NC";
            this.colNC.FieldName = "NC";
            this.colNC.Name = "colNC";
            this.colNC.Visible = true;
            this.colNC.VisibleIndex = 4;
            // 
            // colMachineID
            // 
            this.colMachineID.Caption = "Máy GC";
            this.colMachineID.FieldName = "MayGC";
            this.colMachineID.Name = "colMachineID";
            this.colMachineID.Visible = true;
            this.colMachineID.VisibleIndex = 3;
            // 
            // colSlg
            // 
            this.colSlg.Caption = "Số lượng lệnh";
            this.colSlg.FieldName = "Slglenh";
            this.colSlg.Name = "colSlg";
            this.colSlg.Visible = true;
            this.colSlg.VisibleIndex = 6;
            // 
            // colStart
            // 
            this.colStart.Caption = "Bắt đầu";
            this.colStart.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            this.colStart.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colStart.FieldName = "BatDau";
            this.colStart.Name = "colStart";
            this.colStart.Visible = true;
            this.colStart.VisibleIndex = 13;
            // 
            // colFinish
            // 
            this.colFinish.Caption = "Kết thúc";
            this.colFinish.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            this.colFinish.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colFinish.FieldName = "KetThuc";
            this.colFinish.Name = "colFinish";
            this.colFinish.Visible = true;
            this.colFinish.VisibleIndex = 14;
            // 
            // colTGGC
            // 
            this.colTGGC.Caption = "Thời gian gia công";
            this.colTGGC.FieldName = "TGGC";
            this.colTGGC.Name = "colTGGC";
            this.colTGGC.Visible = true;
            this.colTGGC.VisibleIndex = 11;
            // 
            // colTGGL
            // 
            this.colTGGL.Caption = "Thời gian gá lắp";
            this.colTGGL.FieldName = "TGGL";
            this.colTGGL.Name = "colTGGL";
            this.colTGGL.Visible = true;
            this.colTGGL.VisibleIndex = 12;
            // 
            // colKHCT
            // 
            this.colKHCT.Caption = "Ký hiệu chi tiết";
            this.colKHCT.FieldName = "TenChiTiet";
            this.colKHCT.Name = "colKHCT";
            this.colKHCT.OptionsColumn.FixedWidth = true;
            this.colKHCT.Visible = true;
            this.colKHCT.VisibleIndex = 2;
            this.colKHCT.Width = 300;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Từ ngày:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ca:";
            // 
            // dateToDate
            // 
            this.dateToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateToDate.Location = new System.Drawing.Point(76, 29);
            this.dateToDate.Name = "dateToDate";
            this.dateToDate.Size = new System.Drawing.Size(98, 20);
            this.dateToDate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Đến ngày:";
            // 
            // dateFromDate
            // 
            this.dateFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateFromDate.Location = new System.Drawing.Point(76, 3);
            this.dateFromDate.Name = "dateFromDate";
            this.dateFromDate.Size = new System.Drawing.Size(98, 20);
            this.dateFromDate.TabIndex = 4;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(358, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 7;
            this.btnView.Text = "Xem";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(439, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(358, 30);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Import KHX";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cbShift
            // 
            this.cbShift.FormattingEnabled = true;
            this.cbShift.Items.AddRange(new object[] {
            "T0",
            "T1",
            "T2",
            "T3"});
            this.cbShift.Location = new System.Drawing.Point(231, 3);
            this.cbShift.Name = "cbShift";
            this.cbShift.Size = new System.Drawing.Size(121, 21);
            this.cbShift.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDept);
            this.panel1.Controls.Add(this.cbShift);
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.dateFromDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.dateToDate);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 57);
            this.panel1.TabIndex = 2;
            // 
            // cbDept
            // 
            this.cbDept.FormattingEnabled = true;
            this.cbDept.Location = new System.Drawing.Point(231, 29);
            this.cbDept.Name = "cbDept";
            this.cbDept.Size = new System.Drawing.Size(121, 21);
            this.cbDept.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(181, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Bộ phận:";
            // 
            // Frm_ImportKH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 501);
            this.Controls.Add(this.GridControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Frm_ImportKH";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kế hoạch gia công";
            this.Load += new System.EventHandler(this.Frm_ImportKH_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn colTT;
        private DevExpress.XtraGrid.Columns.GridColumn colTH;
        private DevExpress.XtraGrid.Columns.GridColumn colSoJig;
        private DevExpress.XtraGrid.Columns.GridColumn colDKM;
        private DevExpress.XtraGrid.Columns.GridColumn colDept;
        private DevExpress.XtraGrid.Columns.GridColumn colProcessInFreely;
        private DevExpress.XtraGrid.Columns.GridColumn colShift;
        private DevExpress.XtraGrid.Columns.GridColumn colK;
        private DevExpress.XtraGrid.GridControl GridControl1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnDelete;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn col10;
        private DevExpress.XtraGrid.Columns.GridColumn col20;
        private DevExpress.XtraGrid.Columns.GridColumn colViaCheck2;
        private DevExpress.XtraGrid.Columns.GridColumn colViaGhiChu;
        private DevExpress.XtraGrid.Columns.GridColumn colOder;
        private DevExpress.XtraGrid.Columns.GridColumn colNC;
        private DevExpress.XtraGrid.Columns.GridColumn colMachineID;
        private DevExpress.XtraGrid.Columns.GridColumn colSlg;
        private DevExpress.XtraGrid.Columns.GridColumn colStart;
        private DevExpress.XtraGrid.Columns.GridColumn colFinish;
        private DevExpress.XtraGrid.Columns.GridColumn colTGGC;
        private DevExpress.XtraGrid.Columns.GridColumn colTGGL;
        private DevExpress.XtraGrid.Columns.GridColumn colKHCT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateFromDate;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ComboBox cbShift;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbDept;
        private System.Windows.Forms.Label label4;
    }
}