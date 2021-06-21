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
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                if (configuration.GetValue<bool>("SeedData:AutoMigrate"))
                    context.Database.Migrate();

                var adminName = configuration["SeedData:Admin:Username"];
                var adminEmail = configuration["SeedData:Admin:Email"];
                var adminPassword = configuration["SeedData:Admin:Password"];
                if (await userManager.FindByNameAsync(adminName) == null)
                {
                    if (!await roleManager.RoleExistsAsync("admin"))
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = "admin"});
                    }
                    var admin = new ApplicationUser
                    {
                        UserName = adminName,
                        Email = adminEmail,          
                    };
                    admin.PasswordHash = userManager.PasswordHasher.HashPassword(admin, adminPassword);
                    await userManager.CreateAsync(admin);
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
