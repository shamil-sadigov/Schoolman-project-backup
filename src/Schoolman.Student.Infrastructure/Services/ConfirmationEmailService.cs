using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Models;
using Schoolman.Student.Infrastructure.Helpers;
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
        /// <summary>
        /// Token that is initialized in SetConfirmationOptions() method
        /// </summary>
        string confirmationToken;
        /// <summary>
        /// Initialized in SetConfirmationOptions() method
        /// </summary>
        string username;
        /// <summary>
        /// Contains information to configure smpt
        /// </summary>
        readonly EmailOptions emailOptions;
        readonly EmailTemplate emailTemplate;

        public ConfirmationEmailService(IOptionsMonitor<EmailOptions> emailOps,
                                        IOptionsMonitor<EmailTemplate> templateOps)
        {
            this.emailOptions = emailOps.Get("Confirmation");
            emailTemplate = templateOps.Get("Confirmation");
        }

        public async Task<Result> SendAsync(string email)
        {
            var htmlMessage = new StringBuilder
                               (File.ReadAllText(emailTemplate.Path))
                               .AddConfirmationToken(confirmationToken)
                               .AddUserName(username);

            var message = BuildMessage(email, htmlMessage.ToString());

            try
            {
                using (var smpt = new SmtpClient())
                {
                    await smpt.ConnectAsync(emailOptions.Host, emailOptions.Port, emailOptions.EnableSSL);
                    await smpt.AuthenticateAsync(emailOptions.Username, emailOptions.Password);
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

        /// <summary>
        /// Sets confirmation token. Must be called before sending email
        /// </summary>
        /// <param name="token"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public IConfirmationEmailService SetConfirmationOptions(string token, string username)
        {
            (this.confirmationToken, this.username) = (token, username);
            return this;
        }

        #region Local helper methods

    
        private MimeMessage BuildMessage(string email, string htmlTemplate)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailOptions.Username));
            message.To.Add(new MailboxAddress(email));
            message.Subject = "Account Confirmation";
            message.Body = new TextPart(TextFormat.Html) { Text = htmlTemplate };
            return message;
        }





        #endregion

    }
}
