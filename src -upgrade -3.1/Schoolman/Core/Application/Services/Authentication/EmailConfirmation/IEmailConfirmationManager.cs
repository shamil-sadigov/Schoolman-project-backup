using Domain;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Token.Validators.User_Token_Validator
{
    public interface IEmailConfirmationManager:
        ITokenValidator<EmailTokenValidationParameters, Result>,
        ITokenFactory<EmailTokenCreationParameters, string>
    {
        Task<Result> SendConfirmationEmailAsync(User user, string token);
    }
}
