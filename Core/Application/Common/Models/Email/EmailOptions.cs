using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Core.Application.Common.Models
{
    public class EmailOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
    }
}
