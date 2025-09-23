using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DatabaseExecution
{
    public interface IDatabaseBase<T>
    {
        List<T> LoadGridByStr<T>(string sqlQuery) where T : new();
        List<T> LoadGridByStr<T,U>(string sqlQuery, U[] parameter) where T : new();
        List<T> LoadGridByStore<T, U>(string store, U[] parameter) where T : new();
        List<T> LoadGridByStore<T>(string store) where T : new();
        //public DataTable LoadGridByStr(string sql);
        int ExecuteQuery(string sql);
        int ExecuteQuery<T,U>(string sql, U[] parameter);
        int ExecuteStore<T, U>(string store, U[] parameter);
        int ExecuteStore<T>(string store);
    }
}
