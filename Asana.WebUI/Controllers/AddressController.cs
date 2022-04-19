using Asana.Application.Common.Interfaces;
using Asana.Application.DTOs;
using Asana.Application.Utilities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Asana.WebUI.Controllers
{
    [Authorize]
    public class AddressController : ApiControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet()]
        public async Task<IActionResult> index()
        {
            var result = await _addressService.GetAddressesAsync();

            return result.Succeeded ? JsonResponseStatus.Success(result.Value)
                : JsonResponseStatus.BadRequest(result.Errors);
        }

        [HttpGet("setAddress/{locationId}")]
        public async Task<IActionResult> SetDefaultAddress(long locationId)
        {

            var result = await _addressService.
                SetDefaultAddressAsync(locationId);

            return result.Succeeded ? JsonResponseStatus.Success(result.Value)
                : JsonResponseStatus.BadRequest(result.Errors);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAddress(AddressCreateDto addressDto)
        {
            var result = await _addressService.CreateAddressAsync(addressDto);

            return result.Succeeded ? JsonResponseStatus.SuccessCreated() :
                JsonResponseStatus.Error(result.Errors);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAddress(AddressUpdateDto addressDto)
        {
            var result = await _addressService.UpdateAddressAsync(addressDto);

            return result.Succeeded ? JsonResponseStatus.Success() :
                JsonResponseStatus.NotFound(result.Errors);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAddress(long id)
        {
            var result = await _addressService.DeleteAddressAsync(id);
            return result.Succeeded ? JsonResponseStatus.Success() :
                JsonResponseStatus.Error();
        }

        [HttpGet("allProvince")]
        public async Task<IActionResult> AllProvince()
        {
            var result = await _addressService.GetAllProvinceCityOptionsAsync();

            return result.Succeeded ? JsonResponseStatus.Success(result.Value) :
                            JsonResponseStatus.Error();
        }

    }
}
