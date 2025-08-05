using Application.Exceptions;
using Application.Features.Identity.Roles;
using Azure.Core;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITenantInfo _tenantInfo;

        public RoleService(RoleManager<ApplicationRole> roleManager , UserManager<ApplicationUser> userManager , ApplicationDbContext dbContext , ITenantInfo tenantInfo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _tenantInfo = tenantInfo;
        }
        public async Task<string> CreateAsync(CreateRolerequest request)
        {
            var newRole = new ApplicationRole()
            {
                Name = request.Name,
                Description = request.Description,
            };
            var result = await _roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to Create a role" , GetIdentityResultErrorDescription(result));
            }
            return newRole.Name;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var roleInDb = await _roleManager.FindByIdAsync(id)
            ?? throw new NotFoundException("Role dose not exists");
            if (RoleConstants.IsDefault(roleInDb.Name))
            {
                throw new ConflictException($"not allowed on {roleInDb.Name} role.");
            }
            if((await _userManager.GetUsersInRoleAsync(roleInDb.Name)).Count > 0)
            {
                throw new ConflictException($"not allowed To Delete {roleInDb.Name} role. as is currently assigned to users");
            }
            var result = await _roleManager.DeleteAsync(roleInDb);
            if(!result.Succeeded)
            {
                throw new IdentityException("Failed to Delete Role", GetIdentityResultErrorDescription(result));
            }
            return roleInDb.Name;
        }

        public async Task<bool> DoseItExistsAsync(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }

        public async Task<RoleDto> GetRoleByIdAsync(string Id , CancellationToken cancellationToken)
        {
            var roleInDb = await _dbContext.Roles.SingleOrDefaultAsync(r=>r.Id == Id , cancellationToken)
                ?? throw new NotFoundException("Role dose not exists");
            return roleInDb.Adapt<RoleDto>();
        }

        public async Task<List<RoleDto>> GetRolesAsync(CancellationToken cancellationToken)
        {
            var roleInDb = await _roleManager.Roles.ToListAsync(cancellationToken);
            return  roleInDb.Adapt<List<RoleDto>>();
        }

        public async Task<RoleDto> GetRoleWithPermissionsAsync(string id, CancellationToken cancellationToken)
        {
            var roleDto = await GetRoleByIdAsync(id, cancellationToken);
            
            roleDto.Permission = await _dbContext.RoleClaims.Where(rc=>rc.RoleId == id && rc.ClaimType == ClaimConstants.Permission)
                .Select(rc=>rc.ClaimValue).ToListAsync(cancellationToken);
            return roleDto;
        }

        public async Task<string> UpdateAsync(UpdateRoleRequest request)
        {
           var roleInDb = await _roleManager.FindByIdAsync(request.Id)
                ?? throw new NotFoundException("Role dose not exists");
            if (RoleConstants.IsDefault(roleInDb.Name))
            {
                throw new ConflictException($"Change not allowed on {request} role.");
            }

            roleInDb.Name = request.Name;
            roleInDb.Description = request.Description;
            roleInDb.NormalizedName = request.Name.ToUpperInvariant();

            var result = await _roleManager.UpdateAsync(roleInDb);
            if(!result.Succeeded)
            {
                throw new IdentityException("Failed to Update Role", GetIdentityResultErrorDescription(result));
            }
            return roleInDb.Name;
        }

        public async Task<string> UpdatePermissionsAsync(UpdateRolePermissions request)
        {
            var roleInDb = await _roleManager.FindByIdAsync(request.RoleId)
             ?? throw new NotFoundException("Role dose not exists");
            if(roleInDb.Name == RoleConstants.Admin)
            {
                throw new ConflictException($"Not Allowed to Change Permission on {request} role.");
            }
            if(_tenantInfo.Id != TenancyConstants.Root.Id)
            {
                request.Permissions.RemoveAll(a => a.StartsWith("Permission.Root."));
            }
            var currentClaims = await _roleManager.GetClaimsAsync(roleInDb);


            // remove previously assigned permissions and not current assign per incoming request  
            foreach(var claim in currentClaims.Where(c=> !request.Permissions.Any(p=>p == c.Value)))
            {
                var result = await _roleManager.RemoveClaimAsync(roleInDb, claim);
                if(!result.Succeeded)
                {
                    throw new IdentityException("Failed to Remove  Permission", GetIdentityResultErrorDescription(result));
                }
            }

            // Assign newly Selected Permission
            foreach(var permission in request.Permissions.Where(p=>!currentClaims.Any(c=>c.Value == p)))
            {
                await _dbContext.RoleClaims.AddAsync(new IdentityRoleClaim<string>
                {
                    RoleId = roleInDb.Id,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = permission
                });
                await _dbContext.SaveChangesAsync();
            }

            return "Permission Updated successfully";
        }
        private List<string> GetIdentityResultErrorDescription(IdentityResult identityResult)
        {
            var errorDescription = new List<string>();
            foreach(var error in identityResult.Errors)
            {
                errorDescription.Add(error.Description);
            }
            return errorDescription;
        }
    }
}
