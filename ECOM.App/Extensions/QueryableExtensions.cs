using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using ECOM.Shared.Library.Enums.Common;
using ECOM.Shared.Library.Models.Dtos.Common;

namespace ECOM.App.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies filtering, sorting, and pagination to an <see cref="IQueryable{T}"/>
        /// based on the provided <see cref="PaginatedRequest{TFilterModel}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the queryable.</typeparam>
        /// <typeparam name="TFilterModel">The type of the global filter model.</typeparam>
        /// <param name="source">The <see cref="IQueryable{T}"/> to process.</param>
        /// <param name="request">The <see cref="PaginatedRequest{TFilterModel}"/> containing filtering, sorting, and pagination criteria.</param>
        /// <returns>A new <see cref="IQueryable{T}"/> with the applied filtering, sorting, and pagination.</returns>
        public static IQueryable<T> ApplyPaginatedRequest<T, TFilterModel>(this IQueryable<T> source, PaginatedRequest<TFilterModel> request)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (request == null)
            {
                return source;
            }

            source = source.ApplyFilteringByColumns(request.Columns)
                           .ApplySortingByColumns(request.Columns)
                           .ApplyPagination(request.PageNumber, request.PageSize);

            return source;
        }

        /// <summary>
        /// Applies column-specific filtering to an <see cref="IQueryable{T}"/> based on the provided filter columns.
        /// Throws an exception for unsupported filter operators.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the queryable.</typeparam>
        /// <param name="source">The <see cref="IQueryable{T}"/> to filter.</param>
        /// <param name="columns">The collection of <see cref="PaginatedFilterColumn"/> containing filtering criteria.</param>
        /// <returns>A new <see cref="IQueryable{T}"/> with the column-specific filters applied.</returns>
        /// <exception cref="NotSupportedException">Thrown when an unsupported <see cref="PaginationFilterOperator"/> is encountered.</exception>
        public static IQueryable<T> ApplyFilteringByColumns<T>(this IQueryable<T> source, ICollection<PaginatedFilterColumn> columns)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (columns == null || !columns.Any(c => c.Operator.HasValue && !string.IsNullOrEmpty(c.FilterValue) && !string.IsNullOrEmpty(c.Name)))
            {
                return source;
            }

            var parameter = Expression.Parameter(typeof(T), "entity");
            Expression? combinedExpression = null;

            foreach (var column in columns.Where(c => c.Operator.HasValue && !string.IsNullOrEmpty(c.FilterValue) && !string.IsNullOrEmpty(c.Name)))
            {
                var property = typeof(T).GetProperty(column.Name);
                if (property == null) continue;

                var propertyAccess = Expression.Property(parameter, property);
                var constant = Expression.Constant(Convert.ChangeType(column.FilterValue, property.PropertyType));

                Expression filterExpression = column.Operator switch
                {
                    PaginationFilterOperator.Equals => Expression.Equal(propertyAccess, constant),
                    PaginationFilterOperator.NotEquals => Expression.NotEqual(propertyAccess, constant),
                    PaginationFilterOperator.Contains when property.PropertyType == typeof(string) =>
                        CreateMethodCallExpression(propertyAccess, "Contains", constant),
                    PaginationFilterOperator.StartsWith when property.PropertyType == typeof(string) =>
                        CreateMethodCallExpression(propertyAccess, "StartsWith", constant),
                    PaginationFilterOperator.EndsWith when property.PropertyType == typeof(string) =>
                        CreateMethodCallExpression(propertyAccess, "EndsWith", constant),
                    PaginationFilterOperator.GreaterThan => Expression.GreaterThan(propertyAccess, constant),
                    PaginationFilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(propertyAccess, constant),
                    PaginationFilterOperator.LessThan => Expression.LessThan(propertyAccess, constant),
                    PaginationFilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(propertyAccess, constant),
                    _ => throw new NotSupportedException($"Unsupported filter operator: {column.Operator}")
                };

                if (filterExpression != null)
                {
                    combinedExpression = combinedExpression == null
                        ? filterExpression
                        : Expression.AndAlso(combinedExpression, filterExpression); // Combine filters with AND
                }
            }

            if (combinedExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                source = source.Where(lambda);
            }

            return source;
        }

        /// <summary>
        /// Applies sorting to an <see cref="IQueryable{T}"/> based on the specified sort columns.
        /// Uses Dynamic LINQ for more concise sorting logic.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the queryable.</typeparam>
        /// <param name="source">The <see cref="IQueryable{T}"/> to sort.</param>
        /// <param name="columns">The collection of <see cref="PaginatedFilterColumn"/> containing sorting criteria.</param>
        /// <returns>A new <see cref="IQueryable{T}"/> with the sorting applied.</returns>
        public static IQueryable<T> ApplySortingByColumns<T>(this IQueryable<T> source, ICollection<PaginatedFilterColumn> columns)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (columns == null || !columns.Any(c => c.SortDirection.HasValue && !string.IsNullOrEmpty(c.Name)))
            {
                return source;
            }

            var orderByClauses = columns
                .Where(c => c.SortDirection.HasValue && !string.IsNullOrEmpty(c.Name))
                .Select(c => $"{c.Name} {(c.SortDirection == PaginationSortDirection.Descending ? "desc" : "asc")}");

            if (orderByClauses.Any())
            {
                source = source.OrderBy(string.Join(", ", orderByClauses));
            }

            return source;
        }

        /// <summary>
        /// Applies pagination to an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the queryable.</typeparam>
        /// <param name="source">The <see cref="IQueryable{T}"/> to paginate.</param>
        /// <param name="pageNumber">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A new <see cref="IQueryable{T}"/> representing the requested page.</returns>
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10; // Default page size
            }

            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Creates a <see cref="MethodCallExpression"/> for calling a <see cref="string"/> method
        /// with a string argument and <see cref="StringComparison.OrdinalIgnoreCase"/>.
        /// This helper method is used for building expression trees for string operations like
        /// Contains, StartsWith, and EndsWith with case-insensitive comparison.
        /// </summary>
        /// <param name="propertyAccess">An <see cref="Expression"/> representing access to the string property.</param>
        /// <param name="methodName">The name of the <see cref="string"/> method to call (e.g., "Contains").</param>
        /// <param name="constant">A <see cref="ConstantExpression"/> representing the string argument for the method.</param>
        /// <returns>A <see cref="MethodCallExpression"/> representing the method call.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the specified <paramref name="methodName"/>
        /// is not found on the <see cref="string"/> type with the expected signature.</exception>
        private static MethodCallExpression CreateMethodCallExpression(Expression propertyAccess, string methodName, ConstantExpression constant)
        {
            var method = typeof(string).GetMethod(methodName, [typeof(string), typeof(StringComparison)]);
            return method == null
                ? throw new InvalidOperationException($"Method '{methodName}' not found.")
                : Expression.Call(propertyAccess, method, constant, Expression.Constant(StringComparison.OrdinalIgnoreCase));
        }
    }
}