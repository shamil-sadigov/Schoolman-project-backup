using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Token
{
    public class AccessTokenValidationParameters
    {
        public string AccessToken { get; set; }
        public TokenValidationParameters ValidationParameters{ get; set; }


        public AccessTokenValidationParameters(string accessToken, TokenValidationParameters validationParameters)
        {
            AccessToken = accessToken;
            ValidationParameters = validationParameters;
        }
    }

}
