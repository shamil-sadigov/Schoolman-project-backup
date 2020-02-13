using Application.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{

    public class UserService : ServiceBase<User,string>, IUserService
    {
        private readonly UserManager<User> userManager;

        public UserService (UserManager<User> userManager,
                           IRepository<User> userRepository,
                           ILogger<UserService> logger):base(userRepository, logger)
        {
            this.userManager = userManager;
        }


        /// <summary>
        /// Creates user and returns creation result and userId. If creation is failed, see result errors.
        /// </summary>
        /// <returns>Creation result</returns>
        public async Task<Result<User>> CreateAsync(User newUser, string password)
        {
            var creationResult = await userManager.CreateAsync(newUser, password);

            var errors = creationResult.Errors.Select(e => e.Description).ToArray();

            if (!creationResult.Succeeded)
            {
                logger.LogWarning("UserService. User creation failed: User.Email {Email}. Validation Errors: {@Errors}",
                                     newUser.Email, errors);

                return Result<User>.Failure(errors);
            }
            return Result<User>.Success(newUser);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await userManager.CheckPasswordAsync(user, password);

    }
}
