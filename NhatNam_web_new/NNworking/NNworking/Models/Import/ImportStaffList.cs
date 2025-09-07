using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace NNworking.Models.Import
{
    public class ImportStaffList : IImport
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
                        var StaffID = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(StaffID))
                        {
                            continue;
                        }

                        var StaffName = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        var SecID = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        var SecName = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                        var GroupID = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                        var Sub_Group = item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                        var DeptCode = item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                        var Email = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                        var ngduyet = item.Cells["I" + line].Value == null ? string.Empty : item.Cells["I" + line].Value.ToString().Trim();
                        var ngduyet2 = item.Cells["J" + line].Value == null ? string.Empty : item.Cells["J" + line].Value.ToString().Trim();
                        var ngduyet3 = item.Cells["K" + line].Value == null ? string.Empty : item.Cells["K" + line].Value.ToString().Trim();
                        var obj = new C222_Staff();
                        obj.StaffID = StaffID;
                        obj.StaffName = StaffName;
                        obj.SecID = SecID;
                        obj.SecName = SecName;
                        obj.GroupID = GroupID;
                        obj.Sub_Group = Sub_Group;
                        obj.DeptCode = DeptCode;
                        obj.Email = Email;
                        obj.ngduyet = ngduyet;
                        obj.ngduyet2 = ngduyet2;
                        obj.ngduyet3 = ngduyet3;
                        db.C222_Staff.Add(obj);
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