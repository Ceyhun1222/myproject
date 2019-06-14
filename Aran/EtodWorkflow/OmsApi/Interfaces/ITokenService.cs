using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace OmsApi.Interfaces
{
    public interface ITokenService
    {
         string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration);            
    }
}