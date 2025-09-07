using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models.ObjectBase
{
    public class ErrorItemNotifyObjectBase : IObjectBase
    {
        public void SetDefaultValue(ref object model)
        {
            var amodel = (C242_ErrorItemNotify_New)model;
            amodel.Customer = string.Empty;
            amodel.DateRaiseErr = DateTime.Now;
            amodel.DateWrite = DateTime.Now;
            amodel.DeceidedTime = 0;
            amodel.DecisionToFine = false;
            amodel.ErrorCause = string.Empty;
            amodel.ErrorContent = 0;
            amodel.ErrorDes = string.Empty;
            amodel.ErrorHappenFrequency = 0;
            amodel.ErrorNo = string.Empty;
            amodel.ErrorProcess = 0;
            amodel.ErrorQty = 0;
            amodel.ErrorType = string.Empty;
            amodel.ID = 0;
            amodel.ManagerRemedies = string.Empty;
            amodel.NotifyDept = string.Empty;
            amodel.NotifyStaff = string.Empty;
            amodel.NotPenalizeDes = string.Empty;
            amodel.OptionID = string.Empty;
            amodel.OrderNo = string.Empty;
            amodel.PartID = string.Empty;
            amodel.PredictErrorCause = 0;
            amodel.Qty = 0;
            amodel.RaiseErrorDept = string.Empty;
            amodel.RaiseErrorStaff = string.Empty;
            amodel.Remedies = string.Empty;
            amodel.Remedies1 = string.Empty;
            amodel.Remedies2 = string.Empty;
            amodel.Supplier = string.Empty;
            amodel.PIC = string.Empty;
            amodel.Completed = false;
        }

        //public C242_ErrorItemNotify_New SetDefaultValue()
        //{
            
        //    var model = new C242_ErrorItemNotify_New();
        //    model.Customer = string.Empty;
        //    model.DateRaiseErr = DateTime.Now;
        //    model.DateWrite = DateTime.Now;
        //    model.DeceidedTime = 0;
        //    model.DecisionToFine = false;
        //    model.ErrorCause = string.Empty;
        //    model.ErrorContent = 0;
        //    model.ErrorDes = string.Empty;
        //    model.ErrorHappenFrequency = 0;
        //    model.ErrorNo = string.Empty;
        //    model.ErrorProcess = 0;
        //    model.ErrorQty = 0;
        //    model.ErrorType = string.Empty;
        //    model.ID = 0;
        //    model.ManagerRemedies = string.Empty;
        //    model.NotifyDept = string.Empty;
        //    model.NotifyStaff = string.Empty;
        //    model.NotPenalizeDes = string.Empty;
        //    model.OptionID = string.Empty;
        //    model.OrderNo = string.Empty;
        //    model.PartID = string.Empty;
        //    model.PredictErrorCause = 0;
        //    model.Qty = 0;
        //    model.RaiseErrorDept = string.Empty;
        //    model.RaiseErrorStaff = string.Empty;
        //    model.Remedies = string.Empty;
        //    model.Remedies1 = string.Empty;
        //    model.Remedies2 = string.Empty;
        //    model.Supplier = string.Empty;
        //    return model;
        //}
    }
}