using NNworking.Models.ObjectBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportStokerTool : IImport
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
                            var ToolNo =               item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                            var ToolName =          item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                            var Unit =             item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                            var Catergory =            item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(ToolNo))
                            {
                                break;
                            }

                            #region
                            var unit = db.C222_StokerUnit.Where(x => x.Name.ToLower().Trim() == Unit.ToLower().Trim()).FirstOrDefault();
                            if (unit == null)
                            {
                                throw new ArgumentException("Đơn vị tính không tồn tại trong danh sách");
                            }

                            var cat = db.C222_StokerCatergory.Where(x => x.Catergory.ToLower().Trim() == Catergory.ToLower().Trim()).FirstOrDefault();
                            if (cat == null)
                            {
                                throw new ArgumentException("Danh mục sản phẩm không tồn tại trong danh sách");
                            }

                            #endregion
                            var obj = new C222_StokerTool();
                            objectBase = new StokerInputObjectBase();
                            var obj1 = (object)obj;
                            objectBase.SetDefaultValue(ref obj1);
                            obj.ToolNo = ToolNo;
                            obj.ToolName = ToolName;
                            obj.Unit = unit.ID;
                            obj.CatergoryID = cat.Alias;
                            db.C222_StokerTool.Add(obj);
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