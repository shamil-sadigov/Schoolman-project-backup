﻿using Schoolman.Student.Core.Application.Models;
using System.Threading.Tasks;

namespace Schoolman.Student.Core.Application.Interfaces
{
    /// <summary>
    /// Manager class that responsible for generating and refreshing authentication tokens
    /// </summary>
    /// <typeparam name="T_user"></typeparam>
    public interface IJwtFactory<T_user> 
        where T_user: class
    {
        /// <summary>
        /// Generatees JWT token for specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<AuthResult> GenerateTokensAsync(T_user user);

        /// <summary>
        /// Get tokens and refresh them.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<AuthResult> RefreshTokensAsync(string jwt, string refreshToken);
    }
}