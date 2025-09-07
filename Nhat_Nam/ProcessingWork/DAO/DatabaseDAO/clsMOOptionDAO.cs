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
    public class clsMOOptionDAO : ClsConnect
    {
        internal bool InputDataToDatabase(List<clsMOOption> listPart)
        {
            DataTable dtBulk = new DataTable();
            using (var reader = ObjectReader.Create(listPart, new string[] {
              nameof(clsMOOption.ID)
             , nameof(clsMOOption.MONo      )
             , nameof(clsMOOption.PartID        )
             , nameof(clsMOOption.MOQty          )
             , nameof(clsMOOption.Deadline          )
             , nameof(clsMOOption.MachineID         )
             , nameof(clsMOOption.OptionID    )
             , nameof(clsMOOption.LastOption          )
             , nameof(clsMOOption.JigType  )
             , nameof(clsMOOption.ProTime    )
             , nameof(clsMOOption.ClampTime        )
             , nameof(clsMOOption.Finished       )
             , nameof(clsMOOption.Finishdate     )
             , nameof(clsMOOption.Shift    )
             , nameof(clsMOOption.Note      )
             , nameof(clsMOOption.Deleted        )
            }))
            {
                dtBulk.Load(reader);
            }
            return InsertBulk(dtBulk, "[242_MOOption]");
        }
    }
}
