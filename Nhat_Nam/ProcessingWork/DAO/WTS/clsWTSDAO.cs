using Base.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ProcessingWork.DataBase;
using Base.Base;

namespace ProcessingWork.DAO.WTS
{
    public class clsWTSDAO : ClsConnect
    {
        internal DataTable GetAllWTS(DateTime fromDate, DateTime toDate, string MachineID, string Shift,string staff)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@fromDate", SqlDbType.NVarChar, fromDate.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsWTS.MachineID)}", SqlDbType.NVarChar, MachineID),
                    CreateParameter($"@{nameof(clsWTS.Shift)}", SqlDbType.NVarChar, Shift),
                    CreateParameter($"@{nameof(clsWTS.StaffID)}", SqlDbType.NVarChar, staff),
                    CreateParameter($"@toDate", SqlDbType.NVarChar, toDate.ToString(Timezone)),
                    CreateParameter($"@StaffCheck", SqlDbType.NVarChar, ClsSession.staffID)
                };
            return DoStoreGetByID("sp_242_WTS_GetAll", parameter);
        }

        internal int Update(clsWTS b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsWTS.Date)}", SqlDbType.NVarChar, b.Date.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsWTS.Deleted)}", SqlDbType.NVarChar, b.Deleted),
                    CreateParameter($"@{nameof(clsWTS.ID)}", SqlDbType.Int, b.ID),
                    CreateParameter($"@{nameof(clsWTS.IDPlan)}", SqlDbType.Int, b.IDPlan),
                    CreateParameter($"@{nameof(clsWTS.MachineID)}", SqlDbType.NVarChar, b.MachineID),
                    CreateParameter($"@{nameof(clsWTS.MONo)}", SqlDbType.NVarChar, b.MONo),
                    CreateParameter($"@{nameof(clsWTS.NGQty)}", SqlDbType.Int, b.NGQty),
                    CreateParameter($"@{nameof(clsWTS.NGNCTruoc)}", SqlDbType.Int, b.NGNCTruoc),
                    CreateParameter($"@{nameof(clsWTS.Note)}", SqlDbType.NVarChar, b.Note),
                    CreateParameter($"@{nameof(clsWTS.OKQty)}", SqlDbType.Int, b.OKQty),
                    CreateParameter($"@{nameof(clsWTS.OptionID)}", SqlDbType.NVarChar, b.OptionID),
                    CreateParameter($"@{nameof(clsWTS.Shift)}", SqlDbType.NVarChar, b.Shift),
                    CreateParameter($"@{nameof(clsWTS.StaffID)}", SqlDbType.NVarChar, b.StaffID),
                    CreateParameter($"@{nameof(clsWTS.Time)}", SqlDbType.Decimal, b.Time),
                    CreateParameter($"@{nameof(clsWTS.WorkID)}", SqlDbType.NVarChar, b.WorkID),
                    CreateParameter($"@{nameof(clsWTS.ClampTime)}", SqlDbType.NVarChar, b.ClampTime),
                    CreateParameter($"@{nameof(clsWTS.PartID)}", SqlDbType.NVarChar, b.PartID),
                    CreateParameter($"@{nameof(clsWTS.ProTime)}", SqlDbType.NVarChar, b.ProTime),
                    CreateParameter($"@{nameof(clsWTS.Note1)}", SqlDbType.NVarChar, b.Note1),
                    CreateParameter($"@{nameof(clsWTS.Note2)}", SqlDbType.NVarChar, b.Note2),
                    CreateParameter($"@{nameof(clsWTS.Status)}", SqlDbType.NVarChar, b.Status),
                    CreateParameter($"@UpdateStaff", SqlDbType.NVarChar, ClsSession.staffID)
                };
            return DoStore("sp_242_WTS_InsertOrUpdate", parameter);
        }

        internal List<clsWTS> GetQtyInputed(clsWTS obj)
        {
            string sql = $@"select {nameof(clsWTS.OptionID)},T2.{nameof(clsBusOrder.Qty)}
                    ,Sum({nameof(clsWTS.OKQty)}) as {nameof(clsWTS.OKQty)}
                    ,Sum({nameof(clsWTS.NGQty)}) as {nameof(clsWTS.NGQty)}  
                from View_242_WTS as t1 left join View_242_BusOder as T2 on T1.{nameof(clsWTS.MONo)} = T2.{nameof(clsBusOrder.BOderNo)}
                where T1.{nameof(clsWTS.MONo)} = @{nameof(clsWTS.MONo)} and ({nameof(clsWTS.OKQty)} + {nameof(clsWTS.NGQty)}) > 0 
                    and T1.{nameof(clsWTS.OptionID)} =  @{nameof(clsWTS.OptionID)}
                    and T1.{nameof(clsWTS.ID)} <> @{nameof(clsWTS.ID)}
                group by {nameof(clsWTS.OptionID)},T2.{nameof(clsBusOrder.Qty)}
                order by {nameof(clsWTS.OptionID)} DESC";
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsWTS.MONo)}", SqlDbType.NVarChar, obj.MONo),
                    CreateParameter($"@{nameof(clsWTS.ID)}", SqlDbType.Int, obj.ID),
                    CreateParameter($"@{nameof(clsWTS.OptionID)}", SqlDbType.NVarChar, obj.OptionID)
                };
            DataTable dt = LoadGridByStr(sql, parameter);
            List<clsWTS> result = new List<clsWTS>();
            foreach (DataRow r in dt.Rows)
            {
                clsWTS item = new clsWTS();
                item.OptionID = r["OptionID"].ToString().Trim();
                item.OKQty = int.Parse(r["OKQty"].ToString().Trim());
                item.NGQty = int.Parse(r["NGQty"].ToString().Trim());
                result.Add(item);
            }

            return result;
        }

        internal DataTable QuyenSuaSauDuyet()
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@staffID", SqlDbType.NVarChar, ClsSession.staffID)
                };
            return DoStoreGetByID("sp_242_WTS_SuaSauDuyet", parameter);
        }

        internal int GetQtyOfOrder(clsWTS obj)
        {
            string sql = $@"select * from View_242_BusOder where {nameof(clsBusOrder.BOderNo)} = @{nameof(clsBusOrder.BOderNo)}";
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsBusOrder.BOderNo)}", SqlDbType.NVarChar, obj.MONo)
                };
            DataTable dt = LoadGridByStr(sql, parameter);
            if(dt.Rows.Count == 0)
            {
                throw new ArgumentException("Không tìm thấy thông tin order trong BusOder");
            }

            int qty = int.Parse(dt.Rows[0]["Qty"].ToString().Trim());
            return qty;
        }
    }
}
