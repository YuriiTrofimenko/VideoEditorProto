using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace VideoEditorProto.Utils
{
    public class Mailer
    {
        public static Boolean sendMessage(
            string _server
            , string _to
            , string _from
            , string _pswd
            , string _name
            , string _email
            , string _subject
            , string _messageString)
        {
            Boolean result = true;
            using (MailMessage message = new MailMessage(_from, _to))
            {
                message.Subject = _subject;
                message.Body = _name + @" ( " + _email + @"): " + _messageString;
                message.IsBodyHtml = false;
                SmtpClient client = new SmtpClient(_server);
                client.Port = 587;
                client.EnableSsl = true;
                NetworkCredential networkCred =
                    new NetworkCredential(_from, _pswd);
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = networkCred;

                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    result = false;
                }
                return result;
            }
        }
    }
}