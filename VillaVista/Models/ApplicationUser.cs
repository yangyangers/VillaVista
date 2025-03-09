﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VillaVista.Models
{
    public class ApplicationUser : IdentityUser
    {
       [Required]
        public string Fullname { get; set; }
        public string Role { get; set; } // Either "Staff" or "Homeowner"
    }
}
