using Domain;
using Domain.Models;

namespace Application.Services.Token.Validators.User_Token_Validator
{
    public class EmailTokenValidationParameters
    {
        public Client Client { get; set; }
        public string Token { get; set; }
    }
}
