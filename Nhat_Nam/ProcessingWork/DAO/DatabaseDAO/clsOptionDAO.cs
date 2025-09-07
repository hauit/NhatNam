using Base.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessingWork.DataBase;
using System.Data;
using FastMember;
using System.Data.SqlClient;

namespace ProcessingWork.DAO.DatabaseDAO
{
    public class clsOptionDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsOption> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
              nameof(clsOption.ID)
             , nameof(clsOption.DeptCode )
             , nameof(clsOption.OptionID)
             , nameof(clsOption.Note)
             , nameof(clsOption.InputTime)
             , nameof(clsOption.Deleted)
            }))
            {
                dtBulk.Load(reader);
            }
            return InsertBulk(dtBulk, "[242_Option]");
        }

        internal DataTable GetAllData()
        {
            string sql = $@"select * from View_242_Option";
            return LoadGridByStr(sql);
        }

        internal int Update(clsOption b)
        {

            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsOption.ID)}", SqlDbType.NVarChar, b.ID),
                    CreateParameter($"@{nameof(clsOption.DeptCode)}"    , SqlDbType.NVarChar, b.DeptCode),
                    CreateParameter($"@{nameof(clsOption.OptionID)}"  , SqlDbType.NVarChar, b.OptionID),
                    CreateParameter($"@{nameof(clsOption.Note)}" , SqlDbType.NVarChar, b.Note),
                    CreateParameter($"@{nameof(clsOption.Deleted)}", SqlDbType.Bit, b.Deleted)
                };
            return DoStore("sp_242_Option_Update", parameter);
        }
    }
}
