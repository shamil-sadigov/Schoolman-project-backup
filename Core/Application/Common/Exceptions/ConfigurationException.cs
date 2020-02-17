using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class ConfigurationException:Exception
    {
        public ConfigurationException (string message) : base(message)
        {

        }
    }
}
