using Microsoft.AspNetCore.Http;

namespace ECOM.Shared.Utilities.Exceptions.HttpStatusException
{
	public class BadRequestException(string message = "Bad Request") : Base.HttpStatusException(StatusCodes.Status400BadRequest, message)
	{
	}
}
