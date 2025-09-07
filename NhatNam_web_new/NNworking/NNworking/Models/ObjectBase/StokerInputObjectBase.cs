using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models.ObjectBase
{
    public class StokerInputObjectBase : IObjectBase
    {
        public void SetDefaultValue(ref object model)
        {
            var amodel = (C222_StokerInput)model;
            amodel.Date = DateTime.Now.Date;
            amodel.Deleted = false;
            amodel.FromDept = string.Empty;
            amodel.ID = 0;
            amodel.Note = string.Empty;
            amodel.Qty = 0;
            amodel.StaffID = string.Empty;
            amodel.PartID = string.Empty;
        }
    }
}