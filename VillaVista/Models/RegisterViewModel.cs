﻿using System.ComponentModel.DataAnnotations;

namespace VillaVista.Models
{
    public class RegisterViewModel
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}
