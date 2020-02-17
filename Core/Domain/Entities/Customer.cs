using Domain.Base;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// Main entity that hold User, Role and Company information
    /// </summary>
    public class Customer : CustomerBase
    {
        public User UserInfo { get; set; }
        public Role Role { get; set; }
        public Company Company { get; set; }

        public string CompanyId { get; set; }

        public RefreshToken RefreshToken { get; set; }


        public ICollection<Instructor> Instructors { get; set; }
    }
}