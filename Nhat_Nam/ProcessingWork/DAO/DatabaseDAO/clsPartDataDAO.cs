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
    public class clsPartDataDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsPartData> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
              nameof(clsPartData.ID)
            , nameof(clsPartData.InputDate)
            , nameof(clsPartData.PartID)
            , nameof(clsPartData.MaterialID)
            , nameof(clsPartData.MaterialType)
            , nameof(clsPartData.Workpiecesize)
            , nameof(clsPartData.shape)
            , nameof(clsPartData.Thickness)
            , nameof(clsPartData.width)
            , nameof(clsPartData.lenght)
            , nameof(clsPartData.Cut)
            , nameof(clsPartData.RawMachine)
            , nameof(clsPartData.HandFinish)
            , nameof(clsPartData.HairLine)
            , nameof(clsPartData.WAnod)
            , nameof(clsPartData.BAnod)
            , nameof(clsPartData.Blast30)
            , nameof(clsPartData.Blast60)
            , nameof(clsPartData.Seal)
            , nameof(clsPartData.Migaki)
            , nameof(clsPartData.Bafu)
            , nameof(clsPartData.Cleanwave)
            , nameof(clsPartData.VacPac)
            , nameof(clsPartData.Helisert)
            , nameof(clsPartData.SerialNo)
            , nameof(clsPartData.PalCoat)
            , nameof(clsPartData.Paint)
            , nameof(clsPartData.BBD)
            , nameof(clsPartData.Otherpro)
            , nameof(clsPartData.Price)
            , nameof(clsPartData.Memo)
            , nameof(clsPartData.Note)
            , nameof(clsPartData.Caciras)
            , nameof(clsPartData.Inside)
            , nameof(clsPartData.MaBong)
            , nameof(clsPartData.InLuoi)
            , nameof(clsPartData.Heru)
            , nameof(clsPartData.Niken)
            , nameof(clsPartData.MaiBongDP)
            , nameof(clsPartData.Deleted)    }))
            {
                dtBulk.Load(reader);
            }
            return InsertBulk(dtBulk, "[242_PartData]");
        }

        internal int UpdateData(clsPartData b)
        {
            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsPartData.ID)}", SqlDbType.NVarChar, b.ID),
                    CreateParameter($"@{nameof(clsPartData.InputDate)}", SqlDbType.NVarChar, b.InputDate),
                    CreateParameter($"@{nameof(clsPartData.PartID)}", SqlDbType.NVarChar, b.PartID),
                    CreateParameter($"@{nameof(clsPartData.MaterialID)}", SqlDbType.NVarChar, b.MaterialID),
                    CreateParameter($"@{nameof(clsPartData.MaterialType)}", SqlDbType.NVarChar, b.MaterialType),
                    CreateParameter($"@{nameof(clsPartData.Workpiecesize)}", SqlDbType.NVarChar, b.Workpiecesize),
                    CreateParameter($"@{nameof(clsPartData.shape)}", SqlDbType.NVarChar, b.shape),
                    CreateParameter($"@{nameof(clsPartData.Thickness)}", SqlDbType.NVarChar, b.Thickness),
                    CreateParameter($"@{nameof(clsPartData.width)}", SqlDbType.NVarChar, b.width),
                    CreateParameter($"@{nameof(clsPartData.lenght)}", SqlDbType.NVarChar, b.lenght),
                    CreateParameter($"@{nameof(clsPartData.Cut)}", SqlDbType.NVarChar, b.Cut),
                    CreateParameter($"@{nameof(clsPartData.RawMachine)}", SqlDbType.NVarChar, b.RawMachine),
                    CreateParameter($"@{nameof(clsPartData.HandFinish)}", SqlDbType.NVarChar, b.HandFinish),
                    CreateParameter($"@{nameof(clsPartData.HairLine)}", SqlDbType.NVarChar, b.HairLine),
                    CreateParameter($"@{nameof(clsPartData.WAnod)}", SqlDbType.NVarChar, b.WAnod),
                    CreateParameter($"@{nameof(clsPartData.BAnod)}", SqlDbType.NVarChar, b.BAnod),
                    CreateParameter($"@{nameof(clsPartData.Blast30)}"   , SqlDbType.NVarChar, b.Blast30),
                    CreateParameter($"@{nameof(clsPartData.Blast60)}", SqlDbType.NVarChar, b.Blast60),
                    CreateParameter($"@{nameof(clsPartData.Seal)}"  , SqlDbType.NVarChar, b.Seal),
                    CreateParameter($"@{nameof(clsPartData.Migaki)}"    , SqlDbType.NVarChar, b.Migaki),
                    CreateParameter($"@{nameof(clsPartData.Bafu)}"  , SqlDbType.NVarChar, b.Bafu),
                    CreateParameter($"@{nameof(clsPartData.Cleanwave)}", SqlDbType.NVarChar, b.Cleanwave),
                    CreateParameter($"@{nameof(clsPartData.VacPac)}"    , SqlDbType.NVarChar, b.VacPac),
                    CreateParameter($"@{nameof(clsPartData.Helisert)}", SqlDbType.NVarChar, b.Helisert),
                    CreateParameter($"@{nameof(clsPartData.SerialNo)}", SqlDbType.NVarChar, b.SerialNo),
                    CreateParameter($"@{nameof(clsPartData.PalCoat)}", SqlDbType.NVarChar, b.PalCoat),
                    CreateParameter($"@{nameof(clsPartData.Paint)}" , SqlDbType.NVarChar, b.Paint),
                    CreateParameter($"@{nameof(clsPartData.BBD)}", SqlDbType.NVarChar, b.BBD),
                    CreateParameter($"@{nameof(clsPartData.Otherpro)}", SqlDbType.NVarChar, b.Otherpro),
                    CreateParameter($"@{nameof(clsPartData.Price)}" , SqlDbType.NVarChar, b.Price),
                    CreateParameter($"@{nameof(clsPartData.Memo)}"  , SqlDbType.NVarChar, b.Memo),
                    CreateParameter($"@{nameof(clsPartData.Note)}"  , SqlDbType.NVarChar, b.Note),
                    CreateParameter($"@{nameof(clsPartData.Caciras)}"   , SqlDbType.NVarChar, b.Caciras),
                    CreateParameter($"@{nameof(clsPartData.Inside)}"    , SqlDbType.NVarChar, b.Inside),
                    CreateParameter($"@{nameof(clsPartData.MaBong)}"    , SqlDbType.NVarChar, b.MaBong),
                    CreateParameter($"@{nameof(clsPartData.InLuoi)}"    , SqlDbType.NVarChar, b.InLuoi),
                    CreateParameter($"@{nameof(clsPartData.Heru)}"  , SqlDbType.NVarChar, b.Heru),
                    CreateParameter($"@{nameof(clsPartData.Niken)}" , SqlDbType.NVarChar, b.Niken),
                    CreateParameter($"@{nameof(clsPartData.MaiBongDP)}", SqlDbType.NVarChar, b.MaiBongDP)
                };
            return DoStore("sp_242_PartData_UpdateMaterial", parameter);
        }
    }
}
