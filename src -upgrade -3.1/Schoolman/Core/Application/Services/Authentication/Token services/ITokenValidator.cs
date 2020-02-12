using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Token
{
    /// <summary>
    /// Basic service for validating tokens
    /// </summary>
    /// <typeparam name="Token"></typeparam>
    /// <typeparam name="Result"></typeparam>
    public interface ITokenValidator<Token, Result> where Result: class
    {
        Task<Result> ValidateTokenAsync(Token token);
    }
}
