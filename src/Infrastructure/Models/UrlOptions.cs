using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Infrastructure.Models
{
    public class UrlOptions
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public int? Port { get; set; }


        /// <summary>
        /// Return null if any of properties is null
        /// </summary>
        /// <returns></returns>
        public bool IsNull() => Scheme == null
                                || Host == null
                                || Path == null;
    }
}
