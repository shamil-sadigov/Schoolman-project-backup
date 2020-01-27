﻿using Domain;
using Schoolman.Student.Core.Application.Common.Models;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Threading.Tasks;

namespace Application
{
    public interface IUserManager
    {
        Task<(Result result, User user)> CreateUser(UserRegisterModel userRegisterModel);
        Task<Result> DeleteUser(string email);
        Task<Result> SendConfirmationEmail(User user);
        Task<Result> ConfirmEmail(string userId, string token);
        Task<(Result, User)> Find(string email, Action<UserSearchOptions> searchOptions = null);
    }
}
