using System;

namespace Application.Common.Exceptions
{
    public class EntityNotFoundException:Exception
    {
        private readonly object searchParameter;
        public EntityNotFoundException(object entity, string message) : base(message) { searchParameter = entity; }
    }
}
