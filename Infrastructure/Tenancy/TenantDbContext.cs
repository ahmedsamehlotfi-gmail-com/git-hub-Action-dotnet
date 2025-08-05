using Finbuckle.MultiTenant.Stores;
using Infrastructure.Persistence.DbConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    public class TenantDbContext(DbContextOptions<TenantDbContext> options):EFCoreStoreDbContext<ABCSchoolTenantInfo>(options)
    {

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ABCSchoolTenantInfo>()
                 .ToTable("Tenants", SchemaNames.Multitenancy);
        }


    }
}
