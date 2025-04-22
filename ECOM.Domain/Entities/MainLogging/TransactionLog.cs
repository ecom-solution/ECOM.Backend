namespace ECOM.Domain.Entities.MainLogging
{
	public class TransactionLog : BaseEntity
    {
        public TransactionLog() { }

		public DateTime CreatedAt_Utc { get; set; }
		public string Level { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public string? Exception { get; set; }
		public string? Properties { get; set; }
		public string? CallerMethod { get; set; }
		public string? CallerFileName { get; set; }
		public int? CallerLineNumber { get; set; }
		public string? IpAddress { get; set; }
		public Guid? UserId { get; set; }
		public Guid TransactionId { get; set; }
	}
}
