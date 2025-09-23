using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DatabaseExecution
{
    public class DataExecutionWrapper<T> : IDatabaseWrapper<T>
    {
        private IDatabaseExecution<T> executor;
        IServiceCollection services = new ServiceCollection();
        private IServiceProvider serviceProvider
        {
            get
            {
                return services.BuildServiceProvider();
            }
        }
        public DataExecutionWrapper(DatabaseInfor.DataFile path)
        {
            executor = (IDatabaseExecution<T>) new SQLiteDatabaseExecution(path);
            //services.AddTransient<IDatabaseExecution<SQLiteConnection>>(provider => new SQLiteDatabaseExecution("blooo"));
            //executor = (IDatabaseExecution<T>)serviceProvider.GetRequiredService<IDatabaseExecution<SQLiteConnection>>();
        }
        public DataExecutionWrapper(DatabaseInfor.ServerSQL svInfor)
        {
            executor = (IDatabaseExecution<T>) new SQLDatabaseExecution(svInfor);
            //services.AddTransient<IDatabaseExecution<SqlConnection>>(provider => new SQLDatabaseExecution("alooo", string.Empty, string.Empty, string.Empty));
            //executor = (IDatabaseExecution<T>)serviceProvider.GetRequiredService<IDatabaseExecution<SqlConnection>>();
        }
        public int ExecuteQuery(string sqlQuery)
        {
            return executor.ExecuteQuery(sqlQuery);
        }

        public int ExecuteQuery<T, U>(string sqlQuery, U[] parameter)
        {
            return executor.ExecuteQuery<T, U>(sqlQuery, parameter);
        }

        public int ExecuteStore<T, U>(string store, U[] parameter)
        {
            return executor.ExecuteStore<T, U>(store, parameter);
        }

        public int ExecuteStore<T>(string store)
        {
            return executor.ExecuteStore<T>(store);
        }

        public List<T> LoadGridByStore<T, U>(string store, U[] parameter) where T : new()
        {
            return executor.LoadGridByStore<T, U>(store, parameter);
        }

        public List<T> LoadGridByStore<T>(string store) where T : new()
        {
            return executor.LoadGridByStore<T>(store);
        }

        public List<T> LoadGridByStr<T>(string sqlQuery) where T : new()
        {
            return executor.LoadGridByStr<T>(sqlQuery);
        }

        public List<T> LoadGridByStr<T, U>(string sqlQuery, U[] parameter) where T : new()
        {
            return executor.LoadGridByStr<T,U>(sqlQuery, parameter);
        }
    }
}
