using Application.Services;
using Application.Services.Business;
using Domain;
using Microsoft.Extensions.Logging;
using Persistence.Helpers;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CompanyService : ServiceBase<Company, string>, ICompanyService
    {
        public CompanyService(IRepository<Company> repository):base(repository){ }

        public async Task<Result<Company>> CreateAsync(Company company)
        {
            await repository.AddAndSaveAsync(company);
            return company;
        }
    }
}
