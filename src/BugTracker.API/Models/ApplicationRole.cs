using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BugTracker.API.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        public ApplicationRole(string roleName) 
            : base(roleName) { }

        protected ApplicationRole() { }
    }
}
