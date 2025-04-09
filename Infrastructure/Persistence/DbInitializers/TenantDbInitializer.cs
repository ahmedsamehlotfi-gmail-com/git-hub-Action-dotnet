using Finbuckle.MultiTenant;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.DbInitializers
{
    public class TenantDbInitializer(TenantDbContext tenantDbContext, IServiceProvider serviceProvider) : ITenantDbInitializer
    {
        private readonly TenantDbContext _tenantDbContext = tenantDbContext;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
        {
            await InitializerDatabaseWithTenantAsync(cancellationToken);
            foreach (var tenant in await _tenantDbContext.TenantInfo.ToListAsync(cancellationToken))
            {
                await InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
            }
        }

        public async Task InitializerDatabaseWithTenantAsync(CancellationToken cancellationToken)
        {
            if (await _tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id], cancellationToken: cancellationToken) is null)
            {
                var rootTenant = new ABCSchoolTenantInfo
                {
                    Id = TenancyConstants.Root.Id,
                    Identifier = TenancyConstants.Root.Name,
                    Name = TenancyConstants.Root.Name,
                    AdminEmail = TenancyConstants.Root.Email,
                    IsActive = true,
                    ValidUpTo = DateTime.UtcNow.AddYears(1)
                };
                await _tenantDbContext.TenantInfo.AddAsync(rootTenant);
                await _tenantDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task InitializeApplicationDbForTenantAsync(ABCSchoolTenantInfo tenant, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
                .MultiTenantContext = new MultiTenantContext<ABCSchoolTenantInfo>()
                {
                    TenantInfo = tenant,
                };

            await _serviceProvider.GetRequiredService<ApplicationDbInitializer>()
                .InitializeDatabaseAsync(cancellationToken);
        }
    }
}
