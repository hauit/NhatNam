using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportPartList : IImport
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
                        //var MONo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        line++;
                        C242_Part part = new C242_Part();
                        part.PartNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(part.PartNo))
                        {
                            continue;
                        }

                        if (CheckExistedOrder(part.PartNo))
                        {
                            throw new ArgumentException($@"Tên chi tiết {part.PartNo} đã tồn tại trong dữ liệu");
                        }

                        //part.PartNoRRC = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        CheckPartNo(part);
                        part.PartName = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        part.CustomerID = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        part.SupplierID = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                        //part.Unit = item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();   
                        string qty = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                        int upQty;
                        if (!int.TryParse(qty, out upQty))
                        {
                            throw new ArgumentException("UpQty phải là kiểu số. Vui lòng kiểm tra lại dữ liệu.");
                        }

                        part.UpQty = upQty;
                        //string gia = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                        //int giaThanh;
                        //if (!int.TryParse(gia, out giaThanh))
                        //{
                        //    giaThanh = 0;
                        //    //throw new ArgumentException("Giá thành phải là kiểu số. Vui lòng kiểm tra lại dữ liệu.");
                        //}
                        //part.GiaThanh = giaThanh;
                        db.C242_Part.Add(part);
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

        private void CheckPartNo(C242_Part part)
        {
            part.PartNoRRC = part.PartNo;
            if (part.PartNo.ToUpper().StartsWith("CSZ0-"))
            {
                part.PartNo = part.PartNo.Replace("CSZ0-", string.Empty); 
            }

            if (part.PartNo.ToUpper().EndsWith("QM"))
            {
                part.PartNo = part.PartNo.Replace("QM", string.Empty);
            }   
        }

        private bool CheckExistedOrder(string partNo)
        {
            using (NN_DatabaseEntities db = new NN_DatabaseEntities())
            {
                var data = db.C242_Part.Where(m => m.PartNo == partNo).Any();
                return data;   
            }    

        }
    }
}