using NNworking.Models.ObjectBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public class ImportStokerOutput : IImport
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
                    List<C222_StokerOutput> exportOrder = new List<C222_StokerOutput>();
                    List<C222_StockerOutputDEtail> listDetail = new List<C222_StockerOutputDEtail>();
                    DateTime inputTime = DateTime.Now;
                    while (line < 1000000)
                    {
                        line++;
                        try
                        {
                            //var PartNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString();
                            var Date =               item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                            var StaffID =          item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                            var OrderNo =             item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                            var Qty =            item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                            var ToDept =        item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                            var OrderNoCo =             item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                            var QtyCo =            item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                            var giayXN =        item.Cells["G" + line].Value == null ? false : (item.Cells["G" + line].Value.ToString().Trim().Length == 0 ? false: true);
                            var Note =              item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(OrderNo))
                            {
                                break;
                            }

                            if (string.IsNullOrEmpty(OrderNoCo))
                            {
                                QtyCo = "0";
                            }
                            #region
                            DateTime date;
                            if(!DateTime.TryParse(Date,out date))
                            {
                                throw new ArgumentException("Ngày không đúng định dạng");
                            }

                            int qty;
                            if(!int.TryParse(Qty,out qty))
                            {
                                throw new ArgumentException("Số lượng xuất không đúng định dạng");
                            }

                            int availableQty;
                            if(!int.TryParse(QtyCo,out availableQty))
                            {
                                throw new ArgumentException("Số lượng ghép không đúng định dạng");
                            }

                            if (!string.IsNullOrEmpty(ToDept))
                            {
                                var dept = db.View_222_Partner.Where(X => X.Code.ToLower() == ToDept.ToLower()).Any();
                                if (!dept)
                                {
                                    throw new ArgumentException("Mã khách hàng không có trong danh sách");
                                }
                            }

                            #endregion
                            var main = exportOrder.Where(x => x.OrderNo == OrderNo.ToLower()).Any();
                            string number = $@"{OrderNo}-{date.ToString("yyMMdd")}-{inputTime.ToString("yyMMddHHmm")}";
                            if (!main)
                            {
                                C222_StokerOutput mainObj = new C222_StokerOutput();
                                mainObj.OrderNo = OrderNo.ToLower();
                                mainObj.Note = Note;
                                mainObj.Qty = qty;
                                mainObj.StaffID = staffID;
                                mainObj.ToDept = ToDept;
                                mainObj.Date = date;
                                mainObj.Number = number;
                                exportOrder.Add(mainObj);
                            }

                            C222_StockerOutputDEtail detail = new C222_StockerOutputDEtail();
                            detail.OderCan = OrderNo;
                            detail.OrderCo = OrderNoCo;
                            detail.SlgCan = qty;
                            detail.SlgCo = availableQty;
                            detail.GiayXN = giayXN;
                            detail.Number = number;
                            listDetail.Add(detail);
                        }
                        catch (Exception ex)
                        {
                            Error.Add(new clsError(line, "Not OK", ex.Message));
                        }
                    }

                    try
                    {
                        foreach(var data in exportOrder)
                        {
                            db.C222_StokerOutput.Add(data);
                        }

                        foreach(var data in listDetail)
                        {
                            db.C222_StockerOutputDEtail.Add(data);
                        }

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