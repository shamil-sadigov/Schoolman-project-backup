using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Helpers
{
    public interface IUserCheckOptions
    {
        IUserCheckOptions ConfirmPassword(string password);
        IUserCheckOptions ConfirmedEmail(bool confirmed);
    }
}
