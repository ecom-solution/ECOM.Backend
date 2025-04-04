using Microsoft.AspNetCore.Http;

namespace ECOM.App.Exceptions.HttpStatus
{
	public class UnauthorizedException(string message = "Unauthorized") : HttpStatusException(StatusCodes.Status401Unauthorized, message)
	{
	}
}
