using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Common.DatabaseExecution
{
    public class SQLDatabaseExecution : IDatabaseExecution<SqlConnection>
    {
        public string Server { get;}  
        public string User { get;}
        public string Password { get;}
        public string Database { get;}

        public SQLDatabaseExecution(DatabaseInfor.ServerSQL svInfor)
        {
            this.Server = svInfor.Server;
            this.User = svInfor.User;
            this.Password = svInfor.Password;
            this.Database = svInfor.Database;
        }

        private SqlConnection GetConnect()
        {
            //this.Server = Properties.Settings.Default.Server;
            //this.User = Properties.Settings.Default.User;
            //this.Password = Properties.Settings.Default.Pass;
            //this.Database = Properties.Settings.Default.Database;
            return new SqlConnection("Data Source= " + this.Server + ";Initial Catalog=" + this.Database + " ;User ID=" + this.User + " ;Password=" + this.Password);
        }

        //List<T> IDatabaseExecution<SqlConnection>.LoadGridByStr<T>(string sqlQuery)
        //{
        //    List<T> resultList = new List<T>();
        //    try
        //    {
        //        using (SqlConnection connection = GetConnect())
        //        {
        //            connection.Open();
        //            ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(sqlQuery, connection));
        //            command.CommandType = CommandType.Text;
        //            SqlDataReader reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                T item = new T();

        //                CreateObjectFromData(reader, item);

        //                resultList.Add(item);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new ArgumentException(ex.Message);
        //    }

        //    return resultList;
        //}
        
        public List<T> LoadGridByStr<T>(string sqlQuery) where T : new()
        {
            List<T> resultList = new List<T>();
            try
            {
                using (SqlConnection connection = GetConnect())
                {
                    connection.Open();
                    ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(sqlQuery, connection));
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        T item = new T();

                        CreateObjectFromData(reader, item);

                        resultList.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return resultList;
        }

        private void CreateObjectFromData<T>(SqlDataReader reader, T item)
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (reader[property.Name] != DBNull.Value)
                {
                    property.SetValue(item, reader[property.Name]);
                }
            }
        }

        public int ExecuteQuery(string sqlQuery)
        {
            try
            {
                int result;
                using (SqlConnection connection = GetConnect())
                {
                    connection.Open();

                    ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(sqlQuery, connection));
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 1000;
                    result = command.ExecuteNonQuery(); 
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public int ExecuteQuery<T,U>(string sqlQuery, U[] parameter)
        {
            try
            {
                int result;
                using (SqlConnection connection = GetConnect())
                {
                    connection.Open();
                    
                    ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(sqlQuery, connection));
                    if (!(parameter is SqlParameter[]))
                    {
                        throw new ArgumentException("Tham số đầu vào không phải là kiểu SqlParameter");
                    }

                    //TODO: Check convert from generic to sqlparameter
                    for (int i = 0; i < parameter.Length; i ++)
                    {
                        PropertyInfo[] properties = typeof(U).GetProperties();
                        foreach (var property in properties)
                        {
                            object value = property.GetValue(parameter[i]);
                            SqlParameter obj = new SqlParameter("@" + property.Name, value ?? DBNull.Value);
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 1000;
                    result = command.ExecuteNonQuery();
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public List<T> LoadGridByStr<T, U>(string sqlQuery, U[] parameter) where T : new()
        {
            List<T> resultList = new List<T>();

            using (SqlConnection connection = GetConnect())
            {
                connection.Open();
                ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(sqlQuery, connection));
                command.CommandType = CommandType.Text;
                if (!(parameter is SqlParameter[]))
                {
                    throw new ArgumentException("Tham số đầu vào không phải là kiểu SqlParameter");
                }

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    T item = new T();

                    CreateObjectFromData(reader, item);

                    resultList.Add(item);
                }
            }

            return resultList;
        }

        public List<T> LoadGridByStore<T, U>(string store, U[] parameter) where T : new()
        {
            List<T> resultList = new List<T>();

            using (SqlConnection connection = GetConnect())
            {
                connection.Open();
                ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(store, connection));
                command.CommandType = CommandType.StoredProcedure;
                if (!(parameter is SqlParameter[]))
                {
                    throw new ArgumentException("Tham số đầu vào không phải là kiểu SqlParameter");
                }

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    T item = new T();

                    CreateObjectFromData(reader, item);

                    resultList.Add(item);
                }
            }

            return resultList;
        }

        public List<T> LoadGridByStore<T>(string store) where T : new()
        {
            List<T> resultList = new List<T>();

            using (SqlConnection connection = GetConnect())
            {
                connection.Open();
                ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(store, connection));
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    T item = new T();

                    CreateObjectFromData(reader, item);

                    resultList.Add(item);
                }
            }

            return resultList;
        }

        public int ExecuteStore<T, U>(string store, U[] parameter)
        {
            try
            {
                int result;
                using (SqlConnection connection = GetConnect())
                {
                    connection.Open();

                    ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(store, connection));
                    if (!(parameter is SqlParameter[]))
                    {
                        throw new ArgumentException("Tham số đầu vào không phải là kiểu SqlParameter");
                    }

                    //TODO: Check convert from generic to sqlparameter
                    for (int i = 0; i < parameter.Length; i++)
                    {
                        PropertyInfo[] properties = typeof(U).GetProperties();
                        foreach (var property in properties)
                        {
                            object value = property.GetValue(parameter[i]);
                            SqlParameter obj = new SqlParameter("@" + property.Name, value ?? DBNull.Value);
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1000;
                    result = command.ExecuteNonQuery();
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public int ExecuteStore<T>(string store)
        {
            try
            {
                int result;
                using (SqlConnection connection = GetConnect())
                {
                    connection.Open();

                    ISQLCommandWrapper command = new SqlCommandWrapper(new SqlCommand(store, connection));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1000;
                    result = command.ExecuteNonQuery();
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
