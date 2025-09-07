using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models.ObjectBase
{
    public class StokerToolObjectBase : IObjectBase
    {
        public void SetDefaultValue(ref object model)
        {
            var amodel = (C222_StokerTool)model;
            amodel.ID = 0;
            amodel.ToolNo = string.Empty;
            amodel.ToolName = string.Empty;
            amodel.Unit = 0;
            amodel.CatergoryID = string.Empty;
        }
    }
}