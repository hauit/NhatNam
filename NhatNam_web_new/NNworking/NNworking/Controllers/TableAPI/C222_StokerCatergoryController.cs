using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace NNworking.Models.Controllers
{
    [Route("api/C222_StokerCatergory/{action}", Name = "C222_StokerCatergoryApi")]
    public class C222_StokerCatergoryController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_stokercatergory = _context.C222_StokerCatergory.Select(i => new {
                i.ID,
                i.MenuType,
                i.Catergory,
                i.Alias,
                i.Note
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_stokercatergory, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetCatergory(DataSourceLoadOptions loadOptions) {
            var c222_stokercatergory = _context.C222_StokerCatergory.Where(x=>x.MenuType > 1).Select(i => new {
                i.ID,
                i.MenuType,
                i.Catergory,
                i.Alias,
                i.Note
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_stokercatergory, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_StokerCatergory();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_StokerCatergory.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.Alias);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToString(form.Get("key"));
            var model = _context.C222_StokerCatergory.FirstOrDefault(item => item.Alias == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_StokerCatergory not found");

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
            var key = Convert.ToString(form.Get("key"));
            var model = _context.C222_StokerCatergory.FirstOrDefault(item => item.Alias == key);

            _context.C222_StokerCatergory.Remove(model);
            _context.SaveChanges();
        }

        public string ConvertToUnaccentedSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            input = input.Replace('đ', 'd').Replace('Đ', 'D');
            // Bước 1: Chuẩn hóa Unicode (FormD tách dấu ra khỏi ký tự)
            string normalized = input.Normalize(NormalizationForm.FormD);

            // Bước 2: Loại bỏ các dấu (combining characters)
            StringBuilder sb = new StringBuilder();
            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            // Bước 3: Chuyển chuỗi về FormC (chuẩn bình thường)
            string cleaned = sb.ToString().Normalize(NormalizationForm.FormC);

            // Bước 4: Đưa về chữ thường
            cleaned = cleaned.ToLowerInvariant();

            // Bước 5: Thay các khoảng trắng bằng dấu gạch ngang
            cleaned = Regex.Replace(cleaned, @"\s+", "-");

            // Bước 6: Loại bỏ các ký tự đặc biệt không mong muốn (giữ lại chữ cái, số, dấu gạch ngang)
            cleaned = Regex.Replace(cleaned, @"[^a-z0-9\-]", "");

            // Bước 7: Loại bỏ dấu gạch ngang thừa (nếu có ở đầu/cuối)
            cleaned = cleaned.Trim('-');

            return cleaned;
        }

        private void PopulateModel(C222_StokerCatergory model, IDictionary values) {
            string ID = nameof(C222_StokerCatergory.ID);
            string MENU_TYPE = nameof(C222_StokerCatergory.MenuType);
            string CATERGORY = nameof(C222_StokerCatergory.Catergory);
            string ALIAS = nameof(C222_StokerCatergory.Alias);
            string NOTE = nameof(C222_StokerCatergory.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(MENU_TYPE)) {
                model.MenuType = Convert.ToInt32(values[MENU_TYPE]);
            }

            if(values.Contains(CATERGORY)) {
                model.Catergory = Convert.ToString(values[CATERGORY]);
                model.Alias = ConvertToUnaccentedSlug(model.Catergory);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
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