using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public interface ICurrentUserService
    {
        string CurrentUserId { get; }
    }
}
