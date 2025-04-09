using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public abstract class BaseDbContext : MultiTenantIdentityDbContext<ApplicationUser , ApplicationRole, string,
        IdentityUserClaim<string>, IdentityUserRole<string> , IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        protected BaseDbContext(ITenantInfo tenantInfo, DbContextOptions options) : base(tenantInfo, options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!string.IsNullOrEmpty(TenantInfo.ConnectionString))
            {
                optionsBuilder.UseSqlServer(TenantInfo.ConnectionString, option =>
                {
                    option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });
            }
        }
    }
}
