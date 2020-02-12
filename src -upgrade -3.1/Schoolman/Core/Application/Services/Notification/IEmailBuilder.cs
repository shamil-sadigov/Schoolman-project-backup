namespace Schoolman.Student.Core.Application.Models
{
    /// <summary>
    /// Builds email
    /// </summary>
    public interface IEmailBuilder
    {
        IEmailBuilder From(string from);
        IEmailBuilder To(string to);
        IEmailBuilder Template(string temaplate);
        IEmailBuilder Subject(string subject);
    }

}
