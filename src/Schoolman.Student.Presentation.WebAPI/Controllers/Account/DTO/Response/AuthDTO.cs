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
            AccessToken = jwt;
            RefreshToken = refresh;
        }

        public AuthDTO(string[] errors)
        {
            this.Errors = errors;
        }

        /// <summary>
        /// JWT
        /// </summary>
        /// <example>eyJhb6IkpXVCJ9.eyJpc3MiOiJ0b3B0YWwuY29tIiwiZXhwIjAwLCJodHRwOi8vdG9wdGFsY2xhaW1zL2lzX2FkbWluIjp0cnVlLCJjb21wYW55IjoiVG9wdGFsIiwiYXdlc29tZSI6dHJ1ZX0.yRQYnWAQK_ZXw</example>
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <example>6F9619FF-8B86-D011-B42D-00CF4FC964FF</example>
        public string RefreshToken { get; set; }
        public string[] Errors { get; set; }
    }


    public class AuthSuccessModel
    {
        public AuthSuccessModel(string jwt, string refresh)
        {
            AccessToken = jwt;
            RefreshToken = refresh;
        }

        /// <example>eyJhb6IkpXVCJ9.eyJpc3MiOiJ0b3B0YWwuY29tIiwiZXhwIjAwLCJodHRwOi8vdG9wdGFsY2xhaW1zL2lzX2FkbWluIjp0cnVlLCJjb21wYW55IjoiVG9wdGFsIiwiYXdlc29tZSI6dHJ1ZX0.yRQYnWAQK_ZXw</example>
        public string AccessToken { get; set; }

        /// <example>6F9619FF-8B86-D011-B42D-00CF4FC964FF</example>
        public string RefreshToken { get; set; }

    }





    public class AuthFailModel
    {

        public AuthFailModel(string[] errors)
        {
            Errors = errors;
        }

        /// <example> User doesnt existt</example>
        public string[] Errors { get; set; }
    }

}
