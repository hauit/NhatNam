namespace WorkingNotification
{
    partial class Frm_LgLDTeam
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
            this.btnImport = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtLinkWeb = new System.Windows.Forms.TextBox();
            this.nTimeSendMail = new System.Windows.Forms.NumericUpDown();
            this.cbReport = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nTimeSendMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(344, 2);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(91, 23);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import List";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(344, 31);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(91, 23);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "Refresh data";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.txtLinkWeb);
            this.panel1.Controls.Add(this.nTimeSendMail);
            this.panel1.Controls.Add(this.cbReport);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 57);
            this.panel1.TabIndex = 102;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(536, 30);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(185, 20);
            this.txtPassword.TabIndex = 12;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(536, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(185, 20);
            this.txtEmail.TabIndex = 12;
            // 
            // txtLinkWeb
            // 
            this.txtLinkWeb.Location = new System.Drawing.Point(218, 31);
            this.txtLinkWeb.Name = "txtLinkWeb";
            this.txtLinkWeb.Size = new System.Drawing.Size(100, 20);
            this.txtLinkWeb.TabIndex = 12;
            this.txtLinkWeb.Text = "10.226.90.26";
            // 
            // nTimeSendMail
            // 
            this.nTimeSendMail.Location = new System.Drawing.Point(84, 31);
            this.nTimeSendMail.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.nTimeSendMail.Name = "nTimeSendMail";
            this.nTimeSendMail.Size = new System.Drawing.Size(120, 20);
            this.nTimeSendMail.TabIndex = 11;
            this.nTimeSendMail.Value = new decimal(new int[] {
            540,
            0,
            0,
            0});
            // 
            // cbReport
            // 
            this.cbReport.FormattingEnabled = true;
            this.cbReport.Items.AddRange(new object[] {
            "",
            "Item list",
            "Project list"});
            this.cbReport.Location = new System.Drawing.Point(84, 4);
            this.cbReport.Name = "cbReport";
            this.cbReport.Size = new System.Drawing.Size(234, 21);
            this.cbReport.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Mail time:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(474, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "SMTP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(474, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Sender:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Data type:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(727, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 40);
            this.button1.TabIndex = 3;
            this.button1.Text = "Import List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 57);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(800, 393);
            this.dataGridView1.TabIndex = 103;
            // 
            // Frm_LgLDTeam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "Frm_LgLDTeam";
            this.Text = "LGEVH.PD Team";
            this.Load += new System.EventHandler(this.Frm_LgLDTeam_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nTimeSendMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cbReport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nTimeSendMail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLinkWeb;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
    }
}