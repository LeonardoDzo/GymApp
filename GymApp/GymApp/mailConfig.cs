using GymApp.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymApp.Models;
using System.IO;

namespace GymApp
{
    public class mailConfig
    {
        private dbGymEntities db = new dbGymEntities();

        public async Task configSMTPasync(IdentityMessage message)
        {

            // Plug in your email service here to send an email.
            var credentialUserName = (from u in db.sender select u.sentaccount).FirstOrDefault();
            var sentFrom = (from u in db.sender select u.sentaccount).FirstOrDefault();
            var pwd = (from u in db.sender where u.sentaccount == sentFrom select u.pass).FirstOrDefault();

            // Configure the client:
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Creatte the credentials:
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(credentialUserName, pwd);
            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination);
            mail.IsBodyHtml = true;

            mail.Subject = message.Subject;

            mail.Body = message.Body;

            await client.SendMailAsync(mail);
        }

        public async Task abrirconexion(List<string> emails, bool caducadas)
        {
            var credentialUserName = (from u in db.sender select u.sentaccount).FirstOrDefault();
            var sentFrom = (from u in db.sender select u.sentaccount).FirstOrDefault();
            var pwd = (from u in db.sender where u.sentaccount == sentFrom select u.pass).FirstOrDefault();

            //var credentialUserName = "kineticcrossfittraining@gmail.com";
            //var sentFrom = "kineticcrossfittraining@gmail.com";
            //var pwd = "AbCd123!";

            // Configure the client:
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Creatte the credentials:
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(credentialUserName, pwd);
            client.EnableSsl = true;
            client.Credentials = credentials;


            foreach (var i in emails)
            {
                //var destination 
                
                // Create the message:
                var mail = new System.Net.Mail.MailMessage(sentFrom, i);
                mail.IsBodyHtml = true;

                if (caducadas == true)
                {
                    mail.Subject = "Kinetic Cross Training ¡Te estamos esperando!";

                }
                else
                {
                    mail.Subject = "Kinetic Cross Training ¡No te quedes fuera!";

                }

                mail.Body = createEmailBody(caducadas);

                await client.SendMailAsync(mail);

            }
 

        }

        private string createEmailBody(bool caducadas)

        {

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            if (caducadas == true)
            {
                using (StreamReader reader = new StreamReader("RecordatorioTemplate/indexCaducada.html"))

                {

                    body = reader.ReadToEnd();

                }

            }
            else
            {
                using (StreamReader reader = new StreamReader("RecordatorioTemplate/index.html"))

                {

                    body = reader.ReadToEnd();

                }


            }

            return body;

        }

    }
}