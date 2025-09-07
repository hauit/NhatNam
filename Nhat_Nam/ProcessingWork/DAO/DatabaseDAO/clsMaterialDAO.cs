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
    public class clsMaterialDAO : ClsConnect
    {
        public int UpdateMaterial(clsMaterial obj)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($@"@{nameof(clsMaterial.DateModified)}", SqlDbType.NVarChar, obj.DateModified),
                    CreateParameter($@"@{nameof(clsMaterial.Density)}", SqlDbType.NVarChar, obj.Density),
                    CreateParameter($@"@{nameof(clsMaterial.ID)}", SqlDbType.NVarChar, obj.ID),
                    CreateParameter($@"@{nameof(clsMaterial.MaterialID)}", SqlDbType.NVarChar, obj.MaterialID),
                    CreateParameter($@"@{nameof(clsMaterial.StaffID)}", SqlDbType.NVarChar, obj.StaffID),
                    CreateParameter($@"@{nameof(clsMaterial.Type)}", SqlDbType.NVarChar, obj.Type),
                    CreateParameter($@"@{nameof(clsMaterial.UnitPrice)}", SqlDbType.NVarChar, obj.UnitPrice)
                };
            return DoStore("sp_242_Material_Update", parameter);
        }

        internal bool InputDataToDatabase(List<clsMaterial> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
              nameof(clsMaterial.ID)
            , nameof(clsMaterial.MaterialID)
            , nameof(clsMaterial.Density)
            , nameof(clsMaterial.UnitPrice)
            , nameof(clsMaterial.Type)
            , nameof(clsMaterial.DateModified)
            , nameof(clsMaterial.StaffID)
            , nameof(clsMaterial.Deleted)}))
            {
                dtBulk.Load(reader);
            }
            return InsertBulk(dtBulk, "[242_Material]");
        }
    }
}









