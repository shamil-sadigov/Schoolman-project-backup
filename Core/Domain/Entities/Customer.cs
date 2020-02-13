using Domain.Base;
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

        // Owned entity
        public RefreshToken RefreshToken { get; set; }
    }
}