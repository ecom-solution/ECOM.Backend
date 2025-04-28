namespace ECOM.Shared.Library.Models.Dtos.Common
{
    /// <summary>
    /// Represents a paginated response containing a list of records and pagination metadata.
    /// </summary>
    /// <typeparam name="TRecordModel">The type of the records in the response.</typeparam>
    public class PaginatedResponse<TRecordModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResponse{TRecordModel}"/> class.
        /// </summary>
        public PaginatedResponse()
        {
            // Default constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResponse{TRecordModel}"/> class with pagination details and records.
        /// </summary>
        /// <param name="totalRecords">The total number of records.</param>
        /// <param name="totalPages">The total number of pages.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="records">The list of records for the current page.</param>
        public PaginatedResponse(int totalRecords, int totalPages, int pageNumber, int pageSize, List<TRecordModel> records)
        {
            TotalRecords = totalRecords;
            TotalPages = totalPages;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Records = records ?? [];
        }

        /// <summary>
        /// Gets or sets the total number of records matching the query before pagination.
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages based on the <see cref="TotalRecords"/> and <see cref="PageSize"/>.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the current page number of the response.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of records per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the list of records for the current page.
        /// Initialized as an empty list.
        /// </summary>
        public List<TRecordModel> Records { get; set; } = [];
    }
}