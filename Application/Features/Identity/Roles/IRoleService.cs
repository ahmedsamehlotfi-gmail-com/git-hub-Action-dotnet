using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles
{
    public interface IRoleService
    {
        Task<string> CreateAsync(CreateRolerequest request);
        Task<string> UpdateAsync(UpdateRoleRequest request);
        Task<string> DeleteAsync(string id);
        Task<string> UpdatePermissionsAsync(UpdateRolePermissions request);
        Task<List<RoleDto>> GetRolesAsync(CancellationToken cancellationToken);
        Task<RoleDto> GetRoleByIdAsync(string Id , CancellationToken cancellationToken);
        Task<RoleDto> GetRoleWithPermissionsAsync(string id, CancellationToken cancellationToken);
        Task<bool> DoseItExistsAsync(string name);

    }
}
