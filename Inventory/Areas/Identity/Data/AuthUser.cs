using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace Inventory.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AuthUser class
public class AuthUser : IdentityUser
{
    [Required]
    public int nameid { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Address { get; set; }
}

