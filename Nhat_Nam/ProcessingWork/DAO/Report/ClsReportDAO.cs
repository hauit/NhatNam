using Base.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Base.Base;
using System.Data.SqlClient;

namespace ProcessingWork.DAO.Report
{
    public class ClsReportDAO : ClsConnect
    {
        internal DataTable GetReportList()
        {
            var parameter = new SqlParameter[]
                   {
                    CreateParameter("@StaffID", SqlDbType.NVarChar, ClsSession.staffID)
                   };
            return DoStoreGetByID("sp_GetReportByStaffID", parameter);
        }

        internal DataTable GetDataForReport(DateTime fromDate, DateTime toDate,string query)
        {
            var parameter = new SqlParameter[]
                   {
                        CreateParameter("@fromdate", SqlDbType.NVarChar, fromDate.ToString(Timezone)),
                        CreateParameter("@todate", SqlDbType.NVarChar, toDate.ToString(Timezone))
                   };
            return DoStoreGetByID(query, parameter);
        }
    }
}
