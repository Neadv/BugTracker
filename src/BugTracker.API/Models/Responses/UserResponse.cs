using System.Collections.Generic;

namespace BugTracker.API.Models.Responses
{
    public class UserResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
