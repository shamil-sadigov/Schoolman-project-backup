using System.ComponentModel.DataAnnotations;

namespace Schoolman.Student.WenApi.Controllers
{
    public class AuthDTO
    {
        public AuthDTO()
        {

        }

        public AuthDTO(string jwt, string refresh)
        {
            access_token = jwt;
            refresh_token = refresh;
        }

        public AuthDTO(string[] errors)
        {
            this.errors = errors;
        }

        /// <summary>
        /// JWT
        /// </summary>
        /// <example>adasdasdasdasdasd</example>
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string[] errors { get; set; }
    }
}
