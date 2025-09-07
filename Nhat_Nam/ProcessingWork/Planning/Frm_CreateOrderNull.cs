using Base.Base;
using DevExpress.XtraGrid.Views.Grid;
using ProcessingWork.Base;
using ProcessingWork.DAO.PlanningDAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessingWork.Planning
{
    public partial class Frm_CreateOrderNull : Form
    {
        public Frm_CreateOrderNull()
        {
            InitializeComponent();
            if (ClsSession.staffID == "1556")
            {
                btnUpdateOrder.Visible = true;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            clsPlanningDAO nullDAO = new clsPlanningDAO();
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
            DateTime time = DateTime.Now;
            List<OrderNullExisted> listExistedOrderNull = new List<OrderNullExisted>();
            try
            {
                OleDbCommand cmd =
                new OleDbCommand("select * from [Sheet1$] ",
                                    excelConnection);
                excelConnection.Open();

                dReader = cmd.ExecuteReader();

                clsPlanningDAO DAO = new clsPlanningDAO();
                DataTable dtJigOrder = DAO.LoadGridByStr("select Max(ID) as ID,OrderNullSave,OrderNull,[Order],PartID,MOQty,JigNo from [242_MachinePlanningOrderNull] where OrderNullSave like 'JIG%' group by OrderNullSave,OrderNull,[Order],PartID,MOQty,JigNo ");
                while (dReader.Read())
                {
                    //Update in null order talbe
                    var part = dReader["PartNo"].ToString().Trim();
                    if(string.IsNullOrEmpty(part))
                    {
                        continue;
                    }

                    var jigNo = dReader["JigNo"].ToString().Trim();
                    var slg = dReader["Slg"].ToString().Trim();
                    if(!string.IsNullOrEmpty(jigNo))
                    {
                        var data = dtJigOrder.Select($"PartID = '{part}'");
                        if(data.Length > 0)
                        {
                            OrderNullExisted obj = new OrderNullExisted();
                            obj.JigNo = jigNo;
                            obj.PartNo = part;
                            obj.Slg = slg;
                            obj.Order = data[0]["OrderNullSave"].ToString().Trim();
                            listExistedOrderNull.Add(obj);
                        }
                    }

                    nullDAO.CreateOrderNull(part, slg, jigNo);
                }
                clsBase.DisplayMessage("OK", this);
                excelConnection.Close();
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
                if (excelConnection.State == ConnectionState.Open)
                {
                    excelConnection.Close();
                }
            }

            if(listExistedOrderNull.Count > 0)
            {
                clsBase.DisplayMessage("Các jig sau đã có order. Vui lòng kiểm tra xem có thể dùng lại được không", this);
                if (GridView1.Columns.Count > 0)
                {
                    GridView1.Columns.Clear();
                }

                GridControl1.DataSource = listExistedOrderNull;
                mnChooseToolStripMenuItem.Visible = true;
            }

            btnView_Click(null,null);
        }

        /// <summary>
        /// check input string is number or not
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool CheckInt(string number)
        {
            try
            {
                int.Parse(number);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// function get list available null order
        /// </summary>
        private void LoadAvailableNullOrder(int status)
        {
            DataTable dt = new DataTable();
            clsPlanningDAO DAO = new clsPlanningDAO();
            switch (status)
            {
                case 0:
                    dt = DAO.DoStoreGetAll("sp_GetAllOrderNull");
                    GridControl1.DataSource = dt;
                    break;
                case 1:
                    dt = DAO.DoStoreGetAll("sp_GetAllOrderJigNull");
                    GridControl1.DataSource = dt;
                    break;
                case 2:
                    dt = DAO.DoStoreGetAll("sp_GetAllOrderNull_Normal");
                    GridControl1.DataSource = dt;
                    break;
                default:
                    dt = DAO.DoStoreGetAll("sp_GetAllOrder");
                    break;
            }

            GridControl1.DataSource = dt;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            mnChooseToolStripMenuItem.Visible = true;
            if (GridView1.Columns.Count > 0)
            {
                GridView1.Columns.Clear();
            }

            if (cbType.SelectedIndex == 0)
            {
                LoadAvailableNullOrder(0);
                return;
            }

            if (cbType.SelectedIndex == 1)
            {
                LoadAvailableNullOrder(1);
                return;
            }

            if (cbType.SelectedIndex == 2)
            {
                LoadAvailableNullOrder(2);
                return;
            }

            LoadAvailableNullOrder(3);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OleDbDataReader dReader;
            FileDialog importFile = new OpenFileDialog();
            importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
            if (importFile.ShowDialog() == DialogResult.OK)
            {
                OleDbConnection excelConnection = null;
                importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
                string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                        importFile.FileName +
                                                        ";Extended Properties=Excel 12.0;Persist Security Info=False";
                excelConnection = new OleDbConnection(excelConnectionString);
                DateTime time = DateTime.Now;
                try
                {
                    OleDbCommand cmd =
                    new OleDbCommand("select [Order null],[Order] from [Sheet1$] ",
                                        excelConnection);
                    excelConnection.Open();

                    dReader = cmd.ExecuteReader();

                    while (dReader.Read())
                    {
                        var order = dReader["Order"].ToString().Trim();
                        var orderNull = dReader["Order null"].ToString().Trim();
                        this.UpdateOrder(order, orderNull);
                    }
                    clsBase.DisplayMessage("OK", this);
                    excelConnection.Close();
                }
                catch (Exception ex)
                {
                    clsBase.DisplayMessage(ex.Message, this);
                    if (excelConnection.State == ConnectionState.Open)
                    {
                        excelConnection.Close();
                    }
                }
            }
            LoadAvailableNullOrder(0);
        }

        private void UpdateOrder(string order, string orderNull)
        {
            if (order.Length == 0)
            {
                return;
            }

            clsPlanningDAO DAO = new clsPlanningDAO();
            DAO.UpdateNullOrder(order, orderNull, "sp_UpdateOrderNull");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            clsBase export = new clsBase();
            export.ExportToExcel(GridView1, 1, true);
        }

        private void GridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.IsNewItemRow(e.RowHandle))
            {
                return;
            }
            try
            {
                var id = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["ID"]).ToString());
                var orderNull = view.GetRowCellValue(e.RowHandle, view.Columns["OrderNull"]).ToString();
                var order = view.GetRowCellValue(e.RowHandle, view.Columns["Order"]).ToString();
                var status = bool.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["status"]).ToString()) == true ? 1 : 0;
                var finished = bool.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["Finished"]).ToString()) == true ? 1 : 0;
                clsPlanningDAO DAO = new clsPlanningDAO();
                switch(status)
                {
                    case 0:
                        DataTable dt = DAO.LoadGridByStr("select * from [242_MachinePlanningOrderNull] where ID = " + id);
                        if (bool.Parse(dt.Rows[0]["status"].ToString()))
                        {
                            clsBase.DisplayMessage("Lệnh đã được sử dụng, không thể chuyển trạng thái", this);
                            return;
                        }
                        break;
                    case 1:
                        DAO.ExecuteStr("update [242_MachinePlanningOrderNull] set status = 1, [Finished] = " + finished + " where ID = " + id);
                        break;
                }

                if (order.Length > 0)
                {
                    UpdateOrder(order, orderNull);
                }

                clsBase.DisplayMessage("Đã sửa xong", this);
                LoadAvailableNullOrder(3);
            }
            catch (Exception ex)
            {
                clsBase.DisplayMessage(ex.Message, this);
            }
        }

        private void btnCreateJigOrder_Click(object sender, EventArgs e)
        {
            btnCreate_Click(sender, e);
        }

        private void btnUpdateOrder_Click(object sender, EventArgs e)
        {
            OleDbDataReader dReader;
            FileDialog importFile = new OpenFileDialog();
            importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
            if (importFile.ShowDialog() == DialogResult.OK)
            {
                OleDbConnection excelConnection = null;
                importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
                string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                        importFile.FileName +
                                                        ";Extended Properties=Excel 12.0;Persist Security Info=False";
                excelConnection = new OleDbConnection(excelConnectionString);
                DateTime time = DateTime.Now;
                try
                {
                    OleDbCommand cmd =
                    new OleDbCommand("select [MONo],[Order] from [Sheet1$] ",
                                        excelConnection);
                    excelConnection.Open();

                    dReader = cmd.ExecuteReader();

                    while (dReader.Read())
                    {
                        //Update in null order talbe
                        var order = dReader["Order"].ToString().Trim();
                        var orderNull = dReader["MONo"].ToString().Trim();
                        this.UpdateOrder(order, orderNull);
                    }
                    clsBase.DisplayMessage("OK", this);
                    excelConnection.Close();
                }
                catch (Exception ex)
                {
                    clsBase.DisplayMessage(ex.Message, this);
                    if (excelConnection.State == ConnectionState.Open)
                    {
                        excelConnection.Close();
                    }
                }
            }
        }

        private void mnFinishUsingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có thực sự muốn kết thúc các lệnh ảo!", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (GridView1.SelectedRowsCount > 0)
                {
                    var row = new DataRow[GridView1.SelectedRowsCount];
                    var frequencyId = new string[GridView1.SelectedRowsCount];
                    int numberofRow;
                    clsPlanningDAO DAO = new clsPlanningDAO();
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        var a = GridView1.GetRowCellValue(numberofRow, "ID").ToString();
                        DAO.ExecuteStr("update [242_MachinePlanningOrderNull] set Finished = 1 where ID = " + a);
                    }

                    LoadAvailableNullOrder(3);
                }
            }
        }

        private void Frm_CreateOrderNull_Load(object sender, EventArgs e)
        {

        }

        private void mnChooseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn tạo order mới cho các jig đã chọn?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (GridView1.SelectedRowsCount > 0)
                {
                    clsPlanningDAO nullDAO = new clsPlanningDAO();
                    var row = new DataRow[GridView1.SelectedRowsCount];
                    var frequencyId = new string[GridView1.SelectedRowsCount];
                    int numberofRow;
                    for (int i = 0; i < GridView1.SelectedRowsCount; i++)
                    {
                        numberofRow = GridView1.GetSelectedRows()[i];
                        var part = GridView1.GetRowCellValue(numberofRow, "PartNo").ToString();
                        var slg = GridView1.GetRowCellValue(numberofRow, "Slg").ToString();
                        var jigNo = GridView1.GetRowCellValue(numberofRow, "JigNo").ToString();
                        nullDAO.CreateOrderNull(part, slg, jigNo);
                    }

                }
            }

            clsBase.DisplayMessage("OK", this);
            btnView_Click(null, null);
        }
    }

    public class OrderNullExisted
    {
        public string Slg { get; set; }
        public string PartNo { get; set; }
        public string JigNo { get; set; }
        public string Order { get; set; }
        public bool choose = false;
        public bool Choose { get { return choose; } set { choose = value; } }
    }
}
