using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Infrastructure.Identity.Constants.PermissionConstants;

namespace Infrastructure.Persistence.DbInitializers
{
    public class ApplicationDbInitializer
    {
        public ApplicationDbInitializer(ABCSchoolTenantInfo tenant, RoleManager<ApplicationRole> roleManager,   UserManager<ApplicationUser> userManager
            ,ApplicationDbContext applicationDbContext)

        {
            _tenant = tenant;
            _roleManager = roleManager;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }
        private readonly ABCSchoolTenantInfo _tenant;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public async Task InitializeDatabaseAsync(CancellationToken cancellationToken) 
        {
            if (_applicationDbContext.Database.GetMigrations().Any())
            {
                if((await _applicationDbContext.Database.GetAppliedMigrationsAsync(cancellationToken)).Any())
                {
                    await _applicationDbContext.Database.MigrateAsync(cancellationToken);
                }
            }

            if(await _applicationDbContext.Database.CanConnectAsync(cancellationToken))
            {
                await InitializeDefaultRolesAsync(cancellationToken);
                await InitializeAdminUserAsync();
            }
        }

        private async Task InitializeDefaultRolesAsync( CancellationToken cancellationToken)
        {
            foreach (var roleName in RoleConstants.DefaultRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken)
                    is not ApplicationRole existingRole)
                {
                    existingRole = new ApplicationRole()
                    {
                        Name = roleName,
                        Description = $"{roleName} Role",
                    };
                    await _roleManager.CreateAsync(existingRole);
                }

                // Assign permissions to the newly added role
                if (roleName == RoleConstants.Basic)
                {
                    await AssignPermissionsToRole( SchoolPermissions.Basic, existingRole, cancellationToken);
                }
                else if (roleName == RoleConstants.Admin)
                {
                    await AssignPermissionsToRole( SchoolPermissions.Admin, existingRole, cancellationToken);
                    if(_tenant.Id == TenancyConstants.Root.Id)
                    {
                        await AssignPermissionsToRole(SchoolPermissions.Root, existingRole, cancellationToken);
                    }
                }
            }
        }
        private async Task AssignPermissionsToRole(
       IReadOnlyList<SchoolPermission> rolePermissions,
       ApplicationRole currentRole,
       CancellationToken cancellationToken)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(currentRole);

            foreach (var rolePermission in rolePermissions)
            {
                if (!currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == rolePermission.Name))
                {
                  await  _applicationDbContext.RoleClaims.AddAsync(new IdentityRoleClaim<string>
                    {
                        RoleId = currentRole.Id,
                        ClaimType = ClaimConstants.Permission,
                        ClaimValue = rolePermission.Name
                    } , cancellationToken);
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                }
            }

        }

        private async Task InitializeAdminUserAsync()
        {
            if (string.IsNullOrWhiteSpace(_tenant.AdminEmail))
            {
                return;
            }

            var adminUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == _tenant.AdminEmail, CancellationToken.None);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    FirstName = TenancyConstants.FirstName,
                    LastName = TenancyConstants.LastName,
                    Email = _tenant.AdminEmail,
                    UserName = _tenant.AdminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = _tenant.AdminEmail.ToUpperInvariant(),
                    NormalizedUserName = _tenant.AdminEmail.ToUpperInvariant(),
                    IsActive = true,
                };

                var passwordHasher = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, TenancyConstants.DefaultPassword);

                await _userManager.CreateAsync(adminUser);
            }

            if (!await _userManager.IsInRoleAsync(adminUser, RoleConstants.Admin))
            {
                await _userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            }
        }


    }
}
