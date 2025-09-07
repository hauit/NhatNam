using NNworking.Models.ObjectBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportBaoLoi : IImport
    {
        private IObjectBase objectBase;
        protected override List<clsError> ImportExecution(ExcelPackage package, string staffID, int type = 0)
        {
            List<clsError> Error = new List<clsError>();
            foreach (var item in package.Workbook.Worksheets)
            {
                if (item.Name.ToUpper() != "SHEET1")
                {
                    continue;
                }

                int line = 1;
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                while (line < 1000000)
                {
                    line++;
                    try
                    {
                        //var PartNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString();
                        var MONo =                  item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        var OptionID =              item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        var PartID =                item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        var ErrorNumber =           item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                        var NotifyDept =            item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                        var ErrorTypeID =           item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                        var ErrorContent =          item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                        var NotifyDate =            item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                        var ReceiveErrorItemDate =  item.Cells["I" + line].Value == null ? string.Empty : item.Cells["I" + line].Value.ToString().Trim();
                        var Note =                  item.Cells["J" + line].Value == null ? string.Empty : item.Cells["J" + line].Value.ToString().Trim();
                        var Supplier =              item.Cells["K" + line].Value == null ? string.Empty : item.Cells["K" + line].Value.ToString().Trim();
                        var RaiseErrorDept =        item.Cells["L" + line].Value == null ? string.Empty : item.Cells["L" + line].Value.ToString().Trim();
                        var ErrorComment =          item.Cells["M" + line].Value == null ? string.Empty : item.Cells["M" + line].Value.ToString().Trim();
                        var CauseOfError =          item.Cells["N" + line].Value == null ? string.Empty : item.Cells["N" + line].Value.ToString().Trim();
                        var evaluate =              item.Cells["O" + line].Value == null ? string.Empty : item.Cells["O" + line].Value.ToString().Trim();

                        if (string.IsNullOrEmpty(MONo))
                        {
                            break;
                        }
                        int qty;
                        if (!int.TryParse(ErrorNumber, out qty))
                        {
                            throw new ArgumentException("Số lượng lỗi phải là kiểu số");
                        }
                            
                        var errorType = db.C242_ErrorType.Where(x => x.ErrorType.ToLower().Trim() == ErrorTypeID.ToLower()).FirstOrDefault();
                        if (errorType == null)
                        {
                            throw new ArgumentException("Kiểu lỗi không đúng");
                        }

                        DateTime notifyDate;
                        if(!DateTime.TryParse(NotifyDate,out notifyDate))
                        {
                            throw new ArgumentException("Ngày thông báo lỗi không đúng định dạng");
                        }

                        DateTime receiveErrorItemDate;
                        if (!DateTime.TryParse(ReceiveErrorItemDate, out receiveErrorItemDate))
                        {
                            throw new ArgumentException("Ngày tiếp nhận lỗi không đúng định dạng");
                        }

                        var dept = db.sp_222_PartnerGetAll().Where(x => x.Code.ToLower().Trim() == NotifyDept.ToLower()).Any();
                        if (!dept)
                        {
                            throw new ArgumentException("Bộ phận báo lỗi không nằm trong danh sách bộ phận");
                        }

                        dept = db.sp_222_PartnerGetAll().Where(x => x.Code.ToLower().Trim() == RaiseErrorDept.ToLower()).Any();
                        if (!dept)
                        {
                            throw new ArgumentException("Bộ phận gây lỗi không nằm trong danh sách bộ phận");
                        }

                        dept = db.View_222_Partner.Where(x => x.Code.ToLower().Trim() == Supplier.ToLower()).Any();
                        if (!dept)
                        {
                            throw new ArgumentException("Nhà cung cấp không nằm trong danh sách trong phần mềm");
                        }

                        int eval;
                        if (!int.TryParse(evaluate, out eval))
                        {
                            throw new ArgumentException("Mã đánh giá phải là kiểu số");
                        }

                        dept = db.C242_ErrorItemEvaluate.Where(x => x.EvaluateIndex == eval).Any();
                        if (!dept)
                        {
                            throw new ArgumentException("Mã đánh giá không đúng");
                        }

                        C242_ErrorItemNotify obj = new C242_ErrorItemNotify();
                        obj.OptionID = OptionID;
                        obj.PartID = PartID;
                        obj.MONo = MONo;
                        obj.ErrorNumber = qty;
                        obj.NotifyDept = NotifyDept;
                        obj.ErrorTypeID = errorType.ID;
                        obj.ErrorContent = ErrorContent;
                        obj.NotifyDate = notifyDate;
                        obj.ReceiveErrorItemDate = receiveErrorItemDate;
                        obj.Note = Note;
                        obj.Supplier = Supplier;
                        obj.RaiseErrorDept = RaiseErrorDept;
                        obj.ErrorComment = ErrorComment;
                        obj.CauseOfError = CauseOfError;
                        obj.Evaluate = eval;
                        db.C242_ErrorItemNotify.Add(obj);
                    }
                    catch (Exception ex)
                    {
                        Error.Add(new clsError(line, "Not OK", ex.Message));
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Error.Add(new clsError(line + 1, "Not OK", "Không nhập được dữ liệu. Vui lòng thử lại sau"));
                }
            }
            return Error;
        }
    }
}