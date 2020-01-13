using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schoolman.Student.WenApi.Controllers.Identity.DTO.Response
{
    public class Register_ResponseModel_OnFail
    {
        public Register_ResponseModel_OnFail()
        {

        }

        public Register_ResponseModel_OnFail(string[] Errors)
        {
            this.Errors = Errors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example> Email is invalid</example>
        public string[] Errors { get; set; }
    }
}
