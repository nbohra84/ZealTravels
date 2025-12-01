using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace ZealTravel.Common.Services
{
    public class EmailService
    {
        private static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static async Task<bool> SendEmail(string email, string subject, string body, byte[] attachmentBytes = null, string attachmentName = null)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    var smtpHost = _configuration["SmtpSettings:Host"];
                    var smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
                    var smtpUsername = _configuration["SmtpSettings:Username"];
                    var smtpPassword = _configuration["SmtpSettings:Password"];
                    var smtpEnableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"]);

                    mail.From = new MailAddress(smtpUsername);
                    mail.To.Add(email);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    if (attachmentBytes != null && !string.IsNullOrEmpty(attachmentName))
                    {
                        // Add the PDF attachment from byte array
                        var attachmentStream = new MemoryStream(attachmentBytes);
                        mail.Attachments.Add(new Attachment(attachmentStream, attachmentName, "application/pdf"));
                    }

                    using (var smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                        smtp.EnableSsl = smtpEnableSsl;

                        await smtp.SendMailAsync(mail);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return false;
            }
        }



    }
}
