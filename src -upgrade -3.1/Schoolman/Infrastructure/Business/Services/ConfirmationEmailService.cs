using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Business.Services
{
    public class ConfirmationEmailService : IConfirmationEmailService
    {
        readonly EmailOptions emailOptions;

        public ConfirmationEmailService(IOptionsMonitor<EmailOptions> emailOps)
        {
            this.emailOptions = emailOps.Get("Confirmation");
        }


        public async Task<Result> SendAsync(Action<IConfirmationEmailBuilder> sendOptions)
        {

            var emailBuilder = new ConfirmationEmailBuilder();
            sendOptions(emailBuilder);
            Email email = emailBuilder.Build();


            try
            {
                MimeMessage message = BuildMessage(email);

                using (var smtp = new SmtpClient())
                {
                    //smtp.LocalDomain = "locahost";
                    await smtp.ConnectAsync(emailOptions.Host, emailOptions.Port, emailOptions.EnableSSL);
                    await smtp.AuthenticateAsync(emailOptions.Username, emailOptions.Password);
                    await smtp.SendAsync(message);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                //throw ex;

                return Result.Failure("Could send message to this email. Ensure you provided a valid email");
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
