using Base.Base;
using DevExpress.XtraGrid.Views.Base;
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

namespace ProcessingWork.InputData.Material
{
    public partial class Frm_Material : Form
    {
        public Frm_Material()
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
            List<clsMaterial> listPart = new List<clsMaterial>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsMaterial part = new clsMaterial();
                    part.DateModified = inputDate;
                    part.StaffID = ClsSession.staffID;
                    part.MaterialID = dReader[nameof(clsMaterial.MaterialID)].ToString();
                    if (string.IsNullOrEmpty(part.MaterialID))
                    {
                        continue;
                    }

                    float density;
                    if (!float.TryParse(dReader[nameof(clsMaterial.Density)].ToString(), out density))
                    {
                        throw new ArgumentException("Density phải là kiểu số.");
                    }

                    part.Density = density;
                    decimal unitPrice;
                    if(!decimal.TryParse(dReader[nameof(clsMaterial.UnitPrice)].ToString().Trim(),out unitPrice))
                    {
                        throw new ArgumentException("UnitPrice phải là kiểu số.");
                    }

                    part.UnitPrice = unitPrice;
                    part.Type = dReader[nameof(clsMaterial.Type)].ToString();
                    
                    listPart.Add(part);
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

        private bool InputDataToDatabase(List<clsMaterial> listPart)
        {
            clsMaterialDAO DAO = new clsMaterialDAO();
            return DAO.InputDataToDatabase(listPart);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            string sql = $"select * from [242_Material] where  isNull(Deleted,0) = 0 and isNull(Deleted,0) = 0";
            clsMaterialDAO DAO = new clsMaterialDAO();
            DataTable dt = DAO.LoadGridByStr(sql);
            BindDataToGrid(dt, sender);
        }

        private void BindDataToGrid(DataTable dt, object sender)
        {
            clsBase.BindDataToGrid(dt, this, GridControl1, GridView1);
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
                    clsMaterialDAO DAO = new clsMaterialDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        string id = GridView1.GetRowCellValue(numberofRow, "ID").ToString();
                        DAO.ExecuteStr($@"update [242_Material]  set Deleted = 1 where ID = {id}");
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
                clsMaterial obj = GetValidateData(e, sender);
                obj.ID = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["ID"]).ToString());
                clsMaterialDAO DAO = new clsMaterialDAO();
                DAO.UpdateMaterial(obj);

                btnView_Click(sender, e);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private clsMaterial GetValidateData(ValidateRowEventArgs e, object sender)
        {
            GridView view = sender as GridView;
            clsMaterial obj = new clsMaterial();
            obj.DateModified = DateTime.Now;
            obj.MaterialID = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsMaterial.MaterialID)]).ToString();
            obj.Density = float.Parse(view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsMaterial.Density)]).ToString());
            obj.UnitPrice = decimal.Parse(view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsMaterial.UnitPrice)]).ToString());
            obj.Type = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsMaterial.Type)]).ToString();
            obj.StaffID = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsMaterial.StaffID)]).ToString();
            return obj;
        }
    }
}
