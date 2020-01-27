using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Schoolman.Student.WenApi.Controllers.Identity.DTO
{
    /// <summary>
    /// DTO
    /// </summary>
    public class TokenModel
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
