using Application.Exceptions;
using Application.Features.Identity.Users;
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
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITenantInfo _tenantInfo;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager , SignInManager<ApplicationUser> signInManager
            ,ApplicationDbContext dbContext , ITenantInfo tenantInfo)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _tenantInfo = tenantInfo;
        }

        public async Task<string> ActivateOrDeactivateAsync(string userId, bool activation)
        {
            var userInDb = await GetUserAsync(userId);
            userInDb.IsActive = activation;
            await _userManager.UpdateAsync(userInDb);
            return userId;
        }

        public async Task<string> AssignRolesAsync(string userId, userRoleRequest request)
        {
            var userInDb = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync() ?? throw new NotFoundException("User Dose'nt Found");
            if(await _userManager.IsInRoleAsync(userInDb , RoleConstants.Admin) && request.userRoles.Any(ur=>!ur.IsAssigned && ur.Name == RoleConstants.Admin))
            {
                var adminUsersCount = (await _userManager.GetUsersInRoleAsync(RoleConstants.Admin)).Count();
                if(userInDb.Email == TenancyConstants.Root.Email)
                {
                    if(_tenantInfo.Id == TenancyConstants.Root.Id)
                    {
                        throw new ConflictException("Not allowed to remove Admin Role For a Root Tenant user");
                    }
                }
                else if(adminUsersCount <= 2)
                {
                    throw new ConflictException("Tenant should have at least three admin users");
                }
            }
            foreach (var userRole in request.userRoles)
            {
                if(await _roleManager.FindByIdAsync(userRole.RoleId) is not null)
                {
                    if(userRole.IsAssigned)
                    {
                        if(!await _userManager.IsInRoleAsync(userInDb , userRole.Name))
                        {
                            await _userManager.AddToRoleAsync(userInDb, userRole.Name);
                        }
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(userInDb, userRole.Name);
                    }
                }
            }
            return userInDb.Id;
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var userInDb = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync() ?? throw new NotFoundException("User Dose'nt Found");

            if(request.NewPassword != request.ConfirmPassword)
            {
                throw new ConflictException("Password do not match");
            }

            var result = await _userManager.ChangePasswordAsync(userInDb , request.CurrentPassword, request.NewPassword);
            if(!result.Succeeded)
            {
                throw new IdentityException("Failed to change password", GetIdentityResultErrorDescription(result));
            }
            return userInDb.Id;
 
        }

        public async Task<string> CreateUserAsync(CreateUserRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ConflictException("Passwords do not match.");
            }

            if (await IsEmailTakenAsync(request.Email))
            {
                throw new ConflictException("Email already taken.");
            }
            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email.Split('@')[0],
                IsActive = request.IsActive,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to create user.", GetIdentityResultErrorDescription(result));
            }

            return newUser.Id;
        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            var userInDb = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync() ?? throw new NotFoundException("User Dose'nt Found");
            _dbContext.Users.Remove(userInDb);
            await _dbContext.SaveChangesAsync();
            return userInDb.Id;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var userInDb = await GetUserAsync(userId , cancellationToken);
            return userInDb.Adapt<UserDto>();
        }

        public async Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
        {
          var userRoles =  new List<UserRoleDto>(); 
            var userInDb = await GetUserAsync(userId , cancellationToken);
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);
            foreach (var role in roles)
            {
                userRoles.Add(new UserRoleDto
                {
                    RoleId = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    IsAssigned = await _userManager.IsInRoleAsync(userInDb, role.Name)
                });
            }
            return userRoles;
        }

        public async Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {
           var usersInDb = await _userManager.Users.AsNoTracking().ToListAsync();
            return usersInDb.Adapt<List<UserDto>>();
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
          return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<string> UpdateUserAsync(UpdateUserRequest request)
        {
            var userInDb = await GetUserAsync(request.Id);
            userInDb.FirstName = request.FirstName;
            userInDb.LastName = request.LastName;
            userInDb.PhoneNumber = request.PhoneNumber;
            var result = await _userManager.UpdateAsync(userInDb);
            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to Update user", GetIdentityResultErrorDescription(result));
            }
            await _signInManager.RefreshSignInAsync(userInDb);
            return userInDb.Id;
        }
        public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            var userInDb = await GetUserAsync(userId, cancellationToken);
            var userRoles = await _userManager.GetRolesAsync(userInDb);
            var permissions = new List<string>();
            foreach (var role in await _roleManager.Roles.Where(r => userRoles.Contains(r.Name)).ToListAsync(cancellationToken))
            {
                permissions.AddRange(await _dbContext.RoleClaims.Where(rc => rc.RoleId == role.Id && 
                rc.ClaimType == ClaimConstants.Permission)
                    .Select(rc => rc.ClaimValue).ToListAsync(cancellationToken));
            } 
            return permissions.Distinct().ToList();
        }
        public async Task<bool> IsPermissionAssignedAsync(string userId, string permission, CancellationToken cancellationToken)
       => (await GetPermissionsAsync(userId, cancellationToken)).Contains(permission);
        private async Task<ApplicationUser> GetUserAsync(string userId, CancellationToken cancellationToken = default)
     => await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync()
     ?? throw new NotFoundException("User Dose'nt Found");

        private List<string> GetIdentityResultErrorDescription(IdentityResult identityResult)
        {
            var errorDescription = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorDescription.Add(error.Description);
            }
            return errorDescription;
        }

    }
}
