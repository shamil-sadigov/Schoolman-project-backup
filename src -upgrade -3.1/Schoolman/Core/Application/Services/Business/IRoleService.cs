using Domain;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Business
{
    /// <summary>
    /// Service that manages Role class
    /// </summary>
    public interface IRoleService:IServiceBase<Role,string>
    {
        Task<Result<Role>> CreateAsync(string rolename);

        // additional methods will be added
    }
}
