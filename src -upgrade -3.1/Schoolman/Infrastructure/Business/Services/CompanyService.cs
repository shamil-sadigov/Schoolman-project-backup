using Application.Services;
using Application.Services.Business;
using Domain;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CompanyService : ServiceBase<Company, string>, ICompanyService
    {
        public CompanyService(IRepository<Company> repository,
                              ILogger<CompanyService> logger):base(repository, logger){ }

        public async Task<Result<Company>> CreateAsync(Company company)
        {
            // ex handling will be added
            await repository.AddAsync(company);
            await repository.SaveChangesAsync();
            return company;
        }
    }
}
