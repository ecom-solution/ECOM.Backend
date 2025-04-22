namespace ECOM.Shared.Library.Models.Dtos.Common
{
	public class PaginatedResponse<TRecordModel>
	{
		public int TotalRecords { get; set; }
		public int TotalPages { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public List<TRecordModel> Records { get; set; } = [];
	}
}
