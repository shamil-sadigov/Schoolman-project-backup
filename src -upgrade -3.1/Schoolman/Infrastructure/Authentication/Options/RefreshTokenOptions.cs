using System;

namespace Authentication.Options
{
    /// <summary>
    /// Refresh token optsion that must be configure in Startup and appsettings.json
    /// </summary>
    public class RefreshTokenOptions
    {
        public TimeSpan ExpirationTime { get; set; }
    }

}
