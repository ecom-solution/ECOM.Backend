namespace ECOM.Shared.Library.Models.Dtos.Common
{
	public class BaseResponse<TResult>
	{
		public bool IsSuccess { get; set; }
		public int StatusCode { get; set; }
		public string? Message { get; set; }
		public TResult? Data { get; set; }

		public void Failure(string message, int statusCode)
		{
			IsSuccess = false;
			StatusCode = statusCode;
			Message = message ?? string.Empty;
		}

		public void Successful(TResult data)
		{
			IsSuccess = true;
			Data = data;
		}
	}
}
