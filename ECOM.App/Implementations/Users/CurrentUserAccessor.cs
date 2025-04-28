using System.IdentityModel.Tokens.Jwt;
using ECOM.App.Interfaces.Users;
using Microsoft.AspNetCore.Http;

namespace ECOM.App.Implementations.Users
{
    public class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public Guid Id 
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = _httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                    if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                    {
                        return userId;
                    }
                }                
                return Guid.Empty;
            }
        }

        public string UserName
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    return _httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value ?? string.Empty;
                }
                return string.Empty;
            }
        }

        public string Email
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    return _httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty;
                }
                return string.Empty;
            }
        }
    }
}
