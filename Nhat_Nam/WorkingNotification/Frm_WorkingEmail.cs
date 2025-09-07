using Base.Connect;
using SendEmailDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkingNotification
{
    public partial class Frm_WorkingEmail : Form
    {
        private List<FucntionObj> listTime;
        public Frm_WorkingEmail()
        {
            listTime = new List<FucntionObj>();
            InitializeComponent();
            this.ControlBox = false;
        }

        private void CheckOnTime()
        {
            while (true)
            {
                try
                {
                    if (listTime.Count == 0)
                    {
                        GetInitData();
                    }

                    DateTime now = DateTime.Now;
                    int currentTime = now.Hour * 60 + now.Minute;
                    if (currentTime == 19)
                    {
                        listTime = new List<FucntionObj>();
                        continue;
                    }

                    var onTime = listTime.Where(x => x.Time.Where(y => y == currentTime).Any()).FirstOrDefault();
                    if (onTime == null)
                    {
                        continue;
                    }
                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        GetAndSendEmail(onTime);
                    }));
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLog(ex.Message);
                }
                Thread.Sleep(60000);
            }
        }

        private void GetAndSendEmail(FucntionObj onTime)
        {
            try
            {
                EmailObject emailObject = new EmailObject();
                GetDataAndExport(onTime, ref emailObject);
                GetReceiverList(onTime, ref emailObject);
                CreateEmailData(onTime, ref emailObject);
                AEmail sendEmail = new SendEmailOnly();
                sendEmail.CreateEmailContent(emailObject);
                sendEmail.SendEmail();
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(ex.Message);
            }
        }

        private void GetReceiverList(FucntionObj onTime, ref EmailObject emailObject)
        {
            WorkingEmail DAO = new WorkingEmail();
            var dt = DAO.GetSEndingEmail(onTime.STT);
            foreach (DataRow r in dt.Rows)
            {
                string sendType = r["SendType"].ToString();
                string email = r["Email"].ToString();
                switch (sendType)
                {
                    case nameof(EmailObject.To):
                        emailObject.To.Add(email);
                        break;
                    case nameof(EmailObject.BCC):
                        emailObject.BCC.Add(email);
                        break;
                    case nameof(EmailObject.CC):
                        emailObject.CC.Add(email);
                        break;
                }
            }
        }

        private void CreateEmailData(FucntionObj onTime, ref EmailObject emailObject)
        {
            emailObject.Body = "Vui lòng xem file đính kèm.";
            emailObject.Pass = string.Empty;
            emailObject.Sender = string.Empty;
            emailObject.SMTP = string.Empty;
            emailObject.Subject = $@"{onTime.Subject} ngày {DateTime.Now.ToString("dd/MM/yy")}";
        }

        private void GetDataAndExport(FucntionObj onTime, ref EmailObject emailObject)
        {
            WorkingEmail DAO = new WorkingEmail();
            var dt = DAO.GetDataForNotification(onTime.FromDate, onTime.ToDate, onTime.Data);
            clsBase.BindDataToGrid(dt, this, gridControl2, gridView2);
            string path = Directory.GetCurrentDirectory() + string.Format(@"\{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMdd"), onTime.FileName); ;
            clsBase.ExportToExcel(this, gridView2, path);
            emailObject.AttachedFiles.Add(path);
        }

        //private void Frm_WorkingEmail_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GetInitData();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteErrorLog(ex.Message);
        //    }
        //}

        private void GetInitData()
        {
            string sql = $@"select T1.*,t2.Caption,t2.Alias,t2.QueryStr,t2.Note,t3.Days 
                from [222_NotificationFunctionList] as t1 left join [222_ReportList] as t2 on t1.Data = t2.ID
                    left join [222_Notification_DataTime] as T3 on t1.DataTime = t3.ID
            ";
            WorkingEmail DAO = new WorkingEmail();
            var dt = DAO.LoadGridByStr(sql);
            clsBase.BindDataToGrid(dt, this, gridControl1, gridView1);
            foreach (DataRow r in dt.Rows)
            {
                FucntionObj obj = new FucntionObj();
                obj.Data = r["QueryStr"].ToString();
                obj.FileName = r["Alias"].ToString();
                obj.STT = int.Parse(r["STT"].ToString());
                obj.Time = new List<int>();
                obj.Subject = r["Subject"].ToString();
                obj.DataIndex = int.Parse(r["DataTime"].ToString());
                obj.DataTimeFrom = int.Parse(r["DataTimeFrom"].ToString());
                obj.DataTimeTo = int.Parse(r["DataTimeTo"].ToString());
                var time = r["TimeSend"].ToString().Split(';');
                foreach (var item in time)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }

                    int timeInt;
                    if (int.TryParse(item, out timeInt))
                    {
                        obj.Time.Add(timeInt);
                    }
                }
                listTime.Add(obj);
            }
        }

        private void Frm_WorkingEmail_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                CheckOnTime();
            }));
            thread.IsBackground = true;
            thread.Start();

        }

        private void Frm_WorkingEmail_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }

    internal class WorkingEmail : ClsConnect
    {
        internal DataTable GetDataForNotification(DateTime fromDate, DateTime toDate, string store)
        {
            var parameter = new SqlParameter[]
                   {
                    CreateParameter("@fromDate", SqlDbType.NVarChar, fromDate.ToString(Timezone)),
                    CreateParameter("@toDate", SqlDbType.NVarChar, toDate.ToString(Timezone))
                   };
            return DoStoreGetByID(store, parameter);
        }

        internal DataTable GetSEndingEmail(int sTT)
        {
            string sql = $@"select * from [222_NotificationSendingEmail] where FunctionSTT = {sTT}";
            return LoadGridByStr(sql);
        }
    }

    internal class FucntionObj
    {
        public int STT { get; set; }
        public List<int> Time { get; set; }
        public string Data { get; set; }
        public string FileName { get; set; }
        public string Subject { get; set; }
        public int DataIndex { get; set; }
        public int DataTimeFrom { get; set; }
        public int DataTimeTo { get; set; }
        public DateTime FromDate
        {
            get
            {
                DateTime result = DateTime.Now.Date;
                switch (this.DataIndex)
                {
                    case 2:
                        result = result.AddDays(-result.DayOfYear);
                        break;
                    case 3:
                        result = result.AddDays(-result.Day);
                        break;
                    case 4:
                    case 5:
                        result = result.AddDays(-DataTimeFrom);
                        break;
                }

                return result;
            }
        }
        public DateTime ToDate 
        {
            get
            {
                DateTime result = DateTime.Now.Date.AddDays(DataTimeTo);
                switch (this.DataIndex)
                {
                    case 1:
                    case 2:
                    case 3:
                        break;
                }

                return result;
            }
        }
    }
}
