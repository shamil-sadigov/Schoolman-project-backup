using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Customers;
using Application.Services;
using Application.Services.Business;
using AutoMapper;
using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerManager : ServiceBase<Customer, string>, ICustomerManager
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly ICompanyService companyService;
        private readonly IMapper mapper;

        public CustomerManager(IRepository<Customer> repository,
                               IUserService userService,
                               IRoleService roleService,
                               ICompanyService companyService,
                               IMapper mapper,
                               ILogger<CustomerManager> logger) : base(repository, logger)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.companyService = companyService;
            this.mapper = mapper;
        }

        public async Task<Result<Customer>> CreateAsync(CustomerRegistrationRequest request, bool throwOnFail)
        {
            #region Create User
            var user = mapper.Map<User>(request);
            var result = await userService.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                logger.LogError("CustomerService. Cusotmer creation failed: User.Email {email}. Creation errors: {@errors}",
                                       request.Email, result.Errors);

                if (throwOnFail)
                    throw new EntityNotCreatedException<User>(user);

                return Result<Customer>.Failure(result.Errors);
            }

            #endregion

            #region Get Role

            User createdUser = result.Response;
            Role role = await roleService.FindOrCreateAsync(AppRoles.Candidate);

            if (role == null)
                throw new EntityNotFoundException(AppRoles.Candidate, "Role with this name wasnt found");

            #endregion

            #region Create customer

            createdUser.FirstName = "Changed";

            var newCustomer = new Customer();
            newCustomer.UserId = createdUser.Id;
            newCustomer.RoleId = role.Id;

            try
            {
                await repository.AddOrUpdateAsync(newCustomer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ICustomerManager. Customer creation failed: Db Exception thrown while creating customer with Email {email}",
                                createdUser.Email);

                if (throwOnFail)
                    throw ex;

                return Result<Customer>.Failure("Unable to create a customer");
            }

            return newCustomer;


            #endregion

        }

        public async Task<bool> AddToCompanyAsync(Customer customer, Company company, bool throwOnFail = false)
        {
            customer.CompanyId = company.Id;
            customer.Company = company;

            try
            {
                await repository.AddOrUpdateAsync(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ICustomerManager. Adding company to customer failed: Db Exception thrown while adding customer to Company" +
                                    "CustomerId {customerId}, CustomerEmail {clientEmail}, CompanyId {companyId}, CompanyName {companyName}",
                                     customer.Id, customer.UserInfo.Email, company.Id, company.Name);
                if (throwOnFail)
                    throw ex;

                return false;
            }

            return true;
        }

        public async Task<bool> AddToRoleAsync(Customer customer, Role role, bool throwOnFail)
        {
            customer.RoleId = role.Id;
            customer.Role = role;

            try
            {
                await repository.AddOrUpdateAsync(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ICustomerManager. Adding role to customer failed: Db Exception thrown while adding role to Client" +
                                    "CustomerId {customerId}, CustomerEmail {customerEmail}, RoleName {roleName}",
                                     customer.Id, customer.UserInfo.Email, role.Name);

                if (throwOnFail)
                    throw ex;

                return false;
            }

            return true;
        }

        public async Task<bool> AddRefreshTokenAsync(Customer customer, RefreshToken refreshToken, bool throwOnFail = false)
        {
            repository.Context.Attach(refreshToken);

            customer.RefreshToken.IssueTime = refreshToken.IssueTime;
            customer.RefreshToken.ExpirationTime = refreshToken.ExpirationTime;
            customer.RefreshToken.Token = refreshToken.Token;


            try
            {
                await repository.AddOrUpdateAsync(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ICustomerManager. Adding refresh tokento customer failed: Db Exception thrown while adding refreshToken to Client" +
                                    "CustomerId {customerId}, CustomerEmail {customerEmail}, RoleName {roleName}",
                                     customer.Id, customer.UserInfo.Email);
                if (throwOnFail)
                    throw ex;

                return false;
            }

            return true;
        }

        public Task<bool> CheckPasswordAsync(Customer customer, string password)
            => userService.CheckPasswordAsync(customer.UserInfo, password);



        public Task<bool> ExistEmailAsync(string email)
            => repository.AnyAsync(c => c.UserInfo.Email == email);


        public async Task<Customer> FindByEmailAsync(string email)
                => await repository.AsQueryable()
                                   .Include(c => c.UserInfo)
                                   .Include(c => c.Role)
                                   .Include(c => c.Company)
                                   .Include(c => c.UserInfo)
                                   .FirstOrDefaultAsync(c => c.UserInfo.Email == email);
        
        public override Task<Customer> FindAsync(Expression<Func<Customer, bool>> predicate)
                     => repository.AsQueryable()
                                    .Include(c => c.UserInfo)
                                    .Include(c => c.Role)
                                    .Include(c => c.Company)
                                    .FirstOrDefaultAsync(predicate);

    }
}
