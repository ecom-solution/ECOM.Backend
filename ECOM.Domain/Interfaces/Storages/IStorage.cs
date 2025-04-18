using ECOM.Shared.Library.Models.Externals.MinIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.Domain.Interfaces.Storages
{
	public interface IStorage
	{
		/// <summary>
		/// Uploads a file to a specified bucket in MinIO.
		/// </summary>
		Task<UploadFileResponse> UploadAsync(string bucketName, string objectName, byte[] data, string contentType);

		/// <summary>
		/// Downloads a file from a specified bucket in MinIO.
		/// </summary>
		Task<byte[]> DownloadAsync(string bucketName, string objectName);

		/// <summary>
		/// Deletes a file from a specified bucket in MinIO.
		/// </summary>
		Task DeleteAsync(string bucketName, string objectName);

		/// <summary>
		/// Gets a presigned URL for accessing a file.
		/// </summary>
		Task<string> GetPresignedUrlAsync(string bucketName, string objectName, int expiryInSeconds = 3600);

		/// <summary>
		/// Checks if a bucket exists; creates it if not.
		/// </summary>
		Task EnsureBucketExistsAsync(string bucketName, bool isPublic = false);

		/// <summary>
		/// Checks if an object exists in a bucket.
		/// </summary>
		Task<bool> ObjectExistsAsync(string bucketName, string objectName);
	}
}
