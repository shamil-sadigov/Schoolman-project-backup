using Domain;

namespace Application.Services.Token.Validators.User_Token_Validator
{
    public class EmailTokenValidationParameters
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
