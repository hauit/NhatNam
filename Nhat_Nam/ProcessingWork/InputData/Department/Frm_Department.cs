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

namespace ProcessingWork.InputData.Department
{
    public partial class Frm_Department : Form
    {
        public Frm_Department()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            clsDepartmentDAO DAO = new clsDepartmentDAO();
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
            catch (Exception ex)
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
            List<clsDepartment> listPart = new List<clsDepartment>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime now = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsDepartment obj = new clsDepartment();
                    obj.DeptCode = dReader[nameof(clsDepartment.DeptCode)].ToString();
                    if (string.IsNullOrEmpty(obj.DeptCode))
                    {
                        continue;
                    }

                    obj.DeptName = dReader[nameof(clsDepartment.DeptName)].ToString();
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

        private bool InputDataToDatabase(List<clsDepartment> listPart)
        {
            clsDepartmentDAO DAO = new clsDepartmentDAO();
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
                    clsDepartmentDAO DAO = new clsDepartmentDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        clsDepartment obj = GetValidateData(numberofRow, sender);
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
        
        private clsDepartment GetValidateData(int rowHandle, object sender)
        {
            clsDepartment obj = new clsDepartment();
            obj.ID = int.Parse(GridView1.GetRowCellValue(rowHandle, nameof(clsDepartment.ID)).ToString());
            obj.DeptCode = GridView1.GetRowCellValue(rowHandle, nameof(clsDepartment.DeptCode)).ToString();
            obj.DeptName = GridView1.GetRowCellValue(rowHandle, nameof(clsDepartment.DeptName)).ToString();
            return obj;
        }

        private void GridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                clsDepartment obj = GetValidateData(e.RowHandle, sender);
                clsDepartmentDAO DAO = new clsDepartmentDAO();
                DAO.Update(obj);

                btnView_Click(sender, e);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }
    }
}
