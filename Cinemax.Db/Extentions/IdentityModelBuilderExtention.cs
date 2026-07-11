using Cinemax.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Db.Extentions
{
    public static class IdentityModelBuilderExtention
    {
        public static void ConfigureIdentityTables(this ModelBuilder mb)
        {
            mb.Entity<ApplicationUser>().ToTable("Users");
            mb.Entity<IdentityRole<Guid>>().ToTable("Roles");
            mb.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            mb.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            mb.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            mb.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            mb.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        }
    }
}
