using System.Threading.Tasks;
using Asana.Application.Common.Interfaces;
using Asana.Application.DTOs;
using Asana.Application.Utilities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asana.WebUI.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    public class AddressController : ApiControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (result,addressDtos) = await _addressService.GetAddressesAsync();

            return result.Succeeded ? JsonResponseStatus.Success(addressDtos)
                : JsonResponseStatus.BadRequest(result.Errors);
        }


        [HttpPost]
        public async Task<IActionResult> Index(AddressCreateDto addressDto)
        {
            var result = await _addressService.CreateAddressAsync(addressDto);

            return result.Succeeded ? JsonResponseStatus.SuccessCreated() :
                JsonResponseStatus.Error(result.Errors);
        }

        [HttpGet("setAddress/{id}")]
        public async Task<IActionResult> SetDefaultAddress(long id)
        {

            var (result,addressDto) = await _addressService.
                SetDefaultAddressAsync(id);

            return result.Succeeded ? JsonResponseStatus.Success(addressDto)
                : JsonResponseStatus.BadRequest(result.Errors);
        }

        
        [HttpGet("create")]
        public async Task<IActionResult> CreateAddress()
        {
            var (result , createInitAddressDto) = await _addressService.InitCreatedAddress();

            return result.Succeeded ? JsonResponseStatus.Success(createInitAddressDto) :
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
        
    }
}
