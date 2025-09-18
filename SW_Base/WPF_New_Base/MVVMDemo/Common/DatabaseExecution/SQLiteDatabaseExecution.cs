using System.Data.SQLite;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace Common.DatabaseExecution
{
    public class SQLiteDatabaseExecution : IDatabaseExecution<SQLiteConnection>
    {
        public string DataFile { get { return dataFile; } }
        private string dataFile = string.Empty;

        public SQLiteDatabaseExecution(DatabaseInfor.DataFile dataFile)
        {
            this.dataFile = dataFile.FilePath;
        }

        public int ExecuteQuery(string sql)
        {
            try
            {
                int result;
                using (SQLiteConnection connection = GetConnect())
                {
                    connection.Open();

                    SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 1000;
                    result = cmd.ExecuteNonQuery();
                }
                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public int ExecuteQuery<T, U>(string sql, U[] parameter)
        {
            try
            {
                int result;
                using (SQLiteConnection connection = GetConnect())
                {
                    connection.Open();

                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    if (!(parameter is SQLiteParameter[]))
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
                            SQLiteParameter obj = new SQLiteParameter("@" + property.Name, value ?? DBNull.Value);
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

        public List<T> LoadGridByStr<T>(string sqlQuery) where T : new()
        {
            List<T> resultList = new List<T>();
            try
            {
                using (SQLiteConnection connection = GetConnect())
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(sqlQuery, connection);

                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        T item = new T();
                        CreateObjectFromData(reader, item);
                        resultList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return resultList;
        }

        public List<T> LoadGridByStr<T, U>(string sqlQuery, U[] parameter) where T : new()
        {
            throw new NotImplementedException();
        }

        private SQLiteConnection GetConnect()
        {
            string folder = Environment.CurrentDirectory;
            return new SQLiteConnection($@"Data Source={DataFile};Version=3;");
        }

        private void CreateObjectFromData<T>(SQLiteDataReader reader, T item)
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

        public List<T> LoadGridByStore<T, U>(string store, U[] parameter) where T : new()
        {
            throw new NotImplementedException();
        }

        public List<T> LoadGridByStore<T>(string store) where T : new()
        {
            throw new NotImplementedException();
        }

        public int ExecuteStore<T, U>(string store, U[] parameter)
        {
            throw new NotImplementedException();
        }

        public int ExecuteStore<T>(string store)
        {
            throw new NotImplementedException();
        }
    }
}
