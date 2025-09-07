using NNworking.Models.ObjectBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportPlanPreparation : IImport
    {
        private IObjectBase objectBase;
        //TODO: Chua lam xong. can lam lai va test
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
                        var tenChiTiet = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(tenChiTiet))
                        {
                            break;
                        }

                        var mayGC = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        var NC = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        var PBJ1Staff = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                        var tgCB = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                        var stt = item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                        var ghiChu = item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                        var phoiKH = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                        var fileCB = item.Cells["I" + line].Value == null ? string.Empty : item.Cells["I" + line].Value.ToString().Trim();
                        var fileKT = item.Cells["J" + line].Value == null ? string.Empty : item.Cells["J" + line].Value.ToString().Trim();
                        var ctCB = item.Cells["K" + line].Value == null ? string.Empty : item.Cells["K" + line].Value.ToString().Trim();
                        var ctKT = item.Cells["L" + line].Value == null ? string.Empty : item.Cells["L" + line].Value.ToString().Trim();
                        var dgCB = item.Cells["M" + line].Value == null ? string.Empty : item.Cells["M" + line].Value.ToString().Trim();
                        var dgKT = item.Cells["N" + line].Value == null ? string.Empty : item.Cells["N" + line].Value.ToString().Trim();
                        var daoCB = item.Cells["O" + line].Value == null ? string.Empty : item.Cells["O" + line].Value.ToString().Trim();
                        var daoKT = item.Cells["P" + line].Value == null ? string.Empty : item.Cells["P" + line].Value.ToString().Trim();
                        var danhGia = item.Cells["Q" + line].Value == null ? string.Empty : item.Cells["Q" + line].Value.ToString().Trim();
                        var ngay = item.Cells["R" + line].Value == null ? string.Empty : item.Cells["R" + line].Value.ToString().Trim();

                        //int chayThu;
                        //if(!int.TryParse(tgCB,out chayThu))
                        //{
                        //    throw new ArgumentException("Thời gian chạy thử phải là kiểu số");
                        //}

                        int ttut;
                        if(!int.TryParse(stt,out ttut))
                        {
                            throw new ArgumentException("Thứ tự ưu tiên phải là kiểu số");
                        }

                        DateTime date;
                        if(!DateTime.TryParse(ngay,out date))
                        {
                            throw new ArgumentException("Ngày lập kế hoạch không đúng định dạng");
                        }

                        C242_MachinePlan_Preparation obj = new C242_MachinePlan_Preparation();
                        obj.Confirmation = danhGia;
                        obj.CT_CB = !string.IsNullOrEmpty(ctCB);
                        obj.CT_KT = !string.IsNullOrEmpty(ctKT);
                        obj.Dao_CB = !string.IsNullOrEmpty(daoCB);
                        obj.Dao_KT = !string.IsNullOrEmpty(daoKT);
                        obj.DG_CB = !string.IsNullOrEmpty(dgCB);
                        obj.DG_KT = !string.IsNullOrEmpty(dgKT);
                        obj.File_CB = !string.IsNullOrEmpty(fileCB);
                        obj.File_KT = !string.IsNullOrEmpty(fileKT);
                        obj.Phoi_KH = !string.IsNullOrEmpty(phoiKH);
                        obj.Date = date;//TODO: need confirm
                        obj.MachineID = mayGC;
                        obj.Note = ghiChu;
                        obj.OptionID = NC;
                        obj.PartID = tenChiTiet;
                        obj.PBJ1_Staff = PBJ1Staff;
                        obj.PBJ1_Time = tgCB;
                        obj.STT = ttut;
                        db.C242_MachinePlan_Preparation.Add(obj);
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