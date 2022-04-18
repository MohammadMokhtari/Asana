using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Domain.Entities.Media;
using Asana.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Asana.Application.Common.Services
{
    public class MediaFileService : IMediaFileService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<MediaFileService> _logger;
        private readonly IGenericRepository<UserMediaFile> _genericRepository;
        private readonly ICurrentUserService _currentUserService;


        public MediaFileService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<MediaFileService> logger,
            IGenericRepository<UserMediaFile> genericRepository,
            ICurrentUserService currentUserService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _genericRepository = genericRepository;
            _currentUserService = currentUserService;
        }


        private const int ThumbnailWidth = 300;
        private const int FullscreenWidth = 1000;

        public async Task ProcessImageAsync(IEnumerable<ProcessImageModel> images, string subFolderName)
        {
            subFolderName ??= "others";

            var totalImage = await _genericRepository.GetEntitiesQuery().CountAsync();

            var tasks = images.Select(image => Task.Run(async () =>
            {
                try
                {
                    using var imageResult = await Image.LoadAsync(image.Content);

                    var folderPath = $"/media/images/{subFolderName}/{totalImage % 1000}";

                    var imageName = $"{Guid.NewGuid()}.jpg";

                    var storagePath = Path.Combine(
                        Directory.GetCurrentDirectory(), $"wwwroot{folderPath}".Replace("/", "\\"));

                    if (!Directory.Exists(storagePath))
                    {
                        Directory.CreateDirectory(storagePath);
                    }

                    await SaveImageAsync(imageResult, storagePath, $"Original{imageName}", imageResult.Width);
                    await SaveImageAsync(imageResult, storagePath, $"FullScreen{imageName}", FullscreenWidth);
                    await SaveImageAsync(imageResult, storagePath, $"Thumbnail{imageName}", ThumbnailWidth);

                    var dbContext = _serviceScopeFactory
                        .CreateAsyncScope()
                            .ServiceProvider
                            .GetRequiredService<IApplicationDbContext>();

                    var userMediaFile = new UserMediaFile();
                    userMediaFile.MediaName = imageName;
                    userMediaFile.Alt = imageName;
                    userMediaFile.CreatedOn = DateTime.Now;
                    userMediaFile.ModifiedOn = DateTime.Now;
                    userMediaFile.FolderPath = folderPath;
                    userMediaFile.IsDelete = false;
                    userMediaFile.Type = MediaType.Image;
                    userMediaFile.UserId = _currentUserService.GuidUserId;

                    dbContext.UserMediaFiles.Add(userMediaFile);

                    await dbContext.SaveChangesAsync(new CancellationToken());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogInformation("Error! Cant Process Images!");
                }

            })).ToList();

            await Task.WhenAll(tasks);

        }

        private async Task SaveImageAsync(Image image, string path, string name, int resizeWidth)
        {
            var width = image.Width;
            var height = image.Height;

            if (width > resizeWidth)
            {
                height = (int)((double)resizeWidth / width * width);
                width = resizeWidth;
            }
            image
            .Mutate(i => i.Resize(new Size(width, height)));

            image.Metadata.ExifProfile = null;

            await image.SaveAsJpegAsync($"{path}/{name}", new JpegEncoder() { Quality = 75, });
        }
    }
}
