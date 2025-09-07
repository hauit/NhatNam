using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportNNDS : IImport
    {
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
                        var id = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(id))
                        {
                            break;
                        }

                        int ID;
                        if (!int.TryParse(id, out ID))
                        {
                            throw new ArgumentException("Mã giấy báo lỗi phải là số nguyên");
                        }

                        var ErrorType = item.Cells["S" + line].Value == null ? string.Empty : item.Cells["S" + line].Value.ToString().Trim();
                        var OptionID = item.Cells["T" + line].Value == null ? string.Empty : item.Cells["T" + line].Value.ToString().Trim();
                        var RaiseErrorStaff = item.Cells["U" + line].Value == null ? string.Empty : item.Cells["U" + line].Value.ToString().Trim();
                        var DecisionToFine = item.Cells["V" + line].Value == null ? string.Empty : item.Cells["V" + line].Value.ToString().Trim();
                        var NotPenalizeDes = item.Cells["W" + line].Value == null ? string.Empty : item.Cells["W" + line].Value.ToString().Trim();
                        var ErrorProcess = item.Cells["X" + line].Value == null ? string.Empty : item.Cells["X" + line].Value.ToString().Trim();
                        var ErrorCause = item.Cells["Y" + line].Value == null ? string.Empty : item.Cells["Y" + line].Value.ToString().Trim();
                        var Remedies = item.Cells["Z" + line].Value == null ? string.Empty : item.Cells["Z" + line].Value.ToString().Trim();
                        var Remedies1 = item.Cells["AA" + line].Value == null ? string.Empty : item.Cells["AA" + line].Value.ToString().Trim();
                        var Remedies2 = item.Cells["AB" + line].Value == null ? string.Empty : item.Cells["AB" + line].Value.ToString().Trim();
                        var ManagerRemedies = item.Cells["AC" + line].Value == null ? string.Empty : item.Cells["AC" + line].Value.ToString().Trim();
                        var PIC = item.Cells["AD" + line].Value == null ? string.Empty : item.Cells["AD" + line].Value.ToString().Trim();
                        var ViTriHang = item.Cells["AE" + line].Value == null ? string.Empty : item.Cells["AE" + line].Value.ToString().Trim();
                        var Completed = item.Cells["AF" + line].Value == null ? string.Empty : item.Cells["AF" + line].Value.ToString().Trim();
                        var shift = item.Cells["AG" + line].Value == null ? string.Empty : item.Cells["AG" + line].Value.ToString().Trim();
                        //ID = 1002;
                        C242_ErrorItemNotify_New obj = db.C242_ErrorItemNotify_New.Find(ID);
                        if(obj == null)
                        {
                            throw new ArgumentException("Không tìm thấy giấy báo lỗi theo mã(có thể đã bị xóa. Vui lòng xem lại!)");
                        }

                        obj.ErrorType = ErrorType;
                        obj.OptionID = ErrorType;
                        obj.RaiseErrorStaff = RaiseErrorStaff;
                        obj.DecisionToFine = bool.Parse(DecisionToFine);
                        obj.NotPenalizeDes = NotPenalizeDes;
                        var errrP = db.C242_ErrorProcess.Where(x => x.Des.ToLower() == ErrorProcess.ToLower()).FirstOrDefault();
                        if(errrP == null)
                        {
                            throw new ArgumentException("Cách xử lý không nằm trong danh sách. Vui lòng xem lại!");
                        }

                        obj.ErrorProcess = errrP.ID;
                        obj.ErrorCause = ErrorCause;
                        obj.Remedies = Remedies;
                        obj.Remedies1 = Remedies1;
                        obj.Remedies2 = Remedies2;
                        obj.ManagerRemedies = ManagerRemedies;
                        obj.PIC = PIC;
                        var vitri = db.sp_222_PartnerGetAll().Where(x => x.Name.ToLower() == ViTriHang.ToLower()).FirstOrDefault();
                        if (vitri == null)
                        {
                            throw new ArgumentException("Vị trí hàng không nằm trong danh sách. Vui lòng xem lại!");
                        }
                        obj.ViTriHang = vitri.Code;
                        obj.Completed = bool.Parse(Completed);
                        obj.Shift = shift;
                        //db.C242_ErrorItemNotify_New.Add(obj);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Error.Add(new clsError(line, "Not OK", ex.Message));
                    }
                }

                try
                {
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