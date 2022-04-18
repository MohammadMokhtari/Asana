using Asana.Application.Common.Interfaces;
using Asana.Domain.Entities.Media;

namespace Asana.Application.Common.Services
{
    public class UrlBuilderService : IUrlBuilderService
    {
        private readonly IHostService _hostService;

        public UrlBuilderService(IHostService hostService) => _hostService = hostService;

        public string BlankProfilePhotoUrl()
        {
            return $"https://{_hostService.HostName}/media/images/profile/blank.jpg";
        }


        public string BuildAbsolutProfilePhotoUrl(UserMediaFile file)
        {
            if (file is null)
                return BlankProfilePhotoUrl();

            return $"https://{_hostService.HostName}{file.FolderPath}/Thumbnail{file.MediaName}";
        }
    }
}
