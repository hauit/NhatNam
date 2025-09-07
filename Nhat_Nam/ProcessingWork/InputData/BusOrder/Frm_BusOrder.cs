using Base.Base;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using FastMember;
using ProcessingWork.Base;
using ProcessingWork.DAO.DatabaseDAO;
using ProcessingWork.DataBase;
using ProcessingWork.Planning;
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

namespace ProcessingWork.InputData.BusOrder
{
    public partial class Frm_BusOrder : Form
    {
        public Frm_BusOrder()
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
                var work = cbDept.SelectedIndex;
                switch(work)
                {
                    case 1:
                        ImportData();
                        break;
                    case 2:
                        ChangeDeadLine();
                        break;
                    case 3:
                        ChangeQty();
                        break;
                    case 4:
                        CancelOrder();
                        break;
                    case 5:
                        FinishOrder();
                        break;
                    case 6:
                        StartOrder();
                        break;
                    case 7:
                        ImportData_FormMoi();
                        break;
                    case 8:
                        ImportPaymentStatus();
                        break;
                    case 9:
                        ImportStatus();
                        break;
                    case 10:
                        ImportCuttingOrder();
                        break;
                    default:
                        throw new ArgumentException("Vui lòng chọn đúng công việc cần thực hiện");
                }

            }
            catch(Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private void ImportCuttingOrder()
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
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }
                    
                    part.OrderCat = dReader[nameof(clsBusOrder.OrderCat)].ToString();
                    listPart.Add(part);
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
                }
            }

            if (listPart.Count > 0)
            {
                if (!BusOrderOrderCat(listPart))
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

        private bool BusOrderOrderCat(List<clsBusOrder> listPart)
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            return DAO.BusOrderOrderCat(listPart);
        }

        private void ImportStatus()
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
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }
                    
                    part.Status = dReader[nameof(clsBusOrder.Status)].ToString();
                    listPart.Add(part);
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
                }
            }

            if (listPart.Count > 0)
            {
                if (!BusOrderStatus(listPart))
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

        private void StartOrder()
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
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }

                    part.Started = true;
                    part.Change = "Start";
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

            btnView_Click(null, null);
        }

        private void ImportPaymentStatus()
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
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }

                    listPart.Add(part);
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
                }
            }

            if (listPart.Count > 0)
            {
                if (!PaymentStatus(listPart))
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

        private bool BusOrderStatus(List<clsBusOrder> listPart)
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            return DAO.BusOrderStatus(listPart);
        }

        private bool PaymentStatus(List<clsBusOrder> listPart)
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            return DAO.PaymentStatus(listPart);
        }

        private void FinishOrder()
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
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }
                    
                    listPart.Add(part);
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
                }
            }

            if (listPart.Count > 0)
            {
                if (!FinishOrder(listPart))
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

        private bool FinishOrder(List<clsBusOrder> listPart)
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            return DAO.FinishOrder(listPart);
        }

        private void CancelOrder()
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
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }

                    part.Finished = true;
                    part.FinishDate = DateTime.Now.Date;
                    part.Change = "Cancel";

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

            btnView_Click(null, null);
        }

        private void ChangeQty()
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
            //DataTable dtExistedOrder = GetAllExistedOrder();
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }

                    int qty;
                    if (!int.TryParse(dReader[nameof(clsBusOrder.Qty)].ToString(), out qty))
                    {
                        throw new ArgumentException($@"Số lượng lệnh({nameof(clsBusOrder.Qty)}) phải là kiểu số nguyên");
                    }

                    part.Qty = qty;
                    part.Change = "Qty change";

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

            btnView_Click(null, null);
        }

        private void ChangeDeadLine()
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
            //DataTable dtExistedOrder = GetAllExistedOrder();
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }

                    part.Note = dReader[nameof(clsBusOrder.Note)].ToString();
                    part.Status = dReader[nameof(clsBusOrder.Status)].ToString();

                    int qty;
                    if (!int.TryParse(dReader[nameof(clsBusOrder.Qty)].ToString(), out qty))
                    {
                        throw new ArgumentException($@"Số lượng lệnh({nameof(clsBusOrder.Qty)}) phải là kiểu số nguyên");
                    }

                    part.Qty = qty;
                    DateTime deadline;
                    if (!DateTime.TryParse(dReader[nameof(clsBusOrder.Deadline)].ToString(), out deadline))
                    {
                        throw new ArgumentException($@"Thời hạn đơn hàng({nameof(clsBusOrder.Deadline)}) không đúng định dạng");
                    }

                    part.Deadline = deadline;
                    part.Change = "DeadLine change";

                    listPart.Add(part);
                }
                catch (Exception ex)
                {
                    error.Add(new clsImportError(line, ex.Message));
                }
            }

            if (listPart.Count > 0)
            {
                if (!UpdateToDatabase(listPart))
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

        private bool UpdateToDatabase(List<clsBusOrder> listPart)
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            //int[] a;
            //a.OrderBy(x => x);
            return DAO.UpdateToDatabase(listPart);
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
            //DataTable dtExistedOrder = GetAllExistedOrder();
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsBusOrder> listExistPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }

                    //if (CheckExistedOrder(dtExistedOrder, part.BOderNo))
                    //{
                    //    throw new ArgumentException($@"Order {part.BOderNo} đã tồn tại trong dữ liệu");
                    //}

                    part.BOderNo = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                    DateTime date;
                    if (!DateTime.TryParse(dReader[nameof(clsBusOrder.Date)].ToString(), out date))
                    {
                        throw new ArgumentException($@"Ngày nhập({nameof(clsBusOrder.Date)}) không đúng định dạng");
                    }

                    part.Date = date;
                    part.PlanNo = dReader[nameof(clsBusOrder.PlanNo)].ToString();
                    part.PartID = dReader[nameof(clsBusOrder.PartID)].ToString();
                    part.OrderType = 0;
                    part.OrderType = GetOrderType(part.PartID, dReader[nameof(clsBusOrder.OrderType)].ToString().Trim()) ;
                    int qty;
                    if (!int.TryParse(dReader[nameof(clsBusOrder.Qty)].ToString(), out qty))
                    {
                        throw new ArgumentException($@"Số lượng lệnh({nameof(clsBusOrder.Qty)}) phải là kiểu số nguyên");
                    }

                    part.Qty = qty;
                    DateTime deadline;
                    if (!DateTime.TryParse(dReader[nameof(clsBusOrder.Deadline)].ToString(), out deadline))
                    {
                        throw new ArgumentException($@"Thời hạn đơn hàng({nameof(clsBusOrder.Deadline)}) không đúng định dạng");
                    }
                    part.Deadline = deadline;

                    part.NoiCat = dReader[nameof(clsBusOrder.NoiCat)].ToString();
                    //DateTime thPhoi;
                    //if (!DateTime.TryParse(dReader[nameof(clsBusOrder.THPhoi)].ToString(), out thPhoi))
                    //{
                    //    throw new ArgumentException($@"Thời hạn phôi({nameof(clsBusOrder.THPhoi)}) không đúng định dạng");
                    //}
                    clsBase ba = new clsBase();
                    part.THPhoi = ba.GetTHPhoi(part);

                    DateTime thVatLieu;
                    if (!DateTime.TryParse(dReader[nameof(clsBusOrder.THVatLieu)].ToString(), out thVatLieu))
                    {
                        throw new ArgumentException($@"Thời hạn vật liệu({nameof(clsBusOrder.THVatLieu)}) không đúng định dạng");
                    }
                    part.THVatLieu = thVatLieu;
                    
                    int rawQty;
                    if (!int.TryParse(dReader[nameof(clsBusOrder.RawQty)].ToString(), out rawQty))
                    {
                        throw new ArgumentException($@"RawQty({nameof(clsBusOrder.RawQty)}) phải là kiểu số");
                    }

                    part.RawQty = rawQty;
                    int helisertQty;
                    if (!int.TryParse(dReader[nameof(clsBusOrder.HelisertQty)].ToString(), out helisertQty))
                    {
                        throw new ArgumentException($@"HelisertQty({nameof(clsBusOrder.HelisertQty)}) phải là kiểu số");
                    }

                    part.HelisertQty = helisertQty;
                    int blastQty;
                    if (!int.TryParse(dReader[nameof(clsBusOrder.BlastQty)].ToString(), out blastQty))
                    {
                        throw new ArgumentException($@"BlastQty({nameof(clsBusOrder.BlastQty)}) phải là kiểu số");
                    }

                    part.BlastQty = blastQty;
                    part.MONo = dReader[nameof(clsBusOrder.MONo)].ToString();
                    int mOQty;
                    if (!int.TryParse(dReader[nameof(clsBusOrder.MOQty)].ToString(), out mOQty))
                    {
                        mOQty = 0;
                    }

                    part.MOQty = mOQty;
                    part.OrderCat = dReader[nameof(clsBusOrder.OrderCat)].ToString();
                    part.OrderGoc = dReader[nameof(clsBusOrder.OrderGoc)].ToString();
                    part.TempOrder = dReader[nameof(clsBusOrder.TempOrder)].ToString().Length > 0 ? true : false;
                    part.Started = dReader[nameof(clsBusOrder.Started)].ToString().Length > 0 ? true : false;
                    part.Started = true;
                    part.Finished = dReader[nameof(clsBusOrder.Finished)].ToString().Length > 0 ? true : false;
                    part.FinishDate = DateTime.Now.AddYears(100);
                    part.Change = dReader[nameof(clsBusOrder.Change)].ToString();
                    part.Imported = dReader[nameof(clsBusOrder.Imported)].ToString().Length > 0 ? true : false;
                    part.ImportFrom = dReader[nameof(clsBusOrder.ImportFrom)].ToString();
                    part.Note = dReader[nameof(clsBusOrder.Note)].ToString();
                    part.Status = dReader[nameof(clsBusOrder.Status)].ToString();
                    if(CheckExistedOrder(part))
                    {
                        listExistPart.Add(part);
                    }
                    else
                    {
                        listPart.Add(part);
                    }
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

            if(listExistPart.Count > 0)
            {
                var check = clsBase.DisplayMessage(@"Có order đã đặt hàng trước đây và bị hủy. Có cập nhật các order này không?","Order đã hủy giờ đặt lại", this);
                if(check == DialogResult.Yes)
                {
                    if (!InputDataToDatabase(listExistPart))
                    {
                        clsBase.DisplayMessage("Không cập nhật được. Vui lòng liên hệ Adim", this);
                    }
                    else
                    {
                        clsBase.DisplayMessage("Đã nhập xong", this);
                    }
                }
            }

            btnView_Click(null, null);
        }

        private int GetOrderType(string partID, string type)
        {
            if(type.ToLower() == "m")
            {
                return 3;
            }
            else if(type.ToLower() == "sx")
            {
                return 4;
            }
            int result = 1;
            clsOptionDataDAO DAO = new clsOptionDataDAO();
            DataTable dt = DAO.GetAnodOption(partID);
            if(dt.Rows.Count > 0)
            {
                result = 2;
            }

            return result;
        }

        private void ImportData_FormMoi()
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
            //DataTable dtExistedOrder = GetAllExistedOrder();
            List<clsBusOrder> listPart = new List<clsBusOrder>();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    line++;
                    clsBusOrder part = new clsBusOrder();
                    part.BOderNo = dReader["Order P"].ToString();
                    if (string.IsNullOrEmpty(part.BOderNo))
                    {
                        continue;
                    }

                    //if (CheckExistedOrder(dtExistedOrder, part.BOderNo))
                    //{
                    //    throw new ArgumentException($@"Order {part.BOderNo} đã tồn tại trong dữ liệu");
                    //}

                    part.BOderNo = dReader["Order P"].ToString();
                    part.Date = DateTime.Now.Date;
                    part.PlanNo = dReader["Order M"].ToString();
                    part.PartID = dReader["Part Name"].ToString();
                    int qty;
                    if (!int.TryParse(dReader[nameof(clsBusOrder.Qty)].ToString(), out qty))
                    {
                        throw new ArgumentException($@"Số lượng lệnh({nameof(clsBusOrder.Qty)}) phải là kiểu số nguyên");
                    }

                    part.Qty = qty;
                    DateTime deadline;
                    if (!DateTime.TryParse(dReader["PO Date"].ToString(), out deadline))
                    {
                        throw new ArgumentException($@"Thời hạn đơn hàng({"PO Date"}) không đúng định dạng");
                    }
                    part.Deadline = deadline;

                    part.NoiCat = dReader["Nơi cắt vật liệu"].ToString();
                    //DateTime thPhoi;
                    //if (!DateTime.TryParse(dReader[nameof(clsBusOrder.THPhoi)].ToString(), out thPhoi))
                    //{
                    //    throw new ArgumentException($@"Thời hạn phôi({nameof(clsBusOrder.THPhoi)}) không đúng định dạng");
                    //}
                    clsBase ba = new clsBase();
                    part.THPhoi = ba.GetTHPhoi(part);

                    DateTime thVatLieu;
                    if (!DateTime.TryParse(dReader[nameof(clsBusOrder.THVatLieu)].ToString(), out thVatLieu))
                    {
                        throw new ArgumentException($@"Thời hạn vật liệu({nameof(clsBusOrder.THVatLieu)}) không đúng định dạng");
                    }
                    part.THVatLieu = thVatLieu;
                    part.RawQty = 0;
                    part.HelisertQty = 0;
                    part.BlastQty = 0;
                    part.MONo = part.BOderNo;
                    int mOQty;
                    if (!int.TryParse(dReader["Qty No"].ToString(), out mOQty))
                    {
                        mOQty = 0;
                    }

                    part.MOQty = mOQty;
                    part.OrderCat = string.Empty;
                    part.OrderGoc = string.Empty;
                    part.TempOrder = false;
                    part.Started = true;
                    part.Finished = false;
                    part.FinishDate = DateTime.Now.AddYears(100);
                    part.Change = string.Empty;
                    part.Status = dReader[nameof(clsBusOrder.Status)].ToString();
                    part.ImportFrom = dReader["Suppliers"].ToString();
                    part.Imported = part.ImportFrom.Length > 0 ? true : false;
                    part.Note = dReader["Cus No."].ToString();

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

            btnView_Click(null, null);
        }

        private bool CheckExistedOrder(clsBusOrder bOderNo)
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            List<clsBusOrder> listData = new List<clsBusOrder>() { bOderNo };
            DataTable dt = DAO.GetOrderData(listData);
            if(dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        private DataTable GetAllExistedOrder()
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            DataTable dt = DAO.GetAllExistedOrder();
            return dt;
        }

        private bool InputDataToDatabase(List<clsBusOrder> listPart)
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            return DAO.InputDataToDatabase(listPart);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            DataTable dt = GetAllExistedOrder();
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
                    clsBusOrderDAO DAO = new clsBusOrderDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        string id = GridView1.GetRowCellValue(numberofRow, "ID").ToString();
                        DAO.ExecuteStr($@"update [242_BusOder] set Deleted = 1 where ID = {id}");
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
                clsBusOrder obj = GetValidateData(e, sender);
                obj.ID = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["ID"]).ToString());
                clsBusOrderDAO DAO = new clsBusOrderDAO();
                DAO.Update(obj);

                btnView_Click(sender, e);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private clsBusOrder GetValidateData(ValidateRowEventArgs e, object sender)
        {
            GridView view = sender as GridView;
            clsBusOrder obj = new clsBusOrder();
            obj.BOderNo = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.BOderNo)]).ToString();
            obj.Date = (DateTime)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.Date)]);
            obj.PlanNo = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.PlanNo)]).ToString();
            obj.PartID = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.PartID)]).ToString();
            obj.Qty = (int)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.Qty)]);
            var deadline = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.Deadline)]).ToString();
            DateTime Deadline;
            if(!DateTime.TryParse(deadline,out Deadline))
            {
                obj.Deadline = obj.Deadline.AddYears(70);
            }
            else
            {
                obj.Deadline = Deadline;
            }

            obj.RawQty = (int)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.RawQty)]);
            obj.HelisertQty = (int)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.HelisertQty)]);
            obj.BlastQty = (int)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.BlastQty)]);
            obj.MONo = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.MONo)]).ToString();
            obj.MOQty = (int)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.MOQty)]);
            obj.Started = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.Started)]);
            obj.Finished = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.Finished)]);
            var finishDate = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.FinishDate)]).ToString();
            DateTime FinishDate;
            if(DateTime.TryParse(finishDate,out FinishDate))
            {
                obj.FinishDate = FinishDate;
            }
            else
            {
                obj.FinishDate = obj.FinishDate.AddYears(70);
            }
            obj.Change = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.Change)]).ToString();
            obj.ChangeDate = DateTime.Now;
            obj.Imported = (bool)view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.Imported)]);
            obj.ImportFrom = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.ImportFrom)]).ToString();
            obj.Note = view.GetRowCellValue(e.RowHandle, view.Columns[nameof(clsBusOrder.Note)]).ToString();
            return obj;
        }

        private void btnOrderPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<clsBusOrder> listOrder = new List<clsBusOrder>();
                listOrder = GetDataFormExcel();
                ExecutePrintOrder(listOrder, sender);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private void ExecutePrintOrder(List<clsBusOrder> listOrder, object sender)
        {
            CheckPermission();
            //listOrder = GetDAtaFace();
            Ds_InToLenh ds = new Ds_InToLenh();
            DataTable dt = GetOrderToPrint(listOrder, ds);
            BindDataToGrid(dt, sender);
            frm_InToLenh frm = new frm_InToLenh();
            frm._Table1 = ds;
            frm.Show();
        }

        private void CheckPermission()
        {
            string sql = $"select * from [view_222_Staff] where DeptCode = 'KH' and staffID = '{ClsSession.staffID}'";
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            var dt = DAO.LoadGridByStr(sql).Rows.Count;
            if(dt == 0)
            {
                throw new ArgumentException("Bạn không có quyền in lệnh");
            }
        }

        private List<clsBusOrder> GetDAtaFace()
        {
            List<clsBusOrder> listOrder = new List<clsBusOrder>();
            List<string> list = new List<string>() {
                "Z997463",
                "Z971175",
                "Z961436",
                "Z956187",
                "Z956104",
                "Z936207",
                "Z925305",
                "Z890893",
                "Z889797",
                "Z884081",
                "Y254339",
                "Y293264",
                "Y298195",
                "Y397492",
                "Y334502",
                "Y216546",
                "Y385581",
                "Y337367",
                "Y337368"};
            foreach(var item in list)
            {
                clsBusOrder ob = new clsBusOrder();
                ob.PartID = item;
                listOrder.Add(ob);
            }

            return listOrder;
        }

        private List<clsBusOrder> GetDataFormExcel()
        {
            List<clsBusOrder> listOrder = new List<clsBusOrder>();
            OleDbDataReader dReader;
            FileDialog importFile = new OpenFileDialog();
            importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
            if (importFile.ShowDialog() != DialogResult.OK)
            {
                return listOrder;
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
            //DataTable dtExistedOrder = GetAllExistedOrder();
            List<clsImportError> error = new List<clsImportError>();
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                line++;
                clsBusOrder bOder = new clsBusOrder();
                bOder.PartID = dReader[nameof(clsBusOrder.BOderNo)].ToString();
                bOder.PlanNo = dReader[nameof(clsBusOrder.PlanNo)].ToString();
                if (string.IsNullOrEmpty(bOder.PartID))
                {
                    continue;
                }

                listOrder.Add(bOder);
            }

            clsBase.DisconnectToExcelFile(excelConnection);
            return listOrder;
        }

        private DataTable GetOrderToPrint(List<clsBusOrder> listOrder, Ds_InToLenh ds)
        {
            DataTable dtMain = ds.Tables["InToLenh"];
            DataTable dtDetail = ds.Tables["Detail"];
            Dictionary<string, int> keyPart = new Dictionary<string, int>();
            List<string> keyOrder = new List<string>();
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            DataTable dt = DAO.GetOrderData(listOrder);
            List<ToLenhDetail> listDetail = new List<ToLenhDetail>();
            int i = 1;
            foreach(DataRow r in dt.Rows)
            {
                var part = r["PartID"].ToString().ToUpper();
                var border = r["BOderNo"].ToString().ToUpper();
                var order = $@"{part}--{border}";
                if (!keyPart.ContainsKey(order))
                {
                    keyPart.Add(order, i);
                    DataRow rMain = dtMain.NewRow();
                    rMain["Date"] = r["Date"];
                    rMain["TenChiTiet"] = r["PartID"];
                    rMain["BusOrder"] = r["BOderNo"];
                    rMain["GiaThanh"] = r["GiaThanh"];
                    rMain["OrderF2"] = r["MONo"];
                    rMain["OrderGoc"] = "";
                    var mg = listOrder.Where(x => x.PartID.ToUpper() == border).FirstOrDefault();
                    if(mg != null)
                    {
                        rMain["MaG"] = mg.PlanNo;
                    }

                    rMain["GhiChu"] = r["Note"];
                    rMain["SoLg"] = r["MOQty"];
                    rMain["KichThuocPhoi"] = r["Workpiecesize"];
                    rMain["SlgPhoi"] = "";
                    rMain["NgayGiaoPhoi"] = "";
                    rMain["NguyenCong"] = r["OptionID"];
                    rMain["MayGC"] = r["MachineID"];
                    rMain["WTS"] = "";
                    rMain["NhanVien"] = "";
                    rMain["HanGiaoHang"] = r["Deadline"];
                    rMain["Memo"] = r["Memo"];
                    rMain["OKQty"] = "";
                    rMain["NGQty"] = "";
                    rMain["TT"] = "";
                    rMain["QRCode"] = r["QRCODE"];
                    rMain["Status"] = r["Status"];
                    rMain["PartName"] = r["PartName"];
                    rMain["GhiChuPhoi"] = r["GhiChuPhoi"];
                    rMain["Key"] = keyPart[order];
                    dtMain.Rows.Add(rMain);
                    i++;
                }

                ToLenhDetail obj = new ToLenhDetail();
                obj.NguyenCong = r["OptionID"].ToString().Trim().ToUpper();
                obj.MayGC = r["MachineID"].ToString().Trim().ToUpper();
                obj.ProTime = r["ProTime"].ToString().Trim();
                obj.ClampTime = r["ClampTime"].ToString().Trim();
                obj.Key = keyPart[order].ToString().Trim().ToUpper();
                obj.NgayIn = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                obj.NguoiIn = $"{ClsSession.staffID}-{ClsSession.StaffName}";

                var existed = listDetail.Where(x => x.NguyenCong == obj.NguyenCong && x.MayGC == obj.MayGC && x.Key == obj.Key).Any();
                if (existed)
                {
                    continue;
                }

                listDetail.Add(obj);
            }

            listDetail = listDetail.OrderBy(x => x.Key).OrderBy(y => y.NguyenCong).ToList();
            using (var reader = ObjectReader.Create(listDetail, new string[] {
                nameof(ToLenhDetail.MayGC),
                nameof(ToLenhDetail.NguyenCong),
                nameof(ToLenhDetail.Key),
                nameof(ToLenhDetail.ProTime),
                nameof(ToLenhDetail.ClampTime),
                nameof(ToLenhDetail.NgayIn),
                nameof(ToLenhDetail.NguoiIn)
            }))
            {
                dtDetail.Load(reader);
            }

            return dt;
        }

        private void PrintOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                if (MessageBox.Show("Bạn có thực sự muốn in order ở dòng đã chọn!", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }

                if (GridView1.SelectedRowsCount > 0)
                {
                    List<clsBusOrder> listOrder = new List<clsBusOrder>();
                    int numberofRow;
                    clsBusOrderDAO DAO = new clsBusOrderDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        clsBusOrder obj = new clsBusOrder();
                        obj.PartID = GridView1.GetRowCellValue(numberofRow, nameof(clsBusOrder.BOderNo)).ToString().ToUpper();
                        var existed = listOrder.Where(x => x.PartID == obj.PartID).Any();
                        if (existed)
                        {
                            continue;
                        }

                        listOrder.Add(obj);
                    }
                    
                    ExecutePrintOrder(listOrder, sender);
                }
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }
    }

    public class ToLenhDetail
    {
        public string Key { get; set; }

        public string NguyenCong { get; set; }

        public string MayGC { get; set; }

        public string ProTime { get; set; }

        public string ClampTime { get; set; }

        public string NgayIn { get; set; }

        public string NguoiIn { get; set; }
    }
}
