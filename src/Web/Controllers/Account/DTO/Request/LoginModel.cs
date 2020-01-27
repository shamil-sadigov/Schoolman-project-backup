using System.ComponentModel.DataAnnotations;

namespace Schoolman.Student.WenApi.Controllers
{
    /// <summary>
    /// DTO
    /// </summary>
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
    }
}
