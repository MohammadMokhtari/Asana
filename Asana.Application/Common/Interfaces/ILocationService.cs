using Asana.Application.Common.Models;
using System;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface ILocationService
    {
        Task<Result> GetLocationsAsync(Guid userId);

        Task<Result> SetDefaultLocationAsync(long locationId, Guid userId);
    }
}
