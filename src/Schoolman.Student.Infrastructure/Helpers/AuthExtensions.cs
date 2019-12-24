using Schoolman.Student.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schoolman.Student.Infrastructure.Helpers
{
    internal static class AuthExtensions
    {
        /// <summary>
        /// Returns bytes of secret keywork in ASCI 
        /// </summary>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string securityKey)
        {
            return Encoding.ASCII.GetBytes(securityKey);
        }
    }
}
