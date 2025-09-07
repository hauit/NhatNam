using Base.Connect;
using FastMember;
using ProcessingWork.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DAO.DatabaseDAO
{
    public class clsP_NhapOrderlapKHGCDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsP_NhapOrderlapKHGC> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
               nameof(clsP_NhapOrderlapKHGC.Date)
             , nameof(clsP_NhapOrderlapKHGC.MONumber)
             , nameof(clsP_NhapOrderlapKHGC.Note)
            }))
            {
                dtBulk.Load(reader);
            }
            return InsertBulk(dtBulk, "[242_P_NhapOrderlapKHGC]");
        }

        internal int DeleteAllExistedData()
        {
            string sql = "Delete [242_P_NhapOrderlapKHGC]";
            return ExecuteStr(sql);
        }
    }
}
