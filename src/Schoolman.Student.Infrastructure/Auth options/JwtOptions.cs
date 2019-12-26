using System;

namespace Schoolman.Student.Infrastructure.AuthOptions
{
    /// <summary>
    /// Jwt Options that should be configure in Startup.cs and appsettings.json
    /// </summary>
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public TimeSpan ExpirationTime { get; set; }
        public DateTime IssueDate { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Jti { get; set; }
    }
}
