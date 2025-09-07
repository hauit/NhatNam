using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace NNworking.Models.Import
{
    public class ImportMaterialStockOutput : ImportMaterial
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
                        var orderNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(orderNo))
                        {
                            continue;
                        }

                        var Supp = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                        CheckOrderExist(orderNo);
                        var MaterialID = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        CheckMaterialExist(MaterialID,Supp);
                        var MaterialConfiguration = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        CheckMaterialConfigurationExist(MaterialConfiguration,MaterialID);
                        var Unit = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                        var qty = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                        int Qty;
                        if(!int.TryParse(qty, out Qty))
                        {
                            throw new ArgumentException("Số lượng phải là kiểu số. Vui lòng xem lại!");
                        }
                        var BPYC = item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                        CheckBPYC(BPYC);
                        var Note = item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                        var shapID = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();

                        int Weight = WeightCalculation(MaterialConfiguration,shapID,MaterialID);
                        var obj = new C222_MaterialStock_Output();
                        obj.Date = Date;
                        obj.BPYC = BPYC;
                        obj.OrderNo = orderNo;
                        obj.MaterialConfiguration = MaterialConfiguration;
                        obj.MaterialID = MaterialID;
                        obj.Note = Note;
                        obj.Unit = Unit;
                        obj.Qty = Qty;
                        obj.Weight = Weight*Qty;
                        obj.Code = $@"{MaterialID}-{Supp}";
                        db.C222_MaterialStock_Output.Add(obj);
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
        
        private int WeightCalculation(string materialConfiguration, string shapID, string materialID)
        {
            double result = 0;
            List<int> a = GetMeasure(materialConfiguration);
            switch (shapID.ToUpper())
            {
                case "PIPE":
                    result = (a[0] * a[0] - a[1] * a[1]) * a[2] * 3.14 / 4;
                    break;
                case "PLATE":
                    result = a[0] * a[1] * a[2];
                    break;
                case "BAR":
                    if(a.Count == 2)
                    {
                        result = (a[0] * a[0] * a[1] * 3.14)/4;
                    }
                    else if(a.Count > 2)
                    {
                        result = a[0] * a[1] * a[2];
                    }
                    break;
                case "ROUND":
                    result = (a[0] * a[0] * a[1] * 3.14)/4;
                    break;
                case "HEXAN":
                    result = (a[0] * a[0] * a[1] * 3.14)/4;
                    break;
            }

            return (int)result;
        }

        private List<int> GetMeasure(string materialConfiguration)
        {
            List<int> result = new List<int>();
            result.Add(0);
            result.Add(0);
            result.Add(0);
            result.Add(0);
            result.Add(0);
            result.Add(0);
            var arr = materialConfiguration.ToLower().Split('x');
            for(int i = 0; i < arr.Length; i++)
            {
                int number;
                if(int.TryParse(arr[i],out number))
                {
                    result[i] = number+5;
                }
            }

            return result;
        }
    }
}





