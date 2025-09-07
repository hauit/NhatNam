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
    public class clsPartDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsPart> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] { nameof(clsPart.ID), nameof(clsPart.PartNo),
                                            nameof(clsPart.PartName), nameof(clsPart.CustomerID), nameof(clsPart.SupplierID),
                                            nameof(clsPart.UpQty), nameof(clsPart.CatID),nameof(clsPart.Unit),nameof(clsPart.IsTool),
                                            nameof(clsPart.GiaThanh), nameof(clsPart.Deleted)
                                            , nameof(clsPart.NVL), nameof(clsPart.LoiNhuan), nameof(clsPart.VanChuyen)
                                            , nameof(clsPart.MotGio), nameof(clsPart.TachNC), nameof(clsPart.DoGa)}))
            {
                dtBulk.Load(reader);
            }
            var parameter = new SqlParameter[]
                {
                    new SqlParameter($"@List242_Part",dtBulk)
                };
            return DoStore("sp_242_Part_Insert", parameter) > 0;
        }

        internal int UpdateData(clsPart b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter("@ID", SqlDbType.NVarChar, b.ID),
                    CreateParameter("@PartNo", SqlDbType.NVarChar, b.PartNo),
                    CreateParameter("@CustomerID", SqlDbType.NVarChar, b.CustomerID),
                    CreateParameter("@PartName", SqlDbType.NVarChar, b.PartName),
                    CreateParameter("@SupplierID", SqlDbType.NVarChar, b.SupplierID),
                    CreateParameter("@GiaThanh", SqlDbType.Int, b.GiaThanh),
                    CreateParameter("@UpQty", SqlDbType.Int, b.UpQty),
                    CreateParameter("@Deleted", SqlDbType.Bit, b.Deleted)
                };
            return DoStore("sp_242_Part_Update", parameter);
        }

        internal DataTable GetAlExistedPart()
        {
            string sql = $"select * from [View_242_Part]";
            return LoadGridByStr(sql);
        }
    }
}
