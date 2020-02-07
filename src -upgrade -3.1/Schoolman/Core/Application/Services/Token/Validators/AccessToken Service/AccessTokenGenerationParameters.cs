using Application.Common.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Token.Validators.Access_Token_Validator
{
    public class AccessTokenGenerationParameters
    {
        public User User { get; set; }
        public IAccessTokenOption AccessTokenOption { get; set; }

        public AccessTokenGenerationParameters(User user, IAccessTokenOption accessTokenOption)
        {
            User = user;
            AccessTokenOption = accessTokenOption;
        }
    }
}
