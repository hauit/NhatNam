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

namespace ProcessingWork.InputData.PartData
{
    public partial class Frm_PartData : Form
    {
        public Frm_PartData()
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
            List<clsPartData> listPart = new List<clsPartData>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsPartData part = new clsPartData();
                    //part.ID = dReader[nameof(clsPartData.ID)].ToString();
                    part.PartID = dReader[nameof(clsPartData.PartID)].ToString();
                    if (string.IsNullOrEmpty(part.PartID))
                    {
                        continue;
                    }
                    
                    part.InputDate = DateTime.Parse(dReader[nameof(clsPartData.InputDate)].ToString());
                    part.MaterialID = dReader[nameof(clsPartData.MaterialID)].ToString();
                    part.MaterialType = dReader[nameof(clsPartData.MaterialType)].ToString();
                    part.Workpiecesize = dReader[nameof(clsPartData.Workpiecesize)].ToString();
                    part.shape = dReader[nameof(clsPartData.shape)].ToString();
                    float thickness;
                    if (!float.TryParse(dReader[nameof(clsPartData.Thickness)].ToString(), out thickness))
                    {
                        throw new ArgumentException("Lenght phải là kiểu số.");
                    }

                    part.Thickness = thickness;
                    part.width = dReader[nameof(clsPartData.width)].ToString();
                    float lenght;
                    if (!float.TryParse(dReader[nameof(clsPartData.lenght)].ToString(), out lenght))
                    {
                        throw new ArgumentException("Lenght phải là kiểu số.");
                    }

                    part.lenght = lenght;
                    part.Cut = dReader[nameof(clsPartData.Cut)].ToString().Length == 0 ? false : true;
                    part.RawMachine = dReader[nameof(clsPartData.RawMachine)].ToString().Length == 0 ? false : true;
                    part.HandFinish = dReader[nameof(clsPartData.HandFinish)].ToString().Length == 0 ? false : true;
                    part.HairLine = dReader[nameof(clsPartData.HairLine)].ToString().Length == 0 ? false : true;
                    part.WAnod = dReader[nameof(clsPartData.WAnod)].ToString().Length == 0 ? false : true;
                    part.BAnod = dReader[nameof(clsPartData.BAnod)].ToString().Length == 0 ? false : true;
                    part.Blast30 = dReader[nameof(clsPartData.Blast30)].ToString().Length == 0 ? false : true;
                    part.Blast60 = dReader[nameof(clsPartData.Blast60)].ToString().Length == 0 ? false : true;
                    part.Seal = dReader[nameof(clsPartData.Seal)].ToString().Length == 0 ? false : true;
                    part.Migaki = dReader[nameof(clsPartData.Migaki)].ToString().Length == 0 ? false : true;
                    part.Bafu = dReader[nameof(clsPartData.Bafu)].ToString().Length == 0 ? false : true;
                    part.Cleanwave = dReader[nameof(clsPartData.Cleanwave)].ToString().Length == 0 ? false : true;
                    part.VacPac = dReader[nameof(clsPartData.VacPac)].ToString().Length == 0 ? false : true;
                    part.Helisert = dReader[nameof(clsPartData.Helisert)].ToString().Length == 0 ? false : true;
                    part.SerialNo = dReader[nameof(clsPartData.SerialNo)].ToString().Length == 0 ? false : true;
                    part.PalCoat = dReader[nameof(clsPartData.PalCoat)].ToString().Length == 0 ? false : true;
                    part.Paint = dReader[nameof(clsPartData.Paint)].ToString().Length == 0 ? false : true;
                    part.BBD = dReader[nameof(clsPartData.BBD)].ToString().Length == 0 ? false : true;
                    part.Otherpro = dReader[nameof(clsPartData.Otherpro)].ToString();
                    decimal price;
                    if (!decimal.TryParse(dReader[nameof(clsPartData.Price)].ToString(), out price))
                    {
                        throw new ArgumentException("Price phải là kiểu số.");
                    }

                    part.Price = price;
                    part.Memo = dReader[nameof(clsPartData.Memo)].ToString();
                    part.Note = dReader[nameof(clsPartData.Note)].ToString();
                    part.Caciras = dReader[nameof(clsPartData.Caciras)].ToString().Length == 0 ? false : true;
                    part.Inside = dReader[nameof(clsPartData.Inside)].ToString().Length == 0 ? false : true;
                    part.MaBong = dReader[nameof(clsPartData.MaBong)].ToString().Length == 0 ? false : true;
                    part.InLuoi = dReader[nameof(clsPartData.InLuoi)].ToString().Length == 0 ? false : true;
                    part.Heru = dReader[nameof(clsPartData.Heru)].ToString().Length == 0 ? false : true;
                    part.Niken = dReader[nameof(clsPartData.Niken)].ToString().Length == 0 ? false : true;
                    part.MaiBongDP = dReader[nameof(clsPartData.MaiBongDP)].ToString().Length == 0 ? false : true;
                    listPart.Add(part);
                    line++;
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

            if (error.Count > 0)
            {
                frm_Error frm = new frm_Error();
                frm.Error = error;
                clsBase.DisplayForm(frm, this);
            }

            btnView_Click(null, null);
        }

        private bool InputDataToDatabase(List<clsPartData> listPart)
        {
            clsPartDataDAO DAO = new clsPartDataDAO();
            return DAO.InputDataToDatabase(listPart);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            var part = cbPartID.Text;
            string sql = $"select * from [242_PartData] where  isNull(Deleted,0) = 0 ";
            clsPartDataDAO DAO = new clsPartDataDAO();
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(part))
            {
                dt = DAO.LoadGridByStr(sql);
            }
            else
            {
                sql = $"select * from [242_PartData] where  isNull(Deleted,0) = 0 and  PartID = N'{part}'";
                dt = DAO.LoadGridByStr(sql);
            }

            BindDataToGrid(dt,sender);
        }

        private void BindDataToGrid(DataTable dt, object sender)
        {
            clsBase.BindDataToGrid(dt, this,GridControl1,GridView1);
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
                    clsPartDataDAO DAO = new clsPartDataDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        string id = GridView1.GetRowCellValue(numberofRow, "ID").ToString();
                        DAO.ExecuteStr($@"update [242_PartData]  set Deleted = 1 where ID = {id}");
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
                clsPartData obj = GetValidateData(e, sender);
                obj.ID = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["ID"]).ToString());
                clsPartDataDAO DAO = new clsPartDataDAO();
                DAO.UpdateData(obj);

                btnView_Click(sender, e);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private clsPartData GetValidateData(ValidateRowEventArgs e, object sender)
        {
            GridView view = sender as GridView;
            clsPartData obj = new clsPartData();
            obj.InputDate = (DateTime)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.InputDate)]);
            obj.PartID = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.PartID)]).ToString();
            obj.MaterialID = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.MaterialID)]).ToString();
            obj.MaterialType = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.MaterialType)]).ToString();
            obj.Workpiecesize = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Workpiecesize)]).ToString();
            obj.shape = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.shape)]).ToString();
            var thich = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Thickness)]).ToString();
            obj.Thickness = float.Parse(view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Thickness)]).ToString());
            obj.width = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.width)]).ToString();
            obj.lenght = float.Parse(view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.lenght)]).ToString());
            obj.Cut = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Cut)]);
            obj.RawMachine = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.RawMachine)]);
            obj.HandFinish = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.HandFinish)]);
            obj.HairLine = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.HairLine)]);
            obj.WAnod = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.WAnod)]);
            obj.BAnod = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.BAnod)]);
            obj.Blast30 = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Blast30)]);
            obj.Blast60 = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Blast60)]);
            obj.Seal = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Seal)]);
            obj.Migaki = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Migaki)]);
            obj.Bafu = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Bafu)]);
            obj.Cleanwave = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Cleanwave)]);
            obj.VacPac = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.VacPac)]);
            obj.Helisert = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Helisert)]);
            obj.SerialNo = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.SerialNo)]);
            obj.PalCoat = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.PalCoat)]);
            obj.Paint = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Paint)]);
            obj.BBD = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.BBD)]);
            obj.Otherpro = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Otherpro)]).ToString();
            obj.Price = decimal.Parse(view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Price)]).ToString());
            obj.Memo = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Memo)]).ToString();
            obj.Note = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Note)]).ToString();
            obj.Caciras = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Caciras)]);
            obj.Inside = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Inside)]);
            obj.MaBong = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.MaBong)]);
            obj.InLuoi = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.InLuoi)]);
            obj.Heru = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Heru)]);
            obj.Niken = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.Niken)]);
            obj.MaiBongDP = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsPartData.MaiBongDP)]);
            return obj;
        }

        private void Frm_PartData_Load(object sender, EventArgs e)
        {
            clsPartDataDAO DAO = new clsPartDataDAO();
            DataTable dt = DAO.LoadGridByStr("select PartNo from [242_Part] where  isNull(Deleted,0) = 0  group by PartNo");
            DataRow r = dt.NewRow();
            r["PartNo"] = string.Empty;
            dt.Rows.InsertAt(r, 0);
            cbPartID.DataSource = dt;
            cbPartID.DisplayMember = "PartNo";
            cbPartID.ValueMember = "PartNo";
        }
    }
}
