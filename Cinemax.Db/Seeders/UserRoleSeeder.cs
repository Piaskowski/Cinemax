using Cinemax.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cinemax.Db.Seeders
{
    public class UserRoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (UserRole role in Enum.GetValues<UserRole>())
            {
                var roleName = role.ToString();

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<Guid>(roleName));
                }
            }
        }
    }
}
