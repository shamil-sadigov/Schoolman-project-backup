using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Services
{
    public class DatabaseSeedingException<TEntity>:Exception
    {
        public DatabaseSeedingException(TEntity entity, string message):base(message)
        {
            Entity = entity;
        }

        public TEntity Entity { get; }
    }
}
