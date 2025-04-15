using AutoMapper;
using ECOM.App.Services.Common;
using ECOM.App.Services.Interfaces;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.Repositories;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Shared.Library.Models.Externals.MinIO;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.Options;

namespace ECOM.App.Services.Implementations
{
	public class FileEntityService(
        IMapper mapper, IEcomLogger logger,
        IOptions<AppSettings> appSettings,
        IUnitOfWork<MainDbContext> mainUnitOfWork)
        : BaseService(mapper, logger, appSettings, mainUnitOfWork), IFileEntityService
    {
        public async Task InsertFileEntityAsync(UploadFileResponse uploadFile)
        {
            try
            {
                var fileEntity = new FileEntity()
                {
                    FileName = uploadFile.FileName,
                    ContentType = uploadFile.ContentType,
                    FileUrl = uploadFile.FileUrl,
                    FileSize = uploadFile.FileSize,
                    BucketName = uploadFile.BucketName
                };

                await _mainUnitOfWork.Repository<FileEntity>().InsertAsync(fileEntity);
                await _mainUnitOfWork.SaveChangesAsync();

                _logger.Information($"Add new FileEntity(URL={fileEntity.FileUrl}) successfully!");
            }
            catch (Exception ex)
            {
                _logger.Error($"Add new FileEntity failed!", ex);
                throw;
            }

        }
    }
}
