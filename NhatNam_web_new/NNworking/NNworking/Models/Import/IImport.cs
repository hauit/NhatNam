using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NNworking.Models.ObjectBase;
using System.IO;

namespace NNworking.Models.Import
{
    public abstract class IImport
    {
        protected DateTime Date;
        protected abstract List<clsError> ImportExecution(ExcelPackage package, string staffID, int type = 0);
        public bool ImportData(string fname, string staffID, out List<clsError> Error, int type = 0)
        {
            Error = new List<clsError>();
            ExcelPackage.License.SetNonCommercialPersonal("Alo1234");
            using (ExcelPackage package = new ExcelPackage(new FileInfo(fname)))
            {
                var b = package.Workbook.Worksheets.Count;
                if (b == 0)
                {
                    return false;
                }
                
                Error = ImportExecution(package,staffID,type);
            }
            return true;
        }

        public bool ImportData(string fname, string staffID,DateTime date, out List<clsError> Error, int type = 0)
        {
            Date = date;
            return ImportData(fname, staffID, out Error,type);
        }
    }
    
    public enum ImportBOderType
    {
        ImportBOderList,
        ImportTDTK
    }

}
