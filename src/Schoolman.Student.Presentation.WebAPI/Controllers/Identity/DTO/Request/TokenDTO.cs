using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Schoolman.Student.WenApi.Controllers.Identity.DTO
{
    public class TokenDTO
    {
        [Required]
        public string access_token { get; set; }
        [Required]
        public string refresh_token { get; set; }
    }
}
