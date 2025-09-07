using DevExpress.XtraGrid.Views.Grid;
using ProcessingWork.Base;
using ProcessingWork.DAO.DatabaseDAO;
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
using DevExpress.XtraGrid.Views.Base;

namespace ProcessingWork.InputData.Part
{
    public partial class Frm_PartList : Form
    {
        public Frm_PartList()
        {
            InitializeComponent();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            clsBase export = new clsBase();
            export.ExportToExcel(GridView1, 1, true);
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
            DataTable dtExisted = GetAlExistedPart();
            List<clsPart> listPart = new List<clsPart>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsPart part = new clsPart();
                    part.PartNo = dReader[nameof(clsPart.PartNo)].ToString();
                    if (string.IsNullOrEmpty(part.PartNo))
                    {
                        continue;
                    }

                    if (CheckExistedOrder(dtExisted, part.PartNo))
                    {
                        throw new ArgumentException($@"Tên chi tiết {part.PartNo} đã tồn tại trong dữ liệu");
                    }

                    part.CustomerID = dReader[nameof(clsPart.CustomerID)].ToString();
                    part.PartName = dReader[nameof(clsPart.PartName)].ToString();
                    part.SupplierID = dReader[nameof(clsPart.SupplierID)].ToString();
                    int upQty;
                    if (!int.TryParse(dReader[nameof(clsPart.UpQty)].ToString(), out upQty))
                    {
                        throw new ArgumentException("UpQty phải là kiểu số. Vui lòng kiểm tra lại dữ liệu.");
                    }

                    part.UpQty = upQty;
                    listPart.Add(part);
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
                }
            }

            if(listPart.Count > 0 )
            {
                if(!InputDataToDatabase(listPart))
                {
                    clsBase.DisplayMessage("Không nhập được dữ liệu vào Database", this);
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

        private bool CheckExistedOrder(DataTable dtExisted, string partNo)
        {
            var a = dtExisted.Select($"PartNo = '{partNo}'");
            if (a == null || a.Length == 0)
            {
                return false;
            }

            return true;
        }

        private bool InputDataToDatabase(List<clsPart> listPart)
        {
            clsPartDAO DAO = new clsPartDAO();
            return DAO.InputDataToDatabase(listPart);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            DataTable dt = GetAlExistedPart(); 
            GridControl1.DataSource = dt;
            GridView1.BestFitColumns();
        }

        private DataTable GetAlExistedPart()
        {
            clsPartDAO DAO = new clsPartDAO();
            return DAO.GetAlExistedPart();
        }

        private void mnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có thực sự muốn xóa các dòng đã chọn!", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }

                if (GridView1.SelectedRowsCount > 0)
                {
                    var row = new DataRow[GridView1.SelectedRowsCount];
                    var frequencyId = new string[GridView1.SelectedRowsCount];
                    int numberofRow;
                    clsPartDAO DAO = new clsPartDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        string id = GridView1.GetRowCellValue(numberofRow, "ID").ToString();
                        clsPart obj = GetValidateData(numberofRow, sender);
                        obj.Deleted = true;
                        DAO.UpdateData(obj);
                    }
                    btnView_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private void GridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            try
            {
                clsPart obj = GetValidateData(e.RowHandle, sender);
                clsPartDAO DAO = new clsPartDAO();
                DAO.UpdateData(obj);

                btnView_Click(sender, e);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private clsPart GetValidateData(int rowHandle, object sender)
        {
            GridView view = sender as GridView;
            clsPart obj = new clsPart();
            string id = GridView1.GetRowCellValue(rowHandle, "ID").ToString();
            obj.ID = int.Parse(GridView1.GetRowCellValue(rowHandle, $"{nameof(clsPart.ID)}").ToString());
            obj.PartNo = GridView1.GetRowCellValue(rowHandle, nameof(clsPart.PartNo)).ToString();
            obj.PartName = GridView1.GetRowCellValue(rowHandle, nameof(clsPart.PartName)).ToString();
            obj.CustomerID = GridView1.GetRowCellValue(rowHandle, nameof(clsPart.CustomerID)).ToString();
            obj.SupplierID = GridView1.GetRowCellValue(rowHandle, nameof(clsPart.SupplierID)).ToString();
            obj.UpQty = int.Parse(GridView1.GetRowCellValue(rowHandle, nameof(clsPart.UpQty)).ToString());
            obj.GiaThanh = int.Parse(GridView1.GetRowCellValue(rowHandle, nameof(clsPart.GiaThanh)).ToString());
            return obj;
        }

        private void button1_Click(object sender, EventArgs e)
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
            DataTable dtExisted = GetAlExistedPart();
            List<clsPart> listPart = new List<clsPart>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    string sql = $@"insert into [242_Work] ([WorkID]
                      ,[WorkName]
                      ,[DirectWork]
                      ,[OptionDataWork]
                      ,[MassWork]
                      ,[InspecWork]
                      ,[StopWork]
                      ,[MachWork]
                      ,[MachInDWork]
                      ,[MachineRun]
                      ,[GroupID]
                      ,[DeptCode]
                      ,[SortingNumber]
                      ,[DeptOption]) values (
                       N'{dReader["WorkID"].ToString()}'
                      ,N'{dReader["WorkName"].ToString()}'
                      ,N'{dReader["DirectWork"].ToString()}'
                      ,N'{dReader["OptionDataWork"].ToString()}'
                      ,N'{dReader["MassWork"].ToString()}'
                      ,N'{dReader["InspecWork"].ToString()}'
                      ,N'{dReader["StopWork"].ToString()}'
                      ,N'{dReader["MachWork"].ToString()}'
                      ,N'{dReader["MachInDWork"].ToString()}'
                      ,N'{dReader["MachineRun"].ToString()}'
                      ,N'{dReader["GroupID"].ToString()}'
                      ,N'{dReader["DeptCode"].ToString().Replace(';','+')}'
                      ,N'{dReader["SortingNumber"].ToString()}'
                      ,N'{dReader["DeptOption"].ToString()}')";
                    clsPartDAO DAO = new clsPartDAO();
                    DAO.ExecuteStr(sql);
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
                }
            }

            if (listPart.Count > 0)
            {
                if (!InputDataToDatabase(listPart))
                {
                    clsBase.DisplayMessage("Không nhập được dữ liệu vào Database", this);
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
    }
}
