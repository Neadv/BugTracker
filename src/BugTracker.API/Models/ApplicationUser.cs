using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace BugTracker.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}