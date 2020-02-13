using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;


namespace Business.Services
{
    public class ConfirmationEmailSender : IEmailSender<IConfirmationEmailBuilder>
    {
        readonly EmailOptions emailOptions;
        private readonly ILogger<ConfirmationEmailSender> logger;

        public ConfirmationEmailSender(IOptionsMonitor<EmailOptions> emailOps, 
                                        ILogger<ConfirmationEmailSender> logger)
        {
            this.emailOptions = emailOps.Get("Confirmation");
            this.logger = logger;
        
        }


        public async Task<Result> SendEmailAsync(Action<IConfirmationEmailBuilder> sendOptions)
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
                logger.LogError(ex, "ConfirmationEmailSender.Send() Sending email failed: Email.To {Email}, Subject {subject}",
                                email.To, email.Subject);

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
