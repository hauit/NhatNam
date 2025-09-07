using Base.Base;
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
    public class clsBusOrderDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsBusOrder> listPart)
        {
            DataTable dtBulk = ConvertFormListToDataTable(listPart);

            var parameter = new SqlParameter[]
                {
                    new SqlParameter($"@BusOrderList",dtBulk)
                };
            return DoStore("sp_242_BusOder_Insert", parameter) > 0;
        }

        private DataTable ConvertFormListToDataTable(List<clsBusOrder> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
               nameof(clsBusOrder.BOderNo      )
             , nameof(clsBusOrder.Date        )
             , nameof(clsBusOrder.PlanNo          )
             , nameof(clsBusOrder.PartID          )
             , nameof(clsBusOrder.Qty         )
             , nameof(clsBusOrder.Deadline    )
             , nameof(clsBusOrder.RawQty          )
             , nameof(clsBusOrder.HelisertQty  )
             , nameof(clsBusOrder.BlastQty    )
             , nameof(clsBusOrder.MONo        )
             , nameof(clsBusOrder.MOQty       )
             , nameof(clsBusOrder.Started     )
             , nameof(clsBusOrder.Finished    )
             , nameof(clsBusOrder.FinishDate      )
             , nameof(clsBusOrder.Change          )
             , nameof(clsBusOrder.ChangeDate      )
             , nameof(clsBusOrder.Imported    )
             , nameof(clsBusOrder.ImportFrom      )
             , nameof(clsBusOrder.Note        )
             , nameof(clsBusOrder.Deleted        )
             , nameof(clsBusOrder.OrderGoc        )
             , nameof(clsBusOrder.OrderCat        )
             , nameof(clsBusOrder.NoiCat        )
             , nameof(clsBusOrder.THVatLieu        )
             , nameof(clsBusOrder.THPhoi        )
             , nameof(clsBusOrder.TempOrder        )
             , nameof(clsBusOrder.Status        )
             , nameof(clsBusOrder.Paid        )
             , nameof(clsBusOrder.PayDate        )
            }))
            {
                dtBulk.Load(reader);
            }
            return dtBulk;
        }

        internal int Update(clsBusOrder b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsBusOrder.ID)}", SqlDbType.NVarChar, b.ID),
                    CreateParameter($"@{nameof(clsBusOrder.BOderNo)}", SqlDbType.NVarChar, b.BOderNo),
                    CreateParameter($"@{nameof(clsBusOrder.Date)}", SqlDbType.NVarChar, b.Date.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsBusOrder.PlanNo)}", SqlDbType.NVarChar, b.PlanNo),
                    CreateParameter($"@{nameof(clsBusOrder.PartID)}", SqlDbType.NVarChar, b.PartID),
                    CreateParameter($"@{nameof(clsBusOrder.Qty)}", SqlDbType.NVarChar, b.Qty),
                    CreateParameter($"@{nameof(clsBusOrder.Deadline)}", SqlDbType.NVarChar, b.Deadline.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsBusOrder.RawQty)}", SqlDbType.NVarChar, b.RawQty),
                    CreateParameter($"@{nameof(clsBusOrder.HelisertQty)}", SqlDbType.NVarChar, b.HelisertQty),
                    CreateParameter($"@{nameof(clsBusOrder.BlastQty)}", SqlDbType.NVarChar, b.BlastQty),
                    CreateParameter($"@{nameof(clsBusOrder.MONo)}", SqlDbType.NVarChar, b.MONo),
                    CreateParameter($"@{nameof(clsBusOrder.MOQty)}", SqlDbType.NVarChar, b.MOQty),
                    CreateParameter($"@{nameof(clsBusOrder.Started)}", SqlDbType.NVarChar, b.Started),
                    CreateParameter($"@{nameof(clsBusOrder.Finished)}", SqlDbType.NVarChar, b.Finished),
                    CreateParameter($"@{nameof(clsBusOrder.FinishDate)}", SqlDbType.NVarChar, b.FinishDate.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsBusOrder.Change)}", SqlDbType.NVarChar, b.Change),
                    CreateParameter($"@{nameof(clsBusOrder.ChangeDate)}", SqlDbType.NVarChar, b.ChangeDate.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsBusOrder.Imported)}", SqlDbType.NVarChar, b.Imported),
                    CreateParameter($"@{nameof(clsBusOrder.ImportFrom)}", SqlDbType.NVarChar, b.ImportFrom),
                    CreateParameter($"@{nameof(clsBusOrder.Note)}", SqlDbType.NVarChar, b.Note)
                };
            return DoStore("sp_242_BusOder_Update", parameter);
        }

        public DataTable GetAllExistedOrder()
        {
            string sql = $"select * from [View_242_BusOder]";
            return LoadGridByStr(sql);
        }

        internal bool PaymentStatus(List<clsBusOrder> listPart)
        {
            DataTable dtBulk = ConvertFormListToDataTable(listPart);

            var parameter = new SqlParameter[]
                {
                    new SqlParameter($"@BusOrderList",dtBulk),
                    CreateParameter($"@StaffID", SqlDbType.NVarChar, ClsSession.staffID),
                };
            return DoStore("sp_242_BusOder_PaymentStatus", parameter) > 0;
        }

        internal bool BusOrderOrderCat(List<clsBusOrder> listPart)
        {
            DataTable dtBulk = ConvertFormListToDataTable(listPart);

            var parameter = new SqlParameter[]
                {
                    new SqlParameter($"@BusOrderList",dtBulk),
                    CreateParameter($"@StaffID", SqlDbType.NVarChar, ClsSession.staffID),
                };
            return DoStore("sp_242_BusOder_OrderCat", parameter) > 0;
        }

        internal bool FinishOrder(List<clsBusOrder> listPart)
        {
            DataTable dtBulk = ConvertFormListToDataTable(listPart);
            var parameter = new SqlParameter[]
                {
                    new SqlParameter($"@BusOrderList",dtBulk),
                    CreateParameter($"@StaffID", SqlDbType.NVarChar, ClsSession.staffID),
                };
            return DoStore("sp_242_BusOder_Finish", parameter) > 0;
        }

        internal DataTable GetOrderData(List<clsBusOrder> listOrder)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listOrder, new string[] {
                nameof(clsBusOrder.PartID)
            }))
            {
                dtBulk.Load(reader);
            }

            var parameter = new SqlParameter[]
                {
                    new SqlParameter($"@BusOrderList",dtBulk)
                };
            return DoStoreGetByID("sp_242_BusOder_InToLenh", parameter);
        }

        internal bool BusOrderStatus(List<clsBusOrder> listPart)
        {
            DataTable dtBulk = ConvertFormListToDataTable(listPart);

            var parameter = new SqlParameter[]
                {
                    new SqlParameter($"@BusOrderList",dtBulk),
                    CreateParameter($"@StaffID", SqlDbType.NVarChar, ClsSession.staffID),
                };
            return DoStore("sp_242_BusOder_Status", parameter) > 0;
        }

        internal bool UpdateToDatabase(List<clsBusOrder> listPart)
        {
            DataTable dtBulk = ConvertFormListToDataTable(listPart);

            var parameter = new SqlParameter[]
                {
                    new SqlParameter($"@BusOrderList",dtBulk),
                    CreateParameter($"@StaffID", SqlDbType.NVarChar, ClsSession.staffID),
                };
            return DoStore("sp_242_BusOder_ChangeDeadLine", parameter) > 0;
        }
    }
}
