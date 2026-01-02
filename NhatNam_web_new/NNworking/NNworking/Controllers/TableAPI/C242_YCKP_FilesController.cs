using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using NNworking.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.UI.WebControls;

namespace NNworking.Controllers
{
    [Route("api/C242_YCKP_Files/{action}", Name = "C242_YCKP_FilesApi")]
    public class C242_YCKP_FilesController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var c242_yckp_files = _context.C242_YCKP_Files.Select(i => new
            {
                i.ID,
                i.OrderNo,
                i.ResponseId,
                i.Date,
                i.StaffId,
                i.Path
            });

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_yckp_files, loadOptions));
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetByOrderNoDateStaff()
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs()
                    .ToDictionary(x => x.Key, x => x.Value);

                string orderNo = queryParams.ContainsKey("OrderNo") ? queryParams["OrderNo"] : "";
                string staffId = queryParams.ContainsKey("StaffId") ? queryParams["StaffId"] : "";
                string dateStr = queryParams.ContainsKey("Date") ? queryParams["Date"] : "";

                DateTime date;
                DateTime.TryParse(dateStr, out date);
                var nextSecond = date.AddSeconds(1);
                var prevSecond = date.AddSeconds(-1);

                var record = await _context.C242_YCKP_Files
                    .Where(x =>
                        x.OrderNo == orderNo &&
                        (x.Date > prevSecond && x.Date < nextSecond) &&
                        x.StaffId == staffId
                    ).ToListAsync();

                return Request.CreateResponse(HttpStatusCode.OK, record);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UploadFiles()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                var uploadedFiles = new List<C242_YCKP_Files>();

                var orderNo = HttpContext.Current.Request.Form["OrderNo"];
                var staffId = HttpContext.Current.Request.Form["StaffId"];

                TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime vietnamNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

                var todayFolder = vietnamNow.Date.ToString("dd-MM-yyyy");
                var baseFolder = HttpContext.Current.Server.MapPath("~/Files");
                var folderPath = Path.Combine(baseFolder, todayFolder);
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);

                var fileParts = provider.Contents
                    .Where(c => c.Headers.ContentDisposition.FileName != null);

                foreach (var file in fileParts)
                {
                    var rawFileName = file.Headers.ContentDisposition?.FileName?.Trim('"');
                    var cleanFileName = string.Concat(rawFileName.Split(Path.GetInvalidFileNameChars()));

                    var ext = Path.GetExtension(cleanFileName);
                    var nameWithoutExt = Path.GetFileNameWithoutExtension(cleanFileName);

                    var guid = Guid.NewGuid().ToString();
                    var filename = $"{nameWithoutExt}_{guid}{ext}";

                    var fileBytes = await file.ReadAsByteArrayAsync();
                    var filePath = System.IO.Path.Combine(folderPath, filename);

                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    var record = new C242_YCKP_Files
                    {
                        OrderNo = orderNo,
                        StaffId = staffId,
                        Date = vietnamNow,  // Sử dụng thời gian từ backend
                        Path = $"/Files/{todayFolder}/{filename}"
                    };

                    _context.C242_YCKP_Files.Add(record);
                    uploadedFiles.Add(record);
                }

                await _context.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, uploadedFiles.Select(f => new
                {
                    f.ID
                }));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var model = new C242_YCKP_Files();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_YCKP_Files.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_YCKP_Files.FirstOrDefaultAsync(item => item.ID == key);
            if (model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "Object not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_YCKP_Files.FirstOrDefaultAsync(item => item.ID == key);

            _context.C242_YCKP_Files.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C242_YCKP_Files model, IDictionary values)
        {
            string ID = nameof(C242_YCKP_Files.ID);
            string ORDER_NO = nameof(C242_YCKP_Files.OrderNo);
            string RESPONSE_ID = nameof(C242_YCKP_Files.ResponseId);
            string DATE = nameof(C242_YCKP_Files.Date);
            string STAFF_ID = nameof(C242_YCKP_Files.StaffId);
            string PATH = nameof(C242_YCKP_Files.Path);

            if (values.Contains(ID))
            {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(ORDER_NO))
            {
                model.OrderNo = Convert.ToString(values[ORDER_NO]);
            }

            if (values.Contains(RESPONSE_ID))
            {
                model.ResponseId = Convert.ToString(values[RESPONSE_ID]);
            }

            if (values.Contains(DATE))
            {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if (values.Contains(STAFF_ID))
            {
                model.StaffId = Convert.ToString(values[STAFF_ID]);
            }

            if (values.Contains(PATH))
            {
                model.Path = Convert.ToString(values[PATH]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}