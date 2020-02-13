using System;

namespace Application.Common.Exceptions
{
    public class EntityNotFoundException:Exception
    {
        public readonly object SearchParameter;
        public EntityNotFoundException(object entity, string message) : base(message) { SearchParameter = entity; }
    }
}
