using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IEntity<Tkey>
    {
        Tkey Id { get; set; }
    }

}
