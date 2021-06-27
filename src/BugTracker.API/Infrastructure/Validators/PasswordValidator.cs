using FluentValidation;
using System.Text.RegularExpressions;

namespace BugTracker.API.Infrastructure.Validators
{
    public abstract class PasswordValidator<T> : AbstractValidator<T>
    {
        protected readonly string InvalidPassword = "Password must contain at least 6, uppercase, lowercase, numbers and a special character";

        protected bool HasValidPassword(string pw)
        {
            var lowercase = new Regex(@"[a-z]+");
            var uppercase = new Regex(@"[A-Z]+");
            var digit = new Regex(@"(\d)+");
            var symbol = new Regex(@"(\W)+");

            return (lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && digit.IsMatch(pw) && symbol.IsMatch(pw));
        }
    }
}
