using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace ProcessingWork.Planning
{
    public partial class Rp_InToLenh_New : DevExpress.XtraReports.UI.XtraReport
    {

        public Rp_InToLenh_New()
        {
            InitializeComponent();
        }

        public Rp_InToLenh_New(Ds_InToLenhTest _Table1)
        {
            InitializeComponent();
            this.DataSource = _Table1;
            this.DataMember = _Table1.Tables["InToLenh"].TableName;
            DetailReport.DataMember = "InToLenh.FK_InToLenh_Detail";
        }
    }
}
