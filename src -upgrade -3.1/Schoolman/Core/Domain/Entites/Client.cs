﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// Many-to-many table
    /// </summary>
    public class Client : IdentityUserRole<string>, IEntity<string>
    {
        public string Id { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
        public Company Company { get; set; }

        public string CompanyId { get; set; }

        // Owned entity
        public RefreshToken RefreshToken { get; set; }

    }
}