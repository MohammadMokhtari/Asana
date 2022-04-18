using Asana.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IMediaFileService
    {
        Task ProcessImageAsync(IEnumerable<ProcessImageModel> images, string subFolderName = default);
    }
}
