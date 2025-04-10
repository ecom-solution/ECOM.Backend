using ECOM.App.DTOs.Modules.File;
using ECOM.Infrastructure.External.Services.Common;
using ECOM.Infrastructure.External.Services.Interfaces;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Shared.Utilities.Helpers;
using ECOM.Shared.Utilities.Settings;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace ECOM.Infrastructure.External.Services.Implementations
{
	public class MinIOStorageService(
		IEcomLogger logger,
		IOptions<AppSettings> appSettings,
		IMinioClient client) 
		: BaseExternalService(logger, appSettings), IMinIOStorageService
	{
		private readonly IMinioClient _client = client;

		public async Task<UploadFileResult> UploadAsync(string bucketName, string objectName, byte[] data, string contentType)
		{
			using var ms = new MemoryStream(data);
			return await UploadAsync(bucketName, objectName, ms, contentType);
		}
				
		public async Task<UploadFileResult> UploadAsync(string bucketName, string objectName, Stream stream, string contentType)
		{
			await EnsureBucketExistsAsync(bucketName, true);

			return await RetryHelper.RetryAsync(async () =>
			{
				await _client.PutObjectAsync(new PutObjectArgs()
				.WithBucket(bucketName)
				.WithObject(objectName)
				.WithStreamData(stream)
				.WithObjectSize(stream.Length)
				.WithContentType(contentType));

				var fileUrl = $"{_client.Config.BaseUrl}/{bucketName}/{objectName}";

				var fileSize = stream.Length;

				return new UploadFileResult
				{
					BucketName = bucketName,
					FileName = objectName,
					FileUrl = fileUrl,
					FileSize = fileSize,
					ContentType = contentType
				};
			}, TimeSpan.FromSeconds(_appSettings.DbContext.Retry.IntervalInSeconds), _appSettings.DbContext.Retry.MaxAttemptCount);			
		}

		public async Task<byte[]> DownloadAsync(string bucketName, string objectName)
		{
			using var ms = new MemoryStream();
			await _client.GetObjectAsync(new GetObjectArgs()
				.WithBucket(bucketName)
				.WithObject(objectName)
				.WithCallbackStream(stream => stream.CopyTo(ms))
			);

			return ms.ToArray();
		}

		public async Task DeleteAsync(string bucketName, string objectName)
		{
			await _client.RemoveObjectAsync(new RemoveObjectArgs()
				.WithBucket(bucketName)
				.WithObject(objectName));
		}

		public async Task<string> GetPresignedUrlAsync(string bucketName, string objectName, int expiryInSeconds = 3600)
		{
			return await _client.PresignedGetObjectAsync(new PresignedGetObjectArgs()
				.WithBucket(bucketName)
				.WithObject(objectName)
				.WithExpiry(expiryInSeconds));
		}

		public async Task EnsureBucketExistsAsync(string bucketName, bool isPublic = false)
		{
			bool exists = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
			if (!exists)
			{
				await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));

				if (isPublic)
				{
					var policyJson = GeneratePublicReadPolicy(bucketName);

					await _client.SetPolicyAsync(new SetPolicyArgs()
						.WithBucket(bucketName)
						.WithPolicy(policyJson));
				}
			}
		}

		public async Task<bool> ObjectExistsAsync(string bucketName, string objectName)
		{
			try
			{
				await _client.StatObjectAsync(new StatObjectArgs()
					.WithBucket(bucketName)
					.WithObject(objectName));
				return true;
			}
			catch (Minio.Exceptions.ObjectNotFoundException)
			{
				return false;
			}
		}

		private static string GeneratePublicReadPolicy(string bucketName)
		{
			return @$"{{
					""Version"": ""2012-10-17"",
					""Statement"": [
						{{
							""Effect"": ""Allow"",
							""Principal"": {{""AWS"": [""*""]}},
							""Action"": [""s3:GetObject""],
							""Resource"": [""arn:aws:s3:::{bucketName}/*""]
						}}
					]
				}}";
		}
	}
}
