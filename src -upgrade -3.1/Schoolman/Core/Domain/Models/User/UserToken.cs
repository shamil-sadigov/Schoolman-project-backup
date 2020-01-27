using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class UserToken : IdentityUserToken<string>
    {
        public User User { get; set; }
        /// <summary>
        /// Alson know as JTI
        /// </summary>
        public string AccessTokenId { get; set; }
    }


}
