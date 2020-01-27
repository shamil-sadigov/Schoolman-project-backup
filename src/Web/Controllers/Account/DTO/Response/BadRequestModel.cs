using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schoolman.Student.WenApi.Controllers.Identity.DTO.Response
{
    /// <summary>
    /// DTO
    /// </summary>
    public class BadRequestModel
    {
        public BadRequestModel()
        {

        }

        public BadRequestModel(string[] Errors)
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
