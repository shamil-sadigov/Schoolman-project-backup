using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Models;
using Schoolman.Student.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Services
{
    public class ConfirmationEmailService : IConfirmationEmailService
    {
        private EmailOptions options;

        public ConfirmationEmailService(IOptionsMonitor<EmailOptions> emailOptions)
        {
            this.options = emailOptions.Get("EmailConfirmationOptions");
        }

        public async Task<Result> SendAsync(string email, string htmlContent , string subject = "Account Confirmation")
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(options.Sender));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = htmlContent };

            try
            {
                using (var smpt = new SmtpClient())
                {
                    await smpt.ConnectAsync(options.Host, options.Port, options.EnableSSL);
                    await smpt.AuthenticateAsync(options.Username, options.Password);
                    await smpt.SendAsync(message);
                    await smpt.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }

            return Result.Success();
        }
    }
}
