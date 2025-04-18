using Microsoft.AspNetCore.Http;

namespace ECOM.Shared.Library.Exceptions.HttpStatus
{
	public class BadRequestException(string message = "Bad Request") : HttpStatusException(StatusCodes.Status400BadRequest, message)
	{
	}
}
