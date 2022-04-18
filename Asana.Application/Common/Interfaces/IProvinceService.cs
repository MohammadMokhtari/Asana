using Asana.Application.Common.Models;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IProvinceService
    {
        Task<Result> AllProvinceOptionAsync();

    }
}
