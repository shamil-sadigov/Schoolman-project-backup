using Application.Customers;
using Domain;
using Domain.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services.Business
{
    /// <summary>
    /// Service that manages Customer class
    /// </summary>
    public interface ICustomerManager:IServiceBase<Customer, string>
    {
        Task<Result<Customer>> CreateAsync(CustomerRegistrationRequest userDto, bool throwOnFail = false);
        Task<bool> AddToRoleAsync(Customer customer, Role role, bool throwOnFail = false);
        Task<bool> AddToCompanyAsync(Customer customer, Company company, bool throwOnFail = false);
        Task<bool> AddRefreshToken(Customer customer, RefreshToken refreshToken, bool throwOnFail = false);
        Task<bool> CheckPasswordAsync(Customer customer, string password);

        /// <summary>
        /// Finds by Email, if not found, null will be returned
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Customer> FindByEmailAsync(string email);
        Task<bool> ExistEmailAsync(string email);

    }
}
