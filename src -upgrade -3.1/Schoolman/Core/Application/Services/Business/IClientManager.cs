using Application.Users;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Application.Services.Business
{
    public interface IClientManager:IServiceBase<Client, string>
    {
        public IUserService UserService { get; }
        public IRoleService RoleService { get; }
        public ICompanyService CompanyService { get; }


        Task<Result<Client>> CreateAsync(ClientRegistraionRequest clientName);
        Task<bool> AddToRoleAsync(Client client, Role role);
        Task<bool> AddToCompanyAsync(Client client, Company company);
        Task<bool> AddRefreshToken(Client user, RefreshToken refreshToken);
    }
}
