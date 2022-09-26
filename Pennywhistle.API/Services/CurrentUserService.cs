using Microsoft.AspNetCore.Http;
using Pennywhistle.Application.Common.Contracts;
using System.Security.Claims;

namespace Pennywhistle.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserId { get; }

    }
}
