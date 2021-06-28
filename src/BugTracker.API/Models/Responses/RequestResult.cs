using System.Collections.Generic;
using System.Linq;

namespace BugTracker.API.Models.Responses
{
    public class RequestResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public static RequestResult CreateSucceeded() => new RequestResult { Succeeded = true };
        public static RequestResult CreateError(IEnumerable<string> errors) => new RequestResult { Succeeded = false, Errors = errors };
    }

    public class RequestResult<TModel> : RequestResult
    {
        public TModel Model { get; set; }

        public static RequestResult<TModel> CreateSucceeded(TModel model) => new RequestResult<TModel> { Succeeded = true, Model = model };
        public static new RequestResult<TModel> CreateError(IEnumerable<string> errors) => new RequestResult<TModel>{ Succeeded = false, Errors = errors };

    }
}
