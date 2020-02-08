using Application.Services;
using Application.Users;
using AutoMapper;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UserService(UserManager<User> userManager,
                           IRepository<User> userRepository,
                           IMapper mapper)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.mapper = mapper;
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
                var errors = creationResult.Errors.Select(e => e.Description).ToArray();
                return Result<User>.Failure(errors);
            }

            return Result<User>.Success(newUser);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await userManager.CheckPasswordAsync(user, password);


        public async Task<int> UpdateRefreshTokenAsync(User user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken;
            userRepository.Context.Entry(user).State = EntityState.Modified;
            return await userRepository.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Expression<Func<User, bool>> predicate)
            => await userRepository.Set.AnyAsync(predicate);


        #region Deleting

        /// <summary>
        /// Delete user by Id. If deletion is failed, see result errors
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Deletion result</returns>
        public async Task<Result> DeleteAsync(string userId)
        {
            var userToDelete = new User() { Id = userId };

            userRepository.Context.Entry(userToDelete).State = EntityState.Deleted;

            return await userRepository.SaveChangesAsync() > 0 ? true : false;

            // soon will be added some logging and expcetions catching
        }

        public async Task<Result> DeleteAsync(User user)
        {
            userRepository.Set.Remove(user);
            return await userRepository.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<Result> DeleteAsync(Expression<Func<User, bool>> predicate)
        {
            var users = userRepository.Set.Where(predicate).AsNoTracking();
            return await DeleteRangeAsync(users);
        }

        public async Task<Result> DeleteRangeAsync(IEnumerable<User> users)
        {
            userRepository.Set.RemoveRange(users);
            return await userRepository.SaveChangesAsync() > 0 ? true : false;
        }


        #endregion

        #region Reading


        public async Task<User> FindAsync(string id)
           => await userRepository.Set.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User> FindFirstAsync(Expression<Func<User, bool>> expression)
              => await userRepository.Set.AsNoTracking().SingleOrDefaultAsync(expression);

        public IAsyncEnumerable<User> FindRangeAsync(Expression<Func<User, bool>> predicate)
            => userRepository.Set.AsNoTracking().Where(predicate).AsAsyncEnumerable();



        #endregion

        #region Updating

        public async Task<Result> UpdateAsync(User user)
        {
            userRepository.Set.Update(user);
            //  will be changed soon sensi Update method updates all properties even if they are not changed
            return await userRepository.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<Result> UpdateRange(IEnumerable<User> entites)
        {
            userRepository.Set.UpdateRange(entites);
            return await userRepository.SaveChangesAsync() > 0 ? true : false;
        }


        #endregion
    }
}
