using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.API.Models.Responses
{
    public class RequestResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public static RequestResult CreateSucceeded() => new RequestResult { Succeeded = true };
        public static RequestResult CreateError(IEnumerable<string> errors) => new RequestResult { Succeeded = false, Errors = errors };
    }
}
