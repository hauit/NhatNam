using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace NNworking.Models.Email
{
    public abstract class AEmail
    {
        public string Sender { get; set; }
        public string Pass { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public List<string> AttachedFiles { get; set; }
        public string Subject { get; set; }
        public string SMTP { get; set; }
        protected string EmailContent { get; set; }

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

        private void Send()
        {
            string sender = string.IsNullOrEmpty(Sender) ? "nguyenvuhau@cokhinhatnam.com.vn" : Sender;
            string pass = string.IsNullOrEmpty(Pass) ? "k*M,eq_I%S@y" : Pass;
            NetworkCredential loginInfo = new NetworkCredential(sender, pass);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(sender);
            foreach (var item in To)
            {
                var itemTemp = item.Trim();
                if (string.IsNullOrEmpty(itemTemp))
                {
                    continue;
                }

                msg.To.Add(new MailAddress(itemTemp));
            }

            foreach (var item in CC)
            {
                var itemTemp = item.Trim();
                if (string.IsNullOrEmpty(itemTemp))
                {
                    continue;
                }

                msg.CC.Add(new MailAddress(itemTemp));
            }

            foreach (var item in BCC)
            {
                var itemTemp = item.Trim();
                if (string.IsNullOrEmpty(itemTemp))
                {
                    continue;
                }

                msg.Bcc.Add(new MailAddress(itemTemp));
            }
            
            foreach (var item in AttachedFiles)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                msg.Attachments.Add(new Attachment(item));
            }

            msg.Bcc.Add(new MailAddress("nguyenvuhau@cokhinhatnam.com.vn"));
            msg.Subject = Subject;
            msg.Body = EmailContent;
            msg.IsBodyHtml = true;
            string domain = string.IsNullOrEmpty(SMTP) ? "mail99217.maychuemail.com" : SMTP;
            SmtpClient client = new SmtpClient(domain);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = loginInfo;
            //client.Port = 587;
            client.Timeout = 10000000;
            client.Send(msg);
        }
    }
}