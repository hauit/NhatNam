using ATFunctions;
using Base.Base;
using Newtonsoft.Json.Linq;
using ProcessingWork.Base;
using ProcessingWork.DAO.DatabaseDAO;
using ProcessingWork.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZaloDotNetSDK;

namespace ProcessingWork
{
    public partial class Frm_ViecLatVat : Form
    {
        public Frm_ViecLatVat()
        {
            InitializeComponent();
        }

        private DataTable GetListMONo()
        {
            string sql = "select * from [242_BusOder] where date >= '20170101' and len(MONo) = 7";
            clsPlanningDAO DAO1 = new clsPlanningDAO();
            return DAO1.LoadGridByStr(sql);
        }

        private void btnUpdateNewOrder_Click(object sender, EventArgs e)
        {
            FileDialog importFile = new OpenFileDialog();
            importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
            if (importFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            OleDbDataReader dReader;
            OleDbConnection excelConnection = null;
            string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                    importFile.FileName +
                                                    ";Extended Properties=Excel 12.0;Persist Security Info=False";
            excelConnection = new OleDbConnection(excelConnectionString);
            excelConnection.Open();
            DataTable dt = new DataTable();
            dt = GetListMONo();
            try
            {
                OleDbCommand cmd =
                    new OleDbCommand("Select * from [Sheet1$]",
                                        excelConnection);
                dReader = cmd.ExecuteReader();
                int line = 0;
                Dictionary<string, string> listData = new Dictionary<string, string>();
                while (dReader.Read())
                {
                    line++;
                    Thread.Sleep(1);
                    try
                    {
                        string orderF2 = dReader["Order cu F2"].ToString();
                        string ressionID = dReader["Order Moi Scaw"].ToString();
                        if (string.IsNullOrEmpty(orderF2))
                        {
                            continue;
                        }

                        var data = dt.Select($@"MONo = '{orderF2}'").Any();
                        if (!data)
                        {
                            continue;
                        }

                        if (listData.ContainsKey(orderF2))
                        {
                            continue;
                        }
                        listData.Add(orderF2, ressionID);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                line = 0;
                while (true)
                {
                    Thread.Sleep(1);
                    foreach (var item in listData)
                    {
                        Thread.Sleep(10);
                        Thread thread = new Thread(new ThreadStart(() => {
                            clsPlanningDAO DAO1 = new clsPlanningDAO();
                            DAO1.UpdateNullOrder(item.Value, item.Key, "sp_UpdateOrderNull_ChuyenOrderScaw");
                        }));
                        thread.Start();
                    }

                }

                if (excelConnection.State == ConnectionState.Open)
                {
                    excelConnection.Close();
                }

                MessageBox.Show("OK");
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

        private void button1_Click(object sender, EventArgs e)
        {
            clsPlanningDAO DAO1 = new clsPlanningDAO();
            string sql = $@"select * from [View_242_WTS] where left(optionID,1) not in ('A','G') 
                        and isnull(ProTime,0) + isnull(ClampTime,0) > 0 ";
            var listWTS = DAO1.LoadGridByStr(sql);
            foreach(DataRow r in listWTS.Rows)
            {
                string staffID = r["StaffID"].ToString();
                string partID = r["PartID"].ToString();
                string optionID = r["OptionID"].ToString();
                float pro;
                float clamp;
                if(!float.TryParse(r["ProTime"].ToString(),out pro))
                {
                    pro = 0;
                }

                if (!float.TryParse(r["ClampTime"].ToString(), out clamp))
                {
                    clamp = 0;
                }

                sql = $@"select * from [242_StandTime] where machineID = N'{staffID}' and PartNo = N'{partID}' and optionID = N'{optionID}'";
                var dt1 = DAO1.LoadGridByStr(sql);
                if(dt1.Rows.Count > 0)
                {
                    float pro1;
                    float clamp1;
                    if (!float.TryParse(dt1.Rows[0]["ProTime"].ToString(), out pro1))
                    {
                        pro1 = 0;
                    }

                    if (!float.TryParse(dt1.Rows[0]["ClampTime"].ToString(), out clamp1))
                    {
                        clamp1 = 0;
                    }

                    pro = pro > pro1 ? pro1 : pro;
                    clamp = clamp > clamp1 ? clamp1 : clamp;
                    var id = dt1.Rows[0]["ID"].ToString();
                    DAO1.ExecuteStr($@"update [242_StandTime] set ProTime = {pro}, ClampTime = {clamp} where id = {id}");
                    continue;
                }

                sql = $@"insert [242_StandTime] ([Date]
                  ,[PartNo]
                  ,[MachineID]
                  ,[OptionID]
                  ,[ProTime]
                  ,[ClampTime]
                  ,[PJ61&PKN1]
                  ,[PZE1]
                  ,[PBJ1]
                  ,[PU71]
                  ,[PH71]
                  ,[Note]
                  ,[Note2]
                  ,[NumberOption]
                  ,[Status]
                  ,[MachineGroup]) values (
                    N'20220413',
                    N'{partID}',
                    N'{staffID}',
                    N'{optionID}',
                    N'{pro}',
                    N'{clamp}',
                    N'0',
                    N'0',
                    N'0',
                    N'0',
                    N'0',
                    N'',
                    N'',
                    N'0',
                    N'1',
                    N''
                    )";
                DAO1.ExecuteStr(sql);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var listPart = new List<clsBusOrder>();
            var obj = new clsBusOrder();
            obj.BOderNo = "Z997463";
            listPart.Add(obj);
            ClsSession.staffID = "0170";
            FinishOrder(listPart);
        }


        private bool FinishOrder(List<clsBusOrder> listPart)
        {
            clsBusOrderDAO DAO = new clsBusOrderDAO();
            return DAO.FinishOrder(listPart);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = $@"select * from [View_242_WTS] where left(optionID,1) not in ('A','G') 
                        and isnull(ProTime,0) + isnull(ClampTime,0) > 0 ";
            clsPlanningDAO DAO1 = new clsPlanningDAO();
            var listWTS = DAO1.LoadGridByStr(sql);
            foreach (DataRow r in listWTS.Rows)
            {
                string dateTime = r[""].ToString();
                string Order = r[""].ToString();
                string MayGC = r[""].ToString();
                string NC = r[""].ToString();
                string Shift = r[""].ToString();
                sql = $@" select * from [242_MachinePlanning] where NC like 'A%' and isnull(deleted,0) = 0
                        and Date = convert(Date,'{dateTime}') 
                        and order = N'{Order}'
                        and MayGC = N'{MayGC}'
                        and NC = N'{NC}'
                        and Shift = N'{Shift}'";
                DataTable dt1 = DAO1.LoadGridByStr(sql);
                if(dt1.Rows.Count > 0)
                {
                    continue;
                }


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var alo = new aloo();
            testc11(alo.MyProperty);
            testc11(alo.MyProperty);
            testc11(alo.MyProperty);
            //clsBusOrder alo = new clsBusOrder();
            //alo.PartID = "1234567890";
            //ITest obj = new Test();
            //var b = (object)alo;
            //obj.SetDefault(ref b);
            //bool alo = true;
            //testc(ref alo);
            //var a = alo;
            //ZaloClient client = new ZaloClient(@"ZeS1Vhsv3bQDZ09-khSDLO621Nkxe0SC_ki5B8wKIs
            //    B5oYSTbkjrGi3I1oZsZLK4jDGO9Dcl5q_ecsKwfiu55v2TV7ZvrXmtj91aIjsi1nczsKz1Zhu-5
            //    DJbD4cxZLSSxF1PRTQsF4oGq44uyUKD4fQGJN7WyJ8bhiizUkUKOIIqsH05_9rTCOYcNsJvmGqg
            //    jwL7KjNFD0M2YKqipV0USBh4NZl_l2Xxflvm6F2u5LU_ztyxpxaYSRZZGbpubG06jTTEQF3jEWM
            //    XZYrEw-DDUh2ZD0k3v5P7rhrORWCCEXcvfGXM");

            //JObject result = client.getListFollower(0, 20);
        }

        private void testc11(List<int> a)
        {
            if (a == null)
            {
                a = new List<int>();
            }

            a.Add(1);
        }

        private void testc(ref bool alo)
        {
            alo = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clsBase ba = new clsBase();
            ba.GetTHPhoi("czxc","Y459242");
            ba.GetTHPhoi("","");
        }

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private void alo1()
        {
            alo1();
        }
        const uint WM_KEYDOWN = 0x0100; 
        private void button6_Click(object sender, EventArgs e)
        {
            //alo1();
            Process[] alo = Process.GetProcessesByName("Notepad");
            foreach(Process item in alo)
            {
                SendKeys.Send("aloooooooooooooo");
                PostMessage(item.MainWindowHandle,WM_KEYDOWN,(int)Keys.A,0);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            FileDialog importFile = new OpenFileDialog();
            importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
            if (importFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            OleDbDataReader dReader;
            OleDbConnection excelConnection = null;
            string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                    importFile.FileName +
                                                    ";Extended Properties=Excel 12.0;Persist Security Info=False";
            excelConnection = new OleDbConnection(excelConnectionString);
            excelConnection.Open();
            DataTable dt = new DataTable();
            try
            {
                OleDbCommand cmd =
                    new OleDbCommand("Select * from [Sheet1$]",
                                        excelConnection);
                dReader = cmd.ExecuteReader();
                int line = 0;
                Dictionary<string, string> listData = new Dictionary<string, string>();
                while (dReader.Read())
                {
                    line++;
                    Thread.Sleep(1);
                    try
                    {
                        string orderF2 = dReader["Order P cũ"].ToString();
                        string order_new = dReader["Order P mới"].ToString();
                        string part_new = dReader["Tên chi tiết mới"].ToString();
                        string planno = dReader["Order M mới"].ToString();
                        string qty = dReader["Qty"].ToString();
                        if (string.IsNullOrEmpty(orderF2))
                        {
                            continue;
                        }

                        string sql = $@"update [242_BusOder] set BOderNo = N'{order_new}',MoNo = N'{order_new}',partID = N'{part_new}',planno = N'{planno}',Qty = N'{qty}',MOQty = N'{qty}' where BOderNo = '{orderF2}'";
                        clsPlanningDAO DAO1 = new clsPlanningDAO();
                        DAO1.ExecuteStr(sql);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (excelConnection.State == ConnectionState.Open)
                {
                    excelConnection.Close();
                }

                MessageBox.Show("OK");
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

        private void button8_Click(object sender, EventArgs e)
        {
            
            FileDialog importFile = new OpenFileDialog();
            importFile.Filter = "Excel 2007-2013|*.xlsx|Excel 97-2003 (*.xls)|*.xls";
            if (importFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            OleDbDataReader dReader;
            OleDbConnection excelConnection = null;
            string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                    importFile.FileName +
                                                    ";Extended Properties=Excel 12.0;Persist Security Info=False";
            excelConnection = new OleDbConnection(excelConnectionString);
            excelConnection.Open();
            DataTable dt = new DataTable();
            try
            {
                OleDbCommand cmd =
                    new OleDbCommand("Select * from [Sheet1$]",
                                        excelConnection);
                dReader = cmd.ExecuteReader();
                int line = 0;
                Dictionary<string, string> listData = new Dictionary<string, string>();
                while (dReader.Read())
                {
                    line++;
                    Thread.Sleep(1);
                    try
                    {
                        string orderF2 = dReader["Order P"].ToString();
                        string po = dReader["Số PO"].ToString();
                        string cus = dReader["Cus No"].ToString();
                        string status = dReader["Status"].ToString();
                        if (string.IsNullOrEmpty(orderF2))
                        {
                            continue;
                        }

                        string sql = $@"update [242_BusOder] set planno = N'{po}',Note = N'{cus}',status = N'{status}' where deleted = 0 and BOderNo = '{orderF2}'";
                        clsPlanningDAO DAO1 = new clsPlanningDAO();
                        DAO1.ExecuteStr(sql);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (excelConnection.State == ConnectionState.Open)
                {
                    excelConnection.Close();
                }

                MessageBox.Show("OK");
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

        private OleDbDataAdapter DA;
        private void button9_Click(object sender, EventArgs e)
        {
            int b = 500;
            int c = 65000;
            var b1 = b.ToString("#,#");
            var c1 = b.ToString("#.#");
            var b2 = c.ToString("#,#");
            string a1 = "17:05";
            var a2 = TimeSpan.Parse(a1);
            var date = DateTime.Now.Date.Add(a2);
            var a = Application.StartupPath;
            mdlDeclaration.AI.sCNSys = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Admin\Desktop\ChuongTrinhMau2014\SysDB.Dat; Jet OLEDB:Database Password=vung oi mo ra";
            //OleDbConnection connection = new OleDbConnection(sCNSys);

            //// Mở kết nối
            //connection.Open();

            mdlDeclaration.AI.CNSys = new OleDbConnection(mdlDeclaration.AI.sCNSys);
            mdlDeclaration.AI.CNSys.Open();
            var data = mdlDeclaration.FN.FillTable(mdlDeclaration.AI.CNSys, ref this.DA, "SELECT * FROM ListUser where USN='ADMIN'");
            //if(data.Rows.Count > 0) {
            //    MessageBox.Show("OK");
            //    MessageBox.Show(data.Rows[0]["USN"].ToString());
            //}
            //else
            //{
            //    MessageBox.Show("NG");
            //}
            comboBox1.Items.Add((object) data.Rows[0]["USN"].ToString());
            comboBox1.SelectedIndex = 0;
            string sql = $@"Insert into ListUser (USN,PWD,DataFile,PNChild,PNGiaThanh,GhiChep) values ('','','Nam2014.mdb','GD','TK','')";
            sql = $@"update ListUser set DataFile = 'Nam2023.mdb'";
            int num4 = checked (data.Rows.Count - 1);
            //mdlDeclaration.FN.Exe(sql, mdlDeclaration.AI.CNSys);
            sql = $@"SELECT * FROM ThangXL";
            var data01 = mdlDeclaration.FN.FillTable(mdlDeclaration.AI.CNSys, ref this.DA, sql);
            var data1 = mdlDeclaration.FN.FillTable(mdlDeclaration.AI.CNSys, ref this.DA, "SELECT * FROM PMS WHERE ChucNang LIKE 'HT_%' and USN=''");
            foreach(DataRow r in data1.Rows)
            {
                string cn = r["ChucNang"].ToString();
                string th = r["ThucHien"].ToString();
                string xem = r["Xem"].ToString();
                string them = r["Them"].ToString();
                string sua = r["Sua"].ToString();
                string xoa = r["Xoa"].ToString();
                sql = $@"Insert into PMS (USN,ChucNang,ThucHien,Xem,Them,Sua,Xoa) values ('','{cn}',{th},True,True,True,True)";
                //mdlDeclaration.FN.Exe(sql, mdlDeclaration.AI.CNSys);
            }

            var data2 = mdlDeclaration.FN.FillTable(mdlDeclaration.AI.CNSys, ref this.DA, "SELECT * FROM PMS WHERE ChucNang LIKE 'KB_%' and USN='' ");
            foreach(DataRow r in data2.Rows)
            {
                string cn = r["ChucNang"].ToString();
                string th = r["ThucHien"].ToString();
                string xem = r["Xem"].ToString();
                string them = r["Them"].ToString();
                string sua = r["Sua"].ToString();
                string xoa = r["Xoa"].ToString();
                sql = $@"Insert into PMS (USN,ChucNang,ThucHien,Xem,Them,Sua,Xoa) values ('','{cn}',{th},True,True,True,True)";
                //mdlDeclaration.FN.Exe(sql, mdlDeclaration.AI.CNSys);
            }

            var data3 = mdlDeclaration.FN.FillTable(mdlDeclaration.AI.CNSys, ref this.DA, "SELECT * FROM PMS WHERE ChucNang LIKE 'GD_%' and USN='' ");
            foreach(DataRow r in data3.Rows)
            {
                string cn = r["ChucNang"].ToString();
                string th = r["ThucHien"].ToString();
                string xem = r["Xem"].ToString();
                string them = r["Them"].ToString();
                string sua = r["Sua"].ToString();
                string xoa = r["Xoa"].ToString();
                sql = $@"Insert into PMS (USN,ChucNang,ThucHien,Xem,Them,Sua,Xoa) values ('','{cn}',{th},True,True,True,True)";
                //mdlDeclaration.FN.Exe(sql, mdlDeclaration.AI.CNSys);
            }

            var data4 = mdlDeclaration.FN.FillTable(mdlDeclaration.AI.CNSys, ref this.DA, "SELECT * FROM PMS WHERE ChucNang LIKE 'TI_%' and USN='' ");
            foreach(DataRow r in data4.Rows)
            {
                string cn = r["ChucNang"].ToString();
                string th = r["ThucHien"].ToString();
                string xem = r["Xem"].ToString();
                string them = r["Them"].ToString();
                string sua = r["Sua"].ToString();
                string xoa = r["Xoa"].ToString();
                sql = $@"Insert into PMS (USN,ChucNang,ThucHien,Xem,Them,Sua,Xoa) values ('','{cn}',{th},True,True,True,True)";
                //mdlDeclaration.FN.Exe(sql, mdlDeclaration.AI.CNSys);
            }

            var data5 = mdlDeclaration.FN.FillTable(mdlDeclaration.AI.CNSys, ref this.DA, "SELECT * FROM PMS WHERE ChucNang LIKE 'BC_%' and USN='' ");
            foreach(DataRow r in data5.Rows)
            {
                string cn = r["ChucNang"].ToString();
                string th = r["ThucHien"].ToString();
                string xem = r["Xem"].ToString();
                string them = r["Them"].ToString();
                string sua = r["Sua"].ToString();
                string xoa = r["Xoa"].ToString();
                sql = $@"Insert into PMS (USN,ChucNang,ThucHien,Xem,Them,Sua,Xoa) values ('','{cn}',{th},True,True,True,True)";
                //mdlDeclaration.FN.Exe(sql, mdlDeclaration.AI.CNSys);
            }
        }

        string alo = "25123456789";
        private void button10_Click(object sender, EventArgs e)
        {
            string user = "271565";
            string s = "";
            var dt = new DateTime(2025, 7, 8);
            double num1 = ((double) (int) (dt - new DateTime(1900, 1, 1)).TotalDays + 2.0) * 271565.0;
            try
            {
                for (int startIndex = 0; startIndex < user.Length; ++startIndex)
                {
                    int num2 = ((startIndex + 1) * (dt.Month + dt.Day + dt.Year) - int.Parse(user.Substring(startIndex, 1)) - (startIndex + 1)) % 10;
                    s = s + num2.ToString() + (((dt.Day + dt.Month + dt.Year) * int.Parse(user.Substring(startIndex, 1)) + (startIndex + 1) + int.Parse(user.Substring(startIndex, 1))) % 10).ToString();
                }
                var a = (double.Parse(s) + num1).ToString();
                if ((double.Parse(s) + num1).ToString() != alo)
                {

                }
                //return false;
            }
            catch (Exception ex)
            {
            //return false;
            }
            //return true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string str = EncryptString("a12ws5246b4e4133bbce2ea2315a2021", "thi |6|2025|28");
        }



        public static string EncryptString(string key, string plainText)
        {
            byte[] numArray = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = numArray;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            streamWriter.Write(plainText);
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }
        //private void CheckAndInputToPlanning(sp_242_WTS_GetAll_Result data, DateTime dateTime)
        //{
        //    //NN_DatabaseEntities db = new NN_DatabaseEntities();
        //    //var plan = db.C242_MachinePlanning.Where(x => x.Date == dateTime
        //    //                && x.Order.ToUpper() == data.MONo
        //    //                && x.MayGC == data.StaffID
        //    //                && x.NC.ToUpper() == data.OptionID.ToUpper()
        //    //                && x.Shift.ToUpper() == data.Shift.ToUpper()
        //    //                && !x.NC.ToUpper().StartsWith("A")
        //    //                && !x.Deleted).FirstOrDefault();
        //    //if (plan != null)
        //    //{
        //    //    return;
        //    //}

        //    //int slgLenh = 10000;
        //    //var order = db.View_242_BusOder.Where(x => x.BOderNo.ToUpper() == data.MONo.ToUpper()).FirstOrDefault();
        //    //if (order != null)
        //    //{
        //    //    slgLenh = order.MOQty == null ? slgLenh : (int)order.MOQty;
        //    //}

        //    //plan = new C242_MachinePlanning();
        //    //plan.Date = dateTime;
        //    //plan.Order = data.MONo.ToUpper();
        //    //plan.NC = data.OptionID.ToUpper();
        //    //plan.TTNC = GetTTNC(data);
        //    //plan.Shift = data.Shift;
        //    //plan.MayGC = data.StaffID.ToUpper();
        //    //plan.Slglenh = slgLenh;
        //    //db.C242_MachinePlanning.Add(plan);
        //    //db.SaveChanges();
        //}
    }

    class aloo
    {
        public List<int> MyProperty { get; set; }
        public int MyProperty1 { get; set; }
        public int MyProperty2 { get; set; }
    }

    interface ITest
    {
        bool SetDefault(ref object alo);
    }

    class Test : ITest
    {
        public bool SetDefault(ref object alo)
        {
            var a = (clsBusOrder)alo;
            a.PartID = "HauTesst";
            //alo = (object)a;
            return true;
        }
    }

    internal sealed class mdlDeclaration
    {
        public static Functions FN = new Functions();
        public static Catalog.Catalog KB = new Catalog.Catalog(ref mdlDeclaration.AI);
        public static DataClass.DataClass DC = new DataClass.DataClass(ref mdlDeclaration.AI);
        public static AppInfo AI = new AppInfo();
        public static bool OK;
        public static int CheDo;
        public static bool NK_PT_Show;
        public static bool CT_PT_Show;
        public static bool NK_PC_Show;
        public static bool CT_PC_Show;
        public static bool NK_GBN_Show;
        public static bool CT_GBN_Show;
        public static bool NK_GBC_Show;
        public static bool CT_GBC_Show;
        public static bool NK_PCP_Show;
        public static bool CT_PCP_Show;
        public static bool NK_PN_Show;
        public static bool CT_PN_Show;
        public static bool NK_HDDV_Show;
        public static bool CT_HDDV_Show;
        public static bool NK_HDXK_Show;
        public static bool CT_HDXK_Show;
        public static bool NK_PX_Show;
        public static bool CT_PX_Show;
        public static bool NK_PKT_Show;
        public static bool CT_PKT_Show;
        //public static frmChild formChild;
        //public static frmCT_PT formCT_PT;
        //public static frmSoNK_PT formSoNK_PT;
        //public static frmCT_PC formCT_PC;
        //public static frmSoNK_PC formSoNK_PC;
        //public static frmCT_GBN formCT_GBN;
        //public static frmSoNK_GBN formSoNK_GBN;
        //public static frmCT_GBC formCT_GBC;
        //public static frmSoNK_GBC formSoNK_GBC;
        //public static frmCT_PCP formCT_PCP;
        //public static frmSoNK_PCP formSoNK_PCP;
        //public static frmCT_PN formCT_PN;
        //public static frmSoNK_PN formSoNK_PN;
        //public static frmCT_HDDV formCT_HDDV;
        //public static frmSoNK_HDDV formSoNK_HDDV;
        //public static frmCT_HDXK formCT_HDXK;
        //public static frmSoNK_HDXK formSoNK_HDXK;
        //public static frmCT_PX formCT_PX;
        //public static frmSoNK_PX formSoNK_PX;
        //public static frmCT_PKT formCT_PKT;
        //public static frmSoNK_PKT formSoNK_PKT;
        //public static frmBangKH formBangKH;
        //public static frmBKKhauHao formBKKhauHao;
        //public static frmBangTHCPTT formBangTHCPTT;
        //public static frmBangGT formBangGT;
        //public static frmBangPBCPTT formBangPBCPTT = new frmBangPBCPTT();
        //public static frmDDBCDKT formDDBCDKT;
        //public static frmDDKQKD formDDKQKD;
        //public static frmDDBCLCTT formDDLCTT;
        public static bool Check_PT;
        public static bool Check_PC;
        public static bool Check_GBC;
        public static bool Check_GBN;
        public static bool Check_PCP;
        public static bool Check_HDDV;
        public static bool Check_HDXK;
        public static bool Check_PN;
        public static bool Check_PX;
        public static bool Check_PKT;
        public static string LoaiCongThuc;
        public static int iNum;
        public static string[,] aHamBCDKT = new string[20, 3];
        public static string[,] aBTBCDKT = new string[20, 2];
        public static string[,] aHamBCKQKD = new string[20, 3];
        public static string[,] aBTBCKQKD = new string[20, 2];
        public static string[,] aHamBCLCTT = new string[20, 3];
        public static string[,] aBTBCLCTT = new string[20, 2];
        public static bool fChuyenKBVT;
        public static bool fChuyenKBCN;
        public static bool fChuyenKBCPPB;
        public static bool fChuyenKBSP;
        public static bool fChuyenKBTSCD;

        //public static string LayNgay() => DateAndTime.Day(DateAndTime.Today.Date).ToString() + "/" + mdlDeclaration.AI.THANGXL.ToString();
    }
}
