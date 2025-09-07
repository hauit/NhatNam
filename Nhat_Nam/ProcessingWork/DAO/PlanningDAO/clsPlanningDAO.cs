using Base.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessingWork.DAO.Database;
using FastMember;
using System.Data.SqlClient;

namespace ProcessingWork.DAO.PlanningDAO
{
    public class clsPlanningDAO : ClsConnect
    {
        internal DataTable GetOptionDataForPlann()
        {
            return DoStoreGetAll("sp_P_LayNCLam_KH");
        }

        internal DataTable GetProcessingResult(string shift, DateTime fromDate, DateTime toDate)
        {
            var parameter = new SqlParameter[]
                   {
                    CreateParameter("@Shift", SqlDbType.NVarChar, shift),
                    CreateParameter("@fromdate", SqlDbType.NVarChar, fromDate.ToString(Timezone)),
                    CreateParameter("@todate", SqlDbType.NVarChar, toDate.ToString(Timezone))
                   };
            return DoStoreGetByID("sp_242_ResultMachineWTS", parameter);
        }

        internal DataTable GetProcessingResult(DateTime fromDate, DateTime toDate)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter("@fromdate", SqlDbType.NVarChar, fromDate.ToString(Timezone)),
                    CreateParameter("@todate", SqlDbType.NVarChar, toDate.ToString(Timezone))
                };
            return DoStoreGetByID("sp_242_ResultMachineWTS_AllShift", parameter);
        }

        public void CreateOrderNull(string part, string slg, string jigNo)
        {
            var parameter = new SqlParameter[]
                   {
                    CreateParameter("@part", SqlDbType.NVarChar, part),
                    CreateParameter("@slg", SqlDbType.NVarChar, slg),
                    CreateParameter("@jigNo", SqlDbType.NVarChar, jigNo)
                   };
            DoStoreGetByID("sp_242_CreateOrderNull_New", parameter);
        }

        public int UpdateNullOrder(string order, string ordernull, string store)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter("@order", SqlDbType.NVarChar, order),
                    CreateParameter("@ordernull", SqlDbType.NVarChar, ordernull)
                };
            return DoStore(store, parameter);
        }
    }
}
