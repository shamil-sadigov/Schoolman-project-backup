using Application.Services;
using Application.Services.Business;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClientManager : ServiceBase<AppClient, string>, IClientService
    {
        private readonly ILogger<ClientManager> logger;

        public ClientManager(IRepository<AppClient> repository,
                             ILogger<ClientManager> logger):base(repository)
        {
            this.logger = logger;
        }


        public Task<Result<AppClient>> CreateAsync(string clientName)
        {
            throw new NotImplementedException();
        }




        public Task<bool> AddToCompany(AppClient client, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddToRole(AppClient client, string roleName)
        {
            throw new NotImplementedException();
        }

      
    }
}
