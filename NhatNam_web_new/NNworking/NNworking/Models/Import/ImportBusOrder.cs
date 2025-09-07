using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace NNworking.Models.Import
{
    public class ImportBusOrder : IImport
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

                switch (type)
                {
                    case (int)ImportBOderType.ImportTDTK:
                        Error = ImportTDTK(item);
                        break;
                }
            }
            return Error;
        }

        private List<clsError> ImportTDTK(ExcelWorksheet item)
        {
            List<clsError> Error = new List<clsError>();
            int line = 1;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            while (line < 1000000)
            {
                line++;
                try
                {
                    var oldOder = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                    if (string.IsNullOrEmpty(oldOder))
                    {
                        continue;
                    }
                    var data = db.C242_BusOder.Where(x => x.BOderNo.ToLower() == oldOder.ToLower()).FirstOrDefault();
                    if(data == null)
                    {
                        continue;
                    }

                    var newOder = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                    var partID = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                        
                    if (!string.IsNullOrEmpty(newOder))
                    {
                        data.BOderNo = newOder;
                        data.MONo = data.BOderNo;
                    }

                    if (!string.IsNullOrEmpty(partID))
                    {
                        data.PartID = partID;
                    }

                    //db.C242_BusOder.Add(obj);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Error.Add(new clsError(line, "Not OK", ex.Message));
                }
            }

            return Error;
        }
    }
}