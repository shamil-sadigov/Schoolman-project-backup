using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Token
{
    public class RefreshTokenValidationParameters
    {
        string RefreshToken { get; set; }

        public RefreshTokenValidationParameters()
        {

        }
        public RefreshTokenValidationParameters(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
