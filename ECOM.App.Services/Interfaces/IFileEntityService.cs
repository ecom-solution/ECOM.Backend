using ECOM.Shared.Library.Models.Externals.MinIO;

namespace ECOM.App.Services.Interfaces
{
	public interface IFileEntityService
    {
        /// <summary>
        /// Insert FileEntity to DB after upload file to storage
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        Task InsertFileEntityAsync(UploadFileResponse uploadFile);
    }
}
