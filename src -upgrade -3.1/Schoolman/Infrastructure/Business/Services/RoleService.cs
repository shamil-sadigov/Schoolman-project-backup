using Application.Services;
using Application.Services.Business;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class RoleService : ServiceBase<Role, string>, IRoleService
    {
        private readonly RoleManager<Role> roleManager;

        public RoleService(RoleManager<Role> roleManager,
                           IRepository<Role> repository) : base(repository)
        {
            this.roleManager = roleManager;
        }


        public async Task<Result<Role>> CreateAsync(string rolename)
        {
            if (await roleManager.RoleExistsAsync(rolename))
                return Result<Role>.Failure("Role already exist");

            var role = new Role(rolename);
            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
                return Result<Role>.Success(role);

            return Result<Role>.Failure(result.Errors.Select(c => c.Description).ToArray());
        }
    }
}
