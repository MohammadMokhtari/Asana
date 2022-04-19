using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IAddressService
    {
        Task<Result> GetAddressesAsync();

        Task<Result> SetDefaultAddressAsync(long addressId);

        Task<Result> CreateAddressAsync(AddressCreateDto addressDto);

        Task<Result> UpdateAddressAsync(AddressUpdateDto addressDto);

        Task<Result> DeleteAddressAsync(long addressId);
        
        Task<Result> GetAllProvinceCityOptionsAsync();
    }
}
