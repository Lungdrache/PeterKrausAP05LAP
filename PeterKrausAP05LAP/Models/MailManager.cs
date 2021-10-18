using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PeterKrausAP05LAP.Models
{
    public class MailManager
    {
        //TODO: MakeItWork
        public static bool SendEmail(string htmlBody,string subject, string reciever)
        {
            {
                MailMessage message = new MailMessage();

                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress("noreplystockgames.benny@gmail.com");
                message.To.Add(new MailAddress(reciever));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlBody;


                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                #region Credentials
                smtp.Credentials = new NetworkCredential(message.From.Address, "Drachenfeuer10.");
                #endregion
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(message);

                return true;
            }

            //try
            //{
            //    MailMessage message = new MailMessage();

            //    SmtpClient smtp = new SmtpClient();

            //    message.From = new MailAddress("noreplystockgames.benny@gmail.com");
            //    message.To.Add(new MailAddress(reciever));
            //    message.Subject = subject;
            //    message.IsBodyHtml = true; //to make message body as html  
            //    message.Body = htmlBody;
                
                
            //    smtp.Port = 587;
            //    smtp.Host = "smtp.gmail.com"; //for gmail host  
            //    smtp.EnableSsl = true;
            //    smtp.UseDefaultCredentials = false;
            //    #region Credentials
            //    smtp.Credentials = new NetworkCredential(message.From.Address, "Drachenfeuer10.");
            //    #endregion
            //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //    smtp.Send(message);

            //    return true;
            //}
            //catch (Exception exp) 
            //{ 
            //    return false; 
            //}
        }
    }
}