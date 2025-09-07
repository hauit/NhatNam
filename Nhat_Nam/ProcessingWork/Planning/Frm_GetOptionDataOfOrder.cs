using ProcessingWork.Base;
using ProcessingWork.DAO.DatabaseDAO;
using ProcessingWork.DAO.PlanningDAO;
using ProcessingWork.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessingWork.Planning
{
    public partial class Frm_GetOptionDataOfOrder : Form
    {
        private bool viewImportData = false;
        public Frm_GetOptionDataOfOrder()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            viewImportData = false;
            DAO.PlanningDAO.clsPlanningDAO DAO = new DAO.PlanningDAO.clsPlanningDAO();
            DataTable dt = DAO.GetOptionDataForPlann();
            BindDataToGrid(dt);
            ExportDataCannotgetOptiondata();
        }

        private void ExportDataCannotgetOptiondata()
        {
            string sql = $@"
            with T1 as (
            select T1.MONumber,T1.Note,T2.PartID as PartID,T3.PartID as PartID1,T4.PartID as PartID2,T5.MONo from [242_P_NhapOrderlapKHGC] as T1 left join [View_242_BusOder] as T2 on T1.MONumber = T2.MONo
            left join [242_PartData] as T3 on T2.PartID = T3.PartID
            left join [View_242_OptionData] as T4 on T2.PartID = T4.PartID
            left join [View_242_MOOption] as T5 on T1.MONumber = T5.MONo
            group by T1.MONumber,T1.Note,T2.PartID,T3.PartID,T4.PartID,T5.MONo
            ),
            T2 as (
            select MONumber,Note, PartID,PartID1,PartID2,MONo,
	            case when isnull(PartID,'') = '' then N'Order chưa có trong BOderList' 
		            when isnull(PartID1,'') = '' then N'Tên chi tiết chưa có trong PartList'
		            when isnull(PartID2,'') = '' then N'Chi tiết chưa đăng kí nguyên công vào Optiondata'
		            when isnull(PartID2,'') = '' then N'Chi tiết chưa có trong MOOption(Chưa start)'
	            end as [Des]
            from T1 
            )

            select MONumber, STRING_AGG([Des],',') as [Des] from T2 where isnull([Des],'') <> '' group by MONumber";
            DAO.PlanningDAO.clsPlanningDAO DAO = new DAO.PlanningDAO.clsPlanningDAO();
            DataTable dt = DAO.LoadGridByStr(sql);
            if(dt.Rows.Count == 0)
            {
                return;
            }

            var error = new List<clsImportError>();
            int i = 0;
            foreach(DataRow r in dt.Rows)
            {
                i++;
                string order = r["MONumber"].ToString();
                string des = r["Des"].ToString();
                clsImportError obj = new clsImportError(i,des,order);
                error.Add(obj);
            }

            clsBase.DisplayMessage("Dữ liệu có sai sót nên 1 số order không tách được nguyên công. Vui lòng xem form chi tiết và sửa dữ liệu.", this);
            frm_Error frm = new frm_Error();
            frm.Error = error;
            frm.Show();
        }

        private void BindDataToGrid(DataTable dt)
        {
            clsBase.BindDataToGrid(dt, this, GridControl1, GridView1);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OleDbDataReader dReader;
            FileDialog importFile = new OpenFileDialog();
            importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
            if (importFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            OleDbConnection excelConnection = null;
            importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
            string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                    importFile.FileName +
                                                    ";Extended Properties=Excel 12.0;Persist Security Info=False";
            excelConnection = new OleDbConnection(excelConnectionString);
            excelConnection.Open();
            OleDbCommand cmd =
                new OleDbCommand("select * from [Sheet1$]",
                                    excelConnection);
            dReader = cmd.ExecuteReader();
            List<clsP_NhapOrderlapKHGC> listPart = new List<clsP_NhapOrderlapKHGC>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsP_NhapOrderlapKHGC part = new clsP_NhapOrderlapKHGC();
                    part.MONumber = dReader[nameof(clsP_NhapOrderlapKHGC.MONumber)].ToString();
                    if (string.IsNullOrEmpty(part.MONumber))
                    {
                        continue;
                    }

                    DateTime date;
                    if(!DateTime.TryParse(dReader[nameof(clsP_NhapOrderlapKHGC.Date)].ToString(), out date))
                    {
                        throw new ArgumentException("Ngày không đúng định dạng.");
                    }

                    part.Date = date;
                    part.Note = dReader[nameof(clsP_NhapOrderlapKHGC.Note)].ToString();
                    listPart.Add(part);
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
                }
            }

            if (listPart.Count > 0)
            {
                if (!InputOrderNeedGetOptionData(listPart))
                {
                    clsBase.DisplayMessage("Không nhập được dữ liệu vào Database. Vui lòng kiểm tra lại, có thể order bị trùng", this);
                }
                else
                {
                    clsBase.DisplayMessage("Đã nhập xong", this);
                }
            }

            clsBase.DisconnectToExcelFile(excelConnection);

            if (error.Count > 0)
            {
                frm_Error frm = new frm_Error();
                frm.Error = error;
                clsBase.DisplayForm(frm, this);
            }

            btnView_Click(sender, e);
        }

        private bool InputOrderNeedGetOptionData(List<clsP_NhapOrderlapKHGC> listPart)
        {
            try
            {
                clsP_NhapOrderlapKHGCDAO DAO = new clsP_NhapOrderlapKHGCDAO();
                DAO.DeleteAllExistedData();
                return DAO.InputDataToDatabase(listPart);
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private void btnExPort_Click(object sender, EventArgs e)
        {
            clsBase export = new clsBase();
            export.ExportToExcel(GridView1, 1, true);
        }

        private void btnViewImported_Click(object sender, EventArgs e)
        {
            viewImportData = true;
        }
    }
}
