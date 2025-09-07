using ProcessingWork.Base;
using ProcessingWork.DAO.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessingWork.Report
{
    public partial class Frm_Report : Form
    {
        List<string> listReport = new List<string>() { "Lấy tổng WTS nhân viên(Chưa duyệt)",
                    "Lấy tổng WTS nhân viên(Đã duyệt)",
                    "Lấy tổng WTS máy(Chưa duyệt)",
                    "Lấy tổng WTS Máy(Đã duyệt)",
                    "Lấy tổng WTS Máy",
                    "Lấy tổng WTS nhân viên"
            };
        public Frm_Report()
        {
            InitializeComponent();
            LoadReport();
        }

        private void LoadReport()
        {
            ClsReportDAO DAO = new ClsReportDAO();
            DataTable dt = DAO.GetReportList();
            for(int i = listReport.Count - 1; i >= 0; i--)
            {
                DataRow r = dt.NewRow();
                r["QueryStr"] = $@"{i+1}";
                r["Caption"] = listReport[i];
                dt.Rows.InsertAt(r, 0);
            }

            cbReport.DataSource = dt;
            cbReport.DisplayMember = "Caption";
            cbReport.ValueMember = "QueryStr";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            clsBase export = new clsBase();
            export.ExportToExcel(GridView1, 1, true);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (GridView1.Columns.Count > 0)  
            {
                GridView1.Columns.Clear();
            }
            GridView1.ViewCaption = cbReport.Text;
            DataTable dt = new DataTable();
            switch (cbReport.SelectedValue.ToString())
            {
                case "1":
                    dt = GetInputedWTSByStaff(0);
                    break;
                case "2":
                    dt = GetInputedWTSByStaff(1);
                    break;
                case "3":
                    dt = GetInputedWTSByMachine(0);
                    break;
                case "4":
                    dt = GetInputedWTSByMachine(1);
                    break;
                case "5":
                    dt = GetInputedWTSByMachine(2);
                    break;
                case "6":
                    dt = GetInputedWTSByStaff(2);
                    break;
                default:
                    dt = GetData(cbReport.SelectedValue.ToString());
                    break;
            }

            BindDataToGrid(dt, sender);
        }

        private DataTable GetData(string query)
        {
            ClsReportDAO DAO = new ClsReportDAO();
            return DAO.GetDataForReport(dateFrom.Value.Date,dateTo.Value.Date,query);
        }

        private DataTable GetInputedWTSByMachine(int confirmed)
        {
            string sql = "select MachineID";
            string fordate = "(";
            var a = dateTo.Value - dateFrom.Value;
            for (int i = 0; i <= a.Days; i++)
            {
                var coldate = dateFrom.Value.AddDays(i);
                sql += ",sum(case when [" + coldate.ToString("dd") + "] IS NULL then 0 else [" + coldate.ToString("dd") + "] end) as [" + coldate.ToString("dd") + "]";
                fordate += "[" + coldate.ToString("dd") + "]";
                if (i < a.Days)
                {
                    fordate += ",";
                }
            }
            fordate += ")) as x";
            sql += " from ( select *,day(date) as [day] from [View_242_WTS] where convert(date,date) between convert(date,'" + dateFrom.Value.ToString("yyyyMMdd") + "') and convert(date,'" + dateTo.Value.ToString("yyyyMMdd") + "') ) as T2";
            sql = sql + $@" pivot(sum(time) for [day] in {fordate} ";
            if (confirmed <= 1)
            {
                sql += $@" where isnull(MachineID,'') <> '' and isnull(Status,0) = {(confirmed == 1 ? 1 : 0)}";
            }

            sql += $@" group by MachineID order by MachineID";
            ClsReportDAO DAO = new ClsReportDAO();
            return DAO.LoadGridByStr(sql);
        }

        private DataTable GetInputedWTSByStaff(int confirmed)
        {
            string sql = "select StaffID";
            string fordate = "(";
            var a = dateTo.Value - dateFrom.Value;
            for (int i = 0; i <= a.Days; i++)
            {
                var coldate = dateFrom.Value.AddDays(i);
                sql += ",sum(case when [" + coldate.ToString("dd") + "] IS NULL then 0 else [" + coldate.ToString("dd") + "] end) as [" + coldate.ToString("dd") + "]";
                fordate += "[" + coldate.ToString("dd") + "]";
                if (i < a.Days)
                {
                    fordate += ",";
                }
            }
            fordate += ")) as x";
            sql += " from ( select *,day(date) as [day] from [View_242_WTS] where convert(date,date) between convert(date,'" + dateFrom.Value.ToString("yyyyMMdd") + "') and convert(date,'" + dateTo.Value.ToString("yyyyMMdd") + "') ) as T2";
            sql = sql + $@" pivot(sum(time) for [day] in {fordate} ";
            if(confirmed <= 1)
            {
                sql += $@"where isnull(Status,0) = {(confirmed == 1 ? 1 : 0)} ";
            }

            sql += $@" group by StaffID order by StaffID";
            ClsReportDAO DAO = new ClsReportDAO();
            return DAO.LoadGridByStr(sql);
        }

        private void BindDataToGrid(DataTable dt, object sender)
        {
            clsBase.BindDataToGrid(dt, this, GridControl1, GridView1);
        }
    }
}
