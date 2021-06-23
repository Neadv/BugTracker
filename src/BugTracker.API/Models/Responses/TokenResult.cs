using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.API.Models.Responses
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
