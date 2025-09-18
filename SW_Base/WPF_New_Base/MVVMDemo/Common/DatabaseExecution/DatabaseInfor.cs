using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DatabaseExecution
{
    public class DatabaseInfor
    {
        public class DataFile
        {
            public string FilePath { get; set; }
        }

        public class ServerSQL
        {
            public string Server { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string Database { get; set; }
        }
    }
}
