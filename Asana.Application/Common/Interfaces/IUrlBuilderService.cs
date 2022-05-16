using Asana.Domain.Entities.Media;

namespace Asana.Application.Common.Interfaces
{
    public interface IUrlBuilderService
    {
        string BuildAbsolutProfilePhotoUrl(UserMediaFile file);

        string BlankProfilePhotoUrl();

        string BuildAbsolutMediaFilePhotoUrl(string folderPath ,string fileName);

    }
}
