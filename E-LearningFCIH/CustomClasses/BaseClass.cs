using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Twilio;

namespace E_LearningFCIH.CustomClasses
{
    public  class CustomMessage
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int SubjectID { get; set; }
        public string MessageBody { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
    public class customUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public int SubjectID { get; set; }

    }
    public class BaseClass
    {
        public void SendSMS(string to, string msg)
        {
            string AccountSid = "AC96ce42329bf75d91e4999abc8caa4899";
            string AuthToken = "4b1eb3d6e976ac4c8ce0c2a177295f1b";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var message = twilio.SendMessage("+12019493393", "+20" + to, msg);
        }
        

    }


    public class MailHelper
    {
        private const int Timeout = 180000;
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _pass;
        private readonly bool _ssl;

        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string RecipientCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentFile { get; set; }

        public MailHelper()
        {
            //MailServer - Represents the SMTP Server
            _host = ConfigurationManager.AppSettings["MailServer"];
            //Port- Represents the port number
            _port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            //MailAuthUser and MailAuthPass - Used for Authentication for sending email
            _user = ConfigurationManager.AppSettings["MailAuthUser"];
            _pass = ConfigurationManager.AppSettings["MailAuthPass"];
            _ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
        }

        public void Send()
        {
            try
            {
                // We do not catch the error here... let it pass direct to the caller
                Attachment att = null;
                var message = new MailMessage(Sender, Recipient, Subject, Body) { IsBodyHtml = true };
                if (RecipientCC != null)
                {
                    message.Bcc.Add(RecipientCC);
                }
                var smtp = new SmtpClient(_host, _port);

                if (!String.IsNullOrEmpty(AttachmentFile))
                {
                    if (File.Exists(AttachmentFile))
                    {
                        att = new Attachment(AttachmentFile);
                        message.Attachments.Add(att);
                    }
                }

                if (_user.Length > 0 && _pass.Length > 0)
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_user, _pass);
                    smtp.EnableSsl = _ssl;
                }

                smtp.Send(message);

                if (att != null)
                    att.Dispose();
                message.Dispose();
                smtp.Dispose();
            }

            catch (Exception ex)
            {

            }
        }
    }

    public enum Status
    {
        Request=1,
        Approve=2,
        Confirm=3
    }



}