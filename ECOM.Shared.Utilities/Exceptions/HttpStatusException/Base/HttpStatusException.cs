namespace ECOM.Shared.Utilities.Exceptions.HttpStatusException.Base
{
	public class HttpStatusException(int statusCode, string message) : Exception(message)
	{
		public int StatusCode { get; } = statusCode;
	}
}
