using Common.DatabaseExecution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.Models.DAO
{
    public abstract class IDAO<T>
    {
        protected DatabaseInfor.ServerSQL ServerInfor { get; }

        protected IDAO()
        {
            DatabaseInfor.ServerSQL server = new DatabaseInfor.ServerSQL();
            server.Server = Properties.Settings.Default.Server;
            server.User = Properties.Settings.Default.User;
            server.Password = Properties.Settings.Default.Password;
            server.Database = Properties.Settings.Default.Database;
            ServerInfor = server;

        }
        public abstract List<T> GetAllData(string sqlQuery);
        public abstract int ExecuteQuery(string sql);
    }
}
