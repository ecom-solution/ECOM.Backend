namespace ECOM.App.Exceptions
{
	public class HttpStatusException(int statusCode, string message) : Exception(message)
	{
		public int StatusCode { get; } = statusCode;
	}
}
