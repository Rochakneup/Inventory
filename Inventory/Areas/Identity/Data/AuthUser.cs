using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Areas.Identity.Data
{
    public class AuthUser : IdentityUser
    {
        [Required]
        public int nameid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Address { get; set; }

        public DateTime? LoginDate { get; set; }
        public string? Status { get; set; } = "Active";

      
    }
}
