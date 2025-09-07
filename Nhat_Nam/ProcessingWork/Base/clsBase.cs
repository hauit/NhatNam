using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraGrid;
using System.Data.OleDb;
using ProcessingWork.DataBase;

namespace ProcessingWork.Base
{
    public class clsBase
    {
        public void ListFolder(string sPath)
        {
            List<string> FileArr = new List<string> { };
            foreach (string File in Directory.GetFiles(sPath))
                FileArr.Add(File);
        }

        public void ExportToExcel(GridView gridView, int type, bool openNow)
        {
            if (gridView == null) return;

            //First: Change gridView's property PrintSelectedRowOnly to True
            gridView.OptionsPrint.PrintSelectedRowsOnly = true;
            switch (type)
            {
                //0: Export selected rows
                case 0:
                    if (gridView.RowCount > 0)
                    {
                        try
                        {
                            bool success = false;
                            XlsxExportOptions option = new XlsxExportOptions();
                            option.ShowGridLines = true;
                            option.SheetName = gridView.ViewCaption;

                            FileDialog fileDialog = new SaveFileDialog();
                            fileDialog.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
                            fileDialog.FileName = gridView.ViewCaption;
                            if (fileDialog.ShowDialog() == DialogResult.OK)
                            {
                                gridView.BestFitColumns();
                                gridView.ExportToXlsx(fileDialog.FileName, option);
                                success = true;
                            }

                            if (success && openNow && MessageBox.Show("Do you want to open the file?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                System.Diagnostics.Process process = new System.Diagnostics.Process();
                                process.StartInfo.FileName = fileDialog.FileName;
                                process.StartInfo.Verb = "Open";
                                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                                process.Start();

                            }

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("No data to export.", "Message",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                //1: Export all
                case 1:
                    if (gridView.RowCount > 0)
                    {
                        try
                        {
                            bool success = false;
                            XlsxExportOptions option = new XlsxExportOptions();
                            option.ShowGridLines = true;
                            option.SheetName = gridView.ViewCaption;

                            FileDialog fileDialog = new SaveFileDialog();
                            fileDialog.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
                            fileDialog.FileName = gridView.ViewCaption;
                            if (fileDialog.ShowDialog() == DialogResult.OK)
                            {
                                gridView.BestFitColumns();
                                gridView.SelectAll();
                                gridView.ExportToXlsx(fileDialog.FileName, option);
                                success = true;
                            }

                            if (success && openNow && MessageBox.Show("Do you want to open the file?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                System.Diagnostics.Process process = new System.Diagnostics.Process();
                                process.StartInfo.FileName = fileDialog.FileName;
                                process.StartInfo.Verb = "Open";
                                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                                process.Start();

                            }
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("No data to export.", "Message",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                default:
                    break;
            }

        }

        internal static void DisconnectToExcelFile(OleDbConnection excelConnection)
        {
            if(excelConnection == null)
            {
                return;
            }

            if(excelConnection.State == ConnectionState.Open)
            {
                excelConnection.Close();
            }
        }

        internal static void BindDataToGrid(DataTable dt, object sender, GridControl gridControl1, GridView gridView1)
        {
            var frm = (Form)sender;
            if (frm.InvokeRequired)
            {
                frm.Invoke(new MethodInvoker(() =>
                {
                    BindDataToGrid(dt, gridControl1, gridView1);
                }));
                return;
            }

            BindDataToGrid(dt, gridControl1, gridView1);
        }

        private static void BindDataToGrid(DataTable dt, GridControl gridControl1, GridView gridView1)
        {
            if(gridView1.Columns.Count > 0)
            {
                gridView1.Columns.Clear();
            }

            gridControl1.DataSource = dt;
            gridView1.BestFitColumns();
        }

        public static void DisplayMessage(string message, object sender)
        {
            var frm = (Form)sender;
            if(frm.InvokeRequired)
            {
                frm.Invoke(new MethodInvoker(() =>
                {
                    MessageBox.Show(message);
                }));
                return;
            }

            MessageBox.Show(message);
        }

        public static DialogResult DisplayMessage(string message, string caption, object sender)
        {
            var frm = (Form)sender;
            DialogResult result = DialogResult.No;
            if(frm.InvokeRequired)
            {
                frm.Invoke(new MethodInvoker(() =>
                {
                    result = MessageBox.Show(message,caption,MessageBoxButtons.YesNo);
                }));
            }
            else
            {
                result = MessageBox.Show(message,caption,MessageBoxButtons.YesNo);
            }
            
            return result;
        }

        public static void DisplayForm(object needShow, object sender)
        {
            var frm = (Form)sender;
            var shower = (Form)needShow;
            if (frm.InvokeRequired)
            {
                frm.Invoke(new MethodInvoker(() =>
                {
                    shower.Show();
                }));
                return;
            }

            shower.Show();
        }

        public DateTime GetTHPhoi(string partid,string mono)
        {
            var part = new clsBusOrder();
            part.PartID = partid;
            part.MONo = mono;
            var data = GetTHPhoi(part);
            return data;
        }
        
        public DateTime GetTHPhoi(clsBusOrder part)
        {
            string sql = $@"select Isnull(Max(t1.date),dateadd(year,50,getdate())) as Date
                from [View_242_InventoryData] as T1 
                left join View_242_InventoryReceivedDetail as T2 on T1.ID = T2.VoucherID
                where t2.PartNo = N'{part.PartID}' and T2.OrderNumber = N'{part.MONo}' ";
            BaseDAO DAO = new BaseDAO();
            var dt = DAO.LoadGridByStr(sql);

            return DateTime.Parse(dt.Rows[0]["Date"].ToString()).Date;
        }
    }
}
