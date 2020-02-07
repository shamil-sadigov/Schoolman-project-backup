using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Token.Validators.User_Token_Validator
{
    public interface IEmailTokenService:
        ITokenValidator<EmailConfirmationTokenParameters, Result>,
        ITokenFactory<EmailGenerationTokenParameters, string>
    { }
}
