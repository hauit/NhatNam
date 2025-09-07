using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessingWork.Planning
{
    public partial class frm_InToLenh : DevExpress.XtraEditors.XtraForm
    {
        public DataTable _Table;
        public Ds_InToLenh _Table1;

        public frm_InToLenh()
        {
            InitializeComponent();
        }

        private void frm_InCauHinhHuy_Load(object sender, EventArgs e)
        {
            Rp_InToLenh Report = new Rp_InToLenh(_Table1);
            //Report.DataSource = _Table;
            //Report.DataMember = "Table";
            Report.CreateDocument();
            documentViewer1.DocumentSource = Report;
        }
    }
}
