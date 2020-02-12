using Application.Services;
using Application.Services.Business;
using Application.Customers;
using AutoMapper;
using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Helpers;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;

namespace Business.Services
{
    public class CustomerManager : ServiceBase<Customer, string>, ICustomerManager
    {
        private readonly IUserService userService;
        private readonly ILogger<CustomerManager> logger;
        private readonly IRoleService roleService;
        private readonly ICompanyService companyService;

        private readonly IMapper mapper;

        public CustomerManager(IRepository<Customer> repository,
                               IUserService userService,
                               IRoleService roleService,
                               ICompanyService companyService,
                               IMapper mapper,
                               ILogger<CustomerManager> logger) : base(repository)
        {
            this.logger = logger;
            this.userService = userService;
            this.roleService = roleService;
            this.companyService = companyService;
            this.mapper = mapper;
        }


        public async Task<Result<Customer>> CreateAsync(CustomerRegistrationRequest request)
        {
            var user = mapper.Map<User>(request);

            var result = await userService.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                logger.LogError("CustomerService. Cusotmer creation failed: User.Email {email}. Creation errors: {@errors}",
                                       request.Email, result.Errors);

                return Result<Customer>.Failure(result.Errors);
            }

            User createdUser = result.Response;

            var newCustomer = new Customer();
            newCustomer.UserId = createdUser.Id;

            try
            {
                await repository.AddAndSaveAsync(newCustomer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Customer creation failed: Db Exception thrown while creating customer with Email {email}",
                                createdUser.Email);

                return Result<Customer>.Failure("Unable to create a customer");
            }

            return newCustomer;
        }

        public async Task<bool> AddToCompanyAsync(Customer customer, Company company)
        {
            customer.CompanyId = company.Id;
            repository.Context.Entry(customer).State = EntityState.Modified;

            try
            {
                await repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Adding company to customer failed: Db Exception thrown while adding customer to Company" +
                                    "CustomerId {customerId}, CustomerEmail {clientEmail}, CompanyId {companyId}, CompanyName {companyName}",
                                     customer.Id, customer.UserInfo.Email, company.Id, company.Name);

                return false;
            }

            return true;
        }

        public async Task<bool> AddToRoleAsync(Customer customer, Role role)
        {
            customer.RoleId = role.Id;
            repository.Context.Entry(customer).State = EntityState.Modified;

            try
            {
                await repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Adding role to customer failed: Db Exception thrown while adding role to Client" +
                                    "CustomerId {customerId}, CustomerEmail {customerEmail}, RoleName {roleName}",
                                     customer.Id, customer.UserInfo.Email, role.Name);

                return false;
            }

            return true;
        }



        public async Task<bool> AddRefreshToken(Customer customer, RefreshToken refreshToken)
        {
            customer.RefreshToken = refreshToken;
            return await repository.UpdateAndSaveAsync(customer);
        }

        public async Task<bool> CheckPasswordAsync(Customer customer, string password)
        {
            return await userService.CheckPasswordAsync(customer.UserInfo, password);
        }



        public async Task<bool> ExistEmailAsync(string email)
        {
            return await repository.Set.AnyAsync(c => c.UserInfo.Email == email);
        }

        public async Task<Customer> FindByEmailAsync(string email)
        {
            var customer =  await repository.Set.Include(c => c.UserInfo)
                                                .Include(c => c.Role)
                                                .Include(c => c.Company)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(c => c.UserInfo.Email == email);

            if (customer == null)
            {
                logger.LogError("CustomerManager.FindAsync(email): Customer with this email {email} wasnt found");
                throw new EntityNotFoundException(email, "Customer with this email wasn't found");
            }

            return customer;
        }

        public override async Task<Customer> FindAsync(Expression<Func<Customer, bool>> predicate)
        {
            var customer = await  repository.Set
                                      .Include(c => c.UserInfo)
                                      .Include(c => c.Role)
                                      .Include(c => c.Company)
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(predicate);

            if(customer == null)
            {
                logger.LogError("CustomerManager.FindAsync(predicate): Customer wasn't found");
                throw new EntityNotFoundException(predicate, "Customer with thie predicate wasn't found");
            }

            return customer;
        }
             

        public override async Task<ICollection<Customer>> ListAsync()
            => await repository.Set.Include(c => c.UserInfo)
                                   .Include(c => c.Role)
                                   .Include(c => c.Company)
                                   .AsNoTracking()
                                   .ToArrayAsync();

    }
}
