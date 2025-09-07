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

namespace ProcessingWork.InputData.OptionData
{
    public partial class Frm_OptionData : Form
    {
        public Frm_OptionData()
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
            List<clsOptionData> listPart = new List<clsOptionData>();
            List<clsImportError> error = new List<clsImportError>();
            var listOption = GetListOption();
            int line = 0;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsOptionData part = new clsOptionData();
                    part.PartID = dReader[nameof(clsOptionData.PartID)].ToString();
                    if (string.IsNullOrEmpty(part.PartID))
                    {
                        continue;
                    }

                    part.PartID = dReader[nameof(clsOptionData.PartID)].ToString();
                    part.MachineID = dReader[nameof(clsOptionData.MachineID)].ToString();
                    part.OptionID = dReader[nameof(clsOptionData.OptionID)].ToString();
                    if(!CorrectOption(part,listOption))
                    {
                        throw new ArgumentException($"Chưa tồn tại nguyên công {part.OptionID} trong danh sách nguyên công. Vui lòng nhập nguyên công ngày vào danh sách trước.");
                    }

                    part.LastOption = dReader[nameof(clsOptionData.LastOption)].ToString().Length > 0 ? true : false;
                    part.JigID = dReader[nameof(clsOptionData.JigID)].ToString();
                    int toolQty;
                    if(!int.TryParse(dReader[nameof(clsOptionData.ToolQty)].ToString(), out toolQty))
                    {
                        throw new ArgumentException($"{nameof(clsOptionData.ToolQty)} phải là kiểu số");
                    }

                    part.ToolQty = toolQty;
                    double proTime;
                    if (!double.TryParse(dReader[nameof(clsOptionData.ProTime)].ToString(), out proTime))
                    {
                        throw new ArgumentException($"{nameof(clsOptionData.ProTime)} phải là kiểu số");
                    }

                    part.ProTime = proTime;
                    double clampTime;
                    if (!double.TryParse(dReader[nameof(clsOptionData.ClampTime)].ToString(), out clampTime))
                    {
                        throw new ArgumentException($"{nameof(clsOptionData.ClampTime)} phải là kiểu số");
                    }

                    part.ClampTime = clampTime;
                    part.TechDate = dReader[nameof(clsOptionData.TechDate)].ToString().Trim().Length > 0 ? true : false;
                    part.UpdateDay = DateTime.Now;
                    part.StaffID = dReader[nameof(clsOptionData.StaffID)].ToString();
                    part.Note = dReader[nameof(clsOptionData.Note)].ToString();
                    part.AondNote = dReader[nameof(clsOptionData.AondNote)].ToString();
                    part.Memo = dReader[nameof(clsOptionData.Memo)].ToString();
                    part.CLUpdateday = DateTime.Now;
                    part.JigType = dReader[nameof(clsOptionData.JigType)].ToString();
                    part.Jig = dReader[nameof(clsOptionData.Jig)].ToString().Length > 0 ? true : false;
                    part.Tich = 0;
                    part.Doc = 0;
                    part.TimeTreo = 0;
                    part.TimeComplete = 0;
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

        private bool CorrectOption(clsOptionData obj, List<string> listOption)
        {
            var correct = listOption.Where(x => x.ToUpper() == obj.OptionID.ToUpper()).Any();
            return correct;
        }

        private List<string> GetListOption()
        {
            clsOptionDAO DAO = new clsOptionDAO();
            DataTable dt = DAO.GetAllData();
            List<string> result = new List<string>();
            foreach (DataRow r in dt.Rows)
            {
                string optionID = r["OptionID"].ToString().Trim();
                result.Add(optionID);
            }
            return result;
        }

        private bool InputDataToDatabase(List<clsOptionData> listPart)
        {
            clsOptionDataDAO DAO = new clsOptionDataDAO();
            return DAO.InputDataToDatabase(listPart);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            string part = cbPartID.SelectedValue.ToString().Trim();
            string sql = $"select * from [View_242_OptionData]";
            clsOptionDataDAO DAO = new clsOptionDataDAO();
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(part))
            {
                dt = DAO.LoadGridByStr(sql);
            }
            else
            {
                sql = $"select * from [View_242_OptionData] where PartID = N'{part}'";
                dt = DAO.LoadGridByStr(sql);
            }

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
                    clsOptionDataDAO DAO = new clsOptionDataDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        string id = GridView1.GetRowCellValue(numberofRow, "ID").ToString();
                        DAO.ExecuteStr($@"update [242_OptionData] set Deleted = 1 where ID = {id}");
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
                clsOptionData obj = GetValidateData(e, sender);
                obj.ID = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.ID)]).ToString());
                clsOptionDataDAO DAO = new clsOptionDataDAO();
                DAO.UpdateData(obj);

                btnView_Click(sender, e);
                clsBase.DisplayMessage("Đã nhập xong", this);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private clsOptionData GetValidateData(ValidateRowEventArgs e, object sender)
        {
            GridView view = sender as GridView;
            clsOptionData obj = new clsOptionData();
            var AondNote = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.AondNote)]).ToString();
            obj.AondNote = AondNote;
            var ClampTime = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.ClampTime)]).ToString();
            obj.ClampTime = double.Parse(ClampTime);
            var CLUpdateday = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.CLUpdateday)]).ToString();
            obj.CLUpdateday = DateTime.Parse(CLUpdateday);
            var Doc = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.Doc)]).ToString();
            obj.Doc = float.Parse(Doc);
            var InputDate = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.InputDate)]).ToString();
            obj.InputDate = DateTime.Parse(InputDate);
            var Jig = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.Jig)]).ToString();
            obj.Jig = bool.Parse(Jig);
            obj.JigID = (string)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.JigID)]).ToString();
            obj.JigType = (string)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.JigType)]).ToString();
            var LastOption = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.LastOption)]).ToString();
            obj.LastOption = bool.Parse(LastOption);
            obj.MachineID = (string)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.MachineID)]).ToString();
            obj.Memo = (string)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.Memo)]).ToString();
            obj.Note = (string)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.Note)]).ToString();
            obj.OptionID = (string)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.OptionID)]).ToString();
            obj.PartID = (string)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.PartID)]).ToString();
            var ProTime = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.ProTime)]).ToString();
            obj.ProTime = double.Parse(ProTime);
            obj.StaffID = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.StaffID)]).ToString();
            var TechDate = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.TechDate)]).ToString();
            obj.TechDate = bool.Parse(TechDate);
            var Tich = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.Tich)]).ToString();
            obj.Tich = float.Parse(Tich);
            var TimeComplete = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.TimeComplete)]).ToString();
            obj.TimeComplete = float.Parse(TimeComplete);
            var TimeTreo = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.TimeTreo)]).ToString();
            obj.TimeTreo = float.Parse(TimeTreo);
            var ToolQty = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.ToolQty)]).ToString();
            obj.ToolQty = int.Parse(ToolQty);
            var UpdateDay = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsOptionData.UpdateDay)]).ToString();
            obj.UpdateDay = DateTime.Parse(UpdateDay);
            return obj;
        }

        private void Frm_OptionData_Load(object sender, EventArgs e)
        {
            clsOptionDataDAO DAO = new clsOptionDataDAO();
            DataTable dt = DAO.LoadGridByStr("select PartNo from [View_242_Part] group by PartNo");
            DataRow r = dt.NewRow();
            r["PartNo"] = string.Empty;
            dt.Rows.InsertAt(r, 0);
            cbPartID.DataSource = dt;
            cbPartID.DisplayMember = "PartNo";
            cbPartID.ValueMember = "PartNo";
        }
    }
}
