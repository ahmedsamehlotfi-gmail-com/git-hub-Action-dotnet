using Finbuckle.MultiTenant.EntityFrameworkCore;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.DbConfiguration
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .ToTable("Users", SchemaNames.Identity)
                .IsMultiTenant();
        }
    }

    public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder
                .ToTable("Roles", SchemaNames.Identity)
                .IsMultiTenant()
                .AdjustUniqueIndexes();
        }
    }

    public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            builder
                .ToTable("RoleClaims", SchemaNames.Identity)
                .IsMultiTenant()
                ;
        }
    }
    public class ApplicationUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder
                .ToTable("UserRoles", SchemaNames.Identity)
                .IsMultiTenant()
                ;
        }
    }

    public class ApplicationUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
        {
            builder
                .ToTable("UserClaims", SchemaNames.Identity)
                .IsMultiTenant()
                ;
        }
    }
    public class ApplicationUserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
        {
            builder
                .ToTable("UserLogins", SchemaNames.Identity)
                .IsMultiTenant()
                ;
        }
    }

    public class ApplicationUserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
        {
            builder
                .ToTable("UserTokens", SchemaNames.Identity)
                .IsMultiTenant()
                ;
        }
    }
}
