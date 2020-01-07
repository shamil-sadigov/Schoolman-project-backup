using System.ComponentModel.DataAnnotations;

namespace Schoolman.Student.WenApi.Controllers
{
    public class UserDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
    }



}
