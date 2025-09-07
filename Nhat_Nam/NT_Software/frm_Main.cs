using Base.Base;
using Base.Connect;
using Base.Login;
using ProcessingWork;
using ProcessingWork.InputData.BusOrder;
using ProcessingWork.InputData.Department;
using ProcessingWork.InputData.Material;
using ProcessingWork.InputData.Option;
using ProcessingWork.InputData.OptionData;
using ProcessingWork.InputData.Part;
using ProcessingWork.InputData.PartData;
using ProcessingWork.Planning;
using ProcessingWork.Report;
using ProcessingWork.WTS.InputWTS;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT_Software
{
    public partial class frm_Main : Form
    {
        public static string Version { get; private set; }
        string currentFileVersionInfo = string.Empty;
        //OleDbDataReader dReader;
        private ClsConnect connect = new ClsConnect();
        public string ProcessEnviroment
        {
            get
            {
                string processEnviroment = "32bit";
                if (Environment.Is64BitProcess)
                {
                    processEnviroment = "64bit";
                }

                return processEnviroment;
            }
        }

        public frm_Main()
        {
            InitializeComponent();
            currentFileVersionInfo = FileVersionInfo.GetVersionInfo(Directory.GetCurrentDirectory() + "\\WEBWTS.exe").FileVersion;
            Version = currentFileVersionInfo;
            this.Text += $" Version: {Version} - Type: {ProcessEnviroment}";
            ClsLoginDAO DAO = new ClsLoginDAO();
            //DAO.CreateDomainLAN("192.168.1.57 server.com");
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            bool runInServer = RunAppStokingInServer();
            if (runInServer)
            {
                this.Close();
            }

            AutoUpdate();
            LogOut();

            frm_Login frm = new frm_Login();
            frm.Text = "Đăng nhập hệ thống";
            frm.MdiParent = this;
            frm.Login += new EventHandler(Login);
            frm.Show();
        }

        private bool RunAppStokingInServer()
        {
            string svPath = @"Phan mem\AutoUpdate\32bit";
            string appPath = Directory.GetCurrentDirectory();
            if (appPath.IndexOf(svPath) != -1)
            {
                return true;
            }

            svPath = @"Phan mem\AutoUpdate\64bit";
            if (appPath.IndexOf(svPath) != -1)
            {
                return true;
            }

            return false;
        }

        private void AutoUpdate()
        {
            try
            {
                if (CheckHaveNewVersion() == false)
                {
                    return;
                }

                MessageBox.Show("Phần mềm đã có bản nâng cấp. Bản nâng cấp sẽ tự động update khi khởi động phần mềm");
                if (ExecuteAutoUpdate() == false)
                {
                    MessageBox.Show("Có lỗi! Không thể tự động nâng cấp");
                    return;
                }

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ExecuteAutoUpdate()
        {
            try
            {
                var curentPath = Directory.GetCurrentDirectory();
                var logSVPath = curentPath + "\\AutoUpdate.exe";
                if (logSVPath.Length == 0)
                {
                    return false;
                }

                Process.Start(logSVPath);
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        private bool CheckHaveNewVersion()
        {
            try
            {
                string currentVersion = currentFileVersionInfo.Substring(StartIndexOfVersion(currentFileVersionInfo), currentFileVersionInfo.Length - StartIndexOfVersion(currentFileVersionInfo));

                var processName = Process.GetCurrentProcess().ProcessName;
                if (processName.ToLower() == "webwts.vshost")
                {
                    processName = "WEBWTS";
                }

                //string svFolder = connect.GetServerFolder();
                string processEnviroment = "32bit";
                if (Environment.Is64BitProcess)
                {
                    processEnviroment = "64bit";
                }

                string sql = $"Select * from [222_PathOfSofware] where ProcessFile = '{processName}' and ProcessEnviroment = '{processEnviroment}' ";
                string svFolder = connect.LoadGridByStr(sql).Rows[0]["Path"].ToString().Trim();
                if (string.IsNullOrEmpty(svFolder))
                {
                    return false;
                }

                string svfile = svFolder;
                var serverFileVersionInfo = FileVersionInfo.GetVersionInfo(svfile).FileVersion;
                string serverVersion = serverFileVersionInfo.Substring(StartIndexOfVersion(serverFileVersionInfo), serverFileVersionInfo.Length - StartIndexOfVersion(serverFileVersionInfo));

                if (int.Parse(currentVersion) < int.Parse(serverVersion))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private int StartIndexOfVersion(string version)
        {
            return version.LastIndexOf('.') + 1;
        }
        private void tmnConnect_Click(object sender, EventArgs e)
        {

        }

        #region Ẩn - hiện menu khi login - logout
        private void LogOut()
        {

            //tmnViewAll.Enabled = false;
            loginToolStripMenuItem.Enabled = true;
            logOutToolStripMenuItem.Enabled = false;
            PcessingStripMenuItem.Enabled = false;
            //tmnLogin.Enabled = true;
            //tmnSendMail.Enabled = false;
        }

        private void Login(object sender, EventArgs e)
        {

            loginToolStripMenuItem.Enabled = false;
            logOutToolStripMenuItem.Enabled = true;
            bool isQuanLy = IsQuanLy();
            PcessingStripMenuItem.Enabled = true;

        }

        private bool IsQuanLy()
        {
            DataTable dt = connect.DoStoreGetAll("sp_DanhSachNhanVienDacBiet");
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            foreach (DataRow r in dt.Rows)
            {
                string staffID = r["StaffID"].ToString();
                if (ClsSession.staffID != staffID)
                {
                    continue;
                }

                return true;
            }

            return false;
        }
        #endregion

        private void kếtNốiCSDLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Connect frm = new frm_Connect();
            frm.Text = "Kết nối cơ sở dữ liệu";
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void loginToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frm_Login frm = new frm_Login();
            frm.Text = "Đăng nhập hệ thống";
            frm.Login += new EventHandler(Login);
            frm.MdiParent = this;
            frm.Show();
        }

        private void logOutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ClsSession.Pass = "";
            ClsSession.User = "";
            ClsSession.staffID = "";
            ClsSession.StaffName = "";
            LogOut();
        }


        private void ShowForm(Form frm, bool multyWindown = false)
        {
            foreach (Form frmExists in this.MdiChildren)
            {
                if(multyWindown)
                {
                    break;
                }

                if (frm.Name == frmExists.Name)
                {
                    frmExists.Activate();
                    return;
                }
            }

            frm.MdiParent = this;
            frm.Show();
        }

        private void PartListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_PartList frm = new Frm_PartList();
            ShowForm(frm);
        }

        private void PartDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_PartData frm = new Frm_PartData();
            ShowForm(frm);
        }

        private void MaterialListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Material frm = new Frm_Material();
            ShowForm(frm);
        }

        private void BusOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_BusOrder frm = new Frm_BusOrder();
            ShowForm(frm);
        }

        private void OptionDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_OptionData frm = new Frm_OptionData();
            ShowForm(frm);
        }

        private void GetOptionOfPartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_GetOptionDataOfOrder frm = new Frm_GetOptionDataOfOrder();
            ShowForm(frm);
        }

        private void OptionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Option frm = new Frm_Option();
            ShowForm(frm);
        }

        private void departmentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Department frm = new Frm_Department();
            ShowForm(frm);
        }

        private void PlanningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_ImportKH frm = new Frm_ImportKH();
            ShowForm(frm);
        }

        private void nhậpWTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_WTS frm = new Frm_WTS();
            ShowForm(frm);
        }

        private void ProcessingResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_ProcessingResult frm = new Frm_ProcessingResult();
            ShowForm(frm);
        }

        private void NullOrderCreationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_CreateOrderNull frm = new Frm_CreateOrderNull();
            ShowForm(frm);
        }

        private void WTSReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Report frm = new Frm_Report();
            ShowForm(frm,true);
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_ViecLatVat frm = new Frm_ViecLatVat();
            ShowForm(frm, true);
        }
    }
}
