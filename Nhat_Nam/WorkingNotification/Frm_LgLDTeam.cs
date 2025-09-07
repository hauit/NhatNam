using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;

namespace WorkingNotification
{
    public partial class Frm_LgLDTeam : Form
    {
        public Frm_LgLDTeam()
        {
            InitializeComponent();

            txtEmail.Text = Settings.Default.LGSender;
            txtPassword.Text = Settings.Default.LGSMTP;
            txtLinkWeb.Text = Settings.Default.LinkWeb;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {//create table ListItem (ID text,Project text, ItemName text,DueDate datetime,CompleteDate datetime,TransferCompleteDate text, Ower text, Output text, Description text, Document text, Remarked text, PIC text, FeedbackTime datetime, Status text, EmailSentTime datetime);
             // create table Projects (Project text,StartMeeting datetime,Wrapup datetime,FADate datetime);
                var dataIndex = cbReport.SelectedIndex;
                switch (dataIndex) {
                    case 1: ImportItem();
                        break;
                    case 2: ImportProject();
                        break;
                    default: throw new ArgumentException("Please choose data!");
                }
                
                btnView_Click(null,null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ImportProject()
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
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                try
                {
                    //create table ListItem (ID text,Project text, ItemName text,DueDate datetime
                    //    ,CompleteDate datetime,TransferCompleteDate text
                    //    , Ower text, Output text, Description text, Document text
                    //    , Remarked text, PIC text, FeedbackTime datetime, Status text);
                    string project = dReader["Project"].ToString();
                    string faDate = dReader["FADate"].ToString();
                    string wrapup = dReader["Wrapup"].ToString();
                    string startMeeting = dReader["StartMeeting"].ToString();
                    DateTime FADate;
                    if(!DateTime.TryParse(faDate,out FADate))
                    {
                        throw new AggregateException("FADate wrong. Please confirm!");
                    }

                    DateTime Wrapup;
                    if(!DateTime.TryParse(wrapup,out Wrapup))
                    {
                        throw new AggregateException("Wrapup  wrong. Please confirm!");
                    }

                    DateTime StartMeeting;
                    if(!DateTime.TryParse(startMeeting,out StartMeeting))
                    {
                        throw new AggregateException("StartMeeting wrong. Please confirm!");
                    }

                    string sql = $@"Insert into Projects (Project,StartMeeting,Wrapup,FADate) 
                                values 
                                ('{project}'
                                ,'{StartMeeting.ToString("yyyy-MM-dd HH:mm:ss")}'
                                ,'{Wrapup.ToString("yyyy-MM-dd HH:mm:ss")}'
                                ,'{FADate.ToString("yyyy-MM-dd HH:mm:ss")}')";
                    sql = $@"Insert into Projects (Project,StartMeeting,Wrapup,FADate) 
                                values 
                                ('@Project'
                                ,'@StartMeeting'
                                ,'@Wrapup'
                                ,'@FADate')";
                    if(DoInsertProject(project,StartMeeting,Wrapup,FADate) == 0)
                    {
                        throw new AggregateException("Cannot input data to database. Please contact to Admin(minh.cao)!");
                    }
                }
                catch (Exception ex)
                {
                    if(excelConnection.State == ConnectionState.Open)
                    {
                        excelConnection.Close();
                    }
                    throw new AggregateException($@"Row number {line}  occur error {ex.Message} ");
                }
            }
            
            if(excelConnection.State == ConnectionState.Open)
            {
                excelConnection.Close();
            }
        }

        private int DoInsertProject(string project, DateTime startMeeting, DateTime wrapup, DateTime fADate)
        {
            string sql = $@"Insert into Projects (Project,StartMeeting,Wrapup,FADate) 
                        values 
                        ('{project}'
                        ,'{startMeeting.ToString("yyyy-MM-dd HH:mm:ss")}'
                        ,'{wrapup.ToString("yyyy-MM-dd HH:mm:ss")}'
                        ,'{fADate.ToString("yyyy-MM-dd HH:mm:ss")}')";
            sql = $@"Insert into Projects (Project,StartMeeting,Wrapup,FADate) 
                        values 
                        (@Project
                        ,@StartMeeting
                        ,@Wrapup
                        ,@FADate)";

            var parameter = new SQLiteParameter[]
                   {
                    CreateParameter("@Project", "text", project),
                    CreateParameter("@StartMeeting", "datetime", startMeeting.Date),
                    CreateParameter("@Wrapup", "datetime", wrapup.Date),
                    CreateParameter("@FADate", "datetime", fADate.Date)
                   };
            Base dao = new Base();
            return dao.DoInsert(sql, parameter);
        }

        private int DoInsertItem(string id, string project, string item, string transferCompleteDate, string ower, string output, string description, string pIC, DateTime duedate, string status)
        {
            string sql = $@"Insert into ListItem (ID,Project,ItemName,TransferCompleteDate,Ower,Output,Description,PIC,DueDate,Status) 
                                values 
                                (@ID,@Project,@ItemName,@TransferCompleteDate,@Ower,@Output,@Description,@PIC,@DueDate,@Status)";

            var parameter = new SQLiteParameter[]
                   {
                    CreateParameter("@ID", "text", id),
                    CreateParameter("@Project", "text", project),
                    CreateParameter("@ItemName", "text", item),
                    CreateParameter("@TransferCompleteDate", "text", transferCompleteDate),
                    CreateParameter("@Ower", "text", ower),
                    CreateParameter("@Output", "text", output),
                    CreateParameter("@Description", "text", description),
                    CreateParameter("@PIC", "text", pIC),
                    CreateParameter("@DueDate", "datetime", duedate),
                    CreateParameter("@Status", "text", status)
                   };
            Base dao = new Base();
            return dao.DoInsert(sql, parameter);
        }

        public static SQLiteParameter CreateParameter(string parameterName, string type, object value)
        {
            SQLiteParameter parameter = new SQLiteParameter();

            try
            {
                parameter.ParameterName = parameterName;
                parameter.TypeName = type;
                parameter.Value = value;

                return parameter;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error:" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        private void ImportItem()
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
            int line = 0;
            DateTime inputDate = DateTime.Now;
            while (dReader.Read())
            {
                line++;
                try
                {
                    //create table ListItem (ID text,Project text, ItemName text,DueDate datetime
                    //    ,CompleteDate datetime,TransferCompleteDate text
                    //    , Ower text, Output text, Description text, Document text
                    //    , Remarked text, PIC text, FeedbackTime datetime, Status text);
                    string project = dReader["Project"].ToString();
                    string item = dReader["ItemName"].ToString();
                    string transferCompleteDate = dReader["TransferCompleteDate"].ToString();
                    string ower = dReader["Ower"].ToString();
                    string output = dReader["Output"].ToString();
                    string description = dReader["Description"].ToString();
                    string pIC = dReader["PIC"].ToString();
                    if(pIC.IndexOf('@') == -1)
                    {
                        pIC += $@"@lge.com";
                    }
                    string date = dReader["DueDate"].ToString();
                    string status = "Processing";
                    DateTime duedate;
                    if(!DateTime.TryParse(date,out duedate))
                    {
                        throw new AggregateException("Đueate wrong. Please confirm!");
                    }
                    
                    if(DoInsertItem(inputDate.AddYears(20).AddSeconds(line).ToString("yy-MM-dd-HH-mm-ss"),project,item,transferCompleteDate,ower,output,description,pIC,duedate.Date,status) == 0)
                    {
                        throw new AggregateException("Cannot input data to database. Please contact to Admin(minh.cao)!");
                    }
                }
                catch (Exception ex)
                {
                    if(excelConnection.State == ConnectionState.Open)
                    {
                        excelConnection.Close();
                    }
                    throw new AggregateException($@"Row number {line}  occur error {ex.Message} ");
                }
            }
            
            if(excelConnection.State == ConnectionState.Open)
            {
                excelConnection.Close();
            }
        }

        private void Send1(string pic,string subject, string body)
        {
            string sender = "nguyenvuhau@cokhinhatnam.com.vn" ;
            string pass =  "k*M,eq_I%S@y";
            NetworkCredential loginInfo = new NetworkCredential(sender, pass);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(sender);

            
            msg.To.Add(new MailAddress("minh.cao@lge.com"));
            msg.To.Add(new MailAddress(pic));
            msg.Subject = subject.Replace("\n",string.Empty);
            msg.Body = body;
            msg.IsBodyHtml = true;
            string domain = "mail9096.maychuemail.com" ;
            SmtpClient client = new SmtpClient(domain);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = loginInfo;
            client.Port = 587;
            client.Timeout = 10000000;
            client.Send(msg);
        }

        private void Send(string pic,string subject, string body)
        {
            string sender = "minh.cao@lge.com";
            string smtp = "156.147.1.150";
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    sender = txtEmail.Text;
                    smtp = txtPassword.Text;
                }));
            }

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(sender);
            msg.To.Add(new MailAddress("minh.cao@lge.com"));
            msg.To.Add(new MailAddress(pic));
            
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            string domain = smtp;
            SmtpClient client = new SmtpClient(domain);
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = true;
            ////client.Credentials = loginInfo;
            ////client.Port = 587;
            //client.Timeout = 10000000;
            client.Send(msg);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                var dataIndex = cbReport.SelectedIndex;
                switch (dataIndex) {
                    case 1: GetAllItem();
                        break;
                    case 2: GetAllProject();
                        break;
                    default: throw new ArgumentException("Please choose data!");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetAllProject()
        {
            string sql = "";// Delete from ListItem";
            Base dao = new Base();
                //dao.ExecuteStr(sql);
            sql = "select * from Projects";
            var dt = dao.LoadGridByStr(sql);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }

        private void GetAllItem()
        {
            string sql = "";// Delete from ListItem";
            //sql = $@"update ListItem set EmailSentTime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where ID = '43-08-05-20-42-29' ";
            Base dao = new Base();
            //dao.ExecuteStr(sql);
            //sql = $@"select * from ListItem where DueDate <= '{DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")}' ";
            sql = "select * from ListItem";
            var dt = dao.LoadGridByStr(sql);
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //Send();
        }

        private void Frm_LgLDTeam_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                CheckOnTime();
            }));
            thread.IsBackground = true;
            thread.Start();

        }

        private void CheckOnTime()
        {
            while (true)
            {
                Thread.Sleep(1);
                try
                {
                    int time = 540;
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            time = (int)nTimeSendMail.Value;
                        }));
                    }

                    DateTime now = DateTime.Now;
                    int currentTime = now.Hour * 60 + now.Minute;
                    if (currentTime != time)
                    {
                        continue;
                    }

                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        try
                        {
                            GetAndSendEmail();
                        }
                        catch (Exception ex)
                        {
                            Log.WriteErrorLog(ex.Message);
                        }
                    }));
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLog(ex.Message);
                }
                Thread.Sleep(61000);
            }
        }

        private void GetAndSendEmail()
        {
            string sql = $@"select * from ListItem where DueDate <= '{DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")}' and Status <> 'Done'";
            Base dao = new Base();
            var dt = dao.LoadGridByStr(sql);
            if(dt.Rows.Count == 0)
            {
                return;
            }

            foreach(DataRow r in dt.Rows)
            {
                string pIC = r["PIC"].ToString();
                if(pIC.IndexOf('@') == -1)
                {
                    pIC += $@"@lge.com";
                }

                string web = "localhost:50931";
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        web = txtLinkWeb.Text;
                    }));
                }
                string id = HashID(r["ID"].ToString()).Replace("+","%2B");
                string subject = $@"[{r["Project"].ToString()}] Due date of task: : {r["ItemName"].ToString().Replace("'",string.Empty).Replace("\n",string.Empty)}  ";
                string body = $@"Dear PIC, <br> Today is the due date of task:<b>  {r["ItemName"].ToString().Replace("'",string.Empty).Replace("\n",string.Empty)} </b></br>
                        Please visit the link below to update the task status: </br>
                        <a href='http://{web}/home/about?id={id}'>http://192.168.1.1</a> </br>
                        Thank you!
                ";

                Send(pIC,subject,body);

                sql = $@"update ListItem set EmailSentTime = '{DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")}' where ID = '{r["ID"].ToString()}' ";
                dao.ExecuteStr(sql);
            }
        }

        private string HashID(string toEncrypt)
        {
            string s = md5("@LGEVH");
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(s));
            TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
            {
                Key = buffer,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] inArray = provider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }

        private string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        private byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    GetAndSendEmail();
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLog(ex.Message);
                }
            }));
            thread.IsBackground = true;
            thread.Start();
            //Configuration config = ConfigurationManager.OpenExeConfiguration(Application.CommonAppDataPath);
            //var a = config.AppSettings.Settings["LGSender"];
            //Settings.Default.LinkWeb = txtLinkWeb.Text;
            //Settings.Default.LGSender = txtEmail.Text;
            //Settings.Default.LGSMTP = txtPassword.Text;
        }
    }

    internal class Base
    {
        public SQLiteConnection GetConnect()
        {
            return new SQLiteConnection ($@"Data Source=C:\Sqlite\hau_test.db;Version=3;");
        }

        public int ExecuteStr(string sql)
        {
            SQLiteConnection conn = GetConnect();
            try
            {
                int result;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                    
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1000;
                result = cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        

        public int DoInsert(string sql, SQLiteParameter[] parameters)
        {
            SQLiteConnection conn = GetConnect();;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;
            foreach (SQLiteParameter sqlParameter in parameters)
            {
                cmd.Parameters.Add(sqlParameter);
            }

            int result = cmd.ExecuteNonQuery();
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }

            return result;
        }

        public DataTable LoadGridByStr(string sql)
        {
            SQLiteConnection conn = GetConnect();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                DataTable tbl = new DataTable();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 100000;
                SQLiteDataAdapter dar = new SQLiteDataAdapter(cmd);
                dar.Fill(tbl);
                    
                return tbl;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}
