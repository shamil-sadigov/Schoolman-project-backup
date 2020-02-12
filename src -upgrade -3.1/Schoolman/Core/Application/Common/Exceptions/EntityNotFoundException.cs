using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class EntityNotFoundException:Exception
    {
        private object searchParameter;

        public EntityNotFoundException(object entity, string message) : base(message) { }
    }
}
