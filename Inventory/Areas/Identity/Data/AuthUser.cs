using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Inventory.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AuthUser class
public class AuthUser : IdentityUser
{
    public string FullName { get; set; }

    public string Contact { get; set; }

    public string address { get; set; }

}

