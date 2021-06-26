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

        public static RegisterResult CreateSucceeded() => new RegisterResult { Succeeded = true };
        public static RegisterResult CreateError(IEnumerable<string> errors) => new RegisterResult { Succeeded = false, Errors = errors };
    }
}
