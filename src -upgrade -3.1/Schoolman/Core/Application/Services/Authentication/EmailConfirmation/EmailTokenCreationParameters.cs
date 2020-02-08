using Domain;

namespace Application.Services.Token.Validators.User_Token_Validator
{
    public class EmailTokenCreationParameters
    {
        public User User { get;set; }

        public EmailTokenCreationParameters(User user)
        {
            User = user;
        }
    }
}