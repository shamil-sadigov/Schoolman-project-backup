using Schoolman.Student.Core.Application.Models;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public abstract class EmailBuilder: IEmailBuilder
    {
        protected Email email;
        protected string emailTemplatePath;

        public EmailBuilder()
            => this.email = new Email();

        public IEmailBuilder From(string from)
        {
            email.From = from;
            return this;
        }

        public IEmailBuilder To(string to)
        {
            email.To = to;
            return this;
        }

        public  IEmailBuilder Template(string path)
        {
            emailTemplatePath = path;
            return this;
        }

        public IEmailBuilder Subject(string subject)
        {
            email.Subject = subject;
            return this;
        }

        public abstract Email Build();
    }


}
