using Microsoft.AspNetCore.Http;

namespace ECOM.Shared.Utilities.Exceptions.HttpStatusException
{
	public class UnauthorizedException(string message = "Unauthorized") : Base.HttpStatusException(StatusCodes.Status401Unauthorized, message)
	{
	}
}
