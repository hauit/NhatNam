using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEmailDll
{
    public class EmailObject
    {
        private string sender = string.Empty;
        private string pass = string.Empty;
        private List<string> to = new List<string>();
        private List<string> cc = new List<string>();
        private List<string> bcc = new List<string>();
        private List<string> attachedFiles = new List<string>();
        private string subject = string.Empty;
        private string smtp = string.Empty;
        private string body = string.Empty;

        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }
        public List<string> To
        {
            get { return to; }
            set { to = value; }
        }
        public List<string> CC
        {
            get { return cc; }
            set { cc = value; }
        }
        public List<string> BCC
        {
            get { return bcc; }
            set { bcc = value; }
        }
        public List<string> AttachedFiles
        {
            get { return attachedFiles; }
            set { attachedFiles = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string SMTP
        {
            get { return smtp; }
            set { smtp = value; }
        }
        public string Body
        {
            get { return body; }
            set { body = value; }
        }
    }
}
