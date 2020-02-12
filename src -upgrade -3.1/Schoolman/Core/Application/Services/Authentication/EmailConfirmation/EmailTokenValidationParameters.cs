using Domain;
using Domain.Models;

namespace Application.Services.Token.Validators.User_Token_Validator
{
    /// <summary>
    /// Parameters that is needed for IConfirmationEmailService to confirm Customer
    /// 
    /// </summary>
    public class EmailTokenValidationParameters
    {
        public Customer Customer { get; set; }
        public string Token { get; set; }
    }
}
