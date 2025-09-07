using NNworking.Models.ObjectBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportStokerInput : IImport
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
                            var Date =               item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                            var StaffID =          item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                            var orderNo =             item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                            var Qty =            item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                            var FromDept =        item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                            var NCC =        item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                            var giayXN =        item.Cells["G" + line].Value == null ? false : (item.Cells["G" + line].Value.ToString().Trim().Length == 0 ? false: true);
                            var Note =              item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(orderNo))
                            {
                                break;
                            }

                            #region
                            DateTime date;
                            if(!DateTime.TryParse(Date,out date))
                            {
                                throw new ArgumentException("Ngày không đúng định dạng");
                            }

                            var tool = db.C242_BusOder.Where(x => x.BOderNo.ToLower() == orderNo.ToLower()).FirstOrDefault();
                            if (tool == null)
                            {
                                throw new ArgumentException("Số order không tồn tại trong danh sách");
                            }

                            var partID = tool.PartID;
                            int qty;
                            if(!int.TryParse(Qty,out qty))
                            {
                                throw new ArgumentException("Số lượng nhập không đúng định dạng");
                            }

                            if (!string.IsNullOrEmpty(FromDept))
                            {
                                var dept = db.C222_Department.Where(X => X.DeptCode.ToLower() == FromDept.ToLower()).Any();
                                if (!dept)
                                {
                                    throw new ArgumentException("Mã bộ phận không có trong danh sách");
                                }
                            }

                            #endregion
                            var obj = new C222_StokerInput();
                            objectBase = new StokerInputObjectBase();
                            var obj1 = (object)obj;
                            objectBase.SetDefaultValue(ref obj1);
                            obj.OrderNo = orderNo;
                            obj.Date = date;
                            obj.FromDept = FromDept;
                            obj.Note = Note;
                            obj.Qty = qty;
                            obj.StaffID = staffID;
                            obj.PartID = partID;
                            obj.GiayXN = giayXN;
                            db.C222_StokerInput.Add(obj);
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