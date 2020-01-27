namespace Schoolman.Student.Core.Application.Models
{
    public interface IConfirmationEmailBuilder : IEmailBuilder
    {
        IConfirmationEmailBuilder ConfirmationUrl(string token);
    }

}
