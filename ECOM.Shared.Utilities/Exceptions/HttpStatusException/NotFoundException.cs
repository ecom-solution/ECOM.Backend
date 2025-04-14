using Microsoft.AspNetCore.Http;

namespace ECOM.Shared.Utilities.Exceptions.HttpStatusException
{
	public class NotFoundException(string message = "Not Found") : Base.HttpStatusException(StatusCodes.Status404NotFound, message)
	{
	}
}
