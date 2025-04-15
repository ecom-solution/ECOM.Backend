namespace ECOM.Shared.Library.Models.Dtos.Common
{
	public class BaseRequest<TModel>
	{
		public required TModel Model { get; set; }
	}
}
