using Microsoft.AspNetCore.Http;

namespace ECOM.Shared.Library.Exceptions.HttpStatus
{
	public class NotFoundException(string message = "Not Found") : HttpStatusException(StatusCodes.Status404NotFound, message)
	{
	}
}
