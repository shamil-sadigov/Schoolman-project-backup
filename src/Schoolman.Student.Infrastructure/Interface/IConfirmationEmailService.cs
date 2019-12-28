using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Interface
{
    public interface IConfirmationEmailService:IEmailService
    {
        IConfirmationEmailService SetConfirmationOptions(string token, string username);
    }
}
