using Domain;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Business
{
    /// <summary>
    /// Service that manages Company class
    /// </summary>
    public interface ICompanyService:IServiceBase<Company, string>
    {
        Task<Result<Company>> CreateAsync(Company company);
    }
}
