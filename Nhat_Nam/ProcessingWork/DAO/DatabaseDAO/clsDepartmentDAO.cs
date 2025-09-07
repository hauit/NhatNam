using Base.Connect;
using FastMember;
using ProcessingWork.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DAO.DatabaseDAO
{
    public class clsDepartmentDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsDepartment> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
              nameof(clsDepartment.ID)
             , nameof(clsDepartment.DeptCode )
             , nameof(clsDepartment.DeptName)
             , nameof(clsDepartment.Deleted)
            }))
            {
                dtBulk.Load(reader);
            }
            return InsertBulk(dtBulk, "[222_Department]");
        }

        internal int Update(clsDepartment b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsDepartment.ID)}", SqlDbType.NVarChar, b.ID),
                    CreateParameter($"@{nameof(clsDepartment.DeptCode)}", SqlDbType.NVarChar, b.DeptCode),
                    CreateParameter($"@{nameof(clsDepartment.DeptName)}", SqlDbType.NVarChar, b.DeptName),
                    CreateParameter($"@{nameof(clsDepartment.Deleted)}", SqlDbType.NVarChar, b.Deleted)
                };
            return DoStore("sp_222_Department_Update", parameter);
        }

        internal DataTable GetAllData()
        {
            string sql = $@"select * from View_222_Department";
            return LoadGridByStr(sql);
        }
    }
}
