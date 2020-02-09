using Application.Services;
using Application.Users;
using AutoMapper;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence.Helpers;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Services
{

    public class UserService : ServiceBase<User,string>, IUserService
    {
        private readonly IMapper mapper;
        private readonly ILogger<UserService> logger;
        private readonly UserManager<User> userManager;

        public UserService (UserManager<User> userManager,
                           IRepository<User> userRepository,
                           IMapper mapper,
                           ILogger<UserService> logger):base(userRepository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.logger = logger;
        }


        /// <summary>
        /// Creates user and returns creation result and userId. If creation is failed, see result errors.
        /// </summary>
        /// <returns>Creation result</returns>
        public async Task<Result<User>> CreateAsync(UserRegistrationRequest userDto, string password)
        {
            // password and Id are empty
            // will be added by userService
            var newUser = mapper.Map<User>(userDto);

            var creationResult = await userManager.CreateAsync(newUser, userDto.Password);

            if (!creationResult.Succeeded)
            {
                logger.LogInformation("User creation failed: User.Email {Email}. Validation Errors: {@Errors}",
                                     userDto.Email, creationResult.Errors);

                var errors = creationResult.Errors.Select(e => e.Description).ToArray();
                return Result<User>.Failure(errors);
            }
            return Result<User>.Success(newUser);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await userManager.CheckPasswordAsync(user, password);


        public async Task<bool> AddRefreshToken(User user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken;
            return await repository.UpdateAndSaveAsync(user);
        }



    }
}
