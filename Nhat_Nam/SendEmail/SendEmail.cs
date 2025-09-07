using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendEmail
{
    public class ExecuteSendEmail
    {
        public void Send(string receiveEmail, string body, string subject, string path)
        {
            EmailDAO emailDAO = new EmailDAO();
            var senderData = emailDAO.GetEmailData();
            receiveEmail = receiveEmail + $";{senderData.Sender}";
            NetworkCredential loginInfo = new NetworkCredential(senderData.Sender, senderData.Pass);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(senderData.Sender);
            if (receiveEmail.IndexOf(';') == -1)
            {
                msg.To.Add(new MailAddress(receiveEmail));
            }
            else
            {
                var email = receiveEmail.Split(';');
                foreach (var item in email)
                {
                    var itemTemp = item.Trim();
                    if (string.IsNullOrEmpty(itemTemp))
                    {
                        continue;
                    }

                    msg.To.Add(new MailAddress(itemTemp));
                }
            }

            if (!string.IsNullOrEmpty(path))
            {
                if (path.IndexOf(';') == -1)
                {
                    Thread.Sleep(1000);
                    msg.Attachments.Add(new Attachment(path));
                }
                else
                {
                    foreach (var item in path.Split(';'))
                    {
                        if (string.IsNullOrEmpty(item))
                        {
                            continue;
                        }

                        msg.Attachments.Add(new Attachment(item));
                    }
                }
            }

            if (senderData.Admin != string.Empty)
            {
                msg.Bcc.Add(new MailAddress(senderData.Admin));
            }

            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(senderData.STMP);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = loginInfo;
            client.Timeout = 1000000000;
            client.Send(msg);
        }

    }
}
