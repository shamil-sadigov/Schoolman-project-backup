using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Core.Application.Models
{
    /// <summary>
    /// Authentication result that is used in Authentication-related services
    /// </summary>
    //public class AuthResult : Result
    //{
    //    public string JwtToken { get; private set; }
    //    public string RefreshToken { get; private set; }

    //    protected AuthResult(bool IsSucceeded, string[] errors):base(IsSucceeded, errors)
    //    { }

    //    protected AuthResult(string jwt, string refresh)
    //        :base(true, Array.Empty<string>())
    //    {
    //        JwtToken = jwt;
    //        RefreshToken = refresh;
    //    }

    //    public static AuthResult Success(string jwtToken, string refreshToken) =>
    //                new AuthResult(jwtToken, refreshToken);

    //    public new static AuthResult Failure(params string[] errors) =>
    //                  new AuthResult(false, errors);

    //}
}