using Base.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Base.Base;

namespace ProcessingWork.Base
{
    public class BaseDAO : ClsConnect
    {
        internal DataTable AccquireOrderNullPermission()
        {
            var parameter = new SqlParameter[]
            {
                CreateParameter("@StaffID", SqlDbType.NVarChar, ClsSession.staffID)
            };

            return DoStoreGetByID("sp_Hau_242_GetPermissionForOrderNull", parameter);
        }

        internal DataTable AcquirePermissionForUserManagement()
        {
            var parameter = new SqlParameter[]
            {
                CreateParameter("@StaffID", SqlDbType.NVarChar, ClsSession.staffID)
            };

            return DoStoreGetByID("sp_Hau_242_GetPermissionForUserManagement", parameter);
        }

        internal DataTable AcquirePermissionForChangingData()
        {
            var parameter = new SqlParameter[]
            {
                CreateParameter("@StaffID", SqlDbType.NVarChar, ClsSession.staffID)
            };

            return DoStoreGetByID("sp_Hau_242_GetPermissionForChangingData", parameter);
        }

        internal DataTable AcquirePermissionForPlanInputViaRegistration()
        {
            var parameter = new SqlParameter[]
            {
                CreateParameter("@StaffID", SqlDbType.NVarChar, ClsSession.staffID)
            };

            return DoStoreGetByID("sp_Hau_242_GetPermissionForPlanInputViaRegistration", parameter);
        }

        internal DataTable AcquirePermissionForInputtingViaRegistration()
        {
            var parameter = new SqlParameter[]
            {
                CreateParameter("@StaffID", SqlDbType.NVarChar, ClsSession.staffID)
            };

            return DoStoreGetByID("sp_Hau_242_GetPermissionForInputtingViaRegistration", parameter);
        }

        internal DataTable CheckPermissionMrHau()
        {
            var parameter = new SqlParameter[]
            {
                CreateParameter("@StaffID", SqlDbType.NVarChar, ClsSession.staffID)
            };

            return DoStoreGetByID("sp_Hau_242_GetPermissionForMrHauWorking", parameter);
        }
    }
}
