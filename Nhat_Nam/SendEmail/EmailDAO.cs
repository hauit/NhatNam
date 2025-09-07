using Base.Connect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmail
{
    public class EmailDAO : ClsConnect
    {
        public EmailObject GetEmailData()
        {
            string sql = "Select * from [222_SendEmailData]";
            var dt = LoadGridByStr(sql);
            EmailObject obj = new EmailObject();
            foreach (DataRow r in dt.Rows)
            {
                obj.Sender = r["Sender"].ToString().Trim();
                obj.Pass = r["Pass"].ToString().Trim();
                obj.Admin = r["Admin"].ToString().Trim();
                obj.STMP = r["STMP"].ToString().Trim();
            }

            return obj;
        }
    }
}
