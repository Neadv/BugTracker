using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.API.Models.Responses
{
    public class RegisterResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
        public TokenResult Token { get; set; }

        public static RegisterResult CreateSucceeded(TokenResult token) => new RegisterResult { Succeeded = true, Token = token };
        public static RegisterResult CreateError(IEnumerable<string> errors) => new RegisterResult { Succeeded = false, Errors = errors };
    }
}
