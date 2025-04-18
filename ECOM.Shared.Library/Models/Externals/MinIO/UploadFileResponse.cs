namespace ECOM.Shared.Library.Models.Externals.MinIO
{
	public class UploadFileResponse
	{
		public string BucketName { get; set; } = string.Empty;
		public string FileName { get; set; } = string.Empty;
		public string FileUrl { get; set; } = string.Empty;
		public long FileSize { get; set; }
		public string ContentType { get; set; } = string.Empty;
	}
}
