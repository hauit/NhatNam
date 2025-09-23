using Common.DatabaseExecution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Test.Models.Entity;

namespace WPF_Test.Models.DAO
{
    public class WebUrlDAO : IDAO<WebUrl>
    {
        public override int ExecuteQuery(string sqlQuery)
        {
            var dbExecute = new DataExecutionWrapper<SqlConnection>(ServerInfor);
            return dbExecute.ExecuteQuery(sqlQuery);
        }

        public override List<WebUrl> GetAllData(string sqlQuery)
        {
            var dbExecute = new DataExecutionWrapper<SqlConnection>(ServerInfor);
            return dbExecute.LoadGridByStr<WebUrl>(sqlQuery);
        }

    }
}
