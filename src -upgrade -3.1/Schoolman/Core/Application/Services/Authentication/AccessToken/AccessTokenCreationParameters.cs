using Application.Common.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Token.Validators.Access_Token_Validator
{
    public class AccessTokenCreationParameters
    {
        public User User { get; set; }
        public IAccessTokenOptions Options { get; set; }

        public AccessTokenCreationParameters(User user, IAccessTokenOptions options)
        {
            User = user;
            Options = options;
        }
    }
}
