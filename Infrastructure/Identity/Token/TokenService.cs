using Application.Exceptions;
using Application.Features.Identity.Tokens;
using Infrastructure.Identity.Auth.Jwt;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Token
{
    public class TokenService(UserManager<ApplicationUser> userManager , ABCSchoolTenantInfo tenant , IOptions<JwtSettings> jwtSettings) : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ABCSchoolTenantInfo _tenant = tenant;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<TokenResponse> LoginAsync(TokenRequest request)
        {
            var userInDb = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new UnauthorizedException("Authentication Not Successful"); 
            if(userInDb == null)
            {
                throw new UnauthorizedException("Incorrect Username or password");
            }
            if(! await _userManager.CheckPasswordAsync(userInDb , request.Password))
            {
                throw new UnauthorizedException("User Not Active . please Contact the admin");

            }
            if (_tenant.Id != TenancyConstants.Root.Id)
            {
                if(_tenant.ValidUpTo < DateTime.UtcNow)
                {
                    throw new UnauthorizedException("");

                }
            }
            // Generate Token
            return await GenerateTokenAndUpdateUserAsync(userInDb);
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var userprincipal = GetClaimsPrincipalFromExpiredToken(request.CurrentJwtToken);
            var userEmail = userprincipal.GetEmail();
            var userInDb = await _userManager.FindByEmailAsync(userEmail)
            ?? throw new UnauthorizedException("Authentication Not Successful");
            return await GenerateTokenAndUpdateUserAsync(userInDb);
        }

        private ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string expiredtoken)
        {
            var tkValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = ClaimTypes.Role,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principalpal = tokenHandler.ValidateToken(expiredtoken, tkValidationParams , out var securityToken);
           if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256 , StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedException("Invalid token. Failed to create refresh token");
            }
           return principalpal;
        }

        private async Task<TokenResponse> GenerateTokenAndUpdateUserAsync(ApplicationUser user)
        {
            string newToken = GenerateJwt(user);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryTimeInDays);
            await _userManager.UpdateAsync(user);
            return new()
            {
                JwtToken = newToken,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryDate = user.RefreshTokenExpiryTime
            };
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateJwt(ApplicationUser user)
        {
            return GenerateEncryptedToken(GetSigningCredentials(), GetUserClaims(user));
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryTimeInMinutes),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private IEnumerable<Claim> GetUserClaims(ApplicationUser user)
        {
            return
            [
                new (ClaimTypes.NameIdentifier , user.Id),
                new (ClaimTypes.Email , user.Email),
                new (ClaimTypes.Name , user.FirstName),
                new (ClaimTypes.Surname , user.LastName),
                new (ClaimConstants.Tenant , _tenant.Name),
                new (ClaimTypes.MobilePhone , user.PhoneNumber ?? string.Empty),
                ];
        }

    }
}
