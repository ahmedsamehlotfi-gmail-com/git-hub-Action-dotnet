using Application.Features.Tenancy;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.DbInitializers;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public static class PersistenceServicesExtensions
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services , IConfiguration configuration)
        {
            return services.
                AddDbContext<ApplicationDbContext>(option=>option
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddTransient<ITenantDbInitializer , TenantDbInitializer>()
                .AddTransient<ITenantService , TenantService>()
                .AddTransient<ApplicationDbInitializer>();
        }

        public static async Task AddDatabaseInitializerAsync(this IServiceProvider serviceProvider , CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();
           // await scope.ServiceProvider.GetRequiredService<ITenantService>().GetTenantAsync(CancellationToken.None);

            await scope.ServiceProvider.GetRequiredService<ITenantDbInitializer>()
                .InitializeDatabaseAsync(CancellationToken.None);
        }
    }
}
