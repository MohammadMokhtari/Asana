using System.Collections.Generic;
using Asana.Application.Common.Models;
using Asana.Application.DTOs;
using System.Threading.Tasks;

namespace Asana.Application.Common.Interfaces
{
    public interface IAddressService
    {
        Task<(Result result,IEnumerable<AddressDto> addressDtos)> GetAddressesAsync();

        Task<(Result result , AddressDto addressDto)> SetDefaultAddressAsync(long addressId);

        Task<Result> CreateAddressAsync(AddressCreateDto addressDto);

        Task<Result> UpdateAddressAsync(AddressUpdateDto addressDto);

        Task<Result> DeleteAddressAsync(long addressId);
        
        Task<(Result result,IEnumerable<ProvinceDto> provinceDtos)> AllProvinceAsync();

        Task<(Result result,IEnumerable<CityDto> cityDtos)> AllCityAsync();

        Task<(Result result,CreateInitAddressDto createInitAddressDto)> InitCreatedAddress();
    }
}
