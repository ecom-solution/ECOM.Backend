namespace ECOM.Domain.Entities.Main
{
	public class FileEntity : AuditableEntity
    {
		public FileEntity() { }

		public string FileName { get; set; } = string.Empty;
		public string FilePath { get; set; } = string.Empty;
		public string ContentType { get; set; } = string.Empty;
		public long FileSize { get; set; }
		public string? StorageBucket { get; set; }
		public bool IsPublic { get; set; } = false;

		public virtual ICollection<ApplicationUser>? Users { get; set; }
		public virtual ICollection<Language>? Languages { get; set; }
	}
}
