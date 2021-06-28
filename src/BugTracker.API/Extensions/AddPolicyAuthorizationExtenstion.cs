using Microsoft.Extensions.DependencyInjection;

namespace BugTracker.API.Extensions
{
    public static class AddPolicyAuthorizationExtenstion
    {
        public static void AddPolicyAthurization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admins", options => options.RequireRole("admin"));
                options.AddPolicy("ProjectManage", options =>
                {
                    options.RequireRole("admin", "pm");
                });
            });
        }
    }
}
