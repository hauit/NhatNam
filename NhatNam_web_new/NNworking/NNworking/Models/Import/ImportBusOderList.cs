using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportBusOderList : IImport
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
                        line++;
                        C242_BusOder part = new C242_BusOder();
                        part.BOderNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(part.BOderNo))
                        {
                            continue;
                        }
                        string date = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        DateTime dateValue;
                        if (!DateTime.TryParse(date, out dateValue))
                        {
                            throw new ArgumentException($@"Ngày nhập({date}) không đúng định dạng");
                        }

                        part.Date = dateValue;
                        part.PlanNo = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                        part.PartID = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                        string qty = item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                        int qtyValue;
                        if (!int.TryParse(qty, out qtyValue))
                        {
                            throw new ArgumentException($@"Số lượng lệnh({qty}) phải là kiểu số nguyên");
                        }

                        part.Qty = qtyValue;
                        string deadlineText = item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                        DateTime deadline;
                        if (!DateTime.TryParse(deadlineText, out deadline))
                        {
                            throw new ArgumentException($@"Thời hạn đơn hàng({deadlineText}) không đúng định dạng");
                        }
                        part.Deadline = deadline;
                        string odderType = item.Cells["S" + line].Value == null ? string.Empty : item.Cells["S" + line].Value.ToString().Trim();
                        part.OderType = 0;
                        part.OderType = GetOrderType(part.PartID, odderType);

                        part.NoiCat = item.Cells["R" + line].Value == null ? string.Empty : item.Cells["R" + line].Value.ToString().Trim();
                        //DateTime thPhoi;
                        //if (!DateTime.TryParse(dReader[nameof(clsBusOrder.THPhoi)].ToString(), out thPhoi))
                        //{
                        //    throw new ArgumentException($@"Thời hạn phôi({nameof(clsBusOrder.THPhoi)}) không đúng định dạng");
                        //}
                        var a = db.sp_242_BusOder_GetTHPhoi(part.PartID, part.MONo).FirstOrDefault();
                        if (a == null)
                        {
                            part.THPhoi = DateTime.Now.Date.AddYears(50);
                        }
                        else
                        {
                            part.THPhoi = a.Date;
                        }

                        string thVatLieuText = item.Cells["P" + line].Value == null ? string.Empty : item.Cells["P" + line].Value.ToString().Trim();
                        DateTime thVatLieu;
                        if (!DateTime.TryParse(thVatLieuText, out thVatLieu))
                        {
                            throw new ArgumentException($@"Thời hạn vật liệu({thVatLieuText}) không đúng định dạng");
                        }
                        part.THVatLieu = thVatLieu;
                        string rawQtyText = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                        int rawQty;
                        if (!int.TryParse(rawQtyText, out rawQty))
                        {
                            throw new ArgumentException($@"RawQty({rawQtyText}) phải là kiểu số");
                        }

                        part.RawQty = rawQty;
                        string helisertQtyText = item.Cells["I" + line].Value == null ? string.Empty : item.Cells["I" + line].Value.ToString().Trim();
                        int helisertQty;
                        if (!int.TryParse(helisertQtyText, out helisertQty))
                        {
                            throw new ArgumentException($@"HelisertQty({helisertQtyText}) phải là kiểu số");
                        }

                        part.HelisertQty = helisertQty;
                        string blastQtyText = item.Cells["J" + line].Value == null ? string.Empty : item.Cells["J" + line].Value.ToString().Trim();
                        int blastQty;
                        if (!int.TryParse(blastQtyText, out blastQty))
                        {
                            throw new ArgumentException($@"BlastQty({blastQtyText}) phải là kiểu số");
                        }

                        part.BlastQty = blastQty;
                        part.MONo = item.Cells["K" + line].Value == null ? string.Empty : item.Cells["K" + line].Value.ToString().Trim();
                        string mOQtyText = item.Cells["L" + line].Value == null ? string.Empty : item.Cells["L" + line].Value.ToString().Trim();
                        int mOQty;
                        if (!int.TryParse(mOQtyText, out mOQty))
                        {
                            mOQty = 0;
                        }

                        part.MOQty = mOQty;
                        part.OrderCat = item.Cells["Z" + line].Value == null ? string.Empty : item.Cells["Z" + line].Value.ToString().Trim();
                        part.OrderGoc = item.Cells["AA" + line].Value == null ? string.Empty : item.Cells["AA" + line].Value.ToString().Trim();
                        string TempOrder = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        part.TempOrder = TempOrder.Length > 0 ? true : false;
                        string Started = item.Cells["T" + line].Value == null ? string.Empty : item.Cells["T" + line].Value.ToString().Trim();
                        part.Started = Started.Length > 0 ? true : false;
                        part.Started = true;
                        string Finished = item.Cells["U" + line].Value == null ? string.Empty : item.Cells["U" + line].Value.ToString().Trim();
                        part.Finished = Finished.Length > 0 ? true : false;
                        part.FinishDate = DateTime.Now.AddYears(100);
                        part.Change = item.Cells["W" + line].Value == null ? string.Empty : item.Cells["W" + line].Value.ToString().Trim();
                        string Imported = item.Cells["Y" + line].Value == null ? string.Empty : item.Cells["Y" + line].Value.ToString().Trim();
                        part.Imported = Imported.Length > 0 ? true : false;
                        part.ImportFrom = item.Cells["O" + line].Value == null ? string.Empty : item.Cells["O" + line].Value.ToString().Trim();
                        part.Note = item.Cells["M" + line].Value == null ? string.Empty : item.Cells["M" + line].Value.ToString().Trim();
                        part.Status = item.Cells["N" + line].Value == null ? string.Empty : item.Cells["N" + line].Value.ToString().Trim();
                        db.C242_BusOder.Add(part);
                        db.SaveChanges();
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

        private int GetOrderType(string partID, string type)
        {
            if (type.ToLower() == "m")
            {
                return 3;
            }
            else if (type.ToLower() == "sx")
            {
                return 4;
            }
            int result = 1;
            using (NN_DatabaseEntities db = new NN_DatabaseEntities())
            {
                var odder = db.C242_OptionData.Where(p => p.PartID.ToLower() == partID.ToLower() && p.OptionID.ToLower().StartsWith("xox")).Any();
                if (odder)
                {
                    result = 2;
                }
            }

            return result;
        }
    }
}