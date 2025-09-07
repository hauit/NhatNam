using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmailDll
{
    public class EmailErrorItem : AEmail
    {
        public override void CreateEmailContent()
        {
            this.EmailObject.Subject = $@"Thông báo hàng lỗi ngày {DateTime.Now.ToString("dd/MM/yyyy")}";
            this.EmailObject.Body = $@"Gửi các bộ phận liên quan!<br>
                Vui lòng truy cập link bên dưới để kiểm tra, xử lý các thông báo hàng lỗi đến ngày {DateTime.Now.ToString("dd/MM/yyyy")} </br>
                Xin cảm ơn!
            ";
            this.EmailObject.To = new List<string>() { "kythuatnhatnam@cokhinhatnam.com.vn",
            "khsxnhatnam@cokhinhatnam.com.vn",
            "quangnguyenwtc@cokhinhatnam.com.vn",
            "qlsxnhatnam@cokhinhatnam.com.vn",
            "ca1nhatnam@cokhinhatnam.com.vn",
            "phamvanthoong@cokhinhatnam.com.vn",
            "sxcknhatnam@cokhinhatnam.com.vn"
            };
        }
    }
}
