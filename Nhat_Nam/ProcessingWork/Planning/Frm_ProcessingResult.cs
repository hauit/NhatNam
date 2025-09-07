using ProcessingWork.Base;
using ProcessingWork.DAO.PlanningDAO;
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
    public partial class Frm_ProcessingResult : Form
    {
        public Frm_ProcessingResult()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (GridView1.Columns.Count > 0)
            {
                GridView1.Columns.Clear();
            }

            DataTable dt = new DataTable();

            if (cbShift.Text.Trim().Length > 0)
            {
                dt = GetDataDayByDayOneShift();
            }
            else
            {
                dt = GetDataDayByDayAllShift();
            }

            GridControl1.DataSource = dt;
            GridView1.BestFitColumns();
        }

        private DataTable GetDataDayByDayOneShift()
        {
            clsPlanningDAO DAO = new clsPlanningDAO();
            DataTable dt = new DataTable();
            dt = DAO.GetProcessingResult(cbShift.Text.ToString().Trim(), dateFromDate.Value, dateToDate.Value);
            return dt;
        }

        private DataTable GetDataDayByDayAllShift()
        {
            clsPlanningDAO DAO = new clsPlanningDAO();
            DataTable dt = new DataTable();
            dt = DAO.GetProcessingResult(dateFromDate.Value, dateToDate.Value);
            return dt;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            clsBase export = new clsBase();
            export.ExportToExcel(GridView1, 1, true);
        }
    }
}
