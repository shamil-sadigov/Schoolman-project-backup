namespace Schoolman.Student.Core.Application.Models
{
    /// <summary>
    /// Helps us to build confirmation Email
    /// </summary>
    public interface IConfirmationEmailBuilder : IEmailBuilder
    {
        IConfirmationEmailBuilder ConfirmationUrl(string token);
    }

}
