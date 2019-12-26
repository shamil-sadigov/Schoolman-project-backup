namespace Schoolman.Student.Core.Application.Models
{
    /// <summary>
    /// Options mainly used by IUserService to get User from Database based on requirements in properties
    /// </summary>
    public class UserSearchOptions
    {
        public string Email { get; set; }
        public string PasswordToConfirm { get; set; }
        public bool ConfirmedEmail { get; set; }
    }
}
