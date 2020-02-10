using Application.Services;
using Application.Services.Business;
using Application.Users;
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

namespace Business.Services
{
    public class ClientManager : ServiceBase<Client, string>, IClientManager
    {
        private readonly ILogger<ClientManager> logger;
        public  IUserService UserService { get; }
        public  IRoleService RoleService { get; }
        public  ICompanyService CompanyService { get; }

        private readonly IMapper mapper;

        public ClientManager(IRepository<Client> repository,
                             IUserService userService,
                             IRoleService roleService,
                             ICompanyService companyService,
                             IMapper mapper,
                             ILogger<ClientManager> logger) :base(repository)
        {
            this.logger = logger;
            this.UserService = userService;
            this.RoleService = roleService;
            this.CompanyService = companyService;
            this.mapper = mapper;
        }


        public  async Task<Result<Client>> CreateAsync(ClientRegistraionRequest request)
        {
            var user = mapper.Map<User>(request);

            var result = await UserService.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                logger.LogInformation("Client creation failed: User.Email {Email}. Creation errors: {@Errors}",
                                    request.Email, result.Errors);

                return Result<Client>.Failure(result.Errors);
            }

            User createdUser = result.Response;

            var newClient = new Client();
            newClient.UserId = createdUser.Id;

            try
            {
                await repository.AddAndSaveAsync(newClient);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Client creation failed: Db Exception thrown while creating client with Email {email}",
                                createdUser.Email);

                return Result<Client>.Failure("Unable to create client");
            }

            return newClient;
        }

        public async Task<bool> AddToCompanyAsync(Client client, Company company)
        {
            client.CompanyId = company.Id;
            repository.Context.Entry(client).State = EntityState.Modified;

            try
            {
                await repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Adding company to client failed: Db Exception thrown while adding client to Company" +
                                    "ClientId {clientId}, ClientEmail {clientEmail}, CompanyId {companyId}, CompanyName {companyName}",
                                     client.Id, client.User.Email, company.Id, company.Name);

                return false;
            }

            return true;
        }

        public async Task<bool> AddToRoleAsync(Client client,  Role role)
        {
            client.RoleId = role.Id;
            repository.Context.Entry(client).State = EntityState.Modified;

            try
            {
                await repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Adding role to client failed: Db Exception thrown while adding role to Client" +
                                    "ClientId {clientId}, ClientEmail {clientEmail}, RoleName{roleName}",
                                     client.Id, client.User.Email, role.Name);

                return false;
            }

            return true;
        }


        public override async Task<Client> FindAsync(Expression<Func<Client, bool>> expression)
             => await repository.Set
                            .Include(c=> c.User)
                            .Include(c=> c.Role)
                            .Include(c=> c.Company)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(expression);


        public async Task<bool> AddRefreshToken(Client client, RefreshToken refreshToken)
        {
            client.RefreshToken = refreshToken;
            return await repository.UpdateAndSaveAsync(client);
        }


    }
}
