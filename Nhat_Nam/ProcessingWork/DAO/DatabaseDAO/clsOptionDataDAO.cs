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
    public class clsOptionDataDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsOptionData> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
               nameof(clsOptionData.ID         )
              ,nameof(clsOptionData.InputDate  )
              ,nameof(clsOptionData.PartID     )
              ,nameof(clsOptionData.MachineID  )
              ,nameof(clsOptionData.OptionID   )
              ,nameof(clsOptionData.LastOption )
              ,nameof(clsOptionData.JigID      )
              ,nameof(clsOptionData.ToolQty    )
              ,nameof(clsOptionData.ProTime    )
              ,nameof(clsOptionData.ClampTime  )
              ,nameof(clsOptionData.TechDate   )
              ,nameof(clsOptionData.UpdateDay  )
              ,nameof(clsOptionData.StaffID    )
              ,nameof(clsOptionData.Note       )
              ,nameof(clsOptionData.AondNote   )
              ,nameof(clsOptionData.Memo       )
              ,nameof(clsOptionData.CLUpdateday)
              ,nameof(clsOptionData.JigType    )
              ,nameof(clsOptionData.Jig        )
              ,nameof(clsOptionData.Tich       )
              ,nameof(clsOptionData.Doc        )
              ,nameof(clsOptionData.TimeTreo   )
              ,nameof(clsOptionData.TimeComplete)
              ,nameof(clsOptionData.Deleted)}))
            {
                dtBulk.Load(reader);
            }
            return InsertBulk(dtBulk, "[242_OptionData]");
        }

        internal int UpdateData(clsOptionData b)
        {

            var parameter = new SqlParameter[]
                {
                    CreateParameter($"@{nameof(clsOptionData.ID          )}", SqlDbType.NVarChar, b.ID),
                    CreateParameter($"@{nameof(clsOptionData.InputDate  )}", SqlDbType.NVarChar, b.InputDate.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsOptionData.PartID     )}", SqlDbType.NVarChar, b.PartID),
                    CreateParameter($"@{nameof(clsOptionData.MachineID  )}", SqlDbType.NVarChar, b.MachineID),
                    CreateParameter($"@{nameof(clsOptionData.OptionID   )}", SqlDbType.NVarChar, b.OptionID),
                    CreateParameter($"@{nameof(clsOptionData.LastOption )}", SqlDbType.NVarChar, b.LastOption),
                    CreateParameter($"@{nameof(clsOptionData.JigID      )}", SqlDbType.NVarChar, b.JigID),
                    CreateParameter($"@{nameof(clsOptionData.ToolQty    )}", SqlDbType.NVarChar, b.ToolQty),
                    CreateParameter($"@{nameof(clsOptionData.ProTime    )}", SqlDbType.NVarChar, b.ProTime),
                    CreateParameter($"@{nameof(clsOptionData.ClampTime  )}", SqlDbType.NVarChar, b.ClampTime),
                    CreateParameter($"@{nameof(clsOptionData.TechDate   )}", SqlDbType.NVarChar, b.TechDate),
                    CreateParameter($"@{nameof(clsOptionData.UpdateDay  )}", SqlDbType.NVarChar, b.UpdateDay.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsOptionData.StaffID    )}", SqlDbType.NVarChar, b.StaffID),
                    CreateParameter($"@{nameof(clsOptionData.Note       )}", SqlDbType.NVarChar, b.Note),
                    CreateParameter($"@{nameof(clsOptionData.AondNote   )}", SqlDbType.NVarChar, b.AondNote),
                    CreateParameter($"@{nameof(clsOptionData.Memo       )}", SqlDbType.NVarChar, b.Memo),
                    CreateParameter($"@{nameof(clsOptionData.CLUpdateday)}"   , SqlDbType.NVarChar, b.CLUpdateday.ToString(Timezone)),
                    CreateParameter($"@{nameof(clsOptionData.JigType    )}", SqlDbType.NVarChar, b.JigType),
                    CreateParameter($"@{nameof(clsOptionData.Jig        )}"  , SqlDbType.NVarChar, b.Jig),
                    CreateParameter($"@{nameof(clsOptionData.Tich       )}"    , SqlDbType.NVarChar, b.Tich),
                    CreateParameter($"@{nameof(clsOptionData.Doc        )}"  , SqlDbType.NVarChar, b.Doc),
                    CreateParameter($"@{nameof(clsOptionData.TimeTreo   )}", SqlDbType.NVarChar, b.TimeTreo),
                    CreateParameter($"@{nameof(clsOptionData.TimeComplete)}"    , SqlDbType.NVarChar, b.TimeComplete),
                    CreateParameter($"@EditStaffID"    , SqlDbType.NVarChar, ClsSession.staffID)
                };
            return DoStore("sp_242_OptionData_Update", parameter);
        }

        internal DataTable GetAnodOption(string partID)
        {
            string sql = $@"select * from [View_242_OptionData] where PartID = '{partID}' and OptionID like 'XOX%' ";
            return LoadGridByStr(sql);
        }
    }
}
