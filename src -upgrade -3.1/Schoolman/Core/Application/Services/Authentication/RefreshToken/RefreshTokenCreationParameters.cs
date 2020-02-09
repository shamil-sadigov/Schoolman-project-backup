using Application.Common.Models;
using Domain;

namespace Application.Services.Token
{
    public class RefreshTokenCreationParameters
    {
        public User User { get; set; }

        public RefreshTokenCreationParameters(User user)
        {
            User = user;
        }
    }
}