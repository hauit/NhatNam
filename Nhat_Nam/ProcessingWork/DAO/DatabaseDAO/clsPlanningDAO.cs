using Base.Connect;
using FastMember;
using ProcessingWork.DAO.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessingWork.DataBase;

namespace ProcessingWork.DAO.DatabaseDAO
{
    public class clsPlanningDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsPlanning> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
              nameof(clsPlanning.ID)
             , nameof(clsPlanning.K )
             , nameof(clsPlanning.Date)
             , nameof(clsPlanning.Shift    )
             , nameof(clsPlanning.MachineID    )
             , nameof(clsPlanning.OptionID)
             , nameof(clsPlanning.MONo)
             , nameof(clsPlanning.Dept    )
             , nameof(clsPlanning.Note )
             , nameof(clsPlanning.Deleted )
            }))
            {
                dtBulk.Load(reader);
            }
            return InsertBulk(dtBulk, "[242_Planning]");
        }

        internal int Update(clsPlanning b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsPlanning.ID)}", SqlDbType.NVarChar, b.ID),
                    CreateParameter($"@{nameof(clsPlanning.K)}", SqlDbType.NVarChar, b.K),
                    CreateParameter($"@{nameof(clsPlanning.Date)}", SqlDbType.NVarChar, b.Date.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsPlanning.Shift)}", SqlDbType.NVarChar, b.Shift),
                    CreateParameter($"@{nameof(clsPlanning.MachineID)}", SqlDbType.NVarChar, b.MachineID),
                    CreateParameter($"@{nameof(clsPlanning.OptionID)}", SqlDbType.NVarChar, b.OptionID),
                    CreateParameter($"@{nameof(clsPlanning.MONo)}", SqlDbType.NVarChar, b.MONo),
                    CreateParameter($"@{nameof(clsPlanning.Dept)}", SqlDbType.NVarChar, b.Dept),
                    CreateParameter($"@{nameof(clsPlanning.Note)}", SqlDbType.NVarChar, b.Note),
                    CreateParameter($"@{nameof(clsPlanning.Deleted)}", SqlDbType.NVarChar, b.Deleted)
                };
            return DoStore("sp_242_Planning_Update", parameter);
        }

        internal int Update(TablePlanning b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(TablePlanning.ID)}", SqlDbType.NVarChar, b.ID),
                    CreateParameter($"@{nameof(TablePlanning.K)}", SqlDbType.NVarChar, b.K),
                    CreateParameter($"@{nameof(TablePlanning.Date)}", SqlDbType.NVarChar, b.Date.ToString(Timezone)),
                    CreateParameter($"@{nameof(TablePlanning.Shift)}", SqlDbType.NVarChar, b.Shift),
                    CreateParameter($"@{nameof(TablePlanning.MayGC)}", SqlDbType.NVarChar, b.MayGC),
                    CreateParameter($"@{nameof(TablePlanning.NC)}", SqlDbType.NVarChar, b.NC),
                    CreateParameter($"@{nameof(TablePlanning.Order)}", SqlDbType.NVarChar, b.Order),
                    CreateParameter($"@{nameof(TablePlanning.Dept)}", SqlDbType.NVarChar, b.Dept),
                    CreateParameter($"@{nameof(TablePlanning.Note)}", SqlDbType.NVarChar, b.Note),
                    CreateParameter($"@{nameof(TablePlanning.Deleted)}", SqlDbType.NVarChar, b.Deleted)
                };
            return DoStore("sp_242_Planning_Update", parameter);
        }

        internal int UpdateNullOrder(string order, string ordernull, string store)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter("@order", SqlDbType.NVarChar, order),
                    CreateParameter("@ordernull", SqlDbType.NVarChar, ordernull)
                };
            return DoStore(store, parameter);
        }

        internal int Delete(clsPlanning b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsPlanning.ID)}", SqlDbType.NVarChar, b.ID)
                };
            return DoStore("sp_242_Planning_Delete", parameter);
        }

        internal int InsertPlan(TablePlanning b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter("@ID", SqlDbType.Int, b.ID),
                    CreateParameter("@Date", SqlDbType.NVarChar, b.Date.ToString(Timezone)),
                    CreateParameter("@Dept", SqlDbType.NVarChar, b.Dept),
                    CreateParameter("@DKmay", SqlDbType.NVarChar, b.DKmay),
                    CreateParameter("@Fac_DaChuanBi", SqlDbType.Bit, b.Fac_DaChuanBi),
                    CreateParameter("@Fac_Dao", SqlDbType.Bit, b.Fac_Dao),
                    CreateParameter("@Fac_Dc", SqlDbType.Bit, b.Fac_Dc),
                    CreateParameter("@Fac_DeXuat", SqlDbType.NVarChar, b.Fac_DeXuat),
                    CreateParameter("@Fac_File", SqlDbType.Bit, b.Fac_File),
                    CreateParameter("@Fac_Jig", SqlDbType.Bit, b.Fac_Jig),
                    CreateParameter("@Fac_NgayHTTheoKH", SqlDbType.NVarChar, b.Fac_NgayHTTheoKH.ToString(Timezone)),
                    CreateParameter("@Fac_NGTruoc", SqlDbType.NVarChar, b.Fac_NGTruoc),
                    CreateParameter("@Fac_NG_Old", SqlDbType.Int, b.Fac_NG_Old),
                    CreateParameter("@Fac_OK_Old", SqlDbType.Int, b.Fac_OK_Old),
                    CreateParameter("@Fac_Phoi", SqlDbType.Bit, b.Fac_Phoi),
                    CreateParameter("@Fac_TT", SqlDbType.Int, b.Fac_TT),
                    CreateParameter("@Fac_TTFile", SqlDbType.NVarChar, b.Fac_TTFile),
                    CreateParameter("@Fac_TTGC", SqlDbType.Int, b.Fac_TTGC),
                    CreateParameter("@Fac_TTTH", SqlDbType.Int, b.Fac_TTTH),
                    CreateParameter("@Finish", SqlDbType.NVarChar, b.Finish.ToString(Timezone)),
                    CreateParameter("@Jig", SqlDbType.NVarChar, b.Jig),
                    CreateParameter("@K", SqlDbType.NVarChar, b.K),
                    CreateParameter("@MachineID", SqlDbType.NVarChar, b.MachineID),
                    CreateParameter("@NC", SqlDbType.NVarChar, b.NC),
                    CreateParameter("@Note", SqlDbType.NVarChar, b.Note),
                    CreateParameter("@LastOption", SqlDbType.NVarChar, b.LastOption),
                    CreateParameter("@RegisterDate", SqlDbType.NVarChar, b.RegisterDate.ToString(Timezone)),
                    CreateParameter("@Order", SqlDbType.NVarChar, b.Order),
                    CreateParameter("@PartID", SqlDbType.NVarChar, b.PartID),
                    CreateParameter("@Shift", SqlDbType.NVarChar, b.Shift),
                    CreateParameter("@Slg", SqlDbType.Int, b.Slg),
                    CreateParameter("@SoJig", SqlDbType.NVarChar, b.SoJig),
                    CreateParameter("@Start", SqlDbType.NVarChar, b.Start.ToString(Timezone)),
                    CreateParameter("@TGGC", SqlDbType.Float, b.TGGC),
                    CreateParameter("@TGGL", SqlDbType.Float, b.TGGL),
                    CreateParameter("@ThoiHan", SqlDbType.NVarChar, b.ThoiHan.ToString(Timezone)),
                    CreateParameter("@TinhTrang", SqlDbType.Int, b.TinhTrang),
                    CreateParameter("@TTNC", SqlDbType.NVarChar, b.TTNC),
                    CreateParameter("@Via_check1", SqlDbType.NVarChar, b.Via_check1),
                    CreateParameter("@Via_check2", SqlDbType.NVarChar, b.Via_check2),
                    CreateParameter("@Via_ghi_chu", SqlDbType.NVarChar, b.Via_ghi_chu),
                    CreateParameter("@__10", SqlDbType.Int, b.__10),
                    CreateParameter("@__20", SqlDbType.Int, b.__20),
                    CreateParameter("@__80", SqlDbType.Int, b.__80),
                    CreateParameter("@Fac_MachineID", SqlDbType.NVarChar, b.Fac_MachineID),
                    CreateParameter("@KT_Sup", SqlDbType.NVarChar, b.KT_Sup),
                    CreateParameter("@KT_TGUKB1", SqlDbType.Int, b.KT_TGUKB1),
                    CreateParameter("@KT_TongTG", SqlDbType.Float, b.KT_TongTG),
                    CreateParameter("@KT_TTPhoi", SqlDbType.Int, b.KT_TTPhoi),
                    CreateParameter("@KT_GiaHang", SqlDbType.NVarChar, b.KT_GiaHang),
                    CreateParameter("@KT_KHXN", SqlDbType.NVarChar, b.KT_KHXN),
                    CreateParameter("@KT_LyDoKHTKH", SqlDbType.NVarChar, b.KT_LyDoKHTKH),
                    CreateParameter("@KT_DgoiTay", SqlDbType.NVarChar, b.KT_DgoiTay),
                    CreateParameter("@Cua_GCTime", SqlDbType.NVarChar, b.Cua_GCTime.ToString(Timezone)),
                    CreateParameter("@Cua_Factory", SqlDbType.NVarChar, b.Cua_Factory),
                    CreateParameter("@Cua_TotalTime", SqlDbType.Float, b.Cua_TotalTime),
                    CreateParameter("@Cua_Plantime", SqlDbType.Float, b.Cua_Plantime),
                    CreateParameter("@Cua_PrepareDate", SqlDbType.NVarChar, b.Cua_PrepareDate),
                    CreateParameter("@Cua_MachineGroup", SqlDbType.NVarChar, b.Cua_MachineGroup),
                    CreateParameter("@Cua_State", SqlDbType.Bit, b.Cua_State),
                    CreateParameter("@Cua_Size", SqlDbType.NVarChar, b.Cua_Size),
                    CreateParameter("@TongTG", SqlDbType.NVarChar, b.TongTG),
                    CreateParameter("@Nesting", SqlDbType.NVarChar, b.Nesting),
                    CreateParameter("@Phoidd", SqlDbType.NVarChar, b.Phoidd),
                    CreateParameter("@ViTriGia", SqlDbType.NVarChar, b.ViTriGia)
                };
            return DoStore("sp_242_InsertPlan", parameter);
        }
    }
}
