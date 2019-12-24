using System;

namespace Schoolman.Student.Infrastructure.Services
{
    public class RefreshTokenOptions
    {
        public TimeSpan ExpirationTime { get; set; }
    }
}