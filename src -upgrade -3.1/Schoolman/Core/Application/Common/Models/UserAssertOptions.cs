

using System;

namespace Schoolman.Student.Core.Application.Models
{

    /// <summary>
    /// Options mainly used by IUserService to get User from Database based on requirements in properties
    /// </summary>
    public class UserAssertOptions
    {
        public string Password { get; private set; }
        public bool ConfirmEmail { get; private set; }

        public UserAssertOptions PasswordToConfirm(string password)
        {
            Password = password;
            return this;
        }


        public UserAssertOptions ShouldConfirmEmail(bool confirmed)
        {
            ConfirmEmail = confirmed;
            return this;
        }
    }
}
