using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public class AuthenticationCredential
    {
        public string AccessToken { get; set; }
        public string RefresthToken { get; set; }

        public AuthenticationCredential(string accessToken, string refreshToken)
            => (AccessToken, RefresthToken) = (accessToken, refreshToken);

        public AuthenticationCredential()
        {

        }
    }
}