using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public TKey Id { get; set; }
    }
}
