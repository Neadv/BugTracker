using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.API.Models
{
    public class RefreshToken
    {
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }
        
        [Key]
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public bool Active => DateTime.UtcNow <= Expires;
    }
}
