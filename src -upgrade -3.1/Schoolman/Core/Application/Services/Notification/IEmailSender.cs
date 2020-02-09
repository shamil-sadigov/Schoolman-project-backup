using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface IEmailSender<EmailBuilder> where EmailBuilder : IEmailBuilder
    {
        Task<Result> SendEmailAsync(Action<EmailBuilder> sendOptions);
    }


}