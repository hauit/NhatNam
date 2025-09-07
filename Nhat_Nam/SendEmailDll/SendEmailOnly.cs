using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmailDll
{
    public class SendEmailOnly : AEmail
    {
        public override void CreateEmailContent()
        {
            return;
        }
    }
}
