using BugTracker.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.API.Data
{
    public static class SeedDatabaseExtension
    {
        public static void SeedDatabase(this IApplicationBuilder app)
        {
            SeedDatabaseAsync(app).GetAwaiter().GetResult();
        }

        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationContext>();
                var configuration = services.GetRequiredService<IConfiguration>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                if (configuration.GetValue<bool>("SeedData:AutoMigrate"))
                    context.Database.Migrate();

                var adminName = configuration["SeedData:Admin:Username"];
                var adminEmail = configuration["SeedData:Admin:Email"];
                var adminPassword = configuration["SeedData:Admin:Password"];
                if (await userManager.FindByNameAsync(adminName) == null)
                {
                    var adminRole = await roleManager.FindByNameAsync("admin");
                    if (adminRole == null)
                    {
                        adminRole = new ApplicationRole("admin");
                        await roleManager.CreateAsync(adminRole);
                    }
                    var admin = new ApplicationUser
                    {
                        UserName = adminName,
                        Email = adminEmail,
                        IsActivated = true
                    };
                    admin.PasswordHash = userManager.PasswordHasher.HashPassword(admin, adminPassword);
                    admin.Roles.Add(adminRole);
                    await userManager.CreateAsync(admin);
                }
            }
        }
    }
}
