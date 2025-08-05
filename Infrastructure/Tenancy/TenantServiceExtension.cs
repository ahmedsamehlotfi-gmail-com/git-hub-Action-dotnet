using Application.Features.Tenancy;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    public static class TenantServiceExtension
    {
        public static IServiceCollection AddMultitenancyServices(this IServiceCollection services , IConfiguration configuration)
        {
            return services
                .AddDbContext<TenantDbContext>(option => option
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddMultiTenant<ABCSchoolTenantInfo>()
                .WithHeaderStrategy(TenancyConstants.TenantIdName)
                .WithClaimStrategy(ClaimConstants.Tenant)
                .WithCustomQueryStringStrategy(TenancyConstants.TenantIdName)
                .WithEFCoreStore<TenantDbContext, ABCSchoolTenantInfo>()
                .Services
                .AddScoped<ITenantService , TenantService>();
        }

        public static IApplicationBuilder UseMultitenancy(this IApplicationBuilder app)
        {
            return app.UseMultiTenant();
        } 

        private static FinbuckleMultiTenantBuilder<ABCSchoolTenantInfo> WithCustomQueryStringStrategy(
            this FinbuckleMultiTenantBuilder<ABCSchoolTenantInfo> builder , string customQueryStringStrategy)
        {
            return builder
                .WithDelegateStrategy(context =>
                {
                    if (context is not HttpContext httpContext)
                    {
                        return Task.FromResult((string)null);
                    }
                    httpContext.Request.Query.TryGetValue(customQueryStringStrategy, out StringValues tenantIdParam);
                    return Task.FromResult(tenantIdParam.ToString());
                });
        }
    }
}
