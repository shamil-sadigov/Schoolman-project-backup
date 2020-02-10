using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Token
{
    public interface ITokenFactory<Param,Token>
    {
        Task<Token> GenerateTokenAsync(Param parameters);
    }
}
