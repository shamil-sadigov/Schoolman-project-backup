namespace Schoolman.Student.WenApi.Controllers
{
    /// <summary>
    /// DTO
    /// </summary>
    public class LoginSuccess
    {
        public LoginSuccess(string jwt, string refresh)
        {
            AccessToken = jwt;
            RefreshToken = refresh;
        }

        /// <example>eyJhb6IkpXVCJ9.eyJpc3MiOiJ0b3B0YWwuY29tIiwiZXhwIjAwLCJodHRwOi8vdG9wdGFsY2xhaW1zL2lzX2FkbWluIjp0cnVlLCJjb21wYW55IjoiVG9wdGFsIiwiYXdlc29tZSI6dHJ1ZX0.yRQYnWAQK_ZXw</example>
        public string AccessToken { get; set; }

        /// <example>6F9619FF-8B86-D011-B42D-00CF4FC964FF</example>
        public string RefreshToken { get; set; }

    }

}
