using System.Linq;
using System.Threading.Tasks;
using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Validations;
using Asana.Application.DTOs;
using Asana.Application.Utilities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asana.WebUI.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    public class ProfileController : ApiControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserMediaFileService _userMediaFileService;

        public ProfileController(IIdentityService identityService, IUserMediaFileService userMediaFileService)
        {
            _identityService = identityService;
            _userMediaFileService = userMediaFileService;
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var result = await _identityService.GetUserInfoAsync();

            return result.Succeeded ? JsonResponseStatus.Success(result.Value)
                 : JsonResponseStatus.BadRequest(result.Errors);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(UserUpdatDto userDto)
        {
            var validator = new UserUpdateDtoValidation();
            var validationResult = await validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return JsonResponseStatus.BadRequest(errors);
            }

            var result = await _identityService.UpdateUserAsync(userDto);

            return JsonResponseStatus.Success(result);
        }

        [HttpPost("updatePhoto")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> UpdateUserPhoto(IFormFile image)
        {
            if (image is null)
            {
                return JsonResponseStatus.BadRequest(new string[] {"INAVLID INPUT!"});
            }

            var result =   await _identityService.UpdateUserPhotoAsync(image);
                return result.Succeeded ? JsonResponseStatus.Success(result.Value)
                   : JsonResponseStatus.Error();
        }


        [HttpDelete("deletePhoto")]
        public async Task<IActionResult> RemoveUserPhoto() 
        {
           var result =  await _identityService.RemoveUserPhotoAsync();

            return result.Succeeded ? JsonResponseStatus.Success() : JsonResponseStatus.Error();
        }


        [HttpGet("getPhotourl")]
        public async Task<IActionResult> GetUserPhotoUrl()
        {
            var result = await _userMediaFileService.GetPhotoUrl();

            return result.Succeeded ? JsonResponseStatus.Success(result) : JsonResponseStatus.Error();
        }

    }
}
