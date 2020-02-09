using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IDeleteableEntity
    {
        bool IsDeleted { get; set; }
        string DeletedBy { get; set; }
    }
}
