using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace GymApp
{
    public class mailConfig
    {
        public async Task configSMTPasync(IdentityMessage message)
        {

            // Plug in your email service here to send an email.
            var credentialUserName = "kineticcrossfittraining@gmail.com";
            var sentFrom = "kineticcrossfittraining@gmail.com";
            var pwd = "AbCd123!";

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

    }
}