using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    //public interface IEmailService
    //{
    //    Task<Result> SendAsync(string email);
    //}


    // Refined version
    public interface IEmailService<EmailBuilder> where EmailBuilder : IEmailBuilder
    {
        Task<Result> SendAsync(Action<EmailBuilder> sendOptions);
    }

}