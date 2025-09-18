using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DatabaseExecution
{
    public interface ISQLCommandWrapper
    {
        CommandType CommandType { get; set; }
        int CommandTimeout { get; set; }
        SqlParameterCollection Parameters { get; }
        SqlDataReader ExecuteReader();
        int ExecuteNonQuery();
    }

    public class SqlCommandWrapper : ISQLCommandWrapper
    {
        private readonly SqlCommand sqlCommand;

        public CommandType CommandType { get { return this.sqlCommand.CommandType; } set { this.sqlCommand.CommandType = value; } }
        public int CommandTimeout { get { return this.sqlCommand.CommandTimeout; } set { this.sqlCommand.CommandTimeout = value; } }

        public SqlParameterCollection Parameters { get { return this.sqlCommand.Parameters; } }

        public SqlCommandWrapper(SqlCommand sqlCommand)
        {
            this.sqlCommand = sqlCommand;
        }

        public SqlDataReader ExecuteReader()
        {
            return sqlCommand.ExecuteReader();
        }

        public int ExecuteNonQuery()
        {
            return sqlCommand.ExecuteNonQuery();
        }
    }
}
