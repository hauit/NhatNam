using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models.ObjectBase
{
    public class MaterialStock_OutputObjectBase : IObjectBase
    {
        public void SetDefaultValue(ref object model)
        {
            var amodel = (C222_MaterialStock_Output)model;
            amodel.BPYC = string.Empty;
            amodel.Date = DateTime.Now.Date;
            amodel.ID = 0;
            amodel.MaterialConfiguration = string.Empty;
            amodel.MaterialID = string.Empty;
            amodel.Note = string.Empty;
            amodel.OrderNo = string.Empty;
            amodel.Unit = "Gram";
            amodel.Weight = 0;
        }
    }
}