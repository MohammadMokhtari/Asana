using Asana.Application.Common.Models;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IUserMediaFileService
    {
        Task<Result> UpdatUserPhotoAsync(ProcessImageModel image);

        Task<Result> DeleteUserPhotoAsync();

        Task<Result> GetPhotoUrl();
    }
}
