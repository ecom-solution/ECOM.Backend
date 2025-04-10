namespace ECOM.Shared.Utilities.Enums
{
	public static class PaginationEnums
	{
		public enum FilterOperator
		{
			Equals,
			NotEquals,
			Contains,
			StartsWith,
			EndsWith,
			GreaterThan,
			GreaterThanOrEqual,
			LessThan,
			LessThanOrEqual
		}

		public enum SortDirection
		{
			Ascending,
			Descending
		}
	}
}
