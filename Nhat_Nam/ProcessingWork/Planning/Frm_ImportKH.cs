using ProcessingWork.Base;
using ProcessingWork.DAO.Database;
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

namespace ProcessingWork.Planning
{
    public partial class Frm_ImportKH : Form
    {
        public Frm_ImportKH()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            string sql = $@"select * from [242_MachinePlanning_view] where convert(date,Date) between '{dateFromDate.Value.ToString("yyyyMMdd")}' and '{dateToDate.Value.ToString("yyyyMMdd")}' ";
            string dept = cbDept.Text.Trim();
            if (!string.IsNullOrEmpty(dept))
            {
                sql += $@" and dept = '{dept}'";
            }

            string shift = cbShift.Text.Trim();
            if (!string.IsNullOrEmpty(shift))
            {
                sql += $@" and shift = '{shift}'";
            }

            clsPlanningDAO DAO = new clsPlanningDAO();
            DataTable dt = DAO.LoadGridByStr(sql);
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
                string dept = cbDept.Text.Trim();
                //if (string.IsNullOrEmpty(dept))
                //{
                //    throw new ArgumentException("Chưa chọn bộ phận");
                //}
                ImportFactory(dept);
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
            List<clsPlanning> listPart = new List<clsPlanning>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime now = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsPlanning obj = new clsPlanning();
                    obj.MONo = dReader[nameof(clsPlanning.MONo)].ToString();
                    if (string.IsNullOrEmpty(obj.MONo))
                    {
                        continue;
                    }

                    DateTime date;
                    string K = string.Empty;
                    if (!DateTime.TryParse(dReader[nameof(clsPlanning.Date)].ToString(), out date))
                    {
                        throw new ArgumentException("Ngay khong dung dinh dang");
                    }
                    if (dReader["Thực xếp"].ToString().Trim() == "1")
                    {
                        K = "K" + dReader["Thu tu GC"].ToString().Trim();
                    }
                    else if (dReader["Thực xếp"].ToString().Trim() == "1CT")
                    {
                        K = "C" + dReader["Thu tu GC"].ToString().Trim();
                    }
                    else if (dReader["Thực xếp"].ToString().Trim() == "2")
                    {
                        K = "B" + dReader["Thu tu GC"].ToString().Trim();
                    }
                    else if (dReader["Thực xếp"].ToString().Trim() == "5")
                    {
                        K = "L" + dReader["Thu tu GC"].ToString().Trim();
                    }

                    obj.Date = date;
                    obj.Dept = string.Empty;
                    obj.K = K;
                    obj.MachineID = dReader[nameof(clsPlanning.MachineID)].ToString();
                    obj.Note = dReader[nameof(clsPlanning.Note)].ToString();
                    obj.OptionID = dReader[nameof(clsPlanning.OptionID)].ToString();
                    obj.Shift = dReader[nameof(clsPlanning.Shift)].ToString();
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

        /// <summary>
        /// import work plan for Factory
        /// </summary>
        /// <param name="importFile"></param>
        private void ImportFactory(string dept)
        {
            try
            {
                List<clsImportError> Error = new List<clsImportError>();
                FileDialog importFile = new OpenFileDialog();
                importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
                OleDbDataReader dReader;
                DataTable dt = new DataTable();
                if (importFile.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                OleDbConnection excelConnection = null;
                string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                        importFile.FileName +
                                                        ";Extended Properties=Excel 12.0;Persist Security Info=False";
                excelConnection = new OleDbConnection(excelConnectionString);
                DateTime time = DateTime.Now;
                try
                {
                    OleDbCommand cmd =
                    new OleDbCommand("select * from [Sheet1$] ",// where [Thực xếp] <> \'\'",// and convert(dateTime,[Bắt đầu]) <= Convert(DateTime,\'" + time.ToString() + "\') and Convert(DateTime,\'" + time.ToString() + "\') <= convert(DateTime,[Kết thúc])",
                                        excelConnection);
                    excelConnection.Open();

                    dReader = cmd.ExecuteReader();
                    int i = 1;
                    string Oder = "";
                    string TTNC = "";
                    string NC = "";
                    string MachineID = "";
                    string Slg = "0";
                    string Start = "01/01/1900";
                    string Finish = "01/01/1900";
                    string TGGC = "0";
                    string TGGL = "0";
                    string KHCT = "";
                    string TH = "01/01/2050";
                    string TT = "0";
                    string SoJig = "";
                    string DKM = "";
                    string shift = cbShift.Text;
                    if(string.IsNullOrEmpty(shift))
                    {
                        throw new ArgumentException("Chưa chọn ca!");
                    }

                    TablePlanning tablePlanning = new TablePlanning();
                    while (dReader.Read())
                    {
                        i++;
                        try
                        {
                            if (string.IsNullOrEmpty(dReader["Thực xếp"].ToString().Trim()))
                            {
                                continue;
                            }

                            if (i == 4)
                            {

                            }
                            DataRow r = dt.NewRow();
                            Oder = dReader["Số Order"].ToString().Trim();
                            dept = dReader["Dept"].ToString().Trim();
                            TTNC = dReader["Số NC"].ToString().Trim();
                            NC = dReader["NC"].ToString().Trim();
                            MachineID = dReader["Máy GC"].ToString().Trim();
                            Slg = dReader["Slg lệnh"].ToString().Trim();
                            DKM = dReader["ĐKM gốc"].ToString().Trim();
                            //TH = DateTime.Parse(DateTime.Parse(dReader["Thời hạn"].ToString().Trim()).ToShortDateString().Trim() + " 08:00:00").ToString("MM/dd/yyyy hh:mm:ss");
                            var a = dReader["Bắt đầu"].ToString().Trim();
                            //Start = DateTime.Parse(DateTime.Parse(dReader["Bắt đầu"].ToString().Trim()).ToShortDateString().Trim()).ToString("MM/dd/yyyy");
                            //Finish = DateTime.Parse(DateTime.Parse(dReader["Kết thúc"].ToString().Trim()).ToShortDateString().Trim() + " 23:59:59").ToString("MM/dd/yyyy hh:mm:ss");
                            KHCT = dReader["Ký hiệu chi tiết"].ToString().Trim();
                            SoJig = dReader["Số Jig"].ToString().Trim();
                            TT = dReader["Tình trạng"].ToString().Trim().Length == 0 ? "0" : dReader["Tình trạng"].ToString().Trim();
                            if (dReader["TG GC"].ToString().Trim() != "")
                            {
                                TGGC = dReader["TG GC"].ToString().Trim();
                            }
                            if (dReader["TG GL"].ToString().Trim() != "")
                            {
                                TGGL = dReader["TG GL"].ToString().Trim();
                            }
                            string K = "";
                            if (dReader["Thực xếp"].ToString().Trim() == "1")
                            {
                                K = "K" + dReader["Thu tu GC"].ToString().Trim();
                            }
                            else if (dReader["Thực xếp"].ToString().Trim() == "1CT")
                            {
                                K = "C" + dReader["Thu tu GC"].ToString().Trim();
                            }
                            else if (dReader["Thực xếp"].ToString().Trim() == "2")
                            {
                                K = "B" + dReader["Thu tu GC"].ToString().Trim();
                            }
                            else if (dReader["Thực xếp"].ToString().Trim() == "5")
                            {
                                K = "L" + dReader["Thu tu GC"].ToString().Trim();
                            }

                            #region xử lý lệnh chưa có

                            if (Oder.ToUpper().Trim() == "CHUA CO")
                            {
                                Oder = "NULL" + KHCT;
                            }

                            #endregion
                            tablePlanning.Date = dateFromDate.Value;

                            tablePlanning.Order = Oder;
                            if(string.IsNullOrEmpty(tablePlanning.Order))
                            {
                                continue;
                            }

                            tablePlanning.NC = NC;
                            tablePlanning.MachineID = MachineID;
                            tablePlanning.Slg = int.Parse(Slg);
                            tablePlanning.TGGC = float.Parse(TGGC);
                            tablePlanning.PartID = KHCT;
                            tablePlanning.K = K;
                            tablePlanning.Shift = shift;
                            //tablePlanning.Start = DateTime.Parse(dReader["Bắt đầu"].ToString().Trim());
                            //tablePlanning.Finish = DateTime.Parse(dReader["Kết thúc"].ToString().Trim());
                            tablePlanning.ThoiHan = DateTime.Parse(dReader["Thời hạn"].ToString().Trim());
                            tablePlanning.TinhTrang = int.Parse(TT);
                            tablePlanning.SoJig = SoJig;
                            tablePlanning.TTNC = TTNC;
                            tablePlanning.DKmay = DKM;
                            tablePlanning.Dept = dept;
                            tablePlanning.Fac_NgayHTTheoKH = tablePlanning.Finish;
                            tablePlanning.Note = dReader["Ghi chú"].ToString().Trim();
                            tablePlanning.TongTG = dReader["Check time"].ToString().Trim();
                            //var a11 = tablePlanning.NC.Substring(4, 2);
                            //var NCtruoc = "ASTG" + (int.Parse(tablePlanning.NC.Substring(4, 2)) - 1).ToString("0#");
                            var last_machine = string.Empty;
                            var last_date = string.Empty;
                            tablePlanning.Fac_TTFile = dReader["tinh trang file"].ToString().Trim();

                            tablePlanning.Fac_NGTruoc = string.Empty;

                            clsPlanningDAO DAO = new clsPlanningDAO();
                            if (DAO.InsertPlan(tablePlanning) == 0)
                            {
                                Error.Add(new clsImportError(i, "Không nhập được. Cần liên hệ nhân viên PM"));
                            }
                        }
                        catch (Exception ex)
                        {
                            Error.Add(new clsImportError(i, ex.Message));
                        }
                    }
                    MessageBox.Show("OK");
                    if (excelConnection.State == ConnectionState.Open)
                    {
                        excelConnection.Close();
                    }

                    if (Error.Count > 0)
                    {
                        frm_Error frm = new frm_Error();
                        frm.Error = Error;
                        frm.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    if (excelConnection.State == ConnectionState.Open)
                    {
                        excelConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool InputDataToDatabase(List<clsPlanning> listPart)
        {
            clsPlanningDAO DAO = new clsPlanningDAO();
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
                    clsPlanningDAO DAO = new clsPlanningDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        TablePlanning obj = GetValidateData(numberofRow, sender);
                        obj.Deleted = true;
                        string sql = $"update [242_MachinePlanning] set Deleted = 1 where ID = {obj.ID}";
                        DAO.ExecuteStr(sql);
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
            try
            {
                TablePlanning obj = GetValidateData(e.RowHandle, sender);
                clsPlanningDAO DAO = new clsPlanningDAO();
                DAO.Update(obj);
                btnView_Click(sender, e);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private TablePlanning GetValidateData(int rowHandle, object sender)
        {
            TablePlanning obj = new TablePlanning();
            obj.ID = int.Parse(GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.ID)).ToString());
            obj.Date = (DateTime)GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.Date));
            obj.Dept = (string)GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.Dept));
            obj.K = (string)GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.K));
            obj.MayGC = (string)GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.MayGC));
            obj.Order = (string)GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.Order));
            obj.Note = (string)GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.Note));
            obj.NC = (string)GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.NC));
            obj.Shift = (string)GridView1.GetRowCellValue(rowHandle, nameof(TablePlanning.Shift));
            return obj;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            clsBase export = new clsBase();
            export.ExportToExcel(GridView1, 1, true);
        }

        private void Frm_ImportKH_Load(object sender, EventArgs e)
        {
            clsDepartmentDAO DAO = new clsDepartmentDAO();
            DataTable dt = DAO.GetAllData();
            DataRow r = dt.NewRow();
            r["DeptCode"] = string.Empty;
            r["DeptName"] = string.Empty;
            dt.Rows.InsertAt(r, 0);
            cbDept.DataSource = dt;
            cbDept.DisplayMember = "DeptCode";
            cbDept.ValueMember = "DeptCode";
        }
    }
}
