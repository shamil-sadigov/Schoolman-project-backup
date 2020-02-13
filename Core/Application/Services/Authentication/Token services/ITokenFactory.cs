using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Token
{
    /// <summary>
    /// Basic service for generating tokens
    /// </summary>
    /// <typeparam name="Param"></typeparam>
    /// <typeparam name="Token"></typeparam>
    public interface ITokenFactory<Param,Token>
    {
        Task<Token> GenerateTokenAsync(Param parameters);
    }
}
