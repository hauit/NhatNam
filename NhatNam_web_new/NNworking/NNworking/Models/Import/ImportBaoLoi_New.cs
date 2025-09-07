using NNworking.Models.ObjectBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportBaoLoi_New : IImport
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

                using (NN_DatabaseEntities db = new NN_DatabaseEntities())
                {
                    int line = 1;
                    while (line < 1000000)
                    {
                        line++;
                        try
                        {
                            //ReadDataFromFile(item);
                            //var PartNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString();
                            var ErrorNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                            var DateRaiseErr = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                            var DateWrite = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                            var NotifyDept = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                            var RaiseErrorDept = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                            var Supplier = item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                            var NotifyStaff = item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                            var RaiseErrorStaff = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                            var Customer = item.Cells["I" + line].Value == null ? string.Empty : item.Cells["I" + line].Value.ToString().Trim();
                            var ErrorType = item.Cells["J" + line].Value == null ? string.Empty : item.Cells["J" + line].Value.ToString().Trim();
                            var OrderNo = item.Cells["K" + line].Value == null ? string.Empty : item.Cells["K" + line].Value.ToString().Trim();
                            var PartID = item.Cells["L" + line].Value == null ? string.Empty : item.Cells["L" + line].Value.ToString().Trim();
                            var Qty = item.Cells["M" + line].Value == null ? string.Empty : item.Cells["M" + line].Value.ToString().Trim();
                            var OptionID = item.Cells["N" + line].Value == null ? string.Empty : item.Cells["N" + line].Value.ToString().Trim();
                            var ErrorQty = item.Cells["O" + line].Value == null ? string.Empty : item.Cells["O" + line].Value.ToString().Trim();
                            var ErrorDes = item.Cells["P" + line].Value == null ? string.Empty : item.Cells["P" + line].Value.ToString().Trim();
                            var ErrorContent = item.Cells["Q" + line].Value == null ? string.Empty : item.Cells["Q" + line].Value.ToString().Trim();
                            var DeceidedTime = item.Cells["R" + line].Value == null ? string.Empty : item.Cells["R" + line].Value.ToString().Trim();
                            var PredictErrorCause = item.Cells["S" + line].Value == null ? string.Empty : item.Cells["S" + line].Value.ToString().Trim();
                            var ErrorHappenFrequency = item.Cells["T" + line].Value == null ? string.Empty : item.Cells["T" + line].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(ErrorNo))
                            {
                                break;
                            }
                            #region
                            int errorQty;
                            if (!int.TryParse(ErrorQty, out errorQty))
                            {
                                throw new ArgumentException("Số lượng lỗi không đúng định dạng");
                            }

                            int qty;
                            if (!int.TryParse(Qty, out qty))
                            {
                                throw new ArgumentException("Số lượng lệnh không đúng định dạng");
                            }

                            var mono = db.View_242_BusOder.Where(x => x.MONo.ToUpper() == OrderNo.ToUpper()).FirstOrDefault();
                            if (mono != null)
                            {
                                qty = (int)mono.Qty;
                                PartID = mono.PartID;
                            }

                            var dept = db.C222_Department.Where(x => x.DeptCode.ToLower().Trim() == NotifyDept.ToLower()).Any();
                            if (!dept)
                            {
                                throw new ArgumentException("Bộ phận thông báo kông đúng");
                            }

                            dept = db.C222_Department.Where(x => x.DeptCode.ToLower().Trim() == RaiseErrorDept.ToLower()).Any();
                            if (!dept)
                            {
                                throw new ArgumentException("Bộ phận gây lỗi kông đúng");
                            }

                            var dept1 = db.sp_222_PartnerGetAll().Where(x => x.Code.ToLower().Trim() == Customer.ToLower() || x.Name.ToLower().Trim() == Customer.ToLower()).FirstOrDefault();
                            if (dept1 == null)
                            {
                                throw new ArgumentException("Khách hàng không nằm trong danh sách bộ phận");
                            }
                            Customer = dept1.Code;

                            dept1 = db.sp_222_PartnerGetAll().Where(x => x.Code.ToLower().Trim() == Supplier.ToLower() || x.Name.ToLower().Trim() == Supplier.ToLower()).FirstOrDefault();
                            if (dept1 == null)
                            {
                                throw new ArgumentException("Nhà cung cấp không nằm trong danh sách trong phần mềm");
                            }
                            Supplier = dept1.Code;

                            var errorTtype = db.C242_ErrorType.Where(x => x.ErrorType.ToLower().Trim() == ErrorType.ToLower()).FirstOrDefault();
                            if (errorTtype == null)
                            {
                                throw new ArgumentException("Kiểu lỗi không đúng");
                            }

                            DateTime dateWrite;
                            if (!DateTime.TryParse(DateWrite, out dateWrite))
                            {
                                throw new ArgumentException("Ngày viêt giấy lỗi không đúng định dạng");
                            }

                            DateTime dateRaiseErr;
                            if (!DateTime.TryParse(DateRaiseErr, out dateRaiseErr))
                            {
                                throw new ArgumentException("Ngày gây lỗi không đúng định dạng");
                            }

                            var errorContent = db.C242_ErrorContent.Where(x => x.ContentDes.ToLower() == ErrorContent.ToLower()).FirstOrDefault();
                            if (errorContent == null)
                            {
                                throw new ArgumentException("Nội dung lỗi không có trong danh sách");
                            }

                            var deceidedTime = db.C242_DeceidedTime.Where(x => x.Des.ToLower() == DeceidedTime.ToLower()).FirstOrDefault();
                            if (deceidedTime == null)
                            {
                                throw new ArgumentException("Thời điểm phát hiện không có trong danh sách");
                            }

                            var predictErrorCause = db.C242_PredictErrorCause.Where(x => x.Des.ToLower() == PredictErrorCause.ToLower()).FirstOrDefault();
                            if (predictErrorCause == null)
                            {
                                throw new ArgumentException("Nhận định nguyên nhân lỗi không có trong danh sách");
                            }

                            var errorHappenFrequency = db.C242_ErrorHappenFrequency.Where(x => x.Des.ToLower() == ErrorHappenFrequency.ToLower()).FirstOrDefault();
                            if (errorHappenFrequency == null)
                            {
                                throw new ArgumentException("Số lần xuất hiện không có trong danh sách");
                            }
                            #endregion
                            var obj = new C242_ErrorItemNotify_New();
                            objectBase = new ErrorItemNotifyObjectBase();
                            var obj1 = (object)obj;
                            objectBase.SetDefaultValue(ref obj1);
                            obj.ErrorNo = ErrorNo;
                            obj.DateRaiseErr = dateRaiseErr;
                            obj.DateWrite = dateWrite;
                            obj.NotifyDept = NotifyDept;
                            obj.RaiseErrorDept = RaiseErrorDept;
                            obj.Supplier = Supplier;
                            obj.NotifyStaff = NotifyStaff;
                            obj.RaiseErrorStaff = RaiseErrorStaff;
                            obj.OrderNo = OrderNo;
                            obj.PartID = PartID;
                            obj.Qty = qty;
                            obj.OptionID = OptionID;
                            obj.ErrorQty = errorQty;
                            obj.ErrorDes = ErrorDes;
                            obj.ErrorContent = errorContent.ContentIndex;
                            obj.DeceidedTime = deceidedTime.DeceidedTimeIndex;
                            obj.PredictErrorCause = predictErrorCause.PredictErrorCauseIndex;
                            db.C242_ErrorItemNotify_New.Add(obj);
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

            }
            return Error;
        }
    }
}