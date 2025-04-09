using Application.Features.Tenancy;
using Application.Features.Tenancy.Model;
using Finbuckle.MultiTenant;
using Infrastructure.Persistence.DbInitializers;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    public class TenantService : ITenantService
    {
        private readonly IMultiTenantStore<ABCSchoolTenantInfo> _tenantStore;
        private readonly ApplicationDbInitializer _applicationDbInitializer;
        private readonly IServiceProvider _serviceProvider;

        public TenantService(IMultiTenantStore<ABCSchoolTenantInfo> tenantStore , ApplicationDbInitializer applicationDbInitializer , IServiceProvider serviceProvider)
        {
           _tenantStore = tenantStore;
            _applicationDbInitializer = applicationDbInitializer;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> ActivateAsync(string id)
        {
           var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.IsActive = true; 
            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<string> CreateTenantAsync(CreateTenantRequest createTenant , CancellationToken cancellationToken)
        {
            var newTenant = new ABCSchoolTenantInfo
            {
                Id = createTenant.Identifier,
                Identifier = createTenant.Identifier,
                Name = createTenant.Name,
                ConnectionString = createTenant.ConnectionString,
                AdminEmail = createTenant.AdminEmail,
                ValidUpTo = createTenant.ValidUpTo,
                IsActive = createTenant.IsActive,
            };
           await _tenantStore.TryAddAsync(newTenant);
            try
            {
                using var scope = _serviceProvider.CreateScope();
                _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
                    .MultiTenantContext = new MultiTenantContext<ABCSchoolTenantInfo>()
                    {
                        TenantInfo = newTenant,
                    };
                await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
                    .InitializeDatabaseAsync(cancellationToken);

            }
            catch (Exception)
            {
                await _tenantStore.TryRemoveAsync(createTenant.Identifier);
                throw;
            }
            return newTenant.Id;
        }

        public async Task<string> DeactivateAsync(string id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.IsActive = false;
            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<List<TenantDto>> GetTenantAsync()
        {
           var tenantInDb = await _tenantStore.GetAllAsync();
            return tenantInDb.Adapt<List<TenantDto>>();
        }

        public async Task<TenantDto> GetTenantByIdAsync(string id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);
            //return new TenantDto()
            //{
            //    Id = id,
            //    Name = tenantInDb.Name,
            //    AdminEmail = tenantInDb.AdminEmail,
            //    ConnectionString = tenantInDb.ConnectionString,
            //    ValidUpTo = tenantInDb.ValidUpTo,
            //    IsActive = tenantInDb.IsActive,
            //};
            return tenantInDb.Adapt<TenantDto>();
        }

        public async Task<string> UpdateSubscriptionAsync(string id, DateTime newExpiryDate)
        {
           var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.ValidUpTo = newExpiryDate;
            return tenantInDb.Id;
        }
    }
}
