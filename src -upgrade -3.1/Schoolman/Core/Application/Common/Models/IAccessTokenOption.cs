using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public interface IAccessTokenOption
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public TimeSpan ExpirationTime { get; set; }
    }
}
