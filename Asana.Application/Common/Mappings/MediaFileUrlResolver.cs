using Asana.Application.Common.Interfaces;
using Asana.Application.DTOs;
using Asana.Domain.Entities.Media;
using AutoMapper;

namespace Asana.Application.Common.Mappings
{
    public class MediaFileUrlResolver : IValueResolver<MediaFile, MediaFileDto, string>
    {
        private readonly IUrlBuilderService _urlBuilderService;

        public MediaFileUrlResolver(IUrlBuilderService urlBuilderService)
        {
            _urlBuilderService = urlBuilderService;
        }

        public string Resolve(MediaFile source, MediaFileDto destination, string destMember, ResolutionContext context)
        {
            return _urlBuilderService.BuildAbsolutMediaFilePhotoUrl(source.FolderPath, source.MediaName);
        }
    }
}
