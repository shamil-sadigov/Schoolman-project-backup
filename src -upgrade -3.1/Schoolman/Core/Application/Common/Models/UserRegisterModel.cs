using Schoolman.Student.Core.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Models
{
    public class UserRegisterModel
    {
        [Required(AllowEmptyStrings =false)]
        [MaxLength(length:50)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(length: 50)]
        public string LastName{ get; set; }

        [Email]
        [MaxLength(length:50)]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }


            


    }
}
