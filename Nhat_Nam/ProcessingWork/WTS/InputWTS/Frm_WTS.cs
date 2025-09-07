using Base.Base;
using DevExpress.XtraGrid.Views.Grid;
using ProcessingWork.Base;
using ProcessingWork.DAO.WTS;
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

namespace ProcessingWork.WTS.InputWTS
{
    public partial class Frm_WTS : Form
    {
        public Frm_WTS()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            clsWTSDAO DAO = new clsWTSDAO();
            DataTable dt = DAO.GetAllWTS(dateFrom.Value,dateTo.Value, cbMachine.Text.Trim(), cbShift.Text.Trim(), cbStaff.Text.Trim());
            BindDataToGrid(dt, sender);
            GridView1.Columns[0].Visible = false;
            GridView1.Columns[1].Visible = false;
            //GridView1.Columns[18].Visible = false;
            GridView1.Columns[19].Visible = false;
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
                    clsWTSDAO DAO = new clsWTSDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        clsWTS obj = GetValidateData(numberofRow, sender);
                        obj.Deleted = true;
                        DAO.Update(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
            finally
            {
                btnView_Click(sender, e);
            }

        }

        private void ValidateData(clsWTS obj)
        {
            int qty = 0;
            List<clsWTS> listTotal = GetQtyInputed(obj,out qty);
            int currentOK = 0;
            int currentNG = 0;
            if (listTotal.Count > 0)
            {
                currentOK = listTotal[0].OKQty;
                currentNG = listTotal[0].NGQty;
            }

            bool overQty = (currentOK + currentNG + obj.OKQty + obj.NGQty) > qty;
            if (overQty)
            {
                throw new ArgumentException($@"Số lượng OK hiện tại là:{currentOK}, NG hiện tại: {currentNG}, số lượng OK nhập thêm:{obj.OKQty}, NG nhập thêm:{obj.NGQty}, số lượng lệnh là {qty}. Tổng số lượng vượt quá số lượng lệnh");
            }

            if (listTotal.Count <= 1)
            {
                return;
            }

            bool overLastOKQty = listTotal[1].OKQty < (listTotal[0].OKQty + obj.OKQty);
            if (overLastOKQty)
            {
                throw new ArgumentException($@"Số lượng ok của nguyên công trước là {listTotal[1].OKQty}, số lượng ok đã nhập của nguyên công này là:{listTotal[0].OKQty}, số lượng ok của nguyên công này sẽ nhập tiếp là:{obj.OKQty}. Không thể nhập vượt quá số lượng ok của nguyên công trước.");
            }

        }

        private List<clsWTS> GetQtyInputed(clsWTS obj,out int qty)
        {
            clsWTSDAO DAO = new clsWTSDAO();
            List<clsWTS> listTotal = DAO.GetQtyInputed(obj);
            qty = DAO.GetQtyOfOrder(obj);
            return listTotal;
        }

        private void GridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                clsWTS obj = GetValidateData(e.RowHandle, sender);
                clsWTSDAO DAO = new clsWTSDAO();
                if (obj.Status)
                {
                    DataTable data = DAO.QuyenSuaSauDuyet();
                    if(data.Rows.Count == 0)
                    {
                        throw new ArgumentException("WTS đã duyệt không thể sửa");
                    }
                }
                
                ValidateData(obj);
                DAO.Update(obj);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
            finally
            {
                //btnView_Click(sender, e);
            }
        }

        private clsWTS GetValidateData(int rowHandle, object sender)
        {
            clsWTS obj = new clsWTS();
            obj.ID = int.Parse(GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.ID)).ToString());
            obj.Date = (DateTime)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.Date));
            obj.IDPlan = (int)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.IDPlan));
            obj.MachineID = GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.MachineID)).ToString();
            obj.MONo = (string)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.MONo));
            obj.OKQty = (int)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.OKQty));
            obj.NGQty = (int)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.NGQty));
            obj.NGNCTruoc = (int)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.NGNCTruoc));
            obj.OptionID = (string)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.OptionID));
            obj.Note = GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.Note)).ToString();
            obj.Shift = GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.Shift)).ToString();
            obj.StaffID = GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.StaffID)).ToString();
            obj.ClampTime = GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.ClampTime)).ToString();
            obj.ProTime = GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.ProTime)).ToString();
            obj.Time = (decimal)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.Time));
            obj.WorkID = GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.WorkID)).ToString();
            obj.Status = GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.Status)).ToString().Length == 0 ? false : (bool)GridView1.GetRowCellValue(rowHandle, nameof(clsWTS.Status));
            return obj;
        }

        private void btnExPort_Click(object sender, EventArgs e)
        {
            clsBase export = new clsBase();
            export.ExportToExcel(GridView1, 1, true);
        }

        private void Frm_WTS_Load(object sender, EventArgs e)
        {
            string sql = @"select * from View_222_Machine";
            clsWTSDAO DAO = new clsWTSDAO();
            DataTable dt = DAO.LoadGridByStr(sql);
            DataRow r = dt.NewRow();
            r["MachineID"] = string.Empty;
            dt.Rows.InsertAt(r, 0);
            cbMachine.DataSource = dt;
            cbMachine.DisplayMember = "MachineID";
            cbMachine.ValueMember = "MachineID";

            sql = @"select * from View_222_Staff where staffID <> staffName";
            DataTable dt1 = DAO.LoadGridByStr(sql);

            DataRow r1 = dt1.NewRow();
            r1["StaffID"] = string.Empty;
            dt1.Rows.InsertAt(r1, 0);
            cbStaff.DataSource = dt1;
            cbStaff.DisplayMember = "StaffID";
            cbStaff.ValueMember = "StaffID";
        }

        private void OKWTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có thực sự muốn duyệt các dòng đã chọn!", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }

                if (GridView1.SelectedRowsCount > 0)
                {
                    var row = new DataRow[GridView1.SelectedRowsCount];
                    var frequencyId = new string[GridView1.SelectedRowsCount];
                    int numberofRow;
                    clsWTSDAO DAO = new clsWTSDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        clsWTS obj = GetValidateData(numberofRow, sender);
                        obj.Status = true;
                        DAO.Update(obj);

                        GridView1.SetRowCellValue(numberofRow,"LeaderCheck", ClsSession.staffID);
                        GridView1.SetRowCellValue(numberofRow,"Status", true);
                    }
                }
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
            finally
            {
                //btnView_Click(sender, e);
            }

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có thực sự muốn duyệt WTS!", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            clsWTSDAO DAO = new clsWTSDAO();
            DataTable dt = DAO.DoStoreGetAll("sp_242_WTS_Permission");
            if (dt.Rows.Count == 0)
            {
                return;
            }

            if (dt.Select("staffID = '" + ClsSession.staffID + "'").Length == 0)
            {
                MessageBox.Show("Không có quyền duyệt WTS");
                return;
            }

            for (int i = 0; i < GridView1.RowCount; i++)
            {
                string IDCache = GridView1.GetDataRow(i)["ID"].ToString();
                DAO.ExecuteStr($@"update [242_WTS] set Status = 1, LeaderCheck = '{ClsSession.staffID}', CheckTime = getdate() where ID= {IDCache}");
                GridView1.SetRowCellValue(int.Parse(IDCache),"LeaderCheck", ClsSession.staffID);
                GridView1.SetRowCellValue(int.Parse(IDCache),"Status", true);
            }
            
            MessageBox.Show("Nhập dữ liệu xong");
            //btnView_Click(sender, e);
        }

        private void GridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }


                GridView View = sender as GridView;
                string proTime = View.GetRowCellDisplayText(e.RowHandle, View.Columns["ProTime"]);
                string proMay = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Pro_May"]);
                string proOp = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Pro_OptionData"]);
                var time = GridView1.GetRowCellValue(e.RowHandle, nameof(clsWTS.Time)).ToString();
                var ng = GridView1.GetRowCellValue(e.RowHandle, nameof(clsWTS.NGQty)).ToString();
                var ok = GridView1.GetRowCellValue(e.RowHandle, nameof(clsWTS.OKQty)).ToString();
                var clampTime = GridView1.GetRowCellValue(e.RowHandle, nameof(clsWTS.ClampTime)).ToString();
                var work = GridView1.GetRowCellValue(e.RowHandle, nameof(clsWTS.WorkID)).ToString();

                float promay;
                if (!float.TryParse(proMay, out promay))
                {
                    promay = 0;
                }
                float proop;
                if (!float.TryParse(proOp, out proop))
                {
                    proop = 0;
                }

                float pTime;
                if (!float.TryParse(proTime, out pTime))
                {
                    pTime = 0;
                    //throw new ArgumentException("Thời gian làm việc phải là kiểu số");
                }

                float cTime;
                if (!float.TryParse(clampTime, out cTime))
                {
                    cTime = 0;
                    //throw new ArgumentException("Thời gian làm việc phải là kiểu số");
                }

                float Time;
                if (!float.TryParse(time, out Time))
                {
                    Time = 0;
                    //throw new ArgumentException("Thời gian làm việc phải là kiểu số");
                }

                float OK;
                if (!float.TryParse(ok, out OK))
                {
                    OK = 0;
                    //throw new ArgumentException("Thời gian làm việc phải là kiểu số");
                }
                float NG;
                if (!float.TryParse(ng, out NG))
                {
                    NG = 0;
                    //throw new ArgumentException("Thời gian làm việc phải là kiểu số");
                }

                if(promay <= 0)
                {
                    if(proop <= 0)
                    {
                        return;
                    }

                    if(pTime > proop)
                    {
                        e.Appearance.BackColor = Color.YellowGreen;
                    }
                    return;
                }

                if(pTime > promay)
                {
                    e.Appearance.BackColor = Color.Yellow;
                }

                if(work.ToUpper() == "PLJ1")
                {
                    var les = Time < ((pTime + cTime)*(OK + NG) - 3);
                    var more = Time > ((pTime + cTime)*(OK + NG) + 3);
                    if(les || more)
                    {
                        e.Appearance.BackColor = Color.OrangeRed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            try
            {
                ImportData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            List<clsWTS> listPart = new List<clsWTS>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsWTS part = new clsWTS();
                    part.MONo = dReader[nameof(clsWTS.MONo)].ToString();
                    part.Date = dateFrom.Value.Date;
                    part.StaffID = dReader[nameof(clsWTS.StaffID)].ToString();
                    if (string.IsNullOrEmpty(part.StaffID))
                    {
                        continue;
                    }
                    part.Shift = dReader[nameof(clsWTS.Shift)].ToString();
                    part.MachineID = dReader[nameof(clsWTS.MachineID)].ToString();
                    part.OptionID = dReader[nameof(clsWTS.OptionID)].ToString();
                    part.WorkID = dReader[nameof(clsWTS.WorkID)].ToString();
                    int OKQty;
                    if(!int.TryParse(dReader[nameof(clsWTS.OKQty)].ToString(),out OKQty))
                    {
                        throw new ArgumentException("số lượng ok phải là kiểu số");
                    }
                    part.OKQty = OKQty;
                    int NGQty;
                    if(!int.TryParse(dReader[nameof(clsWTS.NGQty)].ToString(),out NGQty))
                    {
                        throw new ArgumentException("số lượng NG phải là kiểu số");
                    }
                    part.NGQty = NGQty;
                    int time;
                    if(!int.TryParse(dReader[nameof(clsWTS.Time)].ToString(),out time))
                    {
                        throw new ArgumentException("Thời gian WTS(Time) phải là kiểu số");
                    }
                    part.Time = time;
                    part.ProTime = dReader[nameof(clsWTS.ProTime)].ToString();
                    part.ClampTime = dReader[nameof(clsWTS.ClampTime)].ToString();

                    clsWTSDAO DAO = new clsWTSDAO();
                    if (DAO.Update(part) == 0)
                    {
                        throw new ArgumentException("Không nhập được dữ liệu");
                    }
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
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
    }
}
