using Application.Services.Token.Validators.User_Token_Validator;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Authentication.Services.EmailConfirmation
{
    public class ConfirmationEmailManager : IConfirmationEmailManager
    {
        private readonly UserManager<User> userManager;
        private readonly UrlService urlService;
        private readonly IEmailSender<IConfirmationEmailBuilder> emailService;
        private readonly ILogger<ConfirmationEmailManager> logger;
        private readonly EmailTemplate emailTemplate;


        public ConfirmationEmailManager(UserManager<User> userManager,
                                        UrlService urlService,
                                        IEmailSender<IConfirmationEmailBuilder> emailConfirmationService,
                                        IOptionsMonitor<EmailTemplate> templateOps,
                                        ILogger<ConfirmationEmailManager> logger)
        {
            this.userManager = userManager;
            this.urlService = urlService;
            this.emailService = emailConfirmationService;
            this.logger = logger;
            this.emailTemplate = templateOps.Get("Confirmation");

        }



        public async Task<Result> SendConfirmationEmailAsync(User user, string token)
        {
            Uri confirmUrl = urlService.UseSpaUrlAddress()
                                       .BuildConfirmationUrl
                                        (user.Id.ToString(), token);

            var result = await emailService.SendEmailAsync
                                            (ops => ops.ConfirmationUrl(confirmUrl.ToString())
                                                       .To(user.Email)
                                                       .Subject("Account Confirmation")
                                                       .Template(emailTemplate.Path));

            return result;
        }

        
        public async Task<string> GenerateTokenAsync(EmailTokenCreationParameters parameters)
        {
            string token =  await userManager.GenerateEmailConfirmationTokenAsync(parameters.User);

            // generated token may contain some invalid characters such as '+' and '='
            // which is considered url-unsafe
            // so you should encode it as below
            return HttpUtility.UrlEncode(token);
            // so, now '+' replaced by '%2b' 
            // and '=' by '%3d'
        }


        public async Task<Result> ValidateTokenAsync(EmailTokenValidationParameters parameters)
        {
            parameters.Token = HttpUtility.UrlDecode(parameters.Token);

            var result = await userManager.ConfirmEmailAsync(parameters.User, parameters.Token);

            if (!result.Succeeded)
            {
                logger.LogWarning("Email confirmation failed: Invalid token have been provided " +
                                  "Email: {email}", parameters.User.Email);

                return Result.Failure(result.Errors.Select(m => m.Description).ToArray());
            }

            return Result.Success();
        }
    }
}