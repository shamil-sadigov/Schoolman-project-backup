using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Business
{
    public interface IClientService:IServiceBase<AppClient, string>
    {
        Task<Result<AppClient>> CreateAsync(string clientName);
        Task<bool> AddToRole(AppClient client, string roleName);
        Task<bool> AddToCompany(AppClient client, string companyId);
    }
}
