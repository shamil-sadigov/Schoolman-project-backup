using System;

namespace Application.Common.Models
{
    public interface IRefreshTokenOptions
    {
        public TimeSpan ExpirationTime { get; set; }
    }

}
