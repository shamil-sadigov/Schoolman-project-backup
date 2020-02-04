using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public interface ISpecification<TModel>
    {
        Task<Result> IsSatisfied(TModel model);
    }
}
