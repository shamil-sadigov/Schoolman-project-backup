namespace Schoolman.Student.Core.Application.Models
{
    public interface IResult
    {
        string[] Errors { get; }
        bool Succeeded { get; }
    }
}