namespace ECOM.App.DTOs.Common
{
	public class BaseResponse<TResult>
	{
		public bool IsSuccess { get; set; }
		public int StatusCode { get; set; }
		public string? Message { get; set; }
		public TResult? Data { get; set; }
		public List<string>? Errors { get; set; }

		public BaseResponse()
		{
		}

		public BaseResponse(TResult data, string? message = null)
		{
			IsSuccess = true;
			StatusCode = 200;
			Data = data;
			Message = message;
			Errors = null;
		}

		public BaseResponse(string message, List<string>? errors = null, int statusCode = 400)
		{
			IsSuccess = false;
			StatusCode = statusCode;
			Message = message;
			Errors = errors;
			Data = default;
		}

		public static BaseResponse<TResult> Success(TResult data, string? message = null)
		{
			return new BaseResponse<TResult>(data, message);
		}

		public static BaseResponse<TResult> Failure(string message, List<string>? errors = null, int statusCode = 400)
		{
			return new BaseResponse<TResult>(message, errors, statusCode);
		}
	}
}
