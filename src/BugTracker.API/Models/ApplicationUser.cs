using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace BugTracker.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
        public bool IsActivated { get; set; } = false;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}