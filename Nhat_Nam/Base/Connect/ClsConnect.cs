using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
//using AutoUpdate;
using Base.Base;
using System.Data.Common;
using System.Threading;

namespace Base.Connect
{
    public class ClsConnect
    {
        Configuration config; //= ConfigurationManager.OpenExeConfiguration(Application.CommonAppDataPath);
        private SqlConnection conn;
        private DataTable dTable;
        protected string Timezone = clsConstant.TimeZone;
        public DataTable DTable { get; set; }
        private SqlDataAdapter dAdapter;
        public SqlDataAdapter DAdapter { get; set; }
        string key = "0310990486";

        public string Server;
        public string User;
        public string Password;
        public string Database;
        public SqlConnection GetConnect()
        {
            lock (this)
            {
                this.Server = Properties.Settings.Default.Server;
                this.User = Properties.Settings.Default.User;
                this.Password = Properties.Settings.Default.Pass;
                this.Database = Properties.Settings.Default.Database;
            }
            //config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            //this.Server = config.AppSettings.Settings["Server"].Value;
            //this.User = config.AppSettings.Settings["User"].Value;
            //this.Password = md5_Decrypt(config.AppSettings.Settings["Pass"].Value);
            //this.Database = config.AppSettings.Settings["Database"].Value;
            return new SqlConnection("Data Source= " + this.Server + ";Initial Catalog=" + this.Database + " ;User ID=" + this.User + " ;Password=" + this.Password);
        }

        public ClsConnect()
        {
            //ReadFile();
        }
        //Các hàm sử dụng với Sql string
        #region Các hàm sử dụng với Sql string
        //Hàm load datatable
        public DataTable LoadGridByStr(string sql)
        {
            try
            {
                lock (this)
                {
                    conn = GetConnect();
                    //if (conn.State != ConnectionState.Open)
                    //{
                        conn.Open();
                    //}

                    DataTable tbl = new DataTable();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 100000;
                    SqlDataAdapter dar = new SqlDataAdapter(cmd);
                    dar.Fill(tbl);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    InsertLogData(sql);
                    return tbl;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        //Hàm thực thi câu lệnh khác
        public int ExecuteStr(string sql)
        {
            try
            {
                lock (this)
                {
                    //InsertLogData(sql);
                    int result;
                    conn = GetConnect();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 1000;
                    result = cmd.ExecuteNonQuery();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public DataTable LoadGridByStr(string sql, SqlParameter[] parameter)
        {
            try
            {
                lock (this)
                {
                    conn = GetConnect();
                    //if (conn.State != ConnectionState.Open)
                    //{
                    conn.Open();
                    //}

                    DataTable tbl = new DataTable();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    foreach (SqlParameter sqlParameter in parameter)
                    {
                        cmd.Parameters.Add(sqlParameter);
                    }

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 100000;
                    SqlDataAdapter dar = new SqlDataAdapter(cmd);
                    dar.Fill(tbl);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    InsertLogData(sql);
                    return tbl;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        //// TODO: control transaction
        #region 
        public bool InsertBulkOneByOne(string listQuery,List<SqlParameter[]> listPar)
        {
            using (SqlConnection connection = GetConnect())
            {
                int i = 0;
                try
                {
                    connection.Open();
                    var a = connection.ConnectionTimeout;
                    using (var tmp = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in listPar)
                            {
                                SqlCommand cmd = new SqlCommand(listQuery, connection);
                                foreach (SqlParameter sqlParameter in item)
                                {
                                    cmd.Parameters.Add(sqlParameter);
                                }
                                cmd.Transaction = tmp;
                                bool inputSuccess = cmd.ExecuteNonQuery() > 0;
                                InsertLogData(listQuery, item);
                                //cmd.CommandText = sqlForLogging;// = new SqlCommand(sqlForLogging, connection);
                                //cmd.ExecuteNonQuery();
                                if (!inputSuccess)
                                {
                                    tmp.Rollback();
                                    return false;
                                }
                                i++;
                            }

                            tmp.Commit();
                        }
                        catch (Exception ex)
                        {
                            tmp.Rollback();
                            return false;
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool InsertBulkOneByOne(List<string> listQuery)
        {
            using (SqlConnection connection = GetConnect())
            {
                int i = 0;
                try
                {
                    connection.Open();
                    var a = connection.ConnectionTimeout;
                    using (var tmp = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in listQuery)
                            {
                                if(i == 1470)
                                {

                                }
                                SqlCommand cmd = new SqlCommand(item, connection);
                                cmd.Transaction = tmp;
                                bool inputSuccess = cmd.ExecuteNonQuery() > 0;
                                string log = item.Replace("'","''");
                                string sqlForLogging = string.Format("insert into [222_Log] (Username,Task,Description,Time,IpAddress,UserPC) values ('{0}','{1}','{2}',getdate(),'{3}','{4}')", ClsSession.User, "DHTD", log, Environment.MachineName, Environment.UserName);
                                cmd.CommandText = sqlForLogging;// = new SqlCommand(sqlForLogging, connection);
                                cmd.ExecuteNonQuery();
                                //if (!inputSuccess)
                                //{
                                //    tmp.Rollback();
                                //    throw new ArgumentException("Rollback: Fail to execute query");
                                //}
                                i++;
                                Thread.Sleep(10);
                            }

                            tmp.Commit();
                        }
                        catch(Exception ex)
                        {
                            tmp.Rollback();
                            throw new ArgumentException("Rollback: " + i + " - " + ex.Message);
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
        #endregion

        public bool InsertBulk(DataTable dataTable, string table)
        {
            conn = GetConnect();
            SqlBulkCopy bulkCopy = new SqlBulkCopy
                (
                conn,
                SqlBulkCopyOptions.TableLock |
                SqlBulkCopyOptions.FireTriggers |
                SqlBulkCopyOptions.UseInternalTransaction,
                null
                );
            bulkCopy.DestinationTableName = table;
            try
            {
                lock (this)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    bulkCopy.WriteToServer(dataTable);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                throw new ArgumentException(ex.Message);
            }
        }

        public int InsertLog(string sql)
        {
            int result = 0;
            try
            {
                lock (this)
                {
                    conn = GetConnect();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 1000;
                    result = cmd.ExecuteNonQuery();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        //thực thi câu lệnh khác kiểu void
        public void ExecuteVoidStr(string sql)
        {
            try
            {
                lock (this)
                {
                    int result;
                    conn = GetConnect();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 1000;
                    result = cmd.ExecuteNonQuery();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        #endregion

        // các hàm sử dụng với Storeprocedure
        #region Các hàm sử dụng với Storeprocedure
        //Hàm lấy table
        public DataTable DoStoreGetAll(string sql)
        {
            try
            {
                lock (this)
                {
                    conn = GetConnect();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;
                    dAdapter = new SqlDataAdapter();
                    dAdapter.SelectCommand = cmd;
                    dTable = new DataTable();
                    dAdapter.Fill(dTable);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return dTable;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return dTable;
            }
        }
        //Hàm lấy table có tham số truyền vào
        public DataTable DoStoreGetByID(string sql, SqlParameter[] parameters)
        {
            try
            {
                lock (this)
                {
                    conn = GetConnect();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;
                    foreach (
                    SqlParameter sqlParameter in parameters)
                    {
                        cmd.Parameters.Add(sqlParameter);
                    }
                    dAdapter = new SqlDataAdapter();
                    dAdapter.SelectCommand = cmd;
                    dTable = new DataTable();
                    dAdapter.Fill(dTable);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    return dTable;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return dTable;
            }
        }

        private void InsertLogData(string sql, SqlParameter[] parameters)
        {
            var a = parameters.ToList();
            string json = sql + ";" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ";" + ClsSession.User + ";";
            foreach (var item in a)
            {
                json += string.Format("[{0}] : '{1}'", item.ParameterName, item.SqlValue);
            }

            json = json.Replace(@"'", "");
            string sqlForLogging = string.Format("insert into [222_Log] (Username,Task,Description,Time,IpAddress,UserPC) values ('{0}','{1}','{2}',getdate(),'{3}','{4}')", ClsSession.User, sql, json,Environment.MachineName,Environment.UserName);
            InsertLog(sqlForLogging);
        }

        private void InsertLogData(string sql)
        {
            if (sql.ToUpper().StartsWith("SELECT"))
            {
                return;
            }

            sql = sql.Replace(@"'", "!!!");
            string sqlForLogging = string.Format("insert into [222_Log] (Username,Description,Time,IpAddress,UserPC) values ('{0}','{1}',getdate(),'{2}','{3}')", ClsSession.User, sql, Environment.MachineName, Environment.UserName);
            InsertLog(sqlForLogging);
        }

        //Hàm thực thi store khác
        public int DoStore(string sql, SqlParameter[] parameters)
        {
            int result = 0;
            try
            {
                lock (this)
                {
                    conn = GetConnect();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 60000;
                    foreach (
                        SqlParameter sqlParameter in parameters)
                    {
                        cmd.Parameters.Add(sqlParameter);
                    }

                    result = cmd.ExecuteNonQuery();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    InsertLogData(sql, parameters);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public void ExecuteStore(string sql)
        {
            lock (this)
            {
                int result;
                conn = GetConnect();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1000000;
                    result = cmd.ExecuteNonQuery();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                catch(Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    throw new ArgumentException(ex.Message);
                }
            }
        }
        #endregion

        #region Các hàm hệ thống khác
        //Hàm tạo create paramerters
        public static SqlParameter CreateParameter(string parameterName, SqlDbType type, object value)
        {
            SqlParameter parameter = new SqlParameter();

            try
            {
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = type;
                parameter.Value = value;

                return parameter;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error:" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        public SqlParameter CreateParameter1(string parameterName, SqlDbType type, object value)
        {
            SqlParameter parameter = new SqlParameter();

            try
            {
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = type;
                parameter.Value = value;

                return parameter;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error:" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        public SqlParameter CreateParameter1(string parameterName, SqlDbType type, string value)
        {
            SqlParameter parameter = new SqlParameter();

            try
            {
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = type;
                parameter.Value = value;

                return parameter;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error:" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        //Hàm đọc thông tin server từ file ini
        public string ReadFile()
        {
            FileStream myFStream = new FileStream("Info.ini", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader ReadFile = new StreamReader(myFStream);
            string folder = ReadFile.ReadLine();
            ReadFile.Close();
            return folder;
        }

        public void CreateDomainLAN(string domain)
        {
            string path = @"C:\Windows\System32\drivers\etc\hosts";
            FileStream myFStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            StreamReader ReadFile = new StreamReader(myFStream);
            string folder = ReadFile.ReadToEnd();
            
            if(folder.ToLower().IndexOf(domain) != -1)
            {
                return;
            }

            StreamWriter WriteFile = new StreamWriter(path, true);
            WriteFile.WriteLine(domain);
            WriteFile.Close();
        }

        //ghi thông tin vào file config
        public void WriteConfig(string server, string user, string pass, string database)
        {
            Properties.Settings.Default.Server = server;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.User = user;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Pass = pass;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Database = database;
            Properties.Settings.Default.Save();
            //config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            //config.AppSettings.Settings.Remove("Server");
            //config.AppSettings.Settings.Add("Server", server);
            //config.AppSettings.Settings.Remove("User");
            //config.AppSettings.Settings.Add("User", user);
            //config.AppSettings.Settings.Remove("Pass");
            //config.AppSettings.Settings.Add("Pass", md5_Encrypt(pass));
            //config.AppSettings.Settings.Remove("Database");
            //config.AppSettings.Settings.Add("Database", database);
            //config.Save(ConfigurationSaveMode.Modified);
        }

        //Hàm ghi thông tin vào file ini
        public void WriteFile(string server, string user, string pass, string database)
        {
            FileStream myFStream = new FileStream("Info.ini", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter WriteFile = new StreamWriter(myFStream);
            pass = md5_Encrypt(pass);
            WriteFile.WriteLine("Server:" + server + ";");
            WriteFile.WriteLine("User:" + user + ";");
            WriteFile.WriteLine("Password:" + pass + ";");
            WriteFile.WriteLine("Database:" + database + ";");
            WriteFile.Flush();
            WriteFile.Close();
        }

        public void WriteFile(string folder)
        {
            FileStream myFStream = new FileStream("Info.ini", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter WriteFile = new StreamWriter(myFStream);
            WriteFile.WriteLine(folder);
            WriteFile.Flush();
            WriteFile.Close();
        }

        //Hàm chek connection database
        public bool CheckConnect(string server, string database, string user, string pass)
        {
            try
            {
                conn = new SqlConnection("Data Source= " + server + ";Initial Catalog=" + database + " ;User ID=" + user + " ;Password=" + pass);
                conn.Open();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Hàm mã hóa mật khẩu của tài khoản
        public string md5_Encrypt(string toEncrypt)
        {
            string s = md5(key);
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
        //Hàm giải mã
        public string md5_Decrypt(string toDecrypt)
        {
            string s = md5(key);
            byte[] inputBuffer = Convert.FromBase64String(toDecrypt);
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(s));
            TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
            {
                Key = buffer,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] bytes = provider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return Encoding.UTF8.GetString(bytes);
        }
        //Hàm mã hóa mật khẩu SQL
        public byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        #endregion

        public void DoInsert(string sql, SqlParameter[] parameters)
        {
            conn = GetConnect();
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            foreach (
                SqlParameter sqlParameter in parameters)
            {
                cmd.Parameters.Add(sqlParameter);
            }

            cmd.ExecuteNonQuery();
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        #region hàm mã hóa mật khẩu dùng chung trong ERP
        public string EncryptPassword(string strEnCrypt, string key)
        {
            try
            {
                byte[] keyArr;
                byte[] EnCryptArr = Encoding.UTF8.GetBytes(strEnCrypt);
                MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider();
                keyArr = MD5Hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider();
                tripDes.Key = keyArr;
                tripDes.Mode = CipherMode.ECB;
                tripDes.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripDes.CreateEncryptor();
                byte[] arrResult = transform.TransformFinalBlock(EnCryptArr, 0, EnCryptArr.Length);
                return Convert.ToBase64String(arrResult, 0, arrResult.Length);
            }
            catch (Exception ex) { }
            return "";
        }
        #endregion

        public string GetServerFolder()
        {
            string folder = ReadFile();
            return folder;
        }

        public void WriteFolderUpdate(string folder)
        {
            WriteFile(folder);
        }

    }
}
