namespace Schoolman.Student.Core.Application.Models
{
    /// <summary>
    /// Options mainly used by IUserService to get User from Database based on requirements in properties
    /// </summary>
    public class UserSearchOptions
    {
        public string Password { get; private set; }
        public bool ConfirmedEmail { get; private set; }

        public UserSearchOptions WithPassword(string password)
        {
            Password = password;
            return this;
        }


        public UserSearchOptions WithConfirmedEmail(bool confirmed)
        {
            ConfirmedEmail = confirmed;
            return this;
        }
    }
}
