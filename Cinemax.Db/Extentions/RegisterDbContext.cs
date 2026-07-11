using Cinemax.Db.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cinemax.Db.Extentions
{
    public static class RegisterDbContext
    {
        public static IServiceCollection AddAppDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            return services;
        }
    }
}
