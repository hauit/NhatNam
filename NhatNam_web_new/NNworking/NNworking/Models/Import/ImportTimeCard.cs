using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace NNworking.Models.Import
{
    public class ImportTimeCard : ImportMaterial
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
                        var code = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(code))
                        {
                            continue;
                        }

                        var date = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        DateTime Date;
                        if (!DateTime.TryParse(date, out Date))
                        {
                            throw new ArgumentException("Ngày không đúng định dạng. Vui lòng xem lại");
                        }

                        var inTime = item.Cells["C" + line].Value.ToString().Trim();
                        TimeSpan InTime;
                        if (!TimeSpan.TryParse(inTime, out InTime))
                        {
                            throw new ArgumentException("Giờ vào không đúng định dạng. Vui lòng xem lại");
                        }

                        var outTime = item.Cells["D" + line].Value.ToString().Trim();
                        TimeSpan OutTime;
                        if (!TimeSpan.TryParse(outTime, out OutTime))
                        {
                            throw new ArgumentException("Giờ ra không đúng định dạng. Vui lòng xem lại");
                        }

                        C222_TimeCard obj = new C222_TimeCard();
                        obj.StaffID = code;
                        obj.Deleted = false;
                        obj.Date = Date;
                        obj.InTime = obj.Date.Date.Add(InTime);
                        obj.OutTime = obj.Date.Date.Add(OutTime);
                        if(obj.OutTime <= obj.InTime)
                        {
                            obj.OutTime = obj.OutTime.AddDays(1);
                        }
                        db.C222_TimeCard.Add(obj);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Error.Add(new clsError(line, "Not OK", ex.Message));
                    }
                }
            }
            return Error;
        }
    }
}