using System;

namespace Application.Common.Exceptions
{
    public class EntityNotCreatedException<T> : Exception
    {
        public T Entity { get; set; }

        public EntityNotCreatedException(T entity):base("Unable to created entity. See entity parameter")
             => Entity = entity;
        
    }
}
