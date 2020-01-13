namespace Schoolman.Student.WenApi.Controllers
{
    /// <summary>
    /// DTO
    /// </summary>
    public class LoginFail
    {

        public LoginFail(string[] errors)
        {
            Errors = errors;
        }

        /// <example> User doesnt existt</example>
        public string[] Errors { get; set; }
    }

}
