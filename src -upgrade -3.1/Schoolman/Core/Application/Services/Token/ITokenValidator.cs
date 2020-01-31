﻿using Domain.Models;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public interface ITokenValidator<T>
    {
        Result<Claim[]> ValidateAccessToken(string accessToken, T validationParameters);
        Result ValidateRefreshToken(RefreshToken refreshToken, string accessTokenId);
    }
}
