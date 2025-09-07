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

namespace ProcessingWork.InputData.Option
{
    public partial class Frm_Option : Form
    {
        public Frm_Option()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            clsOptionDAO DAO = new clsOptionDAO();
            DataTable dt = DAO.GetAllData();
            BindDataToGrid(dt, sender);
        }

        private void BindDataToGrid(DataTable dt, object sender)
        {
            clsBase.BindDataToGrid(dt, this, GridControl1, GridView1);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                ImportData();
            }
            catch(Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private void ImportData()
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
            List<clsOption> listPart = new List<clsOption>();
            DataTable listExistedOption = GetListExistedOption();
            var listDept = GetListDept();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime now = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsOption obj = new clsOption();
                    obj.OptionID = dReader[nameof(clsOption.OptionID)].ToString();
                    if (string.IsNullOrEmpty(obj.OptionID))
                    {
                        continue;
                    }

                    var existed = listExistedOption.Select($@"OptionID = '{obj.OptionID}'").Any();
                    if (existed)
                    {
                        throw new ArgumentException($@"{obj.OptionID} đã tồn tại trong danh sách.");
                    }

                    obj.DeptCode = dReader[nameof(clsOption.DeptCode)].ToString();
                    if(!CorrectDeptCode(obj,listDept))
                    {
                        throw new ArgumentException("Mã bộ phận chưa tồn tại. Yêu cầu nhập thông tin bộ phận trước");
                    }

                    obj.Note = dReader[nameof(clsOption.Note)].ToString();
                    listPart.Add(obj);
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

            btnView_Click(null, null);
        }

        private DataTable GetListExistedOption()
        {
            clsOptionDAO DAO = new clsOptionDAO();
            return DAO.GetAllData();
        }

        private bool CorrectDeptCode(clsOption obj, DataTable listDept)
        {
            var correct = listDept.Select($@"{nameof(clsOption.DeptCode)} = '{obj.DeptCode}'").Any();
            return correct;
        }

        private DataTable GetListDept()
        {
            clsDepartmentDAO DAO = new clsDepartmentDAO();
            return DAO.GetAllData();
        }

        private bool InputDataToDatabase(List<clsOption> listPart)
        {
            clsOptionDAO DAO = new clsOptionDAO();
            return DAO.InputDataToDatabase(listPart);
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
                    clsOptionDAO DAO = new clsOptionDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        clsOption obj = GetValidateData(numberofRow, sender);
                        obj.Deleted = true;
                        DAO.Update(obj);
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
                clsOption obj = GetValidateData(e.RowHandle, sender);
                clsOptionDAO DAO = new clsOptionDAO();
                DAO.Update(obj);

                btnView_Click(sender, e);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private clsOption GetValidateData(int rowHandle, object sender)
        {
            GridView view = sender as GridView;
            clsOption obj = new clsOption();
            obj.ID = int.Parse(GridView1.GetRowCellValue(rowHandle, nameof(clsOption.ID)).ToString());
            obj.DeptCode = (string)GridView1.GetRowCellValue(rowHandle, nameof(clsOption.DeptCode));
            obj.OptionID = (string)GridView1.GetRowCellValue(rowHandle, nameof(clsOption.OptionID));
            obj.Note = (string)GridView1.GetRowCellValue(rowHandle, nameof(clsOption.Note));
            return obj;
        }
    }
}
