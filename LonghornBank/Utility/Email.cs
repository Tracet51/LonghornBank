using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using System.Net.Mail;

namespace LonghornBank.Utility
{
    public static class Email
    {
        public static void PasswordEmail(String toEmailAddress, String emailSubject, String emailBody)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("DoNotReplyLonghornBank@gmail.com", "P@ssword!"),
                EnableSsl = true
            };
            String finalMessage = "The following is a message from Longhorn Bank:\n\n" + emailBody;
            MailAddress senderEmail = new MailAddress("DoNotReplyLonghornBank@gmail.com", "Team 7");
            MailMessage mm = new MailMessage();
            mm.Subject = "Team 7 - " + emailSubject;
            mm.Sender = senderEmail;
            mm.From = senderEmail;
            mm.To.Add(new MailAddress(toEmailAddress));
            mm.Body = finalMessage;
            client.Send(mm);
        }
    }
}