using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using Schoolman.Student.Infrastructure.Helpers;
using Schoolman.Student.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Schoolman.Student.Infrastructure.Services
{

    public class ConfirmationEmailService : IEmailService<ConfirmationEmailBuilder>
    {
        readonly EmailOptions emailOptions;

        public ConfirmationEmailService(IOptionsMonitor<EmailOptions> emailOps)
        {
            this.emailOptions = emailOps.Get("Confirmation");
        }



        public async Task<Result> SendAsync(Action<ConfirmationEmailBuilder> sendOptions)
        {
            var emailBuilder = new ConfirmationEmailBuilder();
            sendOptions(emailBuilder);
            Email email = emailBuilder.Build();
            MimeMessage message = BuildMessage(email);

            
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.LocalDomain = "locahost";
                    await smtp.ConnectAsync(emailOptions.Host, emailOptions.Port, emailOptions.EnableSSL);
                    await smtp.AuthenticateAsync(emailOptions.Username, emailOptions.Password);
                    await smtp.SendAsync(message);
                    await smtp.DisconnectAsync(true);
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

        private MimeMessage BuildMessage(Email email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email.From ?? emailOptions.Username));
            message.To.Add(new MailboxAddress(email.To));
            message.Subject = email.Subject;
            message.Body = new TextPart(TextFormat.Html) { Text = email.Body };
            return message;
        }
    }





}
