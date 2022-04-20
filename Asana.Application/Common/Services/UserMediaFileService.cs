using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using Asana.Domain.Entities.Media;
using Asana.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Asana.Application.Common.Services
{
    public class UserMediaFileService : IUserMediaFileService
    {
        private readonly IMediaFileService _mediaFileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenericRepository<UserMediaFile> _genericRepository;
        private readonly IUrlBuilderService _urlBuilderService;
        private readonly ILogger<UserMediaFileService> _logger;


        public UserMediaFileService(IMediaFileService mediaFileService,
            ICurrentUserService currentUserService,
            IGenericRepository<UserMediaFile> genericRepository,
            IUrlBuilderService urlBuilderService,
            ILogger<UserMediaFileService> logger)
        {
            _mediaFileService = mediaFileService;
            _currentUserService = currentUserService;
            _genericRepository = genericRepository;
            _urlBuilderService = urlBuilderService;
            _logger = logger;
        }

        public async Task<Result> DeleteUserPhotoAsync()
        {
            _logger.LogInformation("DeleteUserPhotoAsync Executed");

            try
            {
                var userMediafile = await _genericRepository.GetEntitiesQuery()
                    .Where(m => m.UserId == _currentUserService.GuidUserId)
                     .SingleOrDefaultAsync();

                _genericRepository.DeleteEntity(userMediafile);

                await _genericRepository.SaveChangeAsync();
                _logger.LogInformation("DeleteUserPhotoAsync  to be successful"); ;

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteUserPhoto Faild!");
                return Result.Failure("CAN_NOT_DELETE_USER_POHOTO");
            }
        }

        public async Task<Result> GetPhotoUrl()
        {
            var file = await _genericRepository.GetEntitiesQuery()
                .SingleOrDefaultAsync(m=>m.UserId ==_currentUserService.GuidUserId);

            var url = _urlBuilderService.BuildAbsolutProfilePhotoUrl(file);

            return Result.Success(url);
        }

        public async Task<Result> UpdatUserPhotoAsync(ProcessImageModel image)
        {
            _logger.LogInformation("UpdatUserPhotoAsync Executed");

            try
            {
                var userMediafile = await _genericRepository.GetEntitiesQuery()
                 .Where(m => m.UserId == _currentUserService.GuidUserId)
                    .SingleOrDefaultAsync();

                if (userMediafile != null)
                {
                    _genericRepository.DeleteEntity(userMediafile);
                    await _genericRepository.SaveChangeAsync();
                }

                var totalImage = await _genericRepository.GetEntitiesQuery().CountAsync();

                await _mediaFileService.ProcessImageAsync(new ProcessImageModel[] { image }, totalImage, "profile");

                var mediaFile = await _genericRepository.GetEntitiesQuery()
                    .SingleOrDefaultAsync(m => m.UserId == _currentUserService.GuidUserId);

                var url = _urlBuilderService.BuildAbsolutProfilePhotoUrl(mediaFile);
                var result = new UserPhotoUpdatDto() { PhotoUrl = url };

                _logger.LogInformation("UpdatUserPhoto to be successful");

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdatUserPhoto Faild!");
                return Result.Failure("CAN_NOT_UPDATE_USER_PHOTO");
            }
        }


    }
}
