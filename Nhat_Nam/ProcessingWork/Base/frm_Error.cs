using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessingWork.Base
{
    public partial class frm_Error : Form
    {
        DataTable dt = new DataTable();
        public List<clsImportError> Error = new List<clsImportError> { };

        public frm_Error()
        {
            InitializeComponent();
            dt.Columns.Add("Dòng");
            dt.Columns.Add("Lỗi");
        }

        private void frm_Error_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Error.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["Dòng"] = Error[i].Line.ToString();
                r["Lỗi"] =  Error[i].Des.ToString();

                dt.Rows.Add(r);
            }
            dataGridView1.DataSource = dt;
        }
    }
}
