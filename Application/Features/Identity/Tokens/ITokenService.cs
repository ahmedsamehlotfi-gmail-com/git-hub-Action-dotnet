using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Tokens
{
    public interface ITokenService
    {
        // Generate New Token
        Task<TokenResponse> LoginAsync(TokenRequest request);
        // Refresh Token
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
