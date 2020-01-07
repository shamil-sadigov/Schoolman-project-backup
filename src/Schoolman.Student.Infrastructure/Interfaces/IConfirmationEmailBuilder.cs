namespace Schoolman.Student.Core.Application.Interfaces
{
    public interface IConfirmationEmailBuilder : IEmailBuilder
    {
        IConfirmationEmailBuilder ConfirmationUrl(string token);
    }

}
