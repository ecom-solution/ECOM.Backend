using ECOM.Shared.Utilities.Enums;

namespace ECOM.App.DTOs.Common
{
	public class PaginatedRequest<TFilterModel>
	{
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public required TFilterModel GlobalFilter { get; set; }
		public ICollection<PaginatedFilterColumn> Columns { get; set; } = [];
	}
	public class PaginatedFilterColumn
	{
		public string Name { get; set; } = string.Empty;
		public PaginationEnums.FilterOperator? Operator { get; set; }
		public string FilterValue { get; set; } = string.Empty;
		public PaginationEnums.SortDirection? SortDirection { get; set; }
	}
}
