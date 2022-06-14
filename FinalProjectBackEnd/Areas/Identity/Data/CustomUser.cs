using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProjectBackEnd.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProjectBackEnd.Areas.Identity.Data;

// Add profile data for application users by adding properties to the CustomUser class
public class CustomUser : IdentityUser
{
    public UserInfo UserInfo { get; set; }
}

