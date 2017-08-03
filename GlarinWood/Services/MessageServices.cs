using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713

    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private IHostingEnvironment _env;
        //private readonly IHttpContextAccessor _http;
        //private readonly IFileProvider _fileProvider;


        public AuthMessageSender( IHostingEnvironment env)
        {
            _env = env;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {

                var emailMessage = new MimeMessage();
                //string d = "/Utilites/LocalizedEmailTemplates/Contact.htm";
                //var webRoot = _env.WebRootPath;
                //string file = Path.Combine(webRoot, d);
                var physicalProvider = _env.ContentRootFileProvider;

                IFileProvider provider = new PhysicalFileProvider(_env.WebRootPath);
                /*   IDirectoryContents contents = provider.GetDirectoryContents("");*/ // the applicationRoot contents
                IFileInfo fileInfo = provider.GetFileInfo("LocalizedEmailTemplates/Contact.htm"); // a file under applicationRoot



                string strEmailBody = File.ReadAllText(fileInfo.PhysicalPath);
                strEmailBody = strEmailBody
                                  .Replace("[NAME]", email)
                                  .Replace("[MAIL]", email)
                                  .Replace("[SUBJECT]", subject)
                                  .Replace("[MESSAGE]", message);
                //From Address
                string FromAddress = email;
                string FromAdressTitle = email;
                //To Address
                string ToAddress = "Admin@gmail.com";
                string ToAdressTitle = "ContactWithUs";
                string Subject = subject;
                string BodyContent = strEmailBody;

                //Smtp Server
                string SmtpServer = "smtp.gmail.com";
                //Smtp Port Number
                int SmtpPortNumber = 587;


                emailMessage.From.Add(new MailboxAddress(System.Text.Encoding.UTF8, FromAdressTitle, FromAddress));
                emailMessage.To.Add(new MailboxAddress(System.Text.Encoding.UTF8, ToAdressTitle, ToAddress));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = BodyContent
                };

                using (var client = new SmtpClient())
                {
                    //client.LocalDomain = "Admin@Info.com"";
                    await client.ConnectAsync(SmtpServer, SmtpPortNumber, false).ConfigureAwait(false);
                    // Note: only needed if the SMTP server requires authentication
                    // Error 5.5.1 Authentication 
                    client.Authenticate(ToAddress,"zxcv@2vcxZ");
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                    
                   //@ViewBag.Message = "Thank you for Contacting us ";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}

