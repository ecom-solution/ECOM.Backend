using ECOM.Shared.Library.Enums.Common;

namespace ECOM.Shared.Library.Models.Dtos.Common
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
		public PaginationFilterOperator? Operator { get; set; }
		public string FilterValue { get; set; } = string.Empty;
		public PaginationSortDirection? SortDirection { get; set; }
	}
}
