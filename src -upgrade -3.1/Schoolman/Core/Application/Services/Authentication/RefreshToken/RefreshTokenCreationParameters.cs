using Application.Common.Models;
using Domain;

namespace Application.Services.Token
{
    public class RefreshTokenCreationParameters
    {
        public User User { get; set; }
        public IRefreshTokenOptions Options { get; }

        public RefreshTokenCreationParameters(User user, IRefreshTokenOptions refreshTokenOptions)
        {
            User = user;
            Options = refreshTokenOptions;
        }
    }
}