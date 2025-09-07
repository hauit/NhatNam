using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace NNworking.Models.Controllers
{
    [Route("api/C242_MachinePlanning/{action}", Name = "C242_MachinePlanningApi")]
    public class C242_MachinePlanningController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage GetForShift(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var fromDate = DateTime.ParseExact(queryParams["fromDate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var shift = queryParams["shift"];
            var toDate = DateTime.ParseExact(queryParams["toDate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var c242_machineplanning = _context.C242_MachinePlanning.Where(x => x.Date >= fromDate && x.Date <= toDate && x.Shift == shift && x.Pre_State == true).ToList();
            if(string.IsNullOrEmpty(shift))
            {
                c242_machineplanning = _context.C242_MachinePlanning.Where(x => x.Date >= fromDate && x.Date <= toDate && x.Pre_State == true).ToList();
            }

            return Request.CreateResponse(DataSourceLoader.Load(c242_machineplanning, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_machineplanning = _context.C242_MachinePlanning.Select(i => new {
                i.ID,
                i.Order,
                i.NC,
                i.MayGC,
                i.Slglenh,
                i.BatDau,
                i.KetThuc,
                i.TGGC,
                i.TGGL,
                i.TenChiTiet,
                i.K,
                i.Date,
                i.Shift,
                i.ThoiHan,
                i.TinhTrang,
                i.DKmay,
                i.SoJig,
                i.TTNC,
                i.Dept,
                i.Jig,
                i.Note,
                i.LastOption,
                i.RegisterDate,
                i.C10_,
                i.C20_,
                i.C80_,
                i.Via_check1,
                i.Via_check2,
                i.Via_ghi_chu,
                i.Fac_NgayHTtheoKH,
                i.Fac_TT,
                i.Fac_NGTruoc,
                i.Fac_TTFile,
                i.Fac_Phoi,
                i.Fac_File,
                i.Fac_Dao,
                i.Fac_Jig,
                i.Fac_TTTH,
                i.Fac_DaChuanBi,
                i.Fac_TTGC,
                i.Fac_DeXuat,
                i.Fac_OK_Old,
                i.Fac_NG_Old,
                i.Fac_Dc,
                i.Fac_MachineID,
                i.Fac_GCXong,
                i.Fac_GCDo,
                i.Fac_TTFileMem,
                i.Fac_ThaoTac,
                i.KT_TGTC_HT,
                i.KT_Sup,
                i.KT_TGUKB1,
                i.KT_TongTG,
                i.KT_TTPhoi,
                i.KT_GiaHang,
                i.KT_KHXN,
                i.KT_LyDoKHTKH,
                i.KT_DgoiTay,
                i.MachineStatus,
                i.Cua_GCTime,
                i.Cua_Factory,
                i.Cua_TotalTime,
                i.Cua_Plantime,
                i.Cua_PrepareDate,
                i.Cua_MachineGroup,
                i.Cua_State,
                i.Cua_Size,
                i.Cua_PrepareState,
                i.Pre_State,
                i.Pre_ConfirmDate,
                i.TongTG,
                i.Phoidd,
                i.Nesting,
                i.ProcessInFreely,
                i.ViTriGia,
                i.Deleted
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_machineplanning, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_MachinePlanning();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_MachinePlanning.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_MachinePlanning.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_MachinePlanning not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public void Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_MachinePlanning.FirstOrDefault(item => item.ID == key);

            _context.C242_MachinePlanning.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_MachinePlanning model, IDictionary values) {
            string ID = nameof(C242_MachinePlanning.ID);
            string ORDER = nameof(C242_MachinePlanning.Order);
            string NC = nameof(C242_MachinePlanning.NC);
            string MAY_GC = nameof(C242_MachinePlanning.MayGC);
            string SLGLENH = nameof(C242_MachinePlanning.Slglenh);
            string BAT_DAU = nameof(C242_MachinePlanning.BatDau);
            string KET_THUC = nameof(C242_MachinePlanning.KetThuc);
            string TGGC = nameof(C242_MachinePlanning.TGGC);
            string TGGL = nameof(C242_MachinePlanning.TGGL);
            string TEN_CHI_TIET = nameof(C242_MachinePlanning.TenChiTiet);
            string K = nameof(C242_MachinePlanning.K);
            string DATE = nameof(C242_MachinePlanning.Date);
            string SHIFT = nameof(C242_MachinePlanning.Shift);
            string THOI_HAN = nameof(C242_MachinePlanning.ThoiHan);
            string TINH_TRANG = nameof(C242_MachinePlanning.TinhTrang);
            string DKMAY = nameof(C242_MachinePlanning.DKmay);
            string SO_JIG = nameof(C242_MachinePlanning.SoJig);
            string TTNC = nameof(C242_MachinePlanning.TTNC);
            string DEPT = nameof(C242_MachinePlanning.Dept);
            string JIG = nameof(C242_MachinePlanning.Jig);
            string NOTE = nameof(C242_MachinePlanning.Note);
            string LAST_OPTION = nameof(C242_MachinePlanning.LastOption);
            string REGISTER_DATE = nameof(C242_MachinePlanning.RegisterDate);
            string C10_ = nameof(C242_MachinePlanning.C10_);
            string C20_ = nameof(C242_MachinePlanning.C20_);
            string C80_ = nameof(C242_MachinePlanning.C80_);
            string VIA_CHECK1 = nameof(C242_MachinePlanning.Via_check1);
            string VIA_CHECK2 = nameof(C242_MachinePlanning.Via_check2);
            string VIA_GHI_CHU = nameof(C242_MachinePlanning.Via_ghi_chu);
            string FAC_NGAY_HTTHEO_KH = nameof(C242_MachinePlanning.Fac_NgayHTtheoKH);
            string FAC_TT = nameof(C242_MachinePlanning.Fac_TT);
            string FAC_NGTRUOC = nameof(C242_MachinePlanning.Fac_NGTruoc);
            string FAC_TTFILE = nameof(C242_MachinePlanning.Fac_TTFile);
            string FAC_PHOI = nameof(C242_MachinePlanning.Fac_Phoi);
            string FAC_FILE = nameof(C242_MachinePlanning.Fac_File);
            string FAC_DAO = nameof(C242_MachinePlanning.Fac_Dao);
            string FAC_JIG = nameof(C242_MachinePlanning.Fac_Jig);
            string FAC_TTTH = nameof(C242_MachinePlanning.Fac_TTTH);
            string FAC_DA_CHUAN_BI = nameof(C242_MachinePlanning.Fac_DaChuanBi);
            string FAC_TTGC = nameof(C242_MachinePlanning.Fac_TTGC);
            string FAC_DE_XUAT = nameof(C242_MachinePlanning.Fac_DeXuat);
            string FAC_OK_OLD = nameof(C242_MachinePlanning.Fac_OK_Old);
            string FAC_NG_OLD = nameof(C242_MachinePlanning.Fac_NG_Old);
            string FAC_DC = nameof(C242_MachinePlanning.Fac_Dc);
            string FAC_MACHINE_ID = nameof(C242_MachinePlanning.Fac_MachineID);
            string FAC_GCXONG = nameof(C242_MachinePlanning.Fac_GCXong);
            string FAC_GCDO = nameof(C242_MachinePlanning.Fac_GCDo);
            string FAC_TTFILE_MEM = nameof(C242_MachinePlanning.Fac_TTFileMem);
            string FAC_THAO_TAC = nameof(C242_MachinePlanning.Fac_ThaoTac);
            string KT_TGTC_HT = nameof(C242_MachinePlanning.KT_TGTC_HT);
            string KT_SUP = nameof(C242_MachinePlanning.KT_Sup);
            string KT_TGUKB1 = nameof(C242_MachinePlanning.KT_TGUKB1);
            string KT_TONG_TG = nameof(C242_MachinePlanning.KT_TongTG);
            string KT_TTPHOI = nameof(C242_MachinePlanning.KT_TTPhoi);
            string KT_GIA_HANG = nameof(C242_MachinePlanning.KT_GiaHang);
            string KT_KHXN = nameof(C242_MachinePlanning.KT_KHXN);
            string KT_LY_DO_KHTKH = nameof(C242_MachinePlanning.KT_LyDoKHTKH);
            string KT_DGOI_TAY = nameof(C242_MachinePlanning.KT_DgoiTay);
            string MACHINE_STATUS = nameof(C242_MachinePlanning.MachineStatus);
            string CUA_GCTIME = nameof(C242_MachinePlanning.Cua_GCTime);
            string CUA_FACTORY = nameof(C242_MachinePlanning.Cua_Factory);
            string CUA_TOTAL_TIME = nameof(C242_MachinePlanning.Cua_TotalTime);
            string CUA_PLANTIME = nameof(C242_MachinePlanning.Cua_Plantime);
            string CUA_PREPARE_DATE = nameof(C242_MachinePlanning.Cua_PrepareDate);
            string CUA_MACHINE_GROUP = nameof(C242_MachinePlanning.Cua_MachineGroup);
            string CUA_STATE = nameof(C242_MachinePlanning.Cua_State);
            string CUA_SIZE = nameof(C242_MachinePlanning.Cua_Size);
            string CUA_PREPARE_STATE = nameof(C242_MachinePlanning.Cua_PrepareState);
            string PRE_STATE = nameof(C242_MachinePlanning.Pre_State);
            string PRE_CONFIRM_DATE = nameof(C242_MachinePlanning.Pre_ConfirmDate);
            string TONG_TG = nameof(C242_MachinePlanning.TongTG);
            string PHOIDD = nameof(C242_MachinePlanning.Phoidd);
            string NESTING = nameof(C242_MachinePlanning.Nesting);
            string PROCESS_IN_FREELY = nameof(C242_MachinePlanning.ProcessInFreely);
            string VI_TRI_GIA = nameof(C242_MachinePlanning.ViTriGia);
            string DELETED = nameof(C242_MachinePlanning.Deleted);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(ORDER)) {
                model.Order = Convert.ToString(values[ORDER]);
            }

            if(values.Contains(NC)) {
                model.NC = Convert.ToString(values[NC]);
            }

            if(values.Contains(MAY_GC)) {
                model.MayGC = Convert.ToString(values[MAY_GC]);
            }

            if(values.Contains(SLGLENH)) {
                model.Slglenh = Convert.ToInt32(values[SLGLENH]);
            }

            if(values.Contains(BAT_DAU)) {
                model.BatDau = Convert.ToDateTime(values[BAT_DAU]);
            }

            if(values.Contains(KET_THUC)) {
                model.KetThuc = Convert.ToDateTime(values[KET_THUC]);
            }

            if(values.Contains(TGGC)) {
                model.TGGC = Convert.ToDouble(values[TGGC]);
            }

            if(values.Contains(TGGL)) {
                model.TGGL = Convert.ToDouble(values[TGGL]);
            }

            if(values.Contains(TEN_CHI_TIET)) {
                model.TenChiTiet = Convert.ToString(values[TEN_CHI_TIET]);
            }

            if(values.Contains(K)) {
                model.K = Convert.ToString(values[K]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(SHIFT)) {
                model.Shift = Convert.ToString(values[SHIFT]);
            }

            if(values.Contains(THOI_HAN)) {
                model.ThoiHan = Convert.ToDateTime(values[THOI_HAN]);
            }

            if(values.Contains(TINH_TRANG)) {
                model.TinhTrang = Convert.ToInt32(values[TINH_TRANG]);
            }

            if(values.Contains(DKMAY)) {
                model.DKmay = Convert.ToString(values[DKMAY]);
            }

            if(values.Contains(SO_JIG)) {
                model.SoJig = Convert.ToString(values[SO_JIG]);
            }

            if(values.Contains(TTNC)) {
                model.TTNC = Convert.ToString(values[TTNC]);
            }

            if(values.Contains(DEPT)) {
                model.Dept = Convert.ToString(values[DEPT]);
            }

            if(values.Contains(JIG)) {
                model.Jig = Convert.ToString(values[JIG]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(LAST_OPTION)) {
                model.LastOption = Convert.ToString(values[LAST_OPTION]);
            }

            if(values.Contains(REGISTER_DATE)) {
                model.RegisterDate = Convert.ToDateTime(values[REGISTER_DATE]);
            }

            if(values.Contains(C10_)) {
                model.C10_ = Convert.ToInt32(values[C10_]);
            }

            if(values.Contains(C20_)) {
                model.C20_ = Convert.ToInt32(values[C20_]);
            }

            if(values.Contains(C80_)) {
                model.C80_ = Convert.ToInt32(values[C80_]);
            }

            if(values.Contains(VIA_CHECK1)) {
                model.Via_check1 = Convert.ToString(values[VIA_CHECK1]);
            }

            if(values.Contains(VIA_CHECK2)) {
                model.Via_check2 = Convert.ToString(values[VIA_CHECK2]);
            }

            if(values.Contains(VIA_GHI_CHU)) {
                model.Via_ghi_chu = Convert.ToString(values[VIA_GHI_CHU]);
            }

            if(values.Contains(FAC_NGAY_HTTHEO_KH)) {
                model.Fac_NgayHTtheoKH = Convert.ToDateTime(values[FAC_NGAY_HTTHEO_KH]);
            }

            if(values.Contains(FAC_TT)) {
                model.Fac_TT = Convert.ToInt32(values[FAC_TT]);
            }

            if(values.Contains(FAC_NGTRUOC)) {
                model.Fac_NGTruoc = Convert.ToString(values[FAC_NGTRUOC]);
            }

            if(values.Contains(FAC_TTFILE)) {
                model.Fac_TTFile = Convert.ToString(values[FAC_TTFILE]);
            }

            if(values.Contains(FAC_PHOI)) {
                model.Fac_Phoi = Convert.ToBoolean(values[FAC_PHOI]);
            }

            if(values.Contains(FAC_FILE)) {
                model.Fac_File = Convert.ToBoolean(values[FAC_FILE]);
            }

            if(values.Contains(FAC_DAO)) {
                model.Fac_Dao = Convert.ToBoolean(values[FAC_DAO]);
            }

            if(values.Contains(FAC_JIG)) {
                model.Fac_Jig = Convert.ToBoolean(values[FAC_JIG]);
            }

            if(values.Contains(FAC_TTTH)) {
                model.Fac_TTTH = Convert.ToInt32(values[FAC_TTTH]);
            }

            if(values.Contains(FAC_DA_CHUAN_BI)) {
                model.Fac_DaChuanBi = Convert.ToBoolean(values[FAC_DA_CHUAN_BI]);
            }

            if(values.Contains(FAC_TTGC)) {
                model.Fac_TTGC = Convert.ToBoolean(values[FAC_TTGC]);
            }

            if(values.Contains(FAC_DE_XUAT)) {
                model.Fac_DeXuat = Convert.ToString(values[FAC_DE_XUAT]);
            }

            if(values.Contains(FAC_OK_OLD)) {
                model.Fac_OK_Old = Convert.ToInt32(values[FAC_OK_OLD]);
            }

            if(values.Contains(FAC_NG_OLD)) {
                model.Fac_NG_Old = Convert.ToInt32(values[FAC_NG_OLD]);
            }

            if(values.Contains(FAC_DC)) {
                model.Fac_Dc = Convert.ToBoolean(values[FAC_DC]);
            }

            if(values.Contains(FAC_MACHINE_ID)) {
                model.Fac_MachineID = Convert.ToString(values[FAC_MACHINE_ID]);
            }

            if(values.Contains(FAC_GCXONG)) {
                model.Fac_GCXong = Convert.ToBoolean(values[FAC_GCXONG]);
            }

            if(values.Contains(FAC_GCDO)) {
                model.Fac_GCDo = Convert.ToBoolean(values[FAC_GCDO]);
            }

            if(values.Contains(FAC_TTFILE_MEM)) {
                model.Fac_TTFileMem = Convert.ToBoolean(values[FAC_TTFILE_MEM]);
            }

            if(values.Contains(FAC_THAO_TAC)) {
                model.Fac_ThaoTac = Convert.ToBoolean(values[FAC_THAO_TAC]);
            }

            if(values.Contains(KT_TGTC_HT)) {
                model.KT_TGTC_HT = Convert.ToDouble(values[KT_TGTC_HT]);
            }

            if(values.Contains(KT_SUP)) {
                model.KT_Sup = Convert.ToString(values[KT_SUP]);
            }

            if(values.Contains(KT_TGUKB1)) {
                model.KT_TGUKB1 = Convert.ToDouble(values[KT_TGUKB1]);
            }

            if(values.Contains(KT_TONG_TG)) {
                model.KT_TongTG = Convert.ToDouble(values[KT_TONG_TG]);
            }

            if(values.Contains(KT_TTPHOI)) {
                model.KT_TTPhoi = Convert.ToInt32(values[KT_TTPHOI]);
            }

            if(values.Contains(KT_GIA_HANG)) {
                model.KT_GiaHang = Convert.ToString(values[KT_GIA_HANG]);
            }

            if(values.Contains(KT_KHXN)) {
                model.KT_KHXN = Convert.ToString(values[KT_KHXN]);
            }

            if(values.Contains(KT_LY_DO_KHTKH)) {
                model.KT_LyDoKHTKH = Convert.ToString(values[KT_LY_DO_KHTKH]);
            }

            if(values.Contains(KT_DGOI_TAY)) {
                model.KT_DgoiTay = Convert.ToString(values[KT_DGOI_TAY]);
            }

            if(values.Contains(MACHINE_STATUS)) {
                model.MachineStatus = Convert.ToBoolean(values[MACHINE_STATUS]);
            }

            if(values.Contains(CUA_GCTIME)) {
                model.Cua_GCTime = Convert.ToDateTime(values[CUA_GCTIME]);
            }

            if(values.Contains(CUA_FACTORY)) {
                model.Cua_Factory = Convert.ToString(values[CUA_FACTORY]);
            }

            if(values.Contains(CUA_TOTAL_TIME)) {
                model.Cua_TotalTime = Convert.ToDouble(values[CUA_TOTAL_TIME]);
            }

            if(values.Contains(CUA_PLANTIME)) {
                model.Cua_Plantime = Convert.ToDouble(values[CUA_PLANTIME]);
            }

            if(values.Contains(CUA_PREPARE_DATE)) {
                model.Cua_PrepareDate = Convert.ToString(values[CUA_PREPARE_DATE]);
            }

            if(values.Contains(CUA_MACHINE_GROUP)) {
                model.Cua_MachineGroup = Convert.ToString(values[CUA_MACHINE_GROUP]);
            }

            if(values.Contains(CUA_STATE)) {
                model.Cua_State = Convert.ToBoolean(values[CUA_STATE]);
            }

            if(values.Contains(CUA_SIZE)) {
                model.Cua_Size = Convert.ToString(values[CUA_SIZE]);
            }

            if(values.Contains(CUA_PREPARE_STATE)) {
                model.Cua_PrepareState = Convert.ToBoolean(values[CUA_PREPARE_STATE]);
            }

            if(values.Contains(PRE_STATE)) {
                model.Pre_State = Convert.ToBoolean(values[PRE_STATE]);
            }

            if(values.Contains(PRE_CONFIRM_DATE)) {
                model.Pre_ConfirmDate = Convert.ToDateTime(values[PRE_CONFIRM_DATE]);
            }

            if(values.Contains(TONG_TG)) {
                model.TongTG = Convert.ToString(values[TONG_TG]);
            }

            if(values.Contains(PHOIDD)) {
                model.Phoidd = Convert.ToString(values[PHOIDD]);
            }

            if(values.Contains(NESTING)) {
                model.Nesting = Convert.ToString(values[NESTING]);
            }

            if(values.Contains(PROCESS_IN_FREELY)) {
                model.ProcessInFreely = Convert.ToBoolean(values[PROCESS_IN_FREELY]);
            }

            if(values.Contains(VI_TRI_GIA)) {
                model.ViTriGia = Convert.ToString(values[VI_TRI_GIA]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}