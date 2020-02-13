using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class BadRequestModel
    {
        public string[] Errors { get; set; }
        public BadRequestModel()
        {

        }
        public BadRequestModel(string[] errors)
        {
            Errors = errors;
        }
    }
}
