using Application.Customers;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Application.Services.Business
{
    /// <summary>
    /// Service that manages Customer class
    /// </summary>
    public interface ICustomerManager:IServiceBase<Customer, string>
    {
        Task<Result<Customer>> CreateAsync(CustomerRegistrationRequest userDto);
        Task<bool> AddToRoleAsync(Customer customer, Role role);
        Task<bool> AddToCompanyAsync(Customer customer, Company company);
        Task<bool> AddRefreshToken(Customer customer, RefreshToken refreshToken);
        Task<bool> CheckPasswordAsync(Customer customer, string password);
    }
}
