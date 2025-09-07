using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SendEmailDll
{
    public abstract class AEmail
    {
        protected EmailObject EmailObject { get; set; }

        public AEmail()
        {
            EmailObject = new EmailObject();
        }

        public virtual bool SendEmail()
        {
            try
            {
                Send();
                return true;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public abstract void CreateEmailContent();
        public virtual void CreateEmailContent(EmailObject EmailObject)
        {
            this.EmailObject.AttachedFiles = EmailObject.AttachedFiles;
            this.EmailObject.BCC = EmailObject.BCC;
            this.EmailObject.CC = EmailObject.CC;
            this.EmailObject.Body = EmailObject.Body;
            this.EmailObject.Pass = EmailObject.Pass;
            this.EmailObject.Sender = EmailObject.Sender;
            this.EmailObject.SMTP = EmailObject.SMTP;
            this.EmailObject.Subject = EmailObject.Subject;
            this.EmailObject.To = EmailObject.To;
        }

        private void Send()
        {
            string sender = string.IsNullOrEmpty(EmailObject.Sender) ? "nguyenvuhau@cokhinhatnam.com.vn" : EmailObject.Sender;
            string pass = string.IsNullOrEmpty(EmailObject.Pass) ? "k*M,eq_I%S@y" : EmailObject.Pass;
            NetworkCredential loginInfo = new NetworkCredential(sender, pass);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(sender);
            foreach (var item in EmailObject.To)
            {
                var itemTemp = item.Trim();
                if (string.IsNullOrEmpty(itemTemp))
                {
                    continue;
                }

                msg.To.Add(new MailAddress(itemTemp));
            }

            //msg.To.Add(new MailAddress("nguyenvuhau0511@gmail.com"));
            foreach (var item in EmailObject.CC)
            {
                var itemTemp = item.Trim();
                if (string.IsNullOrEmpty(itemTemp))
                {
                    continue;
                }

                msg.CC.Add(new MailAddress(itemTemp));
            }

            foreach (var item in EmailObject.BCC)
            {
                var itemTemp = item.Trim();
                if (string.IsNullOrEmpty(itemTemp))
                {
                    continue;
                }

                msg.Bcc.Add(new MailAddress(itemTemp));
            }
            
            foreach (var item in EmailObject.AttachedFiles)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                msg.Attachments.Add(new Attachment(item));
            }

            msg.Bcc.Add(new MailAddress("nguyenvuhau@cokhinhatnam.com.vn"));
            msg.Subject = EmailObject.Subject;
            msg.Body = EmailObject.Body;
            msg.IsBodyHtml = true;
            string domain = string.IsNullOrEmpty(EmailObject.SMTP) ? "mail9096.maychuemail.com" : EmailObject.SMTP;
            SmtpClient client = new SmtpClient(domain);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = loginInfo;
            client.Port = 587;
            client.Timeout = 10000000;
            client.Send(msg);
            DeleteAttachedFiles(EmailObject.AttachedFiles);
        }

        private void DeleteAttachedFiles(List<string> attachedFiles)
        {
            foreach(var item in attachedFiles)
            {
                File.Delete(item);
            }
        }
    }
}
