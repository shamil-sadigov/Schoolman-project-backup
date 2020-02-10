﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public class AuthenticationTokens
    {
        public string AccessToken { get; set; }
        public string RefresthToken { get; set; }

        public AuthenticationTokens(string accessToken, string refreshToken)
            => (AccessToken, RefresthToken) = (accessToken, refreshToken);

        public AuthenticationTokens()
        {

        }
    }
}