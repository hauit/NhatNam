using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace NNworking.Models.Import
{
    public class ImportShiftRegister : IImport
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
                        if(!DateTime.TryParse(date,out Date))
                        {
                            throw new ArgumentException("Ngày không đúng định dạng. Vui lòng xem lại");
                        }
                        
                        var shift = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        var lunch = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                        bool Lunch = string.IsNullOrEmpty(lunch) == true ? false : true;
                        var dinner = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                        bool Dinner = string.IsNullOrEmpty(dinner) == true ? false : true;
                        var breakfast = item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                        bool Breakfast = string.IsNullOrEmpty(breakfast) == true ? false : true;
                        var machineID = item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                        var note = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                        C222_ShiftRegister obj = new C222_ShiftRegister();
                        obj.StaffID = code;
                        obj.Deleted = false;
                        obj.Date = Date;
                        obj.Lunch = Lunch;
                        obj.Dinner = Dinner;
                        obj.Breakfast = Breakfast;
                        obj.Shift = shift;
                        obj.MachineID = machineID;
                        obj.Note = note;
                        GetWorkingTimeByShift(obj);
                        db.C222_ShiftRegister.Add(obj);
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

        private void GetWorkingTimeByShift(C222_ShiftRegister obj)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C222_Shift.Where(x => x.Shift.ToLower() == obj.Shift.ToLower()).FirstOrDefault();
            if(data == null)
            {
                throw new ArgumentException("Không tìm thấy ca làm việc trong danh sách ca. Vui lòng xem lại!");
            }

            obj.Income = obj.Date.Value.Date.AddMinutes(data.Start);
            obj.Outcome = obj.Date.Value.Date.AddMinutes(data.Finish);
            obj.WorkTime = data.Time;
            obj.Milk = data.Milk == true ? true : false;
        }
    }
}