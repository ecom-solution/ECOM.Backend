using ECOM.Shared.Library.Enums.Common;
using ECOM.Shared.Library.Models.Dtos.Common;
using System.Linq.Expressions;

namespace ECOM.App.Services.Extensions
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> ApplyFiltersAndSorts<T>(
			this IQueryable<T> query,
			ICollection<PaginatedFilterColumn> filterColumns)
		{
			if (filterColumns == null || filterColumns.Count == 0)
				return query;

			var parameter = Expression.Parameter(typeof(T), "x");

			foreach (var column in filterColumns)
			{
				if (!string.IsNullOrWhiteSpace(column.FilterValue) && column.Operator.HasValue)
				{
					var property = Expression.Property(parameter, column.Name);
					var propertyType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;

					var filterValue = Convert.ChangeType(column.FilterValue, propertyType);
					var constant = Expression.Constant(filterValue, propertyType);

					Expression? predicate = column.Operator switch
					{
						PaginationFilterOperator.Equals => Expression.Equal(property, constant),
						PaginationFilterOperator.NotEquals => Expression.NotEqual(property, constant),
						PaginationFilterOperator.GreaterThan => Expression.GreaterThan(property, constant),
						PaginationFilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, constant),
						PaginationFilterOperator.LessThan => Expression.LessThan(property, constant),
						PaginationFilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(property, constant),
						PaginationFilterOperator.Contains when propertyType == typeof(string) =>
							Expression.Call(property, nameof(string.Contains), null, constant),
						PaginationFilterOperator.StartsWith when propertyType == typeof(string) =>
							Expression.Call(property, nameof(string.StartsWith), null, constant),
						PaginationFilterOperator.EndsWith when propertyType == typeof(string) =>
							Expression.Call(property, nameof(string.EndsWith), null, constant),
						_ => null
					};

					if (predicate != null)
					{
						var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
						query = query.Where(lambda);
					}
				}

				if (column.SortDirection.HasValue)
				{
					query = ApplyOrdering(query, column.Name, column.SortDirection.Value == PaginationSortDirection.Descending);
				}
			}

			return query;
		}

		private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string propertyName, bool descending)
		{
			var parameter = Expression.Parameter(typeof(T), "x");
			var property = Expression.Property(parameter, propertyName);
			var lambda = Expression.Lambda(property, parameter);

			string methodName = descending ? "OrderByDescending" : "OrderBy";

			var result = Expression.Call(
				typeof(Queryable),
				methodName,
				[typeof(T), property.Type],
				query.Expression,
				Expression.Quote(lambda));

			return query.Provider.CreateQuery<T>(result);
		}
	}
}
