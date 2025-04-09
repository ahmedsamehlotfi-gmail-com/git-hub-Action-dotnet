using Infrastructure.Persistence.DbConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    public class TenancyDbConfiguration : IEntityTypeConfiguration<ABCSchoolTenantInfo>
    {
        public void Configure(EntityTypeBuilder<ABCSchoolTenantInfo> builder)
        {
            builder
                .ToTable("Tenants", SchemaNames.Multitenancy);
        }
    }
}
