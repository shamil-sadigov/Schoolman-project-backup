using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Schoolman.Student.Core.Application.Common.Models
{
    /// <summary>
    /// Email validation attribute. Since default EmailAddressAttribute is not that efficient
    /// </summary>
    class EmailAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string))
                return new ValidationResult("Value must be string type");

            string email = value as string;

            bool IsValid = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            if (IsValid)
                return ValidationResult.Success;
            else
                return new ValidationResult("Email is not valid");
        }
    }
}
