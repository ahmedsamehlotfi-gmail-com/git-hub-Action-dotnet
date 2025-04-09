using Application.Features.Tenancy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy
{
    public interface ITenantService
    {
        Task<string> CreateTenantAsync(CreateTenantRequest createTenant , CancellationToken cancellationToken);
        Task<string> ActivateAsync(string id);
        Task<string> DeactivateAsync(string id);
        Task<string> UpdateSubscriptionAsync(string id, DateTime newExpiryDate);
        Task<List<TenantDto>> GetTenantAsync();

        Task<TenantDto> GetTenantByIdAsync(string id);
    }
}
