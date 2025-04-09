using Application.Features.Schools;
using Infrastructure.Identity;
using Infrastructure.OpenApi;
using Infrastructure.Persistence;
using Infrastructure.Schools;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            return services
                .AddMultitenancyServices(configuration)
                .AddPersistenceService(configuration)
                .AddIdentityService()
                .AddPermissions() 
                .AddjwtAuthentication()
                .AddOPenApiDocumentation(configuration)
                .AddScoped<ISchoolService , SchoolService>();
        }
        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            return app
                .UseAuthentication()
                .UseCurrentUser()
                .UseMultitenancy()
                .UseAuthorization()
                .USeOPenApiDocumentation()
                ;
        }
    }
}
