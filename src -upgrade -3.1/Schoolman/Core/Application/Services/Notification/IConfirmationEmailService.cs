using Schoolman.Student.Core.Application.Models;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface IConfirmationEmailService : IEmailService<IConfirmationEmailBuilder>
    {

    }
}