using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ECOM.Infrastructure.Implementations.Notifications.SignalR.Providers
{
	public class UserIdProvider : IUserIdProvider
	{
		public string? GetUserId(HubConnectionContext connection)
		{
			// Use ClaimTypes.NameIdentifier or "sub" depending on your JWT structure
			return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
