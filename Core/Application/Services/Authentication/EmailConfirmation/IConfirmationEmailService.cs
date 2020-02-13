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
    /// <summary>
    /// Service that manages Email confirmation. Generate and validates email tokens, and sends email
    /// </summary>
    public interface IConfirmationEmailService:
        ITokenValidator<EmailValidationParameters, Result>,
        ITokenFactory<Customer, string>
    {
        /// <summary>
        /// Sends Confirmation email to specified Customer's email with token
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Result> SendConfirmationEmailAsync(Customer customer, string token);
    }
}
