using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Token.Validators.User_Token_Validator
{
    public interface IConfirmationEmailService:
        ITokenValidator<EmailTokenValidationParameters, Result>,
        ITokenFactory<Customer, string>
    {
        Task<Result> SendConfirmationEmailAsync(Customer customer, string token);
    }
}
