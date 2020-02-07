using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IBaseEntity<Tkey>
    {
        Tkey Id { get; set; }
    }
}
