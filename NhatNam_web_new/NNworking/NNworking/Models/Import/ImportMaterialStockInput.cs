using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace NNworking.Models.Import
{
    public class ImportMaterialStockInput : ImportMaterial
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
                        var MaterialID = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(MaterialID))
                        {
                            continue;
                        }

                        var Supp = item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                        CheckSupplierExit(Supp);
                        CheckMaterialExist(MaterialID,Supp);
                        var MaterialConfiguration = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                        CheckMaterialConfigurationExist(MaterialConfiguration,MaterialID);
                        var qty = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        int Qty;
                        if(!int.TryParse(qty, out Qty))
                        {
                            throw new ArgumentException("Số lượng phải là kiểu số. Vui lòng xem lại!");
                        }
                        var Unit = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                        var weight = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                        int Weight;
                        if(!int.TryParse(weight, out Weight))
                        {
                            Weight = 0;
                        }

                        var Note = item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                        var shapID = item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();

                        if (string.IsNullOrEmpty(Unit))
                        {
                            Unit = "Gam";
                        }

                        if(Weight == 0)
                        {
                            Weight = WeightCalculation(MaterialConfiguration,shapID,MaterialID)/1000;// chia 1000 để tính về cm3
                        }

                        var obj = new C222_MaterialStock_Input();
                        obj.Date = Date;
                        obj.MaterialConfiguration = MaterialConfiguration;
                        obj.MaterialID = MaterialID;
                        obj.Note = Note;
                        obj.Unit = Unit;
                        obj.Qty = Qty;
                        obj.Weight = Weight*Qty;
                        obj.Code = $@"{MaterialID}-{Supp}";
                        db.C222_MaterialStock_Input.Add(obj);
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
                case "PIPE":// tròn rỗng
                    result = (a[0] * a[0] - a[1] * a[1]) * a[2] * 3.14 / 4;
                    break;
                case "PLATE"://Tấm 
                    result = a[0] * a[1] * a[2];
                    break;
                case "BAR": // tấm
                    int item = a.Where(x => x > 0).ToList().Count();
                    if(item == 2)
                    {
                        result = ((a[0] * a[0] * a[1] * 3.14)/4);
                    }
                    else if(item > 2)
                    {
                        result = a[0] * a[1] * a[2];
                    }
                    break;
                case "ROUND": // tròn đặc
                    result = (a[0] * a[0] * a[1] * 3.14)/4;
                    break;
                case "HEXAN":
                    result = (a[0] * a[0]*3*Math.Sqrt(3)/8)*a[1];
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
            var arr = materialConfiguration.Replace("B",string.Empty).ToLower().Split('x');
            for(int i = 0; i < arr.Length; i++)
            {
                int number;
                if(int.TryParse(arr[i],out number))
                {
                    result[i] = number;
                }
            }

            return result;
        }
    }
}