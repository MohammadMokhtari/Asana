using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using Asana.Domain.Entities.Media;
using Asana.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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


        public UserMediaFileService(IMediaFileService mediaFileService,
            ICurrentUserService currentUserService,
            IGenericRepository<UserMediaFile> genericRepository,
            IUrlBuilderService urlBuilderService)
        {
            _mediaFileService = mediaFileService;
            _currentUserService = currentUserService;
            _genericRepository = genericRepository;
            _urlBuilderService = urlBuilderService;
        }

        public async Task<Result> DeleteUserPhotoAsync()
        {
            try
            {
                var userMediafile = await _genericRepository.GetEntitiesQuery()
                    .Where(m => m.UserId == _currentUserService.GuidUserId)
                     .SingleOrDefaultAsync();

                _genericRepository.DeleteEntity(userMediafile);

                await _genericRepository.SaveChangeAsync();

                return Result.Success();
            }
            catch (Exception)
            {
                //TODO : Log Error
                return Result.Failure("Error! Can not Delete User Photo");
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

                await _mediaFileService.ProcessImageAsync(new ProcessImageModel[] { image }, "profile");

                var mediaFile = await _genericRepository.GetEntitiesQuery()
                    .SingleOrDefaultAsync(m => m.UserId == _currentUserService.GuidUserId);

                var url = _urlBuilderService.BuildAbsolutProfilePhotoUrl(mediaFile);
                var result = new UserPhotoUpdatDto() { PhotoUrl = url };

                return Result.Success(result);
            }
            catch (Exception)
            {
                //TODO : Log Error
                return Result.Failure("Error! Can not Update User Photo");
            }
        }


    }
}
